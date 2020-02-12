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
using System.Linq;

namespace Carbonfrost.Commons.Spec.ExecutionModel.Output {

    class ConsoleTestRunProblems : ConsoleOutputPart<IList<TestUnitResult>> {

        public bool ShortProblems { get; set; }

        protected override void RenderCore(IList<TestUnitResult> problems) {
            if (problems.Count == 0) {
                return;
            }

            console.WriteLine();
            console.PushIndent();

            int number = 1;
            var pending = new List<TestUnitResult>();

            foreach (var p in problems) {
                if (p.Failed) {
                    ConsoleLogger.DisplayResultDetails(number, context, p);
                    number++;

                } else if (p.IsPending) {
                    pending.Add(p);
                }
            }

            foreach (var p in pending) {
                ConsoleLogger.DisplayResultDetails(-1, context, p);
            }
            console.PopIndent();
        }
    }
}
