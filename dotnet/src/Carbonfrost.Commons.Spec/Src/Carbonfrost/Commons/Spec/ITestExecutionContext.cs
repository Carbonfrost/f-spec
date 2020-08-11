//
// Copyright 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
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
using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.Commons.Spec {

    interface ITestExecutionContext : ITestContext {
        TestCaseResult RunTest(Action<TestExecutionContext> testFunc);
        TestCaseResult RunTest(Action<TestExecutionContext> testFunc, TestOptions options);
        TestCaseResult RunTest(Func<TestExecutionContext, object> testFunc);
        TestCaseResult RunTest(Func<TestExecutionContext, object> testFunc, TestOptions options);
        TestCaseResult RunTest(string name, Action<TestExecutionContext> testFunc);
        TestCaseResult RunTest(string name, Func<TestExecutionContext, object> testFunc);

        TestUnitResults RunTests(ITestDataProvider testDataProvider, Action<TestExecutionContext> testFunc);
        TestUnitResults RunTests(ITestDataProvider testDataProvider, Action<TestExecutionContext> testFunc, TestOptions options);
        TestUnitResults RunTests(ITestDataProvider testDataProvider, Func<TestExecutionContext, object> testFunc);
        TestUnitResults RunTests(ITestDataProvider testDataProvider, Func<TestExecutionContext, object> testFunc, TestOptions options);
        TestUnitResults RunTests(string name, ITestDataProvider testDataProvider, Action<TestExecutionContext> testFunc);
        TestUnitResults RunTests(string name, ITestDataProvider testDataProvider, Func<TestExecutionContext, object> testFunc);
    }
}
