//
// Copyright 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Carbonfrost.Commons.Spec {

    class FspecOptionsFile {

        // Adapted from Ruby shellwords.rb, line 70
        static readonly Regex PATTERN = new Regex(
            @"\G \s*
              (?> ([^\s\\\'\""]+) | '([^\']*)' | ""((?:[^\""\\]|\\.)*)"" | (\\.?) | (\S) )
              (\s|\z)?",
            RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace
        );

        public List<string> Values {
            get;
            private set;
        }

        public FspecOptionsFile() {
            Values = new List<string>();
        }

        public static FspecOptionsFile Parse(string file) {
            var result = new FspecOptionsFile();

            foreach (var line in File.ReadAllLines(file)) {
                result.Values.AddRange(ParseLine(line));
            }
            return result;
        }

        private static string Escape(string e) {
            return Regex.Replace(e, @"\\(.)", @"\$1");
        }

        internal static IEnumerable<string> ParseLine(string line) {
            string[] words = Array.Empty<string>();
            string field = "";
            foreach (Match m in PATTERN.Matches(line)) {
                var word = m.Groups[1];
                var sq = m.Groups[2];
                var dq = m.Groups[3];
                var esc = m.Groups[4];
                var garbage = m.Groups[5];
                var sep = m.Groups[6];

                if (garbage.Success) {
                    throw new FormatException();
                }
                if (word.Success) {
                    field += word.Value;
                } else if (sq.Success) {
                    field += sq.Value;
                } else if (dq.Success) {
                    field += Escape(dq.Value);
                } else if (esc.Success) {
                    field += Escape(esc.Value);
                }
                if (sep.Success) {
                    yield return field;
                    field = "";
                }
            }
        }

        internal static IEnumerable<FspecOptionsFile> FindAll() {
            foreach (var anc in GetParentDirectories()) {
                string fileName = Path.Combine(anc, ".fspecrc");
                if (File.Exists(fileName)) {
                    yield return Parse(fileName);
                }
            }
        }

        private static IEnumerable<string> GetParentDirectories() {
            var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
            while (dir != null) {
                yield return dir.FullName;
                dir = dir.Parent;
            }
        }
    }
}
