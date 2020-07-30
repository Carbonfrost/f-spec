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

    class UserTestClassAdapter : ReflectedTestClass {

        public UserTestClassAdapter(Type type) : base(type) {
        }

        protected override void AfterExecuting(TestExecutionContext testContext) {
            base.AfterExecuting(testContext);
            Adapt(testContext).AfterExecuting(testContext);
        }

        protected override void BeforeExecuting(TestContext testContext) {
            Adapt(testContext).BeforeExecuting(testContext);
            base.BeforeExecuting(testContext);
        }

        private static ITestExecutionFilter Adapt(TestContext testContext) {
            if (testContext is TestExecutionContext exec) {
                return (ITestExecutionFilter) exec.TestObject;
            }
            return TestExecutionFilter.Null;
        }
    }
}
