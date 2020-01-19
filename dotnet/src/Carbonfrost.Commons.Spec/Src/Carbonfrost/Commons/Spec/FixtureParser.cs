//
// Copyright 2016, 2018 Carbonfrost Systems, Inc. (http://carbonfrost.com)
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Carbonfrost.Commons.Spec {

    class FixtureParser {

        static readonly char[] LINE_TERM =  { '\r', '\n' };
        static readonly Regex RECORD = new Regex(@"^\s*-{3,}\s*$");
        static readonly Regex HEREDOC = new Regex(@"^(?<FileName>.+) \s*: \s* (?<Fold>[>+\|-]+?)? \s*$",
                                                  RegexOptions.IgnorePatternWhitespace);

        private readonly Uri _baseUri;
        private Scanner _lex;

        public FixtureParser(Uri baseUri) {
            _baseUri = baseUri;
        }

        public IEnumerable<TestFixtureData> Parse(string text) {
            var all = text.Split(LINE_TERM, StringSplitOptions.RemoveEmptyEntries);
            var result = new List<TestFixtureData>();
            _lex = new Scanner(all);

            while (!EOF) {
                var record = ParseRecord();
                if (record == null) {
                    return null;
                }
                if (record.Values.Count > 0) {
                    result.Add(record);
                }
            }
            if (!EOF) {
                return null;
            }
            return result;
        }

        private bool EOF {
            get {
                return _lex.EOF;
            }
        }

        private string Current {
            get {
                return _lex.Current;
            }
        }

        private bool MoveNext() {
            return _lex.MoveNext();
        }

        private bool SkipBlankLines() {
            while (MoveNext()) {
                if (!IsBlank(Current)) {
                    return true;
                }
            }
            return false;
        }

        private TestFixtureData ParseRecord() {
            var results = new TestFixtureData();
            while (SkipBlankLines()) {
                if (RECORD.IsMatch(Current)) {
                    return results;
                }
                var item = ParseFieldOrHeredoc();
                results.Values.Add(item);
            }
            return results;
        }

        private KeyValuePair<string, string> ParseFieldOrHeredoc() {
            string line = _lex.Current.Trim();
            Match heredoc = HEREDOC.Match(line);
            if (heredoc.Success) {
                return ParseHeredoc(heredoc);
            }

            string[] kvp = line.Split(new[] { ':' }, 2);
            if (kvp.Length != 2) {
                throw new FormatException();
            }
            return new KeyValuePair<string, string>(kvp[0].Trim(), kvp[1].Trim());
        }

        private KeyValuePair<string, string> ParseHeredoc(Match match) {
            string fileName = match.Groups["FileName"].Value;
            FoldLines fold = GetFoldType(match.Groups["Fold"].Value);

            if (!MoveNext()) {
                return new KeyValuePair<string, string>(fileName, string.Empty);
            }

            StringBuilder sb = new StringBuilder();
            string indentation = Regex.Match(Current, @"^\s*").Value;
            if (indentation.Length == 0) {
                // Encountered an empty here doc, treat as value
                _lex.MovePrevious();
                return new KeyValuePair<string, string>(fileName, string.Empty);
            }

            foreach (var canonical in ReadHeredocLines(indentation)) {
                fold(sb, canonical);
            }

            var body = sb.ToString();
            _lex.MovePrevious();
            return new KeyValuePair<string, string>(fileName, body);
        }

        IEnumerable<string> ReadHeredocLines(string indentation) {
            int length = indentation.Length;
            while (!EOF && Current.StartsWith(indentation, StringComparison.Ordinal)) {
                // TODO Review yaml support for \ at end of line?
                // TODO yaml-style indentation
                yield return Current.Substring(length).TrimEnd();

                if (!MoveNext()) {
                    break;
                }
            }
        }

        static FoldLines GetFoldType(string value) {
            switch (value) {
                case "-":
                    return KeepIndentationFold;
                case "|":
                    return TotalFold;
                case ">-":
                    return StandardFoldLineEnds;
                case ">+":
                    return TotalFoldInternalWS;
                case ">":
                default:
                    return StandardFold;
            }
        }

        private static bool IsBlank(string text) {
            return string.IsNullOrWhiteSpace(text) || text.StartsWith("#");
        }

        delegate void FoldLines(StringBuilder sb, string line);

        static void StandardFold(StringBuilder sb, string line) { // >
            if (sb.Length > 0) {
                sb.Append(" "); // fold lines
            }
            sb.Append(line.Trim());
        }

        static void KeepIndentationFold(StringBuilder sb, string line) { // -
            // trim leading indentation, trailing spaces (except if the line ends
            // with a backslash), leave other whitespace, including separators
            if (sb.Length > 0) {
                sb.AppendLine(); // fold lines
            }
            if (line.EndsWith("\\", StringComparison.Ordinal)) {
                line = line.Substring(0, line.Length - 1);
                sb.Append(line);
            } else {
                sb.Append(line.TrimEnd());
            }
        }

        static void TotalFold(StringBuilder sb, string line) { // |
            // remove all whitespace (useful for binary data)
            sb.Append(Regex.Replace(line, @"\s+", ""));
        }

        static void StandardFoldLineEnds(StringBuilder sb, string line) { // >-
            // trim lines, join lines with line sep
            if (sb.Length > 0) {
                sb.AppendLine();
            }
            sb.Append(line.Trim());
        }

        static void TotalFoldInternalWS(StringBuilder sb, string line) { // >+
            // trim lines, join lines as is
            sb.Append(line.TrimEnd());
        }

        class Scanner : IEnumerator<string> {

            private string[] _inner;
            private int _index = -1;

            public Scanner(string[] items) {
                _inner = items;
            }

            public bool EOF {
                get {
                    return _index >= _inner.Length;
                }
            }

            public string Current {
                get {
                    return _inner[_index];
                }
            }

            object System.Collections.IEnumerator.Current {
                get {
                    return Current;
                }
            }

            public void Dispose() {
            }

            public void MovePrevious() {
                _index--;
            }

            public bool MoveNext() {
                _index++;
                return !EOF;
            }

            public void Reset() {
            }
        }
    }
}
