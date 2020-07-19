//
// Copyright 2018, 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
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
using System.Collections.Generic;
using System.Linq;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    class TestCaseCommandWinder {

        private readonly ITestCaseFilter[] _commands;

        // Corresponding wrapper for each command that invokes the command and
        // provides access to the next delegate
        private readonly Action<TestExecutionContext>[] _actionWrappers;

        public TestCaseCommandWinder(IEnumerable<ITestCaseFilter> commands) {
            _commands = commands.ToArray();
            _actionWrappers = _commands.Select((c, i) => ActionWrapper(i)).ToArray();
        }

        private Action<TestExecutionContext> ActionWrapper(int index) {
            return tc => {
                _commands[index].RunTest(
                    tc,
                    _actionWrappers.ElementAtOrDefault(index + 1) ?? EmptyAction
                );
            };
        }

        private static void EmptyAction(TestExecutionContext context) {
        }

        public void RunAll(TestExecutionContext context) {
            _actionWrappers[0].Invoke(context);
        }
    }

}
