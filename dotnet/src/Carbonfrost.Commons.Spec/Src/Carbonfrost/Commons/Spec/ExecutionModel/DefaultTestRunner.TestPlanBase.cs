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

using System;
using System.Collections.Generic;
using System.Linq;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    partial class DefaultTestRunner {

        internal abstract class TestPlanBase {

            private readonly List<TestCaseInfo> _willRun;
            private readonly DefaultTestRunner _runner;
            private readonly TestRunnerOptions _normalizedOpts;
            private readonly RootNode _root;

            internal Node Root {
                get {
                    return _root;
                }
            }

            public TestLog Log {
                get {
                    return Root.InitContext.Log;
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

            internal IEnumerable<Node> PlanOrder {
                get {
                    return _root.DescendantsAndSelf;
                }
            }

            protected TestPlanBase(DefaultTestRunner runner, TestRun testRun, TestRunnerOptions normalized) {
                _normalizedOpts = normalized;
                _runner = runner;
                _root = new RootNode(runner, testRun);
                _root.Initialize();

                normalized.PreviousRun.ApplyTo(testRun);
                if (normalized.RerunPreviousFailures) {
                    if (normalized.PreviousRun.FailureReason.IsFailure()) {
                        normalized.PlanFilter.Tags.Add(
                            TestTagPredicate.Previously(TestStatus.Failed)
                        );
                    } else {
                        Log.Warn("No previous failures, re-running all tests...");
                    }
                }

                // Apply filter rules from the options
                _normalizedOpts.PlanFilter.Apply(testRun, normalized);

                _willRun = PlanOrder.Where(p => p.IsLeafThatWillRun).Select(n => (TestCaseInfo) n.Unit).ToList();
            }

            public abstract TestRunResults RunTests();

            internal void ExecutePlan(Action<Node> operation, Action<Node> after) {
                ExecutePlan(Root, operation, after);
            }

            internal static void ExecutePlan(Node root, Action<Node> operation, Action<Node> after) {
                var stack = new Stack<(Node node, bool after)>();
                stack.Push((root, false));

                while (stack.Count > 0) {
                    var current = stack.Pop();
                    if (current.after) {
                        after(current.node);
                        continue;
                    } else {
                        operation(current.node);
                    }

                    // Mark the end of the children scope
                    stack.Push((current.node, true));

                    foreach (var child in current.node.Children.Reverse()) {
                        stack.Push((child, false));
                    }
                }
            }

        }

    }

}
