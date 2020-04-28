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
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;

namespace Carbonfrost.Commons.Spec {

    // SyncContextImpl implements a synchronization context so that we
    // can capture the result of invoking an async method in the thread pool
    sealed class SyncContextImpl : SynchronizationContext {

        private Exception _error;

        private readonly ManualResetEvent _completed = new ManualResetEvent(false);

        private SyncContextImpl() {
        }

        public static void Run(Action action) {
            var ctxt = new SyncContextImpl();
            ctxt.RunCore(action, action.GetMethodInfo());
        }

        public static object Run(MethodInfo method, object o, object[] args) {
            var ctxt = new SyncContextImpl();
            object result = null;
            Action action = () => {
                result = method.Invoke(o, args);

                var task = result as Task;
                if (task != null) {
                    task.GetAwaiter().OnCompleted(ctxt.OperationCompleted);
                    result = TaskHelper.ResultOf(task);
                }
            };
            ctxt.RunCore(action, method);
            return result;
        }

        private void RunCore(Action action, MethodInfo method) {
            if (IsAsync(method)) {
                try {
                    var ctxt = this;

                    SynchronizationContext.SetSynchronizationContext(ctxt);

                    action();
                    ctxt._completed.WaitOne();

                    // Handle the thread pool exception
                    if (ctxt._error != null) {
                        ExceptionDispatchInfo.Capture(ctxt._error).Throw();
                    }
                }
                finally {
                    SynchronizationContext.SetSynchronizationContext(null);
                }
            } else {
                action();
            }
        }

        public override void Post(SendOrPostCallback d, object state) {
            try {
                d(state);
            }
            catch (Exception ex) {
                _error = ex;
            }
        }

        public override void OperationCompleted() {
            _completed.Set();
        }

        static bool IsAsync(MethodInfo method) {
            return method.IsDefined(typeof(AsyncStateMachineAttribute), false);
        }
    }
}
