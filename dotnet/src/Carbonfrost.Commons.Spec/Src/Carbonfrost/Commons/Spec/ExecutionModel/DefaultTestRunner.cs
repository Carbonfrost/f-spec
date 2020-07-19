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
using System.Diagnostics;
using Carbonfrost.Commons.Spec.ExecutionModel.Output;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    partial class DefaultTestRunner : TestRunner {

        private readonly BufferMessageEventCache _messageLogger;

        public DefaultTestRunner(TestRunnerOptions opts) : base(opts.Normalize()) {
            _messageLogger = new BufferMessageEventCache();
            Logger = new ConsoleLogger(opts);
        }

        public ITestRunnerLogger Logger {
            get;
        }

        internal DefaultTestRunner.TestPlanBase CreatePlan(TestRun run) {
            if (Options.FailFast) {
                return new FailFastTestPlan(this, run, Options);
            }

            return new TestPlan(this, run, Options);
        }

        protected override TestRunResults RunTestsCore(TestRun run) {
            SetupLogger();
            SetupTraceListener();
            DateTime started = DateTime.Now;

            var plan = CreatePlan(run);
            var testsWillRun = plan.WillRunTestCasesCount;

            OnRunnerStarting(new TestRunnerStartingEventArgs(Options, run, testsWillRun));
            OnRunnerStarted(new TestRunnerStartedEventArgs(Options, run, testsWillRun));

            var runResults = plan.RunTests();
            var e = new TestRunnerFinishedEventArgs(run, runResults, Options);
            OnRunnerFinished(e);
            return runResults;
        }

        private void SetupTraceListener() {
            // Remove the default trace listener because it will display assertion
            // UI prompts or will cause the process to exit
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new TraceListenerImpl());
        }

        private void SetupLogger() {
            SpecLog.DidSetupLogger(Logger);

            // Message logger must handle events first
            _messageLogger.Initialize(this, this);
            Logger.Initialize(this, this);
        }
    }
}
