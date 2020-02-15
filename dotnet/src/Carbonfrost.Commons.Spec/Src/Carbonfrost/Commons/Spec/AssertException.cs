//
// Copyright 2013 Outercurve Foundation
// Copyright 2016, 2017 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Runtime.Serialization;
using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.Commons.Spec {

    [Serializable]
    public class AssertException : Exception {

        public AssertException() {
        }

        public AssertException(string userMessage)
            : base(userMessage) {
            UserMessage = userMessage;
        }

        public AssertException(string userMessage, Exception innerException)
            : base(userMessage, innerException) {
            UserMessage = userMessage;
        }

        public AssertException(string userMessage, TestFailure failure, Exception innerException)
            : base(failure.FormatMessage(userMessage), innerException) {

            UserMessage = userMessage;
            TestFailure = failure;
        }

        protected AssertException(SerializationInfo info, StreamingContext context)
            : base(info, context) {
            UserMessage = info.GetString("UserMessage");
        }

        public override string StackTrace {
            get { return FilterStackTrace(base.StackTrace); }
        }

        public TestFailure TestFailure {
            get;
            private set;
        }

        public string UserMessage { get; private set; }

        protected virtual bool ExcludeStackFrame(string stackFrame) {
            if (stackFrame == null) {
                throw new ArgumentNullException("stackFrame");
            }
            if (!EnvironmentHelper.ShouldExcludeStackFrames) {
                return false;
            }

            return stackFrame.StartsWith("at Carbonfrost.Commons.Spec.", StringComparison.Ordinal);
        }

        protected string FilterStackTrace(string stack) {
            if (stack == null) {
                return null;
            }

            var results = new List<string>();

            foreach (string line in SplitLines(stack)) {
                string trimmedLine = line.TrimStart();
                if (!ExcludeStackFrame(trimmedLine)) {
                    results.Add(line);
                }
            }

            return string.Join(Environment.NewLine, results.ToArray());
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("UserMessage", UserMessage);

            base.GetObjectData(info, context);
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
