//
// Copyright 2018, 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Linq;
using System.Text;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    public class ExceptionInfo {

        public string Message {
            get;
            private set;
        }

        public string StackTrace {
            get;
            private set;
        }

        public Type ExceptionType {
            get;
            private set;
        }

        public TestFailure TestFailure {
            get;
            private set;
        }

        private ExceptionInfo() {
        }

        public ExceptionInfo(Type exceptionType, string message, string stackTrace, TestFailure testFailure) {
            Message = message;
            StackTrace = stackTrace;
            ExceptionType = exceptionType;
            TestFailure = testFailure;
        }

        public static ExceptionInfo FromException(Exception ex) {
            var sb = new StringBuilder();
            FormatStackTrace(sb, ex);
            TestFailure failure = null;
            if (ex is AssertException aex) {
                failure = aex.TestFailure;
            }

            return new ExceptionInfo {
                StackTrace = sb.ToString(),
                Message = FormatExceptionMessage(ex),
                ExceptionType = ex.GetType(),
                TestFailure = failure,
            };
        }

        static string FormatExceptionMessage(Exception ex) {
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

        static void FormatStackTrace(StringBuilder sb, Exception ex) {
            if (ex.InnerException != null) {
                sb.Append(" ---> ");
                sb.AppendLine(FormatExceptionMessage(ex.InnerException));
                FormatStackTrace(sb, ex.InnerException);
                sb.AppendLine();
                sb.AppendLine("   --- End of inner exception stack trace ---");
            }
            string stackTrace = ex.StackTrace;
            if (stackTrace != null) {
                sb.Append(stackTrace);
            }
        }
    }
}
