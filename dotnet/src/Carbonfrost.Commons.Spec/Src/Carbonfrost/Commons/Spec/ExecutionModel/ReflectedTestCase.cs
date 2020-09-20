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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    abstract partial class ReflectedTestCase : TestCaseInfo {

        private static readonly IEnumerable<ITestCaseFilter> EMPTY_FILTERS = Array.Empty<ITestCaseFilter>();

        protected sealed override TestCaseResult RunTestCore(TestExecutionContext testContext) {
            var options = NewTestOptions();
            var result = new TestCaseResult((TestCaseInfo) this);
            result.Starting();

            Func<TestExecutionContext, object> testFunc = CoreRunTest;

            var groupedByPhase = options.Filters.GroupBy(GetPhaseForFilter);
            var metadataPhase = groupedByPhase.FirstOrDefault(g => g.Key == Phase.MetadataOnly) ?? EMPTY_FILTERS;
            var execPhase = groupedByPhase.FirstOrDefault(g => g.Key == Phase.Assertion) ?? EMPTY_FILTERS;
            var filters = metadataPhase
                .Concat(new ITestCaseFilter[] {
                    new PredeterminedStatusCommand(result)
                }).Concat(execPhase)
                .Concat(new ITestCaseFilter[] {
                    new RunCommand(testFunc, result, options)
                });

            var winder = new TestCaseCommandWinder(filters);
            try {
                winder.RunAll(testContext);
            } catch (Exception ex) {
                result.SetFailed(ex);
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

        private Phase GetPhaseForFilter(ITestCaseFilter filter) {
            if (filter is TestMatchers.IFactoryMetadataProvider) {
                return Phase.Assertion;
            }
            return Phase.MetadataOnly;
        }

        enum Phase {
            MetadataOnly,
            Assertion,
        }
    }
}
