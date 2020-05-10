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

    struct ExceptionExpectation : IExceptionExpectation {

        private readonly ExpectationCommand<Unit> _cmd;

        public IExpectation<string> Message {
            get {
                return new Expectation<string>(
                    _cmd.CaptureException().Property(ex => ex.Message)
                );
            }
        }

        public IExpectation<Exception> Value {
            get {
                return new Expectation<Exception>(_cmd.CaptureException());
            }
        }

        public IExpectation<Exception> InnerException {
            get {
                return new Expectation<Exception>(
                    _cmd.CaptureException().Property(ex => ex.InnerException)
                );
            }
        }

        public IExpectation Not {
            get {
                throw new NotImplementedException();
            }
        }

        internal ExceptionExpectation(ExpectationCommand<Unit> cmd) {
            _cmd = cmd;
        }

        public new bool Equals(object b) {
            throw new InvalidOperationException("Expectation.Equals should not be used");
        }

        public void Like(ITestMatcher matcher, string message, params object[] args) {
            _cmd.Should(matcher, message, (object[]) args);
        }
    }
}
