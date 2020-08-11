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

            internal abstract void Run(DefaultTestRunner runner, TestContext rootInitContext);
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


            public void Push(TestContext context) {
                Push(this, context, Unit);
            }

            private static void Push(TestUnitNode parent, TestContext context, TestUnit item) {
                if (parent == null) {
                    throw new ArgumentNullException(nameof(parent));
                }
                item.InitializeSafe(context);

                TestUnitNode startNode = parent.AppendStart(item);

                IEnumerable<TestUnit> children = item.Children;
                var opts = context.TestRunnerOptions;
                if (opts.RandomizeSpecs) {
                    children = item.Children.Shuffle(new Random(opts.RandomSeed));
                }
                foreach (var c in children) {
                    Push(startNode, context.WithSelf(c), c);
                }
                startNode.AppendEnd(item);
            }
        }

        class TestCaseNode : TestUnitNode {

            private TestCaseResult _result;

            public TestCaseNode(TestUnitNode parent, TestCaseInfo unit) : base(parent, unit) {
            }

            public override TestUnitResult FindResult() {
                return _result;
            }

            internal override void Run(DefaultTestRunner runner, TestContext rootInitContext) {
                var myCase = (TestCaseInfo) Unit;
                var ctxt = new TestExecutionContext(rootInitContext, myCase, myCase.CreateTestObject());
                _result = ctxt.RunCurrentTest();
                Parent.FindResult().Children.Add(_result);
            }

            internal override void Abort(DefaultTestRunner runner) {
                var myCase = (TestCaseInfo) Unit;
                var result = new TestCaseResult(myCase, TestStatus.Skipped);
                result.Starting();
                result.Done(myCase, runner.Options);
                Parent.FindResult().Children.Add(result);
            }
        }

        internal class RootNode : TestUnitNode {

            private readonly TestRunResults _result;

            public TestContext InitContext {
                get;
            }

            public RootNode(TestRunner runner, TestRun testRun) : base(null, testRun) {
                TestRunnerOptions opts = runner.Options;
                _result = new TestRunResults {
                    RunnerOptions = opts
                };
                InitContext = new RootTestContext(testRun, runner);
            }

            internal override void Run(DefaultTestRunner runner, TestContext rootInitContext) {
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

            internal override void Run(DefaultTestRunner runner, TestContext rootInitContext) {
                bool shouldRun = Unit.NotifyStarting(runner, out var e) && Unit.SetUpError == null;

                if (shouldRun) {
                    Unit.NotifyStarted(runner);
                    _context = rootInitContext.WithSelf(Unit);
                    Unit.BeforeExecutingSafe(_context);
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

            private readonly TestUnitResult _result;

            public StartCompositeTestUnitNode(TestUnitNode parent, TestUnit unit) : base(parent, unit) {
                _result = new TestUnitResults(unit.DisplayName);
                parent.FindResult().Children.Add(_result);
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

            internal override void Run(DefaultTestRunner runner, TestContext rootInitContext) {
                if (Unit == null) {
                    FindResult().Done(Unit, runner.Options);
                    return;
                }

                var result = FindResult();
                var context = FindTestContext();

                if (Unit.SetUpError != null) {
                    result.SetFailed(Unit.SetUpError);
                    result.Reason = "Problem setting up the test";
                }
                result.Done(Unit, runner.Options);
                Unit.NotifyFinished(runner, result);
                Safely.Dispose(context);
            }

            internal override void Abort(DefaultTestRunner runner) {
                var result = FindResult();
                var context = FindTestContext();

                if (result != null) {
                    result.Done(Unit, runner.Options);
                }
                Safely.Dispose(context);
            }
        }

    }

}
