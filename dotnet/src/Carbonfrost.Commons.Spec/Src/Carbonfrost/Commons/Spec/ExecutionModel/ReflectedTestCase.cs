//
// Copyright 2016-2018 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Linq;
using System.Reflection;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    abstract class ReflectedTestCase : TestCase {

        protected override TestCaseResult RunTestCore(TestContext testContext) {
            var opts = new TestOptions {
                PassExplicitly = PassExplicitly,
                Timeout = Timeout,
                DisplayName = DisplayName,
            };
            opts.Filters.AddAll(TestMethod.GetCustomAttributes(false)
                                .OfType<ITestCaseFilter>());
            opts.Filters.AddAll(Filters);

            return testContext.RunTest(CoreRunTest, opts);
        }

        protected sealed override void Initialize(TestContext testContext) {
            Attributes.ApplyMetadata(testContext);

            // If skipped, don't do any further work
            if (Skipped) {
                return;
            }

            InitializeOverride(testContext);
        }

        protected virtual void InitializeOverride(TestContext testContext) {
            // Subclasses should use this to eval the test data
        }

        protected abstract object CoreRunTest(TestContext context);

        public string TypeName {
            get {
                return TestMethod.DeclaringType.FullName;
            }
        }

        protected ReflectedTestCase(MethodInfo method) : base(method) {
        }

        internal object InvokeMethodHelper(object testObject, object[] args) {
            return SyncContextImpl.Run(TestMethod, testObject, args);
        }
    }
}
