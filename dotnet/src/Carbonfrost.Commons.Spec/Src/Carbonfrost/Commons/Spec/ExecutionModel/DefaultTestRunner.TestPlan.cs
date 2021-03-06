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

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    partial class DefaultTestRunner {

        internal class TestPlan : TestPlanBase {

            public TestPlan(DefaultTestRunner runner, TestRun run, TestRunnerOptions normalized)
                : base(runner, run, normalized)
            {
            }

            public override TestRunResults RunTests() {
                ExecutePlan(
                    t => t.Execute(),
                    t => t.AfterExecuting()
                );

                return (TestRunResults) Root.Result;
            }
        }

        internal class FailFastTestPlan : TestPlanBase {

            public FailFastTestPlan(DefaultTestRunner runner, TestRun run, TestRunnerOptions normalized)
                : base(runner, run, normalized)
            {
            }

            public override TestRunResults RunTests() {
                bool aborting = false;
                ExecutePlan(
                    t => {
                        if (aborting) {
                            t.Abort();
                        } else {
                            t.Execute();
                            aborting = t.Result.Failed;
                        }
                    },
                    t => t.AfterExecuting()
                );

                return (TestRunResults) Root.Result;
            }
        }
    }
}
