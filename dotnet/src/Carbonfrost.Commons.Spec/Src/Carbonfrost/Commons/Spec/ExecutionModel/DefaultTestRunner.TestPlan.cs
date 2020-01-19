//
// Copyright 2018 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

            public TestPlan(DefaultTestRunner runner, TestRunnerOptions normalized)
                : base(runner, normalized)
            {
            }

            public override TestRunResults RunTests() {
                foreach (var item in PlanOrder) {
                    item.Run(Runner);
                }
                return (TestRunResults) Root.FindResult();
            }

        }

    }

}
