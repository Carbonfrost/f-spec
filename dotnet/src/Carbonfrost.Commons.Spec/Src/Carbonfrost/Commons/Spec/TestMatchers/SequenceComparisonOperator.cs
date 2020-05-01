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

    abstract class SequenceComparisonOperator : ISequenceComparisonOperator {

        protected abstract ITestMatcher<IEnumerable<T>> CreateMatcher<T>(IEnumerable<T> expected);
        protected abstract ITestMatcher<IEnumerable<T>> CreateMatcher<T>(IEnumerable<T> expected, IEqualityComparer<T> comparer);
        protected abstract ITestMatcher<IEnumerable<T>> CreateMatcher<T>(IEnumerable<T> expected, Comparison<T> comparison);

        public void Apply<T>(Expectation<IEnumerable<T>> e, params T[] expected) {
            Apply<T>(e, expected, message: (string) null);
        }

        public void Apply<T>(Expectation<IEnumerable<T>> e, IEnumerable<T> expected, IEqualityComparer<T> comparer) {
            Apply<T>(e, expected, comparer, message: null);
        }

        public void Apply<T>(Expectation<IEnumerable<T>> e, IEnumerable<T> expected, Comparison<T> comparison) {
            Apply<T>(e, expected, comparison, message: null);
        }

        public void Apply<T>(Expectation<IEnumerable<T>> e, IEnumerable<T> expected) {
            Apply<T>(e, expected, message: (string) null);
        }

        public void Apply<T>(Expectation<IEnumerable<T>> e, IEnumerable<T> expected, IEqualityComparer<T> comparer, string message, params object[] args) {
            e.Should(CreateMatcher(expected, comparer), message, (object[]) args);
        }

        public void Apply<T>(Expectation<IEnumerable<T>> e, IEnumerable<T> expected, Comparison<T> comparison, string message, params object[] args) {
            e.Should(CreateMatcher(expected, comparison), message, (object[]) args);
        }

        public void Apply<T>(Expectation<IEnumerable<T>> e, IEnumerable<T> expected, string message, params object[] args) {
            e.Should(CreateMatcher(expected), message, (object[]) args);
        }
    }

}
