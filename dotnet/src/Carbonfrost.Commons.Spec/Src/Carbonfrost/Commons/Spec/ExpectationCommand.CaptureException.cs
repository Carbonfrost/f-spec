//
// Copyright 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
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
using System.Diagnostics;
using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.Commons.Spec {

    partial class ExpectationCommand {

        internal sealed class CaptureExceptionCommand<T> : ExpectationCommand<Exception> {

            private readonly ExpectationCommand<T> _inner;

            public CaptureExceptionCommand(ExpectationCommand<T> inner) {
                _inner = inner;
            }

            public override ExpectationCommand<Exception> Given(string given) {
                return new CaptureExceptionCommand<T>(_inner.Given(given));
            }

            public override TestFailure Should(ITestMatcher<Exception> matcher) {
                var pp = new CaptureProvider(matcher);
                var failure = _inner.Should(pp);
                if (failure != null) {
                    failure.UpdateActual(pp.Actual.Value);
                }
                return failure;
            }

            public override ExpectationCommand<Exception> Negated() {
                return new CaptureExceptionCommand<T>(_inner.Negated());
            }

            class CaptureProvider : ITestMatcher<T>, ISupportTestMatcher {

                private readonly ITestMatcher<Exception> _real;

                public ITestActualEvaluation<Exception> Actual {
                    get;
                    set;
                }

                public CaptureProvider(ITestMatcher<Exception> real) {
                    _real = real;

                    // Don't be reentrant with the provider type itself
                    Debug.Assert(!_real.GetType().Name.Contains("CaptureProvider"));
                }

                public bool Matches(ITestActualEvaluation<T> actualFactory) {
                    return _real.Matches(
                        Actual = TestActual.Value(actualFactory.Exception)
                    );
                }

                object ISupportTestMatcher.RealMatcher {
                    get {
                        return _real;
                    }
                }
            }
        }
    }
}
