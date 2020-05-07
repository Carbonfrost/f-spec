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
using System.Collections;
using System.Collections.Generic;

namespace Carbonfrost.Commons.Spec.TestMatchers {

    abstract class PredicateOperator : IPredicateOperator {

        protected abstract ITestMatcher<IEnumerable> CreateMatcher();
        protected abstract ITestMatcher<IEnumerable<T>> CreateMatcher<T>(Predicate<T> predicate);

        public void Apply<TValue>(IEnumerableExpectation<TValue> e) {
            Apply(e, null, (object[]) null);
        }

        public void Apply<TValue>(IEnumerableExpectation<TValue> e, string message, params object[] args) {
            e.As<IEnumerable>().Like(CreateMatcher(), message, (object[]) args);
        }

        public void Apply<TValue>(IEnumerableExpectation<TValue> e, Predicate<TValue> predicate) {
            Apply(e, predicate, null, (object[]) null);
        }

        public void Apply<TValue>(IEnumerableExpectation<TValue> e, Predicate<TValue> predicate, string message, params object[] args) {
            e.Like(CreateMatcher(predicate), message, (object[]) args);
        }

    }
}
