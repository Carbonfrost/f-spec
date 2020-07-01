//
// Copyright 2019, 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
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
using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.Commons.Spec {

    internal partial class Asserter {

        internal static readonly Asserter Default = new Asserter(false);
        internal static readonly Asserter Assumptions = new Asserter(true);
        private readonly bool _assumption;

        private Asserter(bool assumption) {
            _assumption = assumption;
        }

        public IExpectationBuilder<IEnumerable> Expect() {
            return new ExpectationBuilder<IEnumerable>(() => Array.Empty<object>(), false, null, _assumption);
        }

        public IExpectationBuilder<T> Expect<T>(T value) {
            return new ExpectationBuilder<T>(() => value, false, null, _assumption);
        }

        public IExpectationBuilder<TEnumerable, T> Expect<TEnumerable, T>(TEnumerable value) where TEnumerable : IEnumerable<T> {
            return new ExpectationBuilder<TEnumerable, T>(() => value, false, null, _assumption);
        }

        public IExpectationBuilder<TValue[], TValue> Expect<TValue>(params TValue[] value) {
            return Expect<TValue[], TValue>(value);
        }

        public IExpectationBuilder Expect(Action value) {
            return new ExpectationBuilder(value, false, null, _assumption);
        }

        public IExpectationBuilder<T> Expect<T>(Func<T> func) {
            return Given().Expect(func);
        }

        internal void That(Action actual, ITestMatcher matcher, string message = null, params object[] args) {
            Expect(actual).To(matcher, message, (object[]) args);
        }

        internal void That<T>(T actual, ITestMatcher<T> matcher, string message = null, params object[] args) {
            Expect(actual).To(matcher, message, (object[]) args);
        }

        internal void NotThat(Action actual, ITestMatcher matcher, string message = null, params object[] args) {
            That(actual, Matchers.Not(matcher), message, (object[]) args);
        }

        internal void NotThat<T>(T actual, ITestMatcher<T> matcher, string message = null, params object[] args) {
            That(actual, Matchers.Not(matcher), message, (object[]) args);
        }
    }
}
