//
// Copyright 2017 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

namespace Carbonfrost.Commons.Spec {

    interface IExpectationBuilderBase {
        Expectation Will { get; }
        SatisfactionExpectation ToSatisfy { get; }

        void To(ITestMatcher matcher, string message = null, params object[] args);
        void NotTo(ITestMatcher matcher, string message = null, params object[] args);
        void ToNot(ITestMatcher matcher, string message = null, params object[] args);
    }

    interface IExpectationBuilderBase<T> {
        Expectation<T> ToBe { get; }
        EnumerableExpectation ToHave { get; }
        SatisfactionExpectation<T> ToSatisfy { get; }

        void To(ITestMatcher<T> matcher, string message = null, params object[] args);
        void NotTo(ITestMatcher<T> matcher, string message = null, params object[] args);
        void ToNot(ITestMatcher<T> matcher, string message = null, params object[] args);
    }

    interface IExpectationBuilder : IExpectationBuilderBase {
        TemporalExpectationBuilder Consistently { get; }
        TemporalExpectationBuilder Eventually { get; }
    }

    interface IExpectationBuilder<T> : IExpectationBuilderBase<T> {
        ExpectationBuilder<T> Not { get; }
        TemporalExpectationBuilder<T> Consistently { get; }
        TemporalExpectationBuilder<T> Eventually { get; }
        ExpectationBuilder<TBase> As<TBase>();
    }
}
