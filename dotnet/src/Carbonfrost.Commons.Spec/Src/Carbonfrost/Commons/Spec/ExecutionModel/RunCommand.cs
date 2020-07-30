//
// Copyright 2018, 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
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
using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    class RunCommand : ITestCaseFilter {

        private readonly Func<TestExecutionContext, object> _testFunc;
        private readonly TestOptions _opts;

        public RunCommand(Func<TestExecutionContext, object> coreRunTest, TestOptions opts) {
            _testFunc = coreRunTest;
            _opts = opts;
        }

        void ITestCaseFilter.RunTest(TestExecutionContext context, Action<TestExecutionContext> next) {
            var adapt = (context.TestObject as ITestExecutionFilter) ?? TestExecutionFilter.Null;
            adapt.BeforeExecuting(context);

            // If the test object implements this ITestCaseFilter interface, then
            // this is a _substitute_ for the default execution logic.  THe main
            // use of this is to handle load exceptions when instantiating types.
            var filter = context.TestObject as ITestCaseFilter;
            if (filter != null) {
                filter.RunTest(context, next);
                adapt.AfterExecuting(context);
                return;
            }

            try {
                var effectiveTimeout = _opts.Timeout.GetValueOrDefault(
                    context.TestRunnerOptions.TestTimeout.GetValueOrDefault()
                );
                context.RunTestWithTimeout(_testFunc, effectiveTimeout);

                next(context);
            } finally {
                adapt.AfterExecuting(context);
            }
        }
    }
}
