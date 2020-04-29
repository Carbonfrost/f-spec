//
// Copyright 2017, 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.Commons.Spec {

    partial class ExpectationCommand {

        public static ExpectationCommand<T> Of<T>(Func<T> thunk) {
            return new SingletonCommand<T>(thunk);
        }

        public static ExpectationCommand<Unit> TestCode(Action action) {
            return new SingletonCommand<Unit>(Unit.Thunk(action));
        }

        class SingletonCommand<T> : ExpectationCommand<T> {

            private readonly Func<T> _thunk;

            public SingletonCommand(Func<T> thunk) {
                _thunk = thunk;
            }

            public override ExpectationCommand<T> Negated() {
                return new NegationCommand<T>(this);
            }

            public override ExpectationCommand<Unit> Untyped() {
                var myThunk = _thunk;
                return TestCode(() => myThunk());
            }

            public override ExpectationCommand<object> ToAll() {
                return new AllCommand<object>(MakeThunkEnum());
            }

            public override ExpectationCommand<object> ToAny() {
                return new AnyCommand<object>(MakeThunkEnum());
            }

            public override ExpectationCommand<object> Cardinality(int? min, int? max) {
                return new CardinalityCommand<object>(MakeThunkEnum(), min, max, false);
            }

            public override ExpectationCommand<T> Eventually(TimeSpan duration) {
                return new EventuallyCommand<T>(duration, _thunk);
            }

            public override ExpectationCommand<T> Consistently(TimeSpan duration) {
                return new ConsistentlyCommand<T>(duration, _thunk);
            }

            public override TestFailure Should(ITestMatcher<T> matcher) {
                var actual = TestActual.Of(_thunk);
                bool matches = matcher.Matches(actual);

                if (!matches) {
                    object reportedActual = actual.Value;
                    if (matcher is ITestMatcherActualException) {
                        reportedActual = actual.Exception;
                    }
                    return TestMatcherLocalizer.Failure(matcher, reportedActual);
                }
                return null;
            }

            public override ExpectationCommand<Exception> CaptureException() {
                return new CaptureExceptionCommand(Unit.DiscardResult(_thunk), false);
            }

            private Func<IEnumerable<object>> MakeThunkEnum() {
                var t = _thunk;
                return () => {
                    IEnumerable e;

                    try {
                        e = ((IEnumerable) t());

                    } catch (InvalidCastException ex) {
                        throw SpecFailure.CastRequiredByMatcherFailure(ex, "sequence");
                    }

                    if (e == null) {
                        throw SpecFailure.SequenceNullConversion();
                    }
                    return e.Cast<object>();
                };
            }
        }
    }

}
