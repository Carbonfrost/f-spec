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
using System.Collections.Generic;
using System.Linq;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    partial class DefaultTestRunner {

        internal abstract class TestUnitNode {

            public readonly TestUnit Unit;
            public readonly TestUnitNode Parent;

            private readonly IList<TestUnitNode> _children = new List<TestUnitNode>(0);

            public IEnumerable<TestUnitNode> DescendantsAndSelf {
                get {
                    return new[] { this }.Concat(Descendants);
                }
            }

            public IEnumerable<TestUnitNode> Descendants {
                get {
                    return Children.SelectMany(c => c.DescendantsAndSelf);
                }
            }

            public IEnumerable<TestUnitNode> Children {
                get {
                    return _children;
                }
            }

            public TestUnitNode(TestUnitNode parent, TestUnit unit) {
                Parent = parent;
                Unit = unit;

                if (parent != null) {
                    parent._children.Add(this);
                }
            }

            internal abstract void Run(DefaultTestRunner runner);
            internal abstract void Abort(DefaultTestRunner runner);

            public virtual TestUnitResult FindResult() {
                if (Parent == null) {
                    return null;
                }
                return Parent.FindResult();
            }

            public virtual TestContext FindTestContext() {
                if (Parent == null) {
                    return null;
                }
                return Parent.FindTestContext();
            }

            public TestUnitNode AppendStart(TestUnit item) {
                TestUnitNode result = StartTestUnitNode.Create(this, item);
                var testCase = item as TestCaseInfo;
                if (testCase != null) {
                    result = new TestCaseNode(result, testCase);
                }

                return result;
            }

            public TestUnitNode AppendEnd(TestUnit item) {
                return new EndTestUnitNode(this, item);
            }
        }

        class TestCaseNode : TestUnitNode {

            private TestCaseResult _result;

            public TestCaseNode(TestUnitNode parent, TestCaseInfo unit) : base(parent, unit) {
            }

            public override TestUnitResult FindResult() {
                return _result;
            }

            internal override void Run(DefaultTestRunner runner) {
                var myCase = (TestCaseInfo) Unit;
                using (var context = myCase.CreateExecutionContext(runner)) {
                    _result = myCase.RunTest(context);
                }
                Parent.FindResult().ContainerOrSelf.Children.Add(_result);
            }

            internal override void Abort(DefaultTestRunner runner) {
                var myCase = (TestCaseInfo) Unit;
                var result = new TestCaseResult(myCase, TestStatus.Skipped);
                result.Done(DateTime.Now);
                Parent.FindResult().ContainerOrSelf.Children.Add(result);
            }
        }

        internal class RootNode : TestUnitNode {

            private readonly TestRunResults _result;

            public RootNode(TestRunnerOptions opts) : base(null, null) {
                _result = new TestRunResults {
                    RunnerOptions = opts
                };
            }

            internal override void Run(DefaultTestRunner runner) {
                _result.RunStarting();
            }

            internal override void Abort(DefaultTestRunner runner) {
            }

            public override TestUnitResult FindResult() {
                return _result;
            }

        }

        abstract class StartTestUnitNode : TestUnitNode {

            private TestContext _context;

            protected StartTestUnitNode(TestUnitNode parent, TestUnit unit) : base(parent, unit) {
                if (parent == null) {
                    throw new ArgumentNullException(nameof(parent));
                }
            }

            internal override void Run(DefaultTestRunner runner) {
                bool shouldRun = Unit.NotifyStarting(runner, out var e) && Unit.SetUpError == null;

                if (shouldRun) {
                    Unit.NotifyStarted(runner);
                    _context = Unit.CreateInitializationContext(runner);
                } else {
                    Unit.ForcePredeterminedStatus(TestUnitFlags.Skip, e.Reason);
                }
            }

            internal override void Abort(DefaultTestRunner runner) {
            }

            public override TestContext FindTestContext() {
                return _context;
            }

            internal static StartTestUnitNode Create(TestUnitNode parent, TestUnit item) {
                if (item.Type == TestUnitType.Case) {
                    return new StartLeafTestUnitNode(parent, (TestCaseInfo) item);
                } else {
                    return new StartCompositeTestUnitNode(parent, item);
                }
            }
        }

        class StartLeafTestUnitNode : StartTestUnitNode {

            public StartLeafTestUnitNode(TestUnitNode parent, TestCaseInfo unit) : base(parent, unit) {
            }
        }

        class StartCompositeTestUnitNode : StartTestUnitNode {

            private readonly TestUnitResults _result;

            public StartCompositeTestUnitNode(TestUnitNode parent, TestUnit unit) : base(parent, unit) {
                _result = new TestUnitResults(unit.DisplayName);
                parent.FindResult().ContainerOrSelf.Children.Add(_result);
            }

            internal override void Abort(DefaultTestRunner runner) {
            }

            public override TestUnitResult FindResult() {
                return _result;
            }
        }

        class EndTestUnitNode : TestUnitNode {

            public EndTestUnitNode(TestUnitNode parent, TestUnit unit) : base(parent, unit) {
            }

            internal override void Run(DefaultTestRunner runner) {
                if (Unit == null) {
                    FindResult().Done(Unit);
                    return;
                }

                var result = FindResult();
                var context = FindTestContext();

                if (Unit.SetUpError != null) {
                    result.SetFailed(Unit.SetUpError);
                    result.Reason = "Problem setting up the test";
                }
                result.IsFocused = Unit.IsFocused;
                result.Done(Unit);
                result.IsSlow = result.ExecutionTime >= runner.Options.SlowTestThreshold;
                Unit.NotifyFinished(runner, result);
                Safely.Dispose(context);
            }

            internal override void Abort(DefaultTestRunner runner) {
                var result = FindResult();
                var context = FindTestContext();

                if (result != null) {
                    result.Done(Unit);
                }
                Safely.Dispose(context);
            }
        }

    }

}
