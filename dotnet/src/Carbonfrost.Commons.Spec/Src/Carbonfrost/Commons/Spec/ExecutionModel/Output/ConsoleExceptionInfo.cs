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
using System.Text.RegularExpressions;
using System.Linq;

namespace Carbonfrost.Commons.Spec.ExecutionModel.Output {

    class ConsoleExceptionInfo : ConsoleOutputPart<ExceptionInfo> {

        public bool ShowNoisyStackFrames {
            get;
            set;
        }

        static readonly Regex ERGO = new Regex(@"\Aat (?<module>.+) in (?<file>.+):line (?<line>\d+)\Z");
        static readonly string[] SPACES = Enumerable.Range(0, 10)
            .Select(m => "// " + new string(' ', m))
            .ToArray();

        protected override void RenderCore(ExceptionInfo exceptionInfo) {
            if (exceptionInfo == null) {
                return;
            }

            console.WriteLine();
            console.WriteLine(exceptionInfo.Message.TrimEnd('\r', '\n'));
            if (exceptionInfo.TestFailure != null) {
                context.Parts.forUserData.Render(context, exceptionInfo.TestFailure.UserData);
            }
            console.WriteLine();

            console.Muted();
            var st = ShowNoisyStackFrames ? exceptionInfo.StackTrace : exceptionInfo.FilteredStackTrace;
            var traces = st.TrimEnd('\r', '\n').Split(
                new[] { "\r\n", "\n" }, StringSplitOptions.None
            );
            foreach (var msg in traces) {
                ErgonomicFormat(msg);
            }
            console.ResetColor();
        }

        // More readable stack traces by reducing leading whitespace, wrapping
        // source lines to next line and using GCC-style pointers
        private void ErgonomicFormat(string stackFrame) {
            var trimmed = stackFrame.TrimStart();
            var m = ERGO.Match(trimmed);

            string ws = SPACES[
                Math.Min(9, Math.Max(0, stackFrame.Length - trimmed.Length - 3))
            ];

            if (!m.Success) {
                console.Write(ws);
                console.WriteLine(trimmed);
                return;
            }

            var module = m.Groups["module"];
            var line = m.Groups["line"].Value;

            console.Write(ws);
            console.Write($"at {module}");
            if (line.Length > 0) {
                var file = Utility.MakeRelativePath(m.Groups["file"].Value);
                console.WriteLine(" in");
                console.Write(ws);
                console.Write($"   {file}:{line}");
            }
            console.WriteLine();
        }
    }
}
