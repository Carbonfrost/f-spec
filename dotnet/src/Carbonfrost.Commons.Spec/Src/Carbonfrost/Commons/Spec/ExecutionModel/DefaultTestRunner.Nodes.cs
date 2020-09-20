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
using System.Collections.Generic;
using System.Linq;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    partial class DefaultTestRunner {

        enum NodeState {
            Start,
            Initialized,
            Executed,
            AfterExecuting,
        }

        internal abstract class Node {

            public readonly TestUnit Unit;
            public readonly TestContext InitContext;
            private NodeState _state;

            public abstract CompositeNode Parent {
                get;
            }

            public abstract IReadOnlyList<Node> Children {
                get;
            }

            public virtual IEnumerable<Node> DescendantsAndSelf {
                get {
                    return new[] { this }.Concat(Descendants);
                }
            }

            public virtual IEnumerable<Node> Descendants {
                get {
                    return Children.SelectMany(c => c.DescendantsAndSelf);
                }
            }

            public virtual bool IsLeafThatWillRun {
                get {
                    return false;
                }
            }

            public abstract TestUnitResult Result {
                get;
            }

            protected Node(TestUnit unit, TestContext initContext) {
                Unit = unit;
                InitContext = initContext;
            }

            public void Initialize() {
                AssertState(NodeState.Start);
                InitializeCore();
                _state = NodeState.Initialized;
            }

            protected abstract void InitializeCore();

            internal void Execute() {
                AssertState(NodeState.Initialized);
                ExecuteSelf();
                _state = NodeState.Executed;
            }

            internal void AfterExecuting() {
                AssertState(NodeState.Executed);

                AfterExecutingChildren();
                _state = NodeState.AfterExecuting;
            }

            internal void Abort() {
                AbortSelf();
            }

            protected virtual void ExecuteSelf() {}
            protected virtual void AbortSelf() {}

            protected virtual void AfterExecutingChildren() {
                var context = InitContext;
                var runner = (DefaultTestRunner) context.TestRunnerEvents;
                if (Unit == null) {
                    Result.Done(Unit, runner.Options);
                    return;
                }

                var result = Result;

                if (Unit.SetUpError != null) {
                    result.SetFailed(Unit.SetUpError);
                    result.Reason = "Problem setting up the test";
                }
                result.Done(Unit, runner.Options);
                Unit.NotifyFinished(runner, result);
                Safely.Dispose(context);

                if (Parent != null) {
                    Parent.Result.Children.Add(Result);
                }
            }

            private void AssertState(NodeState st) {
                if (_state != st) {
                    throw new NotImplementedException("required state: " + st);
                }
            }
        }

        class DefaultNode : CompositeNode {

            private readonly TestUnitResults _results;

            public override TestUnitResult Result {
                get {
                    return _results;
                }
            }

            public DefaultNode(TestUnit unit, CompositeNode parent) : base(unit, parent, new DefaultTestContext(parent.InitContext, unit)) {
                _results = new TestUnitResults(unit);
            }
        }

        internal abstract class CompositeNode : Node {

            private readonly CompositeNode _parent;
            private readonly List<Node> _children = new List<Node>();

            public sealed override CompositeNode Parent {
                get {
                    return _parent;
                }
            }

            public override IReadOnlyList<Node> Children {
                get {
                    return _children;
                }
            }

            public CompositeNode(TestUnit unit, CompositeNode parent, TestContext initContext) : base(unit, initContext) {
                _parent = parent;
            }

            internal Node Append(TestUnit test) {
                var newNode = CreateChildNode(test);
                _children.Add(newNode);
                return newNode;
            }

            protected override void InitializeCore() {
                Unit.InitializeSafe(InitContext);

                var opts = InitContext.TestRunnerOptions;
                var children = Unit.Children.Select(c => CreateChildNode(c));

                if (opts.RandomizeSpecs) {
                    Random random = InitContext.Random;
                    children = children.Shuffle(random);
                }
                _children.Clear();
                _children.AddRange(children);

                foreach (var c in _children) {
                    c.Initialize();
                }
            }

            private Node CreateChildNode(TestUnit unit) {
                if (unit is TestCaseInfo testCaseInfo) {
                    return new CaseNode(testCaseInfo, this);
                }
                if (unit is TestTheory tt) {
                    return new TheoryNode(tt, this);
                }
                return new DefaultNode(unit, this);
            }

            protected override void ExecuteSelf() {
                var runner = (ITestRunnerEventSink) InitContext.TestRunnerEvents;
                bool shouldRun = Unit.NotifyStarting(runner, out var e) && Unit.SetUpError == null;

                if (shouldRun) {
                    Unit.NotifyStarted(runner);
                    Unit.BeforeExecutingSafe(InitContext);
                } else {
                    Unit.ForcePredeterminedStatus(TestUnitFlags.Skip, e.Reason);
                }
            }
        }

        private class RootNode : CompositeNode {

            private readonly TestRunResults _results;

            public override TestUnitResult Result {
                get {
                    return _results;
                }
            }

            public RootNode(TestRunner runner, TestRun unit) : base(unit, null, new RootTestContext(unit, runner)) {
                _results = new TestRunResults(unit) {
                    RunnerOptions = runner.Options
                };
            }

            protected override void ExecuteSelf() {
                _results.RunStarting();
                base.ExecuteSelf();
            }
        }

        internal abstract class NodeExecutionContextBase : TestExecutionContext {

            private readonly Node _node;

            internal Node Node {
                get {
                    return _node;
                }
            }

            protected NodeExecutionContextBase(Node node, TestContext parent, TestUnit self, object testObject)
                : base(parent, self, testObject) {
                _node = node;
            }

            protected override TestExecutionContext CreateChildContext(TestUnit test) {
                var newNode = Node.Parent.Append(test);
                if (newNode is IExecutionNode exe) {
                    return exe.ExecutionContext;
                }
                throw new NotImplementedException();
            }

            internal override TestUnitResult RunCurrentTest() {
                Node.Initialize();
                TestPlanBase.ExecutePlan(
                    Node,
                    t => t.Execute(),
                    t => t.AfterExecuting()
                );
                return Node.Result;
            }
        }

        interface IExecutionNode {
            NodeExecutionContextBase ExecutionContext {
                get;
            }
        }

        class TheoryNode : CompositeNode, IExecutionNode {
            private readonly TestUnitResults _results;
            private TheoryExecutionContext _execution;

            NodeExecutionContextBase IExecutionNode.ExecutionContext {
                get {
                    return _execution;
                }
            }

            public override TestUnitResult Result {
                get {
                    return _results;
                }
            }

            public TheoryNode(TestTheory unit, CompositeNode parent) : base(unit, parent, new DefaultTestContext(parent.InitContext, unit)) {
                _results = new TestUnitResults(unit);
                _execution = new TheoryExecutionContext(this, InitContext, (TestTheory) Unit, null);
            }
        }

        internal class CaseNode : Node, IExecutionNode {
            private readonly NodeExecutionContext _execution;
            private readonly CompositeNode _parent;

            NodeExecutionContextBase IExecutionNode.ExecutionContext {
                get {
                    return _execution;
                }
            }

            public override CompositeNode Parent {
                get {
                    return _parent;
                }
            }

            public override bool IsLeafThatWillRun {
                get {
                    return !Unit.Skipped;
                }
            }

            public TestCaseInfo Case {
                get {
                    return (TestCaseInfo) Unit;
                }
            }

            public override IReadOnlyList<Node> Children {
                get {
                    return Array.Empty<Node>();
                }
            }

            public override IEnumerable<Node> DescendantsAndSelf {
                get {
                    return new[] { this };
                }
            }

            public override IEnumerable<Node> Descendants {
                get {
                    return Array.Empty<Node>();
                }
            }

            public CaseNode(TestCaseInfo unit, CompositeNode parent) : base(unit, new DefaultTestContext(parent.InitContext, unit)) {
                _parent = parent;
                _execution = new NodeExecutionContext(this, InitContext, Case, Case.CreateTestObject());
            }

            public override TestUnitResult Result {
                get {
                    return _execution.Result;
                }
            }

            protected override void InitializeCore() {
                Unit.InitializeSafe(InitContext);
            }

            protected override void ExecuteSelf() {
                ExecuteSelfCore(e => Case.RunTest(e));
            }

            protected override void AbortSelf() {
                ExecuteSelfCore(e => Case.AbortTest(e));
            }

            private void ExecuteSelfCore(Func<NodeExecutionContext, TestCaseResult> act) {
                _execution.Result = act(_execution);
            }
        }

        class TheoryExecutionContext : NodeExecutionContextBase {

            public TheoryExecutionContext(CompositeNode node, TestContext parent, TestTheory self, object testObject)
                : base(node, parent, self, testObject) {
            }
        }

        internal class NodeExecutionContext : NodeExecutionContextBase {

            public TestCaseResult Result {
                get;
                set;
            }

            public NodeExecutionContext(CaseNode node, TestContext parent, TestCaseInfo self, object testObject)
                : base(node, parent, self, testObject) {
            }
        }
    }
}
