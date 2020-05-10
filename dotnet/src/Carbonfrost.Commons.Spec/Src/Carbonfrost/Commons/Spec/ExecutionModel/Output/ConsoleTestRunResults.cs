//
// Copyright 2019-2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

        protected override void RenderCore(TestRunResults results) {
            if (results.Failed) {
                console.Red();
                console.Write("FAILED");
            } else {
                console.Green();
                console.Write("SUCCESS!");
            }

            console.ResetColor();

            console.WriteLine();
            console.WriteLine();

            console.PushIndent();

            console.Green();
            console.WriteLine("{0,4} passed", results.PassedCount);
            console.ResetColor();

            if (results.Failed) {
                console.Red();
            }
            console.Write("{0,4} failed", results.FailedCount);

            if (results.PendingCount > 0) {
                console.Yellow();
                console.Write("  {0,4} pending", results.PendingCount);
            }

            console.ResetColor();

            if (results.SkippedCount > 0) {
                console.Write("  {0,4} skipped", results.SkippedCount);
            }

            if (results.ContainsFocusedUnits) {
                console.Magenta();
                console.Write("   FOCUSED");
                console.ResetColor();
            }
            console.WriteLine();
            console.PopIndent();
       }
    }
}
