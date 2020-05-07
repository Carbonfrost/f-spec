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
using System.Collections.Generic;

namespace Carbonfrost.Commons.Spec.TestMatchers {

    interface IEnumerableComparisonOperator {
        void Apply<TValue>(IEnumerableExpectation<TValue> e, IEnumerable<TValue> expected);
        void Apply<TValue>(IEnumerableExpectation<TValue> e, IEnumerable<TValue> expected, string message, params object[] args);
        void Apply<TValue>(IEnumerableExpectation<TValue> e, IEnumerable<TValue> expected, Comparison<TValue> comparison);
        void Apply<TValue>(IEnumerableExpectation<TValue> e, IEnumerable<TValue> expected, Comparison<TValue> comparison, string message, params object[] args);
        void Apply<TValue>(IEnumerableExpectation<TValue> e, IEnumerable<TValue> expected, IEqualityComparer<TValue> comparer);
        void Apply<TValue>(IEnumerableExpectation<TValue> e, IEnumerable<TValue> expected, IEqualityComparer<TValue> comparer, string message, params object[] args);
        void Apply<TValue>(IEnumerableExpectation<TValue> e, params TValue[] expected);
    }

    abstract class EnumerableComparisonOperator : IEnumerableComparisonOperator {

        protected abstract ITestMatcher<IEnumerable<TValue>> CreateMatcher<TValue>(IEnumerable<TValue> expected);
        protected abstract ITestMatcher<IEnumerable<TValue>> CreateMatcher<TValue>(IEnumerable<TValue> expected, IEqualityComparer<TValue> comparer);
        protected abstract ITestMatcher<IEnumerable<TValue>> CreateMatcher<TValue>(IEnumerable<TValue> expected, Comparison<TValue> comparison);

        public void Apply<TValue>(IEnumerableExpectation<TValue> e, IEnumerable<TValue> expected, IEqualityComparer<TValue> comparer) {
            Apply<TValue>(e, expected, comparer, message: null);
        }

        public void Apply<TValue>(IEnumerableExpectation<TValue> e, IEnumerable<TValue> expected, Comparison<TValue> comparison) {
            Apply<TValue>(e, expected, comparison, message: null);
        }

        public void Apply<TValue>(IEnumerableExpectation<TValue> e, IEnumerable<TValue> expected) {
            Apply<TValue>(e, expected, message: null);
        }

        public void Apply<TValue>(IEnumerableExpectation<TValue> e, IEnumerable<TValue> expected, IEqualityComparer<TValue> comparer, string message, params object[] args) {
            e.Like(CreateMatcher(expected, comparer), message, (object[]) args);
        }

        public void Apply<TValue>(IEnumerableExpectation<TValue> e, IEnumerable<TValue> expected, Comparison<TValue> comparison, string message, params object[] args) {
            e.Like(CreateMatcher(expected, comparison), message, (object[]) args);
        }

        public void Apply<TValue>(IEnumerableExpectation<TValue> e, IEnumerable<TValue> expected, string message, params object[] args) {
            e.Like(CreateMatcher(expected), message, (object[]) args);
        }

        public void Apply<TValue>(IEnumerableExpectation<TValue> e, params TValue[] expected) {
            e.Like(CreateMatcher(expected));
        }
    }

}
