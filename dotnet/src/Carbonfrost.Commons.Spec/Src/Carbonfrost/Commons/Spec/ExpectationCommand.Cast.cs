//
// Copyright 2018 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.Commons.Spec {

    partial class ExpectationCommand {

        class CastCommand<TFrom, T> : ExpectationCommand<T> {

            private readonly ExpectationCommand<TFrom> _inner;

            public CastCommand(ExpectationCommand<TFrom> inner) {
                _inner = inner;
            }

            public override ExpectationCommand<TBase> As<TBase>() {
                return _inner.As<TBase>();
            }

            public override ExpectationCommand<T> Negated() {
                return new CastCommand<TFrom, T>(_inner.Negated());
            }

            public override ExpectationCommand<object> ToAll() {
                return _inner.ToAll();
            }

            public override ExpectationCommand<object> ToAny() {
                return _inner.ToAny();
            }

            public override ExpectationCommand<object> Cardinality(int? min, int? max) {
                return _inner.Cardinality(min, max);
            }

            public override ExpectationCommand<T> Eventually(TimeSpan duration) {
                return new CastCommand<TFrom, T>(_inner.Eventually(duration));
            }

            public override ExpectationCommand<T> Consistently(TimeSpan duration) {
                return new CastCommand<TFrom, T>(_inner.Consistently(duration));
            }

            public override TestFailure Should(ITestMatcher<T> matcher) {
                return _inner.Should(new CastProvider(matcher));
            }

            public override void Implies(CommandCondition c) {
                _inner.Implies(c);
            }

            struct CastProvider : ITestMatcher<TFrom>, ISupportTestMatcher {

                private readonly ITestMatcher<T> _real;

                public CastProvider(ITestMatcher<T> real) {
                    _real = real;

                    // Don't be reentrant with the cast provider type itself
                    Debug.Assert(!_real.GetType().Name.Contains( "CastProvider"));
                }

                object ISupportTestMatcher.RealMatcher {
                    get {
                        return _real;
                    }
                }

                public bool Matches(Func<TFrom> actualFactory) {
                    var real = TestMatcherLocalizer.Code(_real.GetType().GetTypeInfo());
                    Func<T> thunk = () => {
                        var actual = actualFactory();
                        try {
                            return (T) (object) actual;

                        } catch (InvalidCastException e) {
                            throw SpecFailure.CastRequiredByMatcherFailure(e, real);
                        }
                    };

                    return _real.Matches(thunk);
                }

            }
        }
    }
}
