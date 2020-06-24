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
using System.Runtime.ExceptionServices;
using System.Text;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    class ExceptionStackTraceFilter {

        public string Message {
            get;
        }

        public string StackTrace {
            get;
        }

        public string FilteredStackTrace {
            get;
        }

        public ExceptionStackTraceFilter(Exception exception) {
            Message = FormatExceptionMessage(exception);
            StackTrace = FormatStackTrace(exception, false);
            FilteredStackTrace = FormatStackTrace(exception, true);
        }

        public override string ToString() {
            return ToString(false);
        }

        public string ToString(bool full) {
            return string.Join("\n", Message, full ? StackTrace : FilteredStackTrace );
        }

        private static string FormatExceptionMessage(Exception ex) {
            string message = ex.Message;
            string className = ex.GetType().FullName;

            // Don't use assertion exception text since it is redundant
            if (ex is AssertException) {
                return message;
            }
            if (message == null || message.Length <= 0) {
                return className;
            }
            return className + ": " + message;
        }

        private static string FormatStackTrace(Exception ex, bool filter) {
            var sb = new StringBuilder();
            FormatStackTrace(sb, ex, filter);
            return sb.ToString();
        }

        private static void FormatStackTrace(StringBuilder sb, Exception ex, bool filter) {
            ex = ExceptionDispatchInfo.Capture(ex).SourceException ?? ex;
            if (ex.InnerException != null) {
                sb.Append(" ---> ");
                sb.AppendLine(FormatExceptionMessage(ex.InnerException));
                FormatStackTrace(sb, ex.InnerException, filter);
                sb.AppendLine();
                sb.AppendLine("   --- End of inner exception stack trace ---");
            }
            string stackTrace = FilterStackTrace(filter, ex.StackTrace);
            if (stackTrace != null) {
                sb.Append(stackTrace);
            }
        }

        private static string FilterStackTrace(bool enabled, string stackTrace) {
            if (!enabled) {
                return stackTrace;
            }
            if (stackTrace == null) {
                return null;
            }

            var results = new List<string>();

            foreach (string line in SplitLines(stackTrace)) {
                string trimmedLine = line.TrimStart();
                if (!ExcludeStackFrame(trimmedLine)) {
                    results.Add(line);
                }
            }

            return string.Join(Environment.NewLine, results.ToArray());
        }

        static bool ExcludeStackFrame(string stackFrame) {
            if (stackFrame == null) {
                return false;
            }

            return stackFrame.StartsWith("at Carbonfrost.Commons.Spec.", StringComparison.Ordinal);
        }

        static IEnumerable<string> SplitLines(string input) {
            while (true) {
                int idx = input.IndexOf(Environment.NewLine, StringComparison.Ordinal);

                if (idx < 0) {
                    yield return input;
                    break;
                }

                yield return input.Substring(0, idx);
                input = input.Substring(idx + Environment.NewLine.Length);
            }
        }

    }

}
