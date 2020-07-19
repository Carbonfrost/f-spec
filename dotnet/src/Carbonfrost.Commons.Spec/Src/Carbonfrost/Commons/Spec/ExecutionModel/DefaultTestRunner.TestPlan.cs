//
// Copyright 2018, 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    partial class DefaultTestRunner {

        internal class TestPlan : TestPlanBase {

            public TestPlan(DefaultTestRunner runner, TestRun run, TestRunnerOptions normalized)
                : base(runner, run, normalized)
            {
            }

            public override TestRunResults RunTests() {
                foreach (var item in PlanOrder) {
                    item.Run(Runner, RootInitContext);
                }
                return (TestRunResults) Root.FindResult();
            }

        }

        internal class FailFastTestPlan : TestPlanBase {

            public FailFastTestPlan(DefaultTestRunner runner, TestRun run, TestRunnerOptions normalized)
                : base(runner, run, normalized)
            {
            }

            public override TestRunResults RunTests() {
                var e = PlanOrder.GetEnumerator();
;
                while (e.MoveNext()) {
                    var item = e.Current;
                    item.Run(Runner, RootInitContext);

                    if (item.FindResult().Failed) {
                        // Ensure clean up by processing descendants
                        foreach (var d in item.Descendants) {
                            d.Run(Runner, RootInitContext);
                        }
                        break;
                    }
                }

                while (e.MoveNext()) {
                    var item = e.Current;
                    item.Abort(Runner);
                }

                return (TestRunResults) Root.FindResult();
            }

        }

    }

}
