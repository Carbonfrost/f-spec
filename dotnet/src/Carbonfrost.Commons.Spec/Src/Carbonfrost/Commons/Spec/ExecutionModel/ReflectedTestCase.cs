//
// Copyright 2016-2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

    abstract class ReflectedTestCase : TestCaseInfo {

        protected sealed override TestCaseResult RunTestCore(TestExecutionContext testContext) {
            var options = NewTestOptions();
            var result = new TestCaseResult((TestCaseInfo) this);
            result.Starting();

            Func<TestExecutionContext, object> testFunc = CoreRunTest;

            var winder = new TestCaseCommandWinder(options.Filters.Concat(new [] {
                new RunCommand(testFunc, options)
            }));

            winder.RunAll(testContext);

            if (options.PassExplicitly) {
                result.SetFailed(SpecFailure.ExplicitPassNotSet());
            } else {
                result.SetSuccess();
            }
            result.Done(null, testContext.TestRunnerOptions);
            return result;
        }

        protected sealed override void Initialize(TestContext testContext) {
            Metadata.Apply(testContext);

            // If skipped, don't do any further work
            if (Skipped) {
                return;
            }

            InitializeOverride(testContext);
        }

        protected virtual void InitializeOverride(TestContext testContext) {
            // Subclasses should use this to eval the test data
        }

        protected abstract object CoreRunTest(TestExecutionContext context);

        public string TypeName {
            get {
                return TestMethod.DeclaringType.FullName;
            }
        }

        protected ReflectedTestCase(MethodInfo method) : base(method) {
        }

        private protected object InvokeMethodHelper(object testObject, object[] args) {
            return SyncContextImpl.Run(TestMethod, testObject, args);
        }

        private TestOptions NewTestOptions() {
            var options = new TestOptions {
                PassExplicitly = PassExplicitly,
                Timeout = Timeout,
                Name = Name,
            };
            options.Filters.AddAll(Attributes.OfType<ITestCaseFilter>());
            options.Filters.AddAll(Filters);
            return options;
        }
    }
}
