//
// Copyright 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using Carbonfrost.Commons.Spec.MyersDiff;

namespace Carbonfrost.Commons.Spec.ExecutionModel.Output {

    internal class ConsolePatch : ConsoleOutputPart<Patch> {

        public int ContextLines { get; set; }
        public bool ShowWhitespace { get; set; }

        protected override void RenderCore(Patch data) {
            if (data == null) {
                return;
            }

            int contextLines = ContextLines;
            if (contextLines < 0) {
                contextLines = 2;
            }

            WriteLine();
            foreach (var e in data.Edits) {
                if (e.EditType == ChangeType.None) {
                    continue;
                }
                console.Muted();
                var header = string.Format("@@@ -{0},{1} +{2},{3}",
                    1 + Math.Max(0, e.BeginA - contextLines),
                    1 + Math.Min(data.B.Size(), e.EndA + contextLines),
                    1 + Math.Max(0, e.BeginB - contextLines),
                    1 + Math.Min(data.B.Size(), e.EndB + contextLines)
                );
                Write(header);
                WriteLine();

                WriteLines(" ", data.A.EnumerateLines(e.BeginA - contextLines, e.BeginA));
                if (e.EditType == ChangeType.Modified) {
                    WriteLines("-", data.A.EnumerateLines(e.BeginA, e.EndA));
                    WriteLines("+", data.B.EnumerateLines(e.BeginB, e.EndB));
                    continue;
                } else if (e.EditType == ChangeType.Added) {
                    WriteLines("+", data.B.EnumerateLines(e.BeginB, e.EndB));
                } else if (e.EditType == ChangeType.Deleted) {
                    WriteLines("-", data.B.EnumerateLines(e.BeginA, e.EndA));
                }
                WriteLines(" ", data.A.EnumerateLines(e.EndA + 1, e.EndA + contextLines));
            }
        }

        void WriteLines(string prefix, IEnumerable<string> lines) {
            if (prefix == "+") {
                console.Green();
            } else if (prefix == "-") {
                console.Red();
            } else {
                console.ResetColor();
            }

            Action<string> writeLine = (line) => WriteLine(line);
            if (ShowWhitespace) {
                writeLine = line => WriteLine(TextUtility.ShowWhitespace(line) + WhitespaceVisibleString.LF);
            }
            foreach (var e in lines) {
                Write(prefix);
                writeLine(e);
            }
            console.ResetColor();
        }

    }
}
