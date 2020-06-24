//
// Copyright 2016, 2017, 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
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

        public TestFailure TestFailure {
            get;
            private set;
        }

        public string UserMessage {
            get;
            private set;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue(nameof(UserMessage), UserMessage);

            base.GetObjectData(info, context);
        }
    }
}
