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
using System.ComponentModel;

namespace Carbonfrost.Commons.Spec {

    public struct ExceptionExpectation {

        private readonly IExpectationCommand _cmd;

        internal ExceptionExpectation(IExpectationCommand cmd) {
            _cmd = cmd;
        }

        internal void Should(ITestMatcher matcher, string message = null, params object[] args) {
            var failure = _cmd.Should(matcher);

            if (failure != null) {
                throw failure.UpdateTestSubject().UpdateMessage(message, args).ToException();
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This is an override of Object.Equals(). Call Assert.Equal() instead.", true)]
        public new bool Equals(object b) {
            throw new InvalidOperationException("Expectation.Equals should not be used");
        }
    }
}
