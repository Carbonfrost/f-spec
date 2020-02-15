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
using System.Linq;
using Carbonfrost.Commons.Spec.ExecutionModel.Output;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    partial class DefaultTestRunner : TestRunner {

        private readonly TestRunnerOptions _opts;
        private Random _randomCache;

        public DefaultTestRunner(TestRunnerOptions opts) : base(opts) {
            _opts = Options.Normalize();
            Logger = new ConsoleLogger(opts);
        }

        private Random RandomCache {
            get {
                return _randomCache ?? (_randomCache = new Random(_opts.RandomSeed));
            }
        }

        public ITestRunnerLogger Logger {
            get; private set;
        }

        internal DefaultTestRunner.TestPlan CreatePlan(TestRun run) {
            return new TestPlan(this, run, _opts);
        }

        protected override TestRunResults RunTestsCore(TestRun run) {
            SetupLogger();
            DateTime started = DateTime.Now;

            var plan = CreatePlan(run);
            var testsWillRun = plan.WillRunTestCasesCount;

            OnTestRunnerStarting(new TestRunnerStartingEventArgs(_opts, run, testsWillRun));
            OnTestRunnerStarted(new TestRunnerStartedEventArgs(_opts, run, testsWillRun));

            var runResults = plan.RunTests();
            var e = new TestRunnerFinishedEventArgs(run, runResults, _opts);
            OnTestRunnerFinished(e);
            return runResults;
        }

        void SetupLogger() {
            SpecLog.DidSetupLogger(Logger);
            Logger.Initialize(this, this);
        }

        public TestContext NewTestContext(TestUnit unit) {
            return new TestContext(unit, this, RandomCache, unit.FindTestObject());
        }

        static void InheritBiasToChildren(TestUnit unit) {
            foreach (var child in unit.Children) {
                child.Skipped |= unit.Skipped;
                child.IsPending |= unit.IsPending;
                child.IsExplicit |= unit.IsExplicit;
                child.PassExplicitly |= unit.PassExplicitly;
                if (unit.Reason != null) {
                    child.Reason = unit.Reason;
                }
                InheritBiasToChildren(child);
            }
            unit.Seal();
        }

    }
}
