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
using System.Reflection;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    class BespokeTheory : TestTheory, IReflectionTestCaseFactory {

        private readonly TestDataProviderCollection _testDataProviders;
        private readonly TestUnitCollection _children;
        private readonly Func<TestExecutionContext, object> _func;
        private readonly TestName _baseTestName;

        public BespokeTheory(ITestDataProvider provider, TestName baseName, Func<TestExecutionContext, object> func) : base(func.Method) {
            _children = new TestUnitCollection(this);
            _func = func;
            _testDataProviders = TestDataProviderCollection.Create(provider);
            _baseTestName = baseName;
        }

        public sealed override TestUnitCollection Children {
            get {
                return _children;
            }
        }

        public override TestDataProviderCollection TestDataProviders {
            get {
                return _testDataProviders;
            }
        }

        internal override IReflectionTestCaseFactory GetTestCaseFactory(ITestDataProvider provider) {
            return this;
        }

        public TestCaseInfo CreateTestCase(MethodInfo method, TestDataInfo row) {
            // Use _func; it should be the same as the method passed in
            return new BespokeTheoryCase(_func, _baseTestName, row);
        }
    }
}
