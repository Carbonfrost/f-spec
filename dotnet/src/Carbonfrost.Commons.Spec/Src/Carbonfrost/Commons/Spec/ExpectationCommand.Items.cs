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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.Commons.Spec {

    partial class ExpectationCommand {

        internal static ExpectationCommand<IEnumerable<object>> Items(this ExpectationCommand<IEnumerable> inner) {
            // Provide transparent access to either IEnumerable<object> or IEnumerable depending
            // upon what the matcher implements.  This primarily pertains to HaveLengthMatcher so that
            // it can be used with Array and String using their idiosyncratic handling of
            // IEnumerable and IEnumerable<T>
            return new ExpectationCommand.ItemsCommand(inner);
        }

        internal static ExpectationCommand<IEnumerable<T>> Items<T>(this ExpectationCommand<IEnumerable> inner) {
            return new ExpectationCommand.ItemsCommand<T>(inner);
        }

        internal class ItemsCommand : ExpectationCommand<IEnumerable<object>> {

            private readonly ExpectationCommand<IEnumerable> _inner;

            public ItemsCommand(ExpectationCommand<IEnumerable> inner) {
                _inner = inner;
            }

            public override ExpectationCommand<IEnumerable<object>> Negated() {
                return new ItemsCommand(_inner.Negated());
            }

            public override ExpectationCommand<IEnumerable<object>> Given(string given) {
                return new ItemsCommand(_inner.Given(given));
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

            public override TestFailure Should(ITestMatcher<IEnumerable<object>> matcher) {
                return _inner.Should(new ItemsProvider(matcher));
            }

            internal override void Implies(CommandCondition c) {
                _inner.Implies(c);
            }

            struct ItemsProvider : ITestMatcher<IEnumerable>, ISupportTestMatcher {

                private readonly ITestMatcher<IEnumerable<object>> _real;

                public ItemsProvider(ITestMatcher<IEnumerable<object>> real) {
                    _real = real;

                    // Don't be reentrant with the cast provider type itself
                    Debug.Assert(!_real.GetType().Name.Contains("ItemsProvider"));
                }

                object ISupportTestMatcher.RealMatcher {
                    get {
                        return _real;
                    }
                }

                public bool Matches(ITestActualEvaluation<IEnumerable> actualFactory) {
                    // When the matcher can already handle the input, no need to apply
                    // adapter logic
                    if (_real is ITestMatcher<IEnumerable> fast) {
                        return fast.Matches(actualFactory);
                    }

                    var real = TestMatcherName.FromType(_real.GetType());
                    Func<IEnumerable<object>> thunk = () => (
                        actualFactory.Value.Cast<object>()
                    );

                    return _real.Matches(TestActual.Of(thunk));
                }

            }
        }

        internal class ItemsCommand<T> : ExpectationCommand<IEnumerable<T>> {

            private readonly ExpectationCommand<IEnumerable> _inner;

            public ItemsCommand(ExpectationCommand<IEnumerable> inner) {
                _inner = inner;
            }

            public override ExpectationCommand<IEnumerable<T>> Negated() {
                return new ItemsCommand<T>(_inner.Negated());
            }

            public override ExpectationCommand<IEnumerable<T>> Given(string given) {
                return new ItemsCommand<T>(_inner.Given(given));
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

            public override TestFailure Should(ITestMatcher<IEnumerable<T>> matcher) {
                return _inner.Should(new ItemsProvider(matcher));
            }

            internal override void Implies(CommandCondition c) {
                _inner.Implies(c);
            }

            struct ItemsProvider : ITestMatcher<IEnumerable>, ISupportTestMatcher {

                private readonly ITestMatcher<IEnumerable<T>> _real;

                public ItemsProvider(ITestMatcher<IEnumerable<T>> real) {
                    _real = real;

                    // Don't be reentrant with the cast provider type itself
                    Debug.Assert(!_real.GetType().Name.Contains("ItemsProvider"));
                }

                object ISupportTestMatcher.RealMatcher {
                    get {
                        return _real;
                    }
                }

                public bool Matches(ITestActualEvaluation<IEnumerable> actualFactory) {
                    var real = TestMatcherName.FromType(_real.GetType());
                    Func<IEnumerable<T>> thunk = () => (
                        actualFactory.Value.Cast<T>()
                    );

                    return _real.Matches(TestActual.Of(thunk));
                }

            }
        }
    }
}
