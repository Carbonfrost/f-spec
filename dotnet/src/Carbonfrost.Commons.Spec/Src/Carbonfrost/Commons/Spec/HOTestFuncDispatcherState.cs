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
using System.Collections.Generic;

namespace Carbonfrost.Commons.Spec {

    interface IFuncDispatcher<TArgs> {
        int CallCount { get; }
        Exception LastException { get; }
        TestCodeDispatchInfo LastDispatchInfo { get; }
        TArgs LastArgs { get; }
        bool Called { get; }
        TArgs ArgsForCall(int index);

        TestCodeDispatchInfo DispatchInfoForCall(int index);
        Exception ExceptionForCall(int index);
        void RethrowExceptions();
        void Reset();
    }

    // Provides a higher order test func dispatcher that can be used
    // by any of them
    class HOTestFuncDispatcherState<TArgs, TResult> : IFuncDispatcher<TArgs> {

        private readonly List<Action<TArgs>> _before = new List<Action<TArgs>>();
        private readonly List<Action<CallData>> _after = new List<Action<CallData>>();
        private readonly Func<TArgs, TResult> _func;
        private readonly List<CallData> _calls = new List<CallData>();
        private bool _rethrow;

        public IReadOnlyList<CallData> Calls {
            get {
                return _calls;
            }
        }

        public int CallCount {
            get {
                return _calls.Count;
            }
        }

        public Action<TArgs, TResult> Action {
            get {
                return null;
            }
        }

        public Exception LastException {
            get {
                return ExceptionForCall(CallCount - 1);
            }
        }

        public TResult LastResult {
            get {
                return ResultForCall(CallCount - 1);
            }
        }

        public TestCodeDispatchInfo LastDispatchInfo {
            get {
                return DispatchInfoForCall(CallCount - 1);
            }
        }

        public TArgs LastArgs {
            get {
                return ArgsForCall(CallCount - 1);
            }
        }

        public HOTestFuncDispatcherState(Func<TArgs, TResult> func) {
            _func = func;
        }

        public void Before(Action<TArgs> action) {
            _before.Add(action);
        }

        public void After(Action<TArgs> action) {
            _after.Add(a => action(a.args));
        }

        public void After(Action<CallData> action) {
            _after.Add(action);
        }

        public void RethrowExceptions() {
            _rethrow = true;
        }

        public TResult Invoke(TArgs args) {
            TResult result = default;
            foreach (var action in _before) {
                action(args);
            }
            var dispatchInfo = Record.DispatchInfo(
                () => {
                    result = _func(args);
                }
            );
            var call = new CallData(args, result, dispatchInfo);
            foreach (var action in _after) {
                action(call);
            }
            _calls.Add(call);
            if (_rethrow && dispatchInfo.Exception != null) {
                dispatchInfo.Throw();
            }
            return result;
        }

        public TArgs ArgsForCall(int index) {
            return _calls[index].args;
        }

        public TResult ResultForCall(int index) {
            return _calls[index].result;
        }

        public Exception ExceptionForCall(int index) {
            return _calls[index].dispatchInfo.Exception;
        }

        public bool Called {
            get {
                return CallCount > 0;
            }
        }

        public void Reset() {
            _calls.Clear();
        }

        public TestCodeDispatchInfo DispatchInfoForCall(int index) {
            return _calls[index].dispatchInfo;
        }

        internal struct CallData {

            public readonly TArgs args;
            public readonly TResult result;
            public readonly TestCodeDispatchInfo dispatchInfo;

            public CallData(TArgs args, TResult result, TestCodeDispatchInfo dispatchInfo) {
                this.args = args;
                this.result = result;
                this.dispatchInfo = dispatchInfo;
            }
        }
    }
}
