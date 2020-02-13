//
// Copyright 2016, 2017 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Text.RegularExpressions;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    partial class DefaultTestRunner {

        internal abstract class TestPlanBase {

            private readonly List<TestCase> _willRun;
            private readonly DefaultTestRunner _runner;
            private readonly TestRunnerOptions _normalizedOpts;
            private readonly RootNode _root;

            public RootNode Root {
                get {
                    return _root;
                }
            }

            public List<TestCase> WillRunTestCases {
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

            protected TestPlanBase(DefaultTestRunner runner, TestRunnerOptions normalized) {
                _normalizedOpts = normalized;
                _runner = runner;
                _root = new RootNode();

                var run = normalized.TestRun;
                var testContext = new TestContext(run, _runner, _runner.RandomCache, null);
                Push(_root, testContext, run);

                _root.AppendEnd(null);

                // Apply rules from the options
                if (normalized.FocusPatterns.Count > 0) {
                    ApplyPatterns(NewRegex(normalized.FocusPatterns),
                                  _normalizedOpts.TestRun,
                                  t => t.IsFocused = true);
                }
                if (normalized.SkipPatterns.Count > 0) {
                    ApplyPatterns(NewRegex(normalized.SkipPatterns),
                                  _normalizedOpts.TestRun,
                                  t => t.Skipped = true);
                }

                // If any focused nodes, then only run focused nodes
                if (!_normalizedOpts.IgnoreFocus && _normalizedOpts.TestRun.ContainsFocusedUnits) {
                    ApplyFocussing(normalized.TestRun);
                }

                // Look for explicit tests
                SkipExplicitTests(_normalizedOpts.TestRun);

                // Scan the tree for other situations
                InheritBiasToChildren(_normalizedOpts.TestRun);

                _willRun = PlanOrder.OfType<TestCaseNode>()
                    .Select(t => (TestCase) t.Unit)
                    .Where(t => !t.Skipped)
                    .ToList();
            }

            public abstract TestRunResults RunTests();

            void ApplyPatterns(IList<Regex> patterns, TestUnit m, Action<TestUnit> action) {
                if (patterns.Any(t => t.IsMatch(m.DisplayName))) {
                    action(m);
                } else {
                    foreach (var c in m.Children) {
                        ApplyPatterns(patterns, c, action);
                    }
                }
            }

            static IList<Regex> NewRegex(IEnumerable<string> patterns) {
                return patterns.Select(t => new Regex(t, RegexOptions.IgnorePatternWhitespace))
                    .ToList();
            }

            void ApplyFocussing(TestUnit m) {
                if (m.ContainsFocusedUnits) {
                    foreach (var c in m.Children) {
                        ApplyFocussing(c);
                    }

                } else if (m.IsFocused) {

                } else {
                    m.Skipped = true;
                }
            }

            void SkipExplicitTests(TestUnit m) {
                if (m.IsExplicit) {
                    m.Skipped = !m.IsFocused;
                }
                foreach (var c in m.Children) {
                    SkipExplicitTests(c);
                }
            }

            private void Push(TestUnitNode parent, TestContext context, TestUnit item) {
                if (parent == null) {
                    throw new ArgumentNullException("parent");
                }
                item.InitializeSafe(context);

                TestUnitNode startNode = parent.AppendStart(item);

                IEnumerable<TestUnit> children = item.Children;
                if (_normalizedOpts.RandomizeSpecs) {
                    children = item.Children.Shuffle(new Random(_normalizedOpts.RandomSeed));
                }
                foreach (var c in children) {
                    Push(startNode, context.WithSelf(c), c);
                }
                startNode.AppendEnd(item);
            }

        }

    }

}
