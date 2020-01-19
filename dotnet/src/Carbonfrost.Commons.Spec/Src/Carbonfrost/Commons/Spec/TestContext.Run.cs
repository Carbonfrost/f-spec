//
// Copyright 2018 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Threading;
using Carbonfrost.Commons.Spec;
using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.Commons.Spec {

    partial class TestContext {

        public TestCaseResult RunTest(Func<TestContext, object> testFunc) {
            return RunTest(testFunc, null);
        }

        public TestCaseResult RunTest(Func<TestContext, object> testFunc, TestOptions options) {
            if (testFunc == null) {
                throw new ArgumentNullException("testFunc");
            }
            if (options == null) {
                options = new TestOptions();
            }

            var result = new TestCaseResult(options.DisplayName);

            result.Starting();
            options.Filters.Add(new RunCommand(testFunc, options));
            var winder = new TestCaseCommandWinder(options.Filters);
            winder.RunAll(this);

            if (options.PassExplicitly) {
                result.SetFailed(SpecFailure.ExplicitPassNotSet());
            } else {
                result.SetSuccess();
            }
            result.Done();
            return result;
        }

        private void RunTestWithTimeout(Func<TestContext, object> testFunc, TimeSpan timeout) {
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

            private readonly Func<TestContext, object> _testFunc;
            private readonly TestOptions _opts;

            public RunCommand(Func<TestContext, object> coreRunTest, TestOptions opts) {
                _testFunc = coreRunTest;
                _opts = opts;
            }

            void ITestCaseFilter.RunTest(TestContext context, Action<TestContext> next) {
                // If the test object implements this ITestCaseFilter interface, then
                // this is a _substitute_ for the default execution logic.  THe main
                // use of this is to handle load exceptions when instantiating types.
                var filter = context.TestObject as ITestCaseFilter;
                if (filter != null) {
                    filter.RunTest(context, next);
                    return;
                }

                var effectiveTimeout = _opts.Timeout.GetValueOrDefault(
                    context.TestRunnerOptions.TestTimeout.GetValueOrDefault());
                context.RunTestWithTimeout(_testFunc, effectiveTimeout);

                if (next != null) {
                    next(context);
                }
            }
        }

        class TestCaseCommandWinder {

            private readonly IList<ITestCaseFilter> _commands;
            private int _position;

            public TestCaseCommandWinder(IList<ITestCaseFilter> commands) {
                _commands = commands;
            }

            public void RunAll(TestContext context) {
                _commands.First().RunTest(context, MoveNext());
            }

            public Action<TestContext> MoveNext() {
                _position++;
                if (_position < _commands.Count) {
                    int myPosition = _position; // create a copy of the value for the closure
                    Action<TestContext> result = tc => _commands[myPosition].RunTest(tc, MoveNext());
                    return result;
                }
                return null;
            }
        }
    }
}
