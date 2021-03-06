//
// Copyright 2018, 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.Commons.Spec.TestMatchers {

    interface IFactoryMetadataProvider {
    }

    class FactoryMetadataProvider : ITestUnitMetadataProvider, ITestUnitDescendantMetadataProvider, ITestCaseFilter, IFactoryMetadataProvider {

        private readonly ITestMatcherFactory _provider;

        public FactoryMetadataProvider(ITestMatcherFactory provider) {
            _provider = provider;
        }

        public void Apply(TestContext testContext) {
            if (testContext.TestUnit is TestCaseInfo t) {
                t.Filters.Add(this);
            }
        }

        public void ApplyDescendant(TestContext testContext) {
            Apply(testContext);
        }

        void ITestCaseFilter.RunTest(TestExecutionContext testContext, Action<TestExecutionContext> next) {
            var matcher = _provider.CreateMatcher(testContext);
            Action actual = () => next(testContext);
            string message = TestMatcherFactory.GetMessage(_provider);

            new Expectation(ExpectationCommand.TestCode(actual, false, null, AsserterBehavior.Default)).Like(matcher, message, null);
        }
    }

    class FactoryMetadataProvider<T> : ITestUnitMetadataProvider, ITestUnitDescendantMetadataProvider, ITestCaseFilter, IFactoryMetadataProvider {

        private readonly ITestMatcherFactory<T> _provider;

        public FactoryMetadataProvider(ITestMatcherFactory<T> provider) {
            _provider = provider;
        }

        public void Apply(TestContext testContext) {
            if (testContext.TestUnit is TestCaseInfo t) {
                t.Filters.Add(this);
            }
        }

        public void ApplyDescendant(TestContext testContext) {
            Apply(testContext);
        }

        void ITestCaseFilter.RunTest(TestExecutionContext testContext, Action<TestExecutionContext> next) {
            next(testContext);

            Func<T> actualFactory = () => (T) testContext.TestReturnValue;
            var matcher = _provider.CreateMatcher(testContext);
            string message = TestMatcherFactory.GetMessage(_provider);

            new Expectation<T>(ExpectationCommand.Of(actualFactory)).Like(matcher, message, null);
        }

    }
}
