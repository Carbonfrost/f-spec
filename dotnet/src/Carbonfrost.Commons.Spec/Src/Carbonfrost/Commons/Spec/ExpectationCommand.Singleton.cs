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

using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.Commons.Spec {

    partial class ExpectationCommand {

        public static ExpectationCommand<T> Of<T>(Func<T> thunk, bool negated, string given, bool assumption) {
            return new SingletonCommand<T>(thunk, negated, given, assumption);
        }

        public static ExpectationCommand<T> Of<T>(Func<T> thunk) {
            return Of(thunk, false, null, false);
        }

        public static ExpectationCommand<Unit> TestCode(Action action, bool negated, string given, bool assumption) {
            return Of(Unit.Thunk(action), negated, given, assumption);
        }

        public static ExpectationCommand<Unit> TestCode(Action action) {
            return TestCode(action, false, null, false);
        }

        class SingletonCommand<T> : ExpectationCommand<T> {

            private readonly Func<T> _thunk;
            private readonly bool _negated;
            private readonly string _given;
            private readonly bool _assumption;

            public SingletonCommand(Func<T> thunk, bool negated, string given, bool assumption) {
                _thunk = thunk;
                _negated = negated;
                _given = given;
                _assumption = assumption;
            }

            public override ExpectationCommand<T> Negated() {
                return new SingletonCommand<T>(_thunk, !_negated, _given, _assumption);
            }

            public override ExpectationCommand<T> Given(string given) {
                return new SingletonCommand<T>(_thunk, _negated, given, _assumption);
            }

            internal override ExpectationCommand<Unit> Untyped() {
                var myThunk = _thunk;
                return TestCode(() => myThunk(), _negated, _given, _assumption);
            }

            public override ExpectationCommand<object> ToAll() {
                return new AllCommand(this.As<IEnumerable>()).NegateIfNeeded(_negated);
            }

            public override ExpectationCommand<object> ToAny() {
                return new AnyCommand(this.As<IEnumerable>()).NegateIfNeeded(_negated);
            }

            public override ExpectationCommand<object> Cardinality(int? min, int? max) {
                return new CardinalityCommand(this.As<IEnumerable>(), min, max, false).NegateIfNeeded(_negated);
            }

            public override TestFailure Should(ITestMatcher<T> matcher) {
                var actual = TestActual.Of(_thunk);
                if (_negated) {
                    matcher = Matchers.Not(matcher);
                }
                if (matcher.Matches(actual)) {
                    return null;
                }

                var result = TestMatcherLocalizer.Failure(matcher, actual.Value)
                    .UpdateGiven(_given)
                    .UpdateAssumption(_assumption);

                if (matcher is ITestMatcher<Unit> m) {
                    result.UpdateActual(DisplayActual.Exception(actual.Exception));
                }
                return result;
            }
        }
    }

}
