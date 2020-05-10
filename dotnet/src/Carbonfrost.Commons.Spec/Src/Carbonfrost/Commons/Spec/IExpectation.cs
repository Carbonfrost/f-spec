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

namespace Carbonfrost.Commons.Spec {

    public interface IExpectation : IExpectationAsserter {
        IExpectation Not { get; }
    }

    public interface IExpectation<out T> : IExpectationAsserter<T> {
        IExpectation<T> Not { get; }
        IExpectation<T> Approximately<TEpsilon>(TEpsilon epsilon);
        IExpectation<TBase> As<TBase>();

        void InstanceOf<TExpected>();
        void InstanceOf<TExpected>(string message, params object[] args);
        void Item();
        void Item(string message, params object[] args);
        void Items();
        void Items(string message, params object[] args);
    }
}
