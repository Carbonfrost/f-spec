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

    public struct ExceptionExpectation : IExpectation {

        private readonly ExpectationCommand _cmd;

        public Expectation<string> Message {
            get {
                return new Expectation<string>(
                    _cmd.CaptureException().Property(ex => ex.Message)
                );
            }
        }

        public Expectation<Exception> Value {
            get {
                return new Expectation<Exception>(_cmd.CaptureException());
            }
        }

        public Expectation<Exception> InnerException {
            get {
                return new Expectation<Exception>(
                    _cmd.CaptureException().Property(ex => ex.InnerException)
                );
            }
        }

        internal ExceptionExpectation(ExpectationCommand cmd) {
            _cmd = cmd;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This is an override of Object.Equals(). Call Assert.Equal() instead.", true)]
        public new bool Equals(object b) {
            throw new InvalidOperationException("Expectation.Equals should not be used");
        }

        ExpectationCommand IExpectation.ToCommand() {
            return _cmd;
        }
    }
}
