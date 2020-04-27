//
// Copyright 2019, 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
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

using System.Linq;

namespace Carbonfrost.Commons.Spec.ExecutionModel.Output {

    class ConsoleTestRunProblems : ConsoleOutputPart<TestRunResults> {

        public bool Show {
            get;
            set;
        }

        protected override void RenderCore(TestRunResults results) {
            if (!Show) {
                return;
            }
            var problems = results.Problems;
            if (problems.Count == 0) {
                return;
            }

            console.WriteLine();

            // Print pending tests first so as to reduce scrollback
            if (problems.Pending.Any()) {
                console.Yellow();
                console.WriteLine("Pending: ");
                console.ResetColor();

                console.PushIndent();
                foreach (var p in problems.Pending) {
                    parts.forResultDetails.Render(context, p);
                }
                console.PopIndent();
            }

            if (problems.Failures.Any()) {
                console.Red();
                console.WriteLine("Failures: ");
                console.ResetColor();
                console.PushIndent();

                int number = 1;
                foreach (var p in problems.Failures) {
                    if (number > 0) {
                        console.Write(number + ") ");
                    }
                    parts.forResultDetails.Render(context, p);
                    number++;
                }

                console.PopIndent();
            }
        }
    }
}
