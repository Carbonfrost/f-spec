//
// Copyright 2016, 2017 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    class UserTestClassAdapter : ReflectedTestClass {

        public UserTestClassAdapter(Type type) : base(type) {
            // Must implement ITestUnitAdapter
        }

        ITestUnitAdapter Adapter {
            get {
                return (ITestUnitAdapter) FindTestObject();
            }
        }

        protected override void Initialize(TestContext testContext) {
            base.Initialize(testContext);
            Adapter.Initialize(testContext);
        }

        protected override void AfterExecuting(TestContext testContext) {
            Adapter.AfterExecuting(testContext);
        }

        protected override void BeforeExecuting(TestContext testContext) {
            Adapter.BeforeExecuting(testContext);
            base.BeforeExecuting(testContext);
        }

        protected override void BeforeExecutingDescendant(TestContext descendantTestContext) {
            Adapter.BeforeExecutingDescendant(descendantTestContext);
        }

        protected override void AfterExecutingDescendant(TestContext descendantTestContext) {
            Adapter.AfterExecutingDescendant(descendantTestContext);
        }
    }
}
