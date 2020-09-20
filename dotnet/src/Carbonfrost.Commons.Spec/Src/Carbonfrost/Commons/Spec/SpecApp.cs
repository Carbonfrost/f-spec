//
// Copyright 2016, 2017, 2019, 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
using System;
using System.IO;
using Carbonfrost.Commons.Spec.ExecutionModel;
using Carbonfrost.Commons.Spec.ExecutionModel.Output;

namespace Carbonfrost.Commons.Spec {

    public class SpecApp {

        private readonly ProgramOptions _options;

        internal SpecApp(ProgramOptions options) {
            _options = options;
        }

        public static int Run(string[] args) {
            ProgramOptions options = null;

            try {
                options = ProgramOptions.Parse(args);

            } catch (NDesk.Options.OptionException e) {
                return ExitWithMessage(ExitCode.UsageError, e.Message);
            }

            var app = new SpecApp(options);
            return app.Run();
        }

        static int ExitWithMessage(ExitCode exitCode, string message) {
            Console.WriteLine("fspec: " + message);
            return (int) exitCode;
        }

        private int Run() {
            if (_options.Action != null) {
                _options.Action();
                return 0;
            }
            try {
                return RunCore();

            } catch (Exception ex) {
                // TODO Display this appropriately
                ConsoleWrapper.Default.WriteLine(ex.ToString());
            }

            return 1;
        }

        private int RunCore() {
            var testRunnerOptions = _options.Options;

            SpecConfiguration spec = null;
            if (!_options.NoConfig) {
                spec = SpecConfiguration.Create(_options.ProjectDirectory);
                spec.CopyToOptions(testRunnerOptions);
            }
            if (!string.IsNullOrEmpty(_options.ProjectDirectory)) {
                Directory.SetCurrentDirectory(_options.ProjectDirectory);
            }

            SpecLog.DidFinalizeOptions(_options.ToString());

            Assert.UseStrictMode = ProgramOptions.TestVerificationMode.Strict == _options.Verify;

            TestRunner runner = TestRunner.Create(testRunnerOptions);

            SpecLog.DidCreateTestRunner(runner);

            try {
                var result = runner.RunTests();
                if (spec != null) {
                    spec.Save(result);
                }

                if (_options.DebugWait) {
                    Console.WriteLine("Press ENTER to exit ...");
                    Console.ReadLine();
                }
                return ToExitCode(result.FailureReason);

            } catch (SpecException ex) {
                return Fail(ex.Message);
            }
        }

        private int ToExitCode(TestRunFailureReason reason) {
            if (!_options.FailOnPending && reason == TestRunFailureReason.ContainsPendingElements) {
                return 0;
            }
            if (!_options.FailFocused && reason == TestRunFailureReason.ContainsFocusedElements) {
                return 0;
            }

            return (int) reason;
        }

        private int Fail(string message) {
            Console.Error.WriteLine("fatal: " + message);
            return 1;
        }

        internal enum ExitCode {
            Success = 0,
            GenericError = 1,
            UsageError = 2,
        }
    }
}
