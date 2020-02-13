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

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    class TestCaseExecutionFilterSetup : ITestUnitMetadataProvider, ITestCaseFilter {

        private readonly ITestExecutionFilter _provider;

        public TestCaseExecutionFilterSetup(ITestExecutionFilter provider) {
            _provider = provider;
        }

        public void Apply(TestContext testContext) {
            var t = (TestCase) testContext.CurrentTest;
            t.Filters.Add(this);
        }

        void ITestCaseFilter.RunTest(TestContext testContext, Action<TestContext> next) {
            _provider.BeforeExecuting(testContext);
            next(testContext);
            _provider.AfterExecuting(testContext);
        }
    }
}
