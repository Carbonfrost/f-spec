//
// Copyright 2016, 2017, 2019, 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

using Carbonfrost.Commons.Spec;
using Carbonfrost.Commons.Spec.ExecutionModel;
using Carbonfrost.Commons.Spec.ExecutionModel.Output;

namespace Carbonfrost.CFSpec {

    class SpecApp {

        public ProgramOptions Options;

        public int Run() {
            try {
                return RunCore();

            } catch (Exception ex) {
                // TODO Display this appropriately
                ConsoleWrapper.Default.WriteLine(ex.ToString());
            }

            return 1;
        }

        private int RunCore() {
            var testRunnerOptions = new TestRunnerOptions {
                RandomSeed = Options.RandomSeed,
                RandomizeSpecs = !Options.DontRandomizeSpecs,
                PlanTimeout = Options.PlanTimeout,
                TestTimeout = Options.TestTimeout,
                SuppressSummary = Options.NoSummary,
                ShowTestNames = Options.ShowTestNames,
                ContextLines = Options.ContextLines,
                ShowPassExplicitly = Options.ShowPassExplicitly,
                IsSelfTest = Options.SelfTest,
                FailFast = Options.FailFast,
                LoadAssemblyFromPath = System.Runtime.Loader.AssemblyLoadContext.Default.LoadFromAssemblyPath,
                SlowTestThreshold = Options.SlowTestThreshold,
            };
            if (Options.ShowWhitespace) {
                testRunnerOptions.AssertionMessageFormatMode |= AssertionMessageFormatModes.PrintWhitespace;
            }
            if (Options.ShowFullStackTraces) {
                testRunnerOptions.AssertionMessageFormatMode |= AssertionMessageFormatModes.FullStackTraces;
            }
            if (!Options.NoUnifiedDiff) {
                testRunnerOptions.AssertionMessageFormatMode |= AssertionMessageFormatModes.UseUnifiedDiff;
            }
            testRunnerOptions.PlanFilter.CopyFrom(Options.PlanFilter);
            testRunnerOptions.IgnoreFocus = Options.NoFocus;

            testRunnerOptions.FixturePaths.AddAll(Options.FixturePaths);
            testRunnerOptions.FixturePaths.AddAll(EnvironmentHelper.FixturePath);

            testRunnerOptions.LoaderPaths.AddAll(Options.Assemblies);
            testRunnerOptions.LoaderPaths.AddAll(Options.LoaderPaths);
            testRunnerOptions.LoaderPaths.AddAll(EnvironmentHelper.LoaderPath);

            foreach (var s in Options.Packages) {
                testRunnerOptions.PackageReferences.Add(s);
            }
            SpecLog.DidFinalizeOptions(Options.ToString());

            Assert.UseStrictMode = TestVerificationMode.Strict == Options.Verify;

            TestRunner runner = TestRunner.Create(testRunnerOptions);

            SpecLog.DidCreateTestRunner(runner);

            try {
                var result = runner.RunTests();
                return ToExitCode(result.FailureReason);

            } catch (SpecException ex) {
                return Fail(ex.Message);
            }
        }

        private int ToExitCode(TestRunFailureReason reason) {
            if (!Options.FailOnPending && reason == TestRunFailureReason.ContainsPendingElements) {
                return 0;
            }

            return (int) reason;
        }

        private int Fail(string message) {
            Console.Error.WriteLine("fatal: " + message);
            return 1;
        }
    }
}
