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

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    abstract partial class ReflectedTestCase {

        class PredeterminedStatusCommand : ITestCaseFilter {
            private readonly TestCaseResult _result;

            public PredeterminedStatusCommand(TestCaseResult result) {
                _result = result;
            }

            private static TestStatus PredeterminedStatus(TestUnit of) {
                return TestUnit.ConvertToStatus(of).GetValueOrDefault(TestStatus.NotRun);
            }

            void ITestCaseFilter.RunTest(TestExecutionContext context, Action<TestExecutionContext> next) {
                var unit = context.CurrentTest;
                var status = PredeterminedStatus(unit);

                if (status == TestStatus.NotRun) {
                    next(context);
                    return;
                }
                _result.SetPredetermined(status, unit.Reason);
            }
        }
    }
}
