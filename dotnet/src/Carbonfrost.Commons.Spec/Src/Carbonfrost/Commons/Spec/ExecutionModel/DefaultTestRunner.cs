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
using Carbonfrost.Commons.Spec.ExecutionModel.Output;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    partial class DefaultTestRunner : TestRunner {

        private readonly TestRunnerOptions _opts;
        private readonly BufferMessageEventCache _messageLogger;

        public DefaultTestRunner(TestRunnerOptions opts) : base(opts) {
            _opts = Options.Normalize();
            _messageLogger = new BufferMessageEventCache();
            Logger = new ConsoleLogger(opts);
        }

        public ITestRunnerLogger Logger {
            get;
            private set;
        }

        internal DefaultTestRunner.TestPlanBase CreatePlan(TestRun run) {
            if (Options.FailFast) {
                return new FailFastTestPlan(this, run, _opts);
            }

            return new TestPlan(this, run, _opts);
        }

        protected override TestRunResults RunTestsCore(TestRun run) {
            SetupLogger();
            DateTime started = DateTime.Now;

            var plan = CreatePlan(run);
            var testsWillRun = plan.WillRunTestCasesCount;

            OnRunnerStarting(new TestRunnerStartingEventArgs(_opts, run, testsWillRun));
            OnRunnerStarted(new TestRunnerStartedEventArgs(_opts, run, testsWillRun));

            var runResults = plan.RunTests();
            var e = new TestRunnerFinishedEventArgs(run, runResults, _opts);
            OnRunnerFinished(e);
            return runResults;
        }

        void SetupLogger() {
            SpecLog.DidSetupLogger(Logger);

            // Message logger must handle events first
            _messageLogger.Initialize(this, this);
            Logger.Initialize(this, this);
        }
    }
}
