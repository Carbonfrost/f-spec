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

        internal abstract class Node {

            public readonly TestUnit Unit;
            public readonly TestContext InitContext;

            public abstract Node Parent {
                get;
            }

            public abstract IList<Node> Children {
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

            public abstract void Initialize();

            internal void Execute() {
                ExecuteSelf();
            }

            internal void AfterExecuting() {
                AfterExecutingChildren();
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
        }

        class DefaultNode : CompositeNode {

            private readonly TestUnitResults _results;

            public override TestUnitResult Result {
                get {
                    return _results;
                }
            }

            public DefaultNode(TestUnit unit, Node parent) : base(unit, parent, new DefaultTestContext(parent.InitContext, unit)) {
                 _results = new TestUnitResults(unit.DisplayName);
            }
        }

        private abstract class CompositeNode : Node {

            private readonly Node _parent;
            private readonly List<Node> _children = new List<Node>();

            public sealed override Node Parent {
                get {
                    return _parent;
                }
            }

            public override IList<Node> Children {
                get {
                    return _children;
                }
            }

            public CompositeNode(TestUnit unit, Node parent, TestContext initContext) : base(unit, initContext) {
                _parent = parent;
            }

            public override void Initialize() {
                Unit.InitializeSafe(InitContext);

                var opts = InitContext.TestRunnerOptions;
                var children = Unit.Children.Select(c => AppendNode(c));

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

            public Node AppendNode(TestUnit unit) {
                if (unit is TestCaseInfo testCaseInfo) {
                    return new CaseNode(testCaseInfo, this);
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

            public RootNode(TestRunner runner, TestUnit unit) : base(unit, null, new RootTestContext(unit, runner)) {
                _results = new TestRunResults {
                    RunnerOptions = runner.Options
                };
            }

            protected override void ExecuteSelf() {
                _results.RunStarting();
                base.ExecuteSelf();
            }
        }

        private class CaseNode : Node {
            private TestCaseResult _result;
            private readonly Node _parent;

            public override Node Parent {
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

            public override IList<Node> Children {
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

            public CaseNode(TestCaseInfo unit, Node parent) : base(unit, new DefaultTestContext(parent.InitContext, unit)) {
                _parent = parent;
            }

            public override TestUnitResult Result {
                get {
                    return _result;
                }
            }

            public override void Initialize() {
                Unit.InitializeSafe(InitContext);
            }

            protected override void ExecuteSelf() {
                var myCase = Case;
                var ctxt = new TestExecutionContext(InitContext, myCase, myCase.CreateTestObject());
                _result = ctxt.RunCurrentTest();
            }

            protected override void AbortSelf() {
                var myCase = Case;
                var result = new TestCaseResult(myCase, TestStatus.Skipped);
                var runnerOpts = InitContext.TestRunnerOptions;
                result.Starting();
                result.Done(myCase, runnerOpts);
                _result = result;
            }
        }
    }

}
