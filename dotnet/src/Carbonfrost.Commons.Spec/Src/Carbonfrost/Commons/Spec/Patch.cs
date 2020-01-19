//
// Copyright 2016 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using Carbonfrost.Commons.Spec.ExecutionModel;
using Carbonfrost.Commons.Spec.MyersDiff;

namespace Carbonfrost.Commons.Spec {

    class Patch {

        private readonly string _result;

        public int ALineCount { get; private set; }
        public int BLineCount { get; private set; }

        public Patch(string exExpected, string exActual) {
            int contextLines = 2;
            if (TestRunner.Current != null) {
                contextLines = TestRunner.Current.Options.ContextLines;
            }
            if (contextLines < 0) {
                contextLines = 2;
            }
            var a = new StringLinesSequence(exExpected);
            var b = new StringLinesSequence(exActual);
            var ma = new MyersDiffAlgorithm(a, b);

            ALineCount = a.Lines.Length;
            BLineCount = b.Lines.Length;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            foreach (var e in ma.GetEdits()) {
                if (e.EditType == ChangeType.None) {
                    continue;
                }
                sb.AppendFormat("@@@ -{0},{1} +{2},{3}",
                                1 + Math.Max(0, e.BeginA - contextLines),
                                1 + Math.Min(b.Size(), e.EndA + contextLines),
                                1 + Math.Max(0, e.BeginB - contextLines),
                                1 + Math.Min(b.Size(), e.EndB + contextLines));
                sb.AppendLine();
                AppendLines(sb, " ", a.EnumerateLines(e.BeginA - contextLines, e.BeginA));
                if (e.EditType == ChangeType.Modified) {
                    AppendLines(sb, "-", a.EnumerateLines(e.BeginA, e.EndA));
                    AppendLines(sb, "+", b.EnumerateLines(e.BeginB, e.EndB));
                    continue;
                } else if (e.EditType == ChangeType.Added) {
                    AppendLines(sb, "+", b.EnumerateLines(e.BeginB, e.EndB));
                } else if (e.EditType == ChangeType.Deleted) {
                    AppendLines(sb, "-", b.EnumerateLines(e.BeginA, e.EndA));
                }
                AppendLines(sb, " ", a.EnumerateLines(e.EndA + 1, e.EndA + contextLines));
            }
            _result = sb.ToString();
        }

        public override string ToString() {
            return _result;
        }

        static void AppendLines(StringBuilder sb, string prefix, IEnumerable<string> lines) {
            foreach (var e in lines) {
                sb.Append(prefix).AppendLine(e);
            }
        }
    }

}
