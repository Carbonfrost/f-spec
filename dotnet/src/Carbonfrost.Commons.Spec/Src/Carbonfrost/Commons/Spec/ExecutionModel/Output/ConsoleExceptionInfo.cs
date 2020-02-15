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
using System.IO;
using Carbonfrost.Commons.Spec.Resources;

namespace Carbonfrost.Commons.Spec.ExecutionModel.Output {

    class ConsoleExceptionInfo : ConsoleOutputPart<ExceptionInfo> {

        static readonly Regex ERGO = new Regex(@"\Aat (?<module>.+) in (?<file>.+):line (?<line>\d+)\Z");
        static readonly Uri current = new Uri("file://" + Directory.GetCurrentDirectory() + "/");
        static readonly string[] SPACES = Enumerable.Range(0, 10)
            .Select(m => "// " + new string(' ', m))
            .ToArray();
        const int bufferWidth = 6;

        public int ContextLines { get; set; }
        public bool ShowWhitespace { get; set; }

        protected override void RenderCore(ExceptionInfo exceptionInfo) {
            if (exceptionInfo == null) {
                return;
            }

            console.WriteLine();
            console.WriteLine(exceptionInfo.Message.TrimEnd('\r', '\n'));
            if (exceptionInfo.TestFailure != null) {
                Append(exceptionInfo.TestFailure.UserData);
            }
            console.WriteLine();

            console.Muted();
            var traces = exceptionInfo.StackTrace.TrimEnd('\r', '\n').Split(
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
                var file = current.MakeRelativeUri(
                    new Uri("file://" + m.Groups["file"].Value)
                );
                console.WriteLine(" in");
                console.Write(ws);
                console.Write($"   {file}:{line}");
            }
            console.WriteLine();
        }

        private void Append(UserDataCollection data) {
            if (data.Keys.Count <= 0) {
                return;
            }

            int maxLength = data.Keys.Max(t => t.Length);

            WriteLine();
            foreach (var kvp in data) {
                if (data.IsHiddenFromTable(kvp.Key)) {
                    continue;
                }
                WriteLineItem(kvp.Key, ShowWS(kvp.Value), maxLength);
            }

            if (data.Diff != null) {
                WriteLineItem("Diff", "", maxLength);
                WriteLine(data.Diff.ToString(ContextLines));
            }
        }

        private void WriteLineItem(string caption, string data, int length) {
            caption = Caption(caption);
            Write(string.Format("{0," + (bufferWidth + length) + "}: ", caption));
            WriteLine(data);
        }

        private string ShowWS(string text) {
            if (ShowWhitespace) {
                return TextUtility.ShowWhitespace(text);
            }
            return text;
        }

        internal static string Caption(string caption) {
            var cap = "Label" + caption;
            return SR.ResourceManager.GetString(cap) ?? TestMatcherLocalizer.MissingLocalization(caption);
        }
    }
}
