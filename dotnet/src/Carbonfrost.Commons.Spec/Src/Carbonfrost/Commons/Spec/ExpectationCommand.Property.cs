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

        internal class PropertyCommand<T, TProperty> : ExpectationCommand<TProperty> {

            private readonly ExpectationCommand<T> _inner;
            private readonly Func<T, TProperty> _accessor;
            private readonly string _name;

            public PropertyCommand(ExpectationCommand<T> inner, Func<T, TProperty> accessor, string name) {
                _inner = inner;
                _accessor = accessor;
                _name = name;
            }

            public override ExpectationCommand<TProperty> Given(string given) {
                return new PropertyCommand<T, TProperty>(_inner.Given(given), _accessor, _name);
            }

            public override TestFailure Should(ITestMatcher<TProperty> matcher) {
                var pp = new PropertyProvider(this, matcher);
                var failure = _inner.Should(pp);
                if (failure != null) {
                    failure.UpdateActual(pp.Actual.Value);
                    failure.UserData["Property"] = _name;
                }
                return failure;
            }

            public override ExpectationCommand<TProperty> Negated() {
                return new PropertyCommand<T, TProperty>(_inner.Negated(), _accessor, _name);
            }

            class PropertyProvider : ITestMatcher<T>, ISupportTestMatcher {

                private readonly ITestMatcher<TProperty> _real;
                private readonly PropertyCommand<T, TProperty> _parent;

                public ITestActualEvaluation<TProperty> Actual {
                    get;
                    set;
                }

                public PropertyProvider(PropertyCommand<T, TProperty> parent, ITestMatcher<TProperty> real) {
                    _parent = parent;
                    _real = real;

                    // Don't be reentrant with the cast provider type itself
                    Debug.Assert(!_real.GetType().Name.Contains("PropertyProvider"));
                }

                public bool Matches(ITestActualEvaluation<T> actualFactory) {
                    var myParent = _parent;
                    return _real.Matches(
                        Actual = TestActual.Of(() => myParent._accessor(actualFactory.Value))
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
