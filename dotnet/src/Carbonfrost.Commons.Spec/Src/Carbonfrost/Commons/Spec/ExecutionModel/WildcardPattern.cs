//
// Copyright 2010, 2015, 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Text;
using System.Text.RegularExpressions;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    class WildcardPattern {

        private Regex _regexCache;
        private readonly string _text;
        private static readonly char[] IMPLIED_UPGRADE_CHARS = {
            '?',
            '[',
            ']',
            '*',
        };

        public WildcardPattern() : this("*") {}

        public WildcardPattern(string text,
                               RegexOptions options = RegexOptions.None) : this(text, false, options) {
        }

        private WildcardPattern(string text,
                                bool containing,
                                RegexOptions options = RegexOptions.None) {
            if (text == null) {
                throw new ArgumentNullException("text"); // $NON-NLS-1
            }
            _text = text;
            _regexCache = new Regex(TransformPattern(containing), options);
        }

        // Given "text", assumes you meant "*test*"; that is, we upgrade wildcards unless you
        // have specified a wildcard character
        internal static WildcardPattern Containing(string text) {
            return new WildcardPattern(
                text,
                text.IndexOfAny(IMPLIED_UPGRADE_CHARS) < 0,
                RegexOptions.IgnorePatternWhitespace
            );
        }

        string TransformPattern(bool containing) {
            StringBuilder sb = new StringBuilder();

            if (!containing) {
                sb.Append(@"\A");
            }
            var c = ((IEnumerable<char>) _text).GetEnumerator();
            while (c.MoveNext()) {
                if (c.Current == '\\') {
                    c.MoveNext();
                    sb.Append('\\');
                    sb.Append(c.Current);
                }

                string str = MapChar(c.Current);
                if (str == null)
                    sb.Append(c.Current);
                else
                    sb.Append(str);
            }
            if (!containing) {
                sb.Append(@"\Z");
            }
            return sb.ToString();
        }

        static string MapChar(char c) {
            switch (c) {
                case '(':
                case ')':
                case '[':
                case ']':
                case '.':
                case '+':
                case '$':
                case '^':
                    return "\\" + c;
                case '*':
                    return @"[^\s]*";
                case '?':
                    return @"[^\s]";
                default:
                    return null;
            }
        }

        public Regex ToRegex() {
            return _regexCache;
        }

        public override string ToString() {
            return _text;
        }

        public bool IsMatch(string value) {
            if (value == null) {
                return false;
            }

            Match m = ToRegex().Match(value);
            return m.Success;
        }
    }
}
