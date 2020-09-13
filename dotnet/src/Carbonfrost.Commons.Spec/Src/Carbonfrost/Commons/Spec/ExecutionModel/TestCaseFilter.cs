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
using System;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    static class TestCaseFilter {

        public static ITestCaseFilter Create(Action<TestExecutionContext> action) {
            return new Impl(action);
        }

        class Impl : ITestCaseFilter {
            private readonly Action<TestExecutionContext> _action;

            public Impl(Action<TestExecutionContext> action) {
                _action = action;
            }

            public void RunTest(TestExecutionContext context, Action<TestExecutionContext> next) {
                _action(context);
                next(context);
            }
        }
    }

}
