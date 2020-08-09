//
// Copyright 2016, 2017, 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

using System.Collections.Generic;
using System.Linq;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    partial class DefaultTestRunner {

        internal abstract class TestPlanBase {

            private readonly List<TestCaseInfo> _willRun;
            private readonly DefaultTestRunner _runner;
            private readonly TestRunnerOptions _normalizedOpts;
            private readonly RootNode _root;

            protected TestContext RootInitContext {
                get {
                    return _root.InitContext;
                }
            }

            public RootNode Root {
                get {
                    return _root;
                }
            }

            public List<TestCaseInfo> WillRunTestCases {
                get {
                    return _willRun;
                }
            }

            public int WillRunTestCasesCount {
                get {
                    return _willRun.Count;
                }
            }

            public DefaultTestRunner Runner {
                get {
                    return _runner;
                }
            }

            internal IEnumerable<TestUnitNode> PlanOrder {
                get {
                    return _root.DescendantsAndSelf;
                }
            }

            protected TestPlanBase(DefaultTestRunner runner, TestRun testRun, TestRunnerOptions normalized) {
                _normalizedOpts = normalized;
                _runner = runner;
                _root = new RootNode(runner, testRun);

                _root.Push(RootInitContext);

                _root.AppendEnd(null);

                // Apply filter rules from the options
                _normalizedOpts.PlanFilter.Apply(testRun, normalized);

                _willRun = PlanOrder.OfType<TestCaseNode>()
                    .Select(t => (TestCaseInfo) t.Unit)
                    .Where(t => !t.Skipped)
                    .ToList();
            }

            public abstract TestRunResults RunTests();

        }

    }

}
