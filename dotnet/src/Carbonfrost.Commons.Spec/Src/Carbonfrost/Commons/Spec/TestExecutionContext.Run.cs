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
using System.Threading;
using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.Commons.Spec {

    partial class TestExecutionContext {

        public TestCaseResult RunTest(Action<TestExecutionContext> testFunc) {
            return RunTest(testFunc, null);
        }

        public TestCaseResult RunTest(Action<TestExecutionContext> testFunc, TestOptions options) {
            return RunTest(tc => {
                testFunc(tc);
                return null;
            }, options);
        }

        public TestCaseResult RunTest(Func<TestExecutionContext, object> testFunc) {
            return RunTest(testFunc, null);
        }

        public TestCaseResult RunTest(Func<TestExecutionContext, object> testFunc, TestOptions options) {
            if (testFunc == null) {
                throw new ArgumentNullException(nameof(testFunc));
            }
            if (options == null) {
                options = new TestOptions();
            }

            var result = new TestCaseResult(options, (TestCaseInfo) CurrentTest);

            result.Starting();
            options.Filters.Add(new RunCommand(testFunc, options));
            var winder = new TestCaseCommandWinder(options.Filters);
            winder.RunAll(this);

            if (options.PassExplicitly) {
                result.SetFailed(SpecFailure.ExplicitPassNotSet());
            } else {
                result.SetSuccess();
            }
            result.Done(null, TestRunnerOptions);
            return result;
        }

        private void RunTestWithTimeout(Func<TestExecutionContext, object> testFunc, TimeSpan timeout) {
            if (timeout <= TimeSpan.Zero) {
                _testResult = testFunc(this);
                return;
            }

            Exception error = null;
            var cts = new CancellationTokenSource();
            cts.CancelAfter(timeout);
            _cancellationToken = cts.Token;

            ParameterizedThreadStart thunk = syncObject => {
                try {
                    _testResult = testFunc(this);
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

        class RunCommand : ITestCaseFilter {

            private readonly Func<TestExecutionContext, object> _testFunc;
            private readonly TestOptions _opts;

            public RunCommand(Func<TestExecutionContext, object> coreRunTest, TestOptions opts) {
                _testFunc = coreRunTest;
                _opts = opts;
            }

            void ITestCaseFilter.RunTest(TestExecutionContext context, Action<TestExecutionContext> next) {
                var adapt = (context.TestObject as ITestExecutionFilter) ?? TestExecutionFilter.Null;
                adapt.BeforeExecuting(context);

                // If the test object implements this ITestCaseFilter interface, then
                // this is a _substitute_ for the default execution logic.  THe main
                // use of this is to handle load exceptions when instantiating types.
                var filter = context.TestObject as ITestCaseFilter;
                if (filter != null) {
                    filter.RunTest(context, next);
                    adapt.AfterExecuting(context);
                    return;
                }

                try {
                    var effectiveTimeout = _opts.Timeout.GetValueOrDefault(
                        context.TestRunnerOptions.TestTimeout.GetValueOrDefault());
                    context.RunTestWithTimeout(_testFunc, effectiveTimeout);

                    next(context);
                } finally {
                    adapt.AfterExecuting(context);
                }
            }
        }

        class TestCaseCommandWinder {

            private readonly ITestCaseFilter[] _commands;

            // Corresponding wrapper for each command that invokes the command and
            // provides access to the next delegate
            private readonly Action<TestExecutionContext>[] _actionWrappers;

            public TestCaseCommandWinder(IList<ITestCaseFilter> commands) {
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
}
