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
using System;
using System.Collections.Generic;

namespace Carbonfrost.Commons.Spec.ExecutionModel.Output {

    class ConsoleTestRunProblems : ConsoleOutputPart<IList<TestUnitResult>> {

        public bool ShortProblems { get; set; }

        public ConsoleTestRunProblems(IConsoleWrapper console) : base(console) {}

        public override void Render(IList<TestUnitResult> problems) {
            if (problems.Count == 0) {
                return;
            }

            console.WriteLine("Summary of test run");
            console.PushIndent();
            foreach (var p in problems) {
                console.ColorFor(p);
                console.Write(p.DisplayName);

                console.ResetColor();
                ConsoleLogger.DisplayResultDetails(p);
            }
            console.PopIndent();
        }
    }
}
