//
// Copyright 2016, 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using Carbonfrost.Commons.Spec.MyersDiff;

namespace Carbonfrost.Commons.Spec {

    class Patch {
        private readonly StringLinesSequence _a;
        private readonly StringLinesSequence _b;
        private readonly MyersDiffAlgorithm _ma;

        public int ALineCount {
            get {
                return _a.Lines.Length;
            }
        }

        public int BLineCount {
            get {
                return _b.Lines.Length;
            }
        }

        public List<Edit> Edits {
            get {
                return _ma.GetEdits();
            }
        }

        public StringLinesSequence A {
            get {
                return _a;
            }
        }

        public StringLinesSequence B {
            get {
                return _b;
            }
        }

        public Patch(string exExpected, string exActual) {
            _a = new StringLinesSequence(exExpected ?? string.Empty);
            _b = new StringLinesSequence(exActual ?? string.Empty);
            _ma = new MyersDiffAlgorithm(_a, _b);
        }

        public string ToString(int contextLines) {
            if (contextLines < 0) {
                contextLines = 2;
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            foreach (var e in _ma.GetEdits()) {
                if (e.EditType == ChangeType.None) {
                    continue;
                }
                sb.AppendFormat("@@@ -{0},{1} +{2},{3}",
                                1 + Math.Max(0, e.BeginA - contextLines),
                                1 + Math.Min(_b.Size(), e.EndA + contextLines),
                                1 + Math.Max(0, e.BeginB - contextLines),
                                1 + Math.Min(_b.Size(), e.EndB + contextLines));
                sb.AppendLine();
                AppendLines(sb, " ", _a.EnumerateLines(e.BeginA - contextLines, e.BeginA));
                if (e.EditType == ChangeType.Modified) {
                    AppendLines(sb, "-", _a.EnumerateLines(e.BeginA, e.EndA));
                    AppendLines(sb, "+", _b.EnumerateLines(e.BeginB, e.EndB));
                    continue;
                } else if (e.EditType == ChangeType.Added) {
                    AppendLines(sb, "+", _b.EnumerateLines(e.BeginB, e.EndB));
                } else if (e.EditType == ChangeType.Deleted) {
                    AppendLines(sb, "-", _b.EnumerateLines(e.BeginA, e.EndA));
                }
                AppendLines(sb, " ", _a.EnumerateLines(e.EndA + 1, e.EndA + contextLines));
            }
            return sb.ToString();
        }

        public override string ToString() {
            return ToString(2);
        }

        static void AppendLines(StringBuilder sb, string prefix, IEnumerable<string> lines) {
            foreach (var e in lines) {
                sb.Append(prefix).AppendLine(e);
            }
        }
    }

}
