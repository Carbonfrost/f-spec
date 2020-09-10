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
using System.Threading;
using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.Commons.Spec {

    partial class TestExecutionContext {

        internal TestCaseResult RunCurrentTest() {
            var result = (TestCaseResult) ((TestCaseInfo) TestUnit).RunTest(this);
            return result;
        }

        public TestCaseResult RunTest(Action<TestExecutionContext> testFunc) {
            return RunTest(testFunc, null);
        }

        public TestCaseResult RunTest(Action<TestExecutionContext> testFunc, TestOptions options) {
            return RunTest(WrapFunc(testFunc), options);
        }

        public TestCaseResult RunTest(Func<TestExecutionContext, object> testFunc) {
            return RunTest(testFunc, null);
        }

        public TestCaseResult RunTest(string name, Action<TestExecutionContext> testFunc) {
            return RunTest(testFunc, TestOptions.Named(name));
        }

        public TestCaseResult RunTest(string name, Func<TestExecutionContext, object> testFunc) {
            return RunTest(testFunc, TestOptions.Named(name));
        }

        public TestCaseResult RunTest(Func<TestExecutionContext, object> testFunc, TestOptions options) {
            if (testFunc == null) {
                throw new ArgumentNullException(nameof(testFunc));
            }

            var test = new BespokeFact(testFunc, CurrentTest.TestName, options.Name);
            return CreateChildContext(test).RunCurrentTest();
        }

        public TestUnitResults RunTests(ITestDataProvider testDataProvider, Action<TestExecutionContext> testFunc) {
            return RunTests(testDataProvider, testFunc, null);
        }

        public TestUnitResults RunTests(ITestDataProvider testDataProvider, Action<TestExecutionContext> testFunc, TestOptions options) {
            return RunTests(testDataProvider, WrapFunc(testFunc), options);
        }

        public TestUnitResults RunTests(ITestDataProvider testDataProvider, Func<TestExecutionContext, object> testFunc) {
            return RunTests(testDataProvider, testFunc, null);
        }

        public TestUnitResults RunTests(string name, ITestDataProvider testDataProvider, Action<TestExecutionContext> testFunc) {
            return RunTests(testDataProvider, testFunc, TestOptions.Named(name));
        }

        public TestUnitResults RunTests(string name, ITestDataProvider testDataProvider, Func<TestExecutionContext, object> testFunc) {
            return RunTests(testDataProvider, testFunc, TestOptions.Named(name));
        }

        public TestUnitResults RunTests(ITestDataProvider testDataProvider, Func<TestExecutionContext, object> testFunc, TestOptions options) {
            if (testDataProvider is null) {
                throw new ArgumentNullException(nameof(testDataProvider));
            }

            var theoryName = CurrentTest.TestName.WithNameSuffix(options.Name);
            var bs = new BespokeTheory(testDataProvider, theoryName, testFunc);
            bs.InitializeSafe(this);
            foreach (var c in bs.Children) {
                c.InitializeSafe(WithSelf(c));
            }

            var results = new TestUnitResults(bs);
            foreach (var c in bs.Children) {
                var myCase = (TestCaseInfo) c;
                var myResult = CreateChildContext(myCase).RunCurrentTest();
                results.Children.Add(myResult);
            }
            return results;
        }

        internal void RunTestWithTimeout(Func<TestExecutionContext, object> testFunc, TimeSpan timeout) {
            if (timeout <= TimeSpan.Zero) {
                _testReturnValue = testFunc(this);
                return;
            }

            Exception error = null;
            var cts = new CancellationTokenSource();
            cts.CancelAfter(timeout);
            _cancellationToken = cts.Token;

            ParameterizedThreadStart thunk = syncObject => {
                try {
                    _testReturnValue = testFunc(this);
                } catch (Exception ex) {
                    error = ex;
                }
                lock (syncObject) {
                    Monitor.Pulse(syncObject);
                }
            };

            object monitorSync = new object();
            bool timedOut;

            var thread = new Thread(thunk);
            lock (monitorSync) {
                thread.Start(monitorSync);
                timedOut = !Monitor.Wait(monitorSync, timeout);
                cts.Cancel();
            }
            cts.Dispose();

            _cancellationToken = CancellationToken.None;

            if (timedOut) {
                thread.Abort();
                throw SpecFailure.TestTimedOut(timeout);
            }
            if (error != null) {
                throw error;
            }
        }

        private static Func<TestExecutionContext, object> WrapFunc(Action<TestExecutionContext> testFunc) {
            return tc => {
                testFunc(tc);
                return null;
            };
        }

        private TestExecutionContext CreateChildContext(TestCaseInfo test) {
            var result = new TestExecutionContext(this, test, TestObject);
            return result;
        }

    }
}
