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
using System.Collections.Generic;
using System.Collections;

namespace Carbonfrost.Commons.Spec {

    public interface IEnumerableExpectation : IEnumerableExpectation<object>, IExpectationAsserter<IEnumerable> {
        new IExpectation<IEnumerable> Self { get; }
        new IEnumerableExpectation Not { get; }
    }

    public interface IEnumerableExpectation<out T> : IExpectationAsserter<IEnumerable<T>> {
        IExpectation<TBase> As<TBase>();

        IExpectation<IEnumerable<T>> Self { get; }
        IExpectation<T> Any { get; }
        IExpectation<T> All { get; }
        IExpectation<T> Single { get; }
        IExpectation<T> None { get; }
        IExpectation<T> No { get; }
        IEnumerableExpectation<T> Not { get; }

        IExpectation<T> AtLeast(int min);
        IExpectation<T> AtMost(int max);
        IExpectation<T> Between(int min, int max);
        IExpectation<T> Exactly(int count);
        IEnumerableExpectation<TResult> Cast<TResult>();
    }
}
