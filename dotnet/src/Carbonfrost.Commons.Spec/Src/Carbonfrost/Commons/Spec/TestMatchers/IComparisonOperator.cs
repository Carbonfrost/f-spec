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

    interface IComparisonOperator {
        void Apply<T>(IExpectation<T> e, T expected);
        void Apply<T>(IExpectation<T> e, T expected, Comparison<T> comparison);
        void Apply<T>(IExpectation<T> e, T expected, Comparison<T> comparison, string message, params object[] args);
        void Apply<T>(IExpectation<T> e, T expected, IComparer<T> comparer);
        void Apply<T>(IExpectation<T> e, T expected, IComparer<T> comparer, string message, params object[] args);
        void Apply<T>(IExpectation<T> e, T expected, string message, params object[] args);
    }
}
