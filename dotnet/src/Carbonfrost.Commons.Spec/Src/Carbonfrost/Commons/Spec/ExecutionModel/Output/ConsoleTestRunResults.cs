//
// Copyright 2019 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

    class ConsoleTestRunResults : ConsoleOutputPart<TestRunResults> {

        public ConsoleTestRunResults(IConsoleWrapper console) : base(console) {}

        public override void Render(TestRunResults results) {
            if (results.FailedCount > 0 || results.PendingCount > 0) {
                console.Red();
                console.Write("FAILED");
            } else {
                console.Green();
                console.Write("SUCCESS!");
            }

            console.ResetColor();
            console.Write(" -- ");

            console.Green();
            console.Write("{0} Passed", results.PassedCount);

            console.ResetColor();
            console.Write(" | ");

            console.Red();
            console.Write("{0} Failed", results.FailedCount);

            console.ResetColor();
            console.Write(" | ");

            console.Yellow();
            console.Write("{0} Pending", results.PendingCount);

            console.ResetColor();
            console.Write(" | ");
            console.Write("{0} Skipped", results.SkippedCount);

            if (results.ContainsFocusedUnits) {
                console.Write(" - ");
                console.White();
                console.Write("FOCUSED");
            }
            console.WriteLine();
       }
    }
}
