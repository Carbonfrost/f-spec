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

namespace Carbonfrost.Commons.Spec.ExecutionModel.Output {

    class ConsoleOutputParts {

        public readonly ConsoleOutputPart<TestCaseResult> onTestCaseFinished;
        public readonly ConsoleOutputPart<TestUnitResults> onTestTheoryFinished;
        public readonly ConsoleOutputPart<TestRunResults> onTestRunFinished;
        public readonly ConsoleOutputPart<ExceptionInfo> onExceptionInfo;
        public readonly ConsoleOutputPart<UserDataCollection> forUserData;
        public readonly ConsoleOutputPart<Patch> forPatch;
        public readonly ConsoleOutputPart<TestUnitResult> forStatus;

        public ConsoleOutputParts(TestRunnerOptions opts) {
            onTestCaseFinished = ConsoleOutputPart.Compose(
                new ConsoleTestCaseStatus()
            );
            onTestTheoryFinished = ConsoleOutputPart.Compose(
                new ConsoleTestTheoryStatus()
            );
            onTestRunFinished = ConsoleOutputPart.Compose(
                new ConsoleTestRunExplicitlyPassed {
                    Show = opts.ShowPassExplicitly,
                },
                new ConsoleTestRunProblems {
                    Show = !opts.SuppressSummary,
                },
                new ConsoleTestRunResults()
            );
            onExceptionInfo = ConsoleOutputPart.Compose(
                new ConsoleExceptionInfo()
            );
            forUserData = ConsoleOutputPart.Compose(
                new ConsoleUserData {
                    ShowWhitespace = opts.AssertionMessageFormatMode.HasFlag(
                        AssertionMessageFormatModes.PrintWhitespace
                    ),
                }
            );
            forPatch = ConsoleOutputPart.Compose(
                new ConsolePatch {
                    ContextLines = opts.ContextLines,
                    ShowWhitespace = opts.AssertionMessageFormatMode.HasFlag(
                        AssertionMessageFormatModes.PrintWhitespace
                    ),
                }
            );
            forStatus = ConsoleOutputPart.Compose(
                new ConsoleTestUnitStatusBullet()
            );
        }
    }
}
