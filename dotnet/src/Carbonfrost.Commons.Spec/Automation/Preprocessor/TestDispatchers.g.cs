
//
// File was automatically generated at 09/04/2020 20:14:32
//

using System;
using System.Collections.Generic;

namespace Carbonfrost.Commons.Spec {


    public partial class TestActionDispatcher {
        public static TestActionDispatcher Create(Action action) {
            return new TestActionDispatcher(action);
        }

        public static TestActionDispatcher Create() {
            return new TestActionDispatcher();
        }
    }

    public partial class TestActionDispatcher : IFuncDispatcher<Unit> {

        private readonly HOTestFuncDispatcherState<Unit, Unit> _inner;

        public TestActionDispatcher() : this(null) {
        }

        public TestActionDispatcher(Action action) {
            if (action == null) {
                action = () => {};
            }
            _inner = new HOTestFuncDispatcherState<Unit, Unit>(CallAdapter(action));
        }

        public IReadOnlyList<CallInfo> Calls {
            get {
                return _inner.Calls.ProjectedTo(c => new CallInfo(c.args, c.dispatchInfo));
            }
        }

        public struct CallInfo {

            public TestCodeDispatchInfo DispatchInfo { get; }

            public Exception Exception {
                get {
                    return DispatchInfo.Exception;
                }
            }

            internal CallInfo(Unit args, TestCodeDispatchInfo dispatchInfo) {
                DispatchInfo = dispatchInfo;
            }
        }

        public int CallCount {
            get {
                return _inner.CallCount;
            }
        }

        public Exception LastException {
            get {
                return _inner.LastException;
            }
        }

        public TestCodeDispatchInfo LastDispatchInfo {
            get {
                return _inner.LastDispatchInfo;
            }
        }

        public bool Called {
            get {
                return _inner.Called;
            }
        }

        public void After(Action action) {
            _inner.After(ActionAdapter(action));
        }

        public void Before(Action action) {
            _inner.Before(ActionAdapter(action));
        }

        public void Invoke() {
            _inner.Invoke(Unit.Value);
        }

        public Action Handler {
            get {
                return Invoke;
            }
        }

        private Func<Unit, Unit> CallAdapter(Action action) {
            return t => {
                action();
                return Unit.Value;
            };
        }

        private Action<Unit> ActionAdapter(Action action) {
            return t => action();
        }

        public TestCodeDispatchInfo DispatchInfoForCall(int index) {
            return _inner.DispatchInfoForCall(index);
        }

        public Exception ExceptionForCall(int index) {
            return _inner.ExceptionForCall(index);
        }

        public void Reset() {
            _inner.Reset();
        }

        Unit IFuncDispatcher<Unit>.LastArgs {
            get {
                return _inner.LastArgs;
            }
        }

        Unit IFuncDispatcher<Unit>.ArgsForCall(int index) {
            return _inner.ArgsForCall(index);
        }


    }


    public partial class TestFuncDispatcher {
        public static TestFuncDispatcher<TResult> Create<TResult>(Func<TResult> func) {
            return new TestFuncDispatcher<TResult>(func);
        }

        public static TestFuncDispatcher<TResult> Create<TResult>() {
            return new TestFuncDispatcher<TResult>();
        }
    }


    public partial class TestFuncDispatcher<TResult> : IFuncDispatcher<Unit> {

        private readonly HOTestFuncDispatcherState<Unit, TResult> _inner;

        public TestFuncDispatcher() : this(null) {
        }

        public TestFuncDispatcher(Func<TResult> func) {
            if (func == null) {
                func = () => default;
            }
            _inner = new HOTestFuncDispatcherState<Unit, TResult>(CallAdapter(func));
        }

        public IReadOnlyList<CallInfo> Calls {
            get {
                return _inner.Calls.ProjectedTo(c => new CallInfo(c.args, c.result, c.dispatchInfo));
            }
        }

        public struct CallInfo {

            public TResult Result { get; }
            public TestCodeDispatchInfo DispatchInfo { get; }

            public Exception Exception {
                get {
                    return DispatchInfo.Exception;
                }
            }

            internal CallInfo(Unit args, TResult result, TestCodeDispatchInfo dispatchInfo) {
                Result = result;
                DispatchInfo = dispatchInfo;
            }
        }

        public int CallCount {
            get {
                return _inner.CallCount;
            }
        }

        public Exception LastException {
            get {
                return _inner.LastException;
            }
        }

        public TResult LastResult {
            get {
                return _inner.LastResult;
            }
        }

        public TestCodeDispatchInfo LastDispatchInfo {
            get {
                return _inner.LastDispatchInfo;
            }
        }

        public bool Called {
            get {
                return _inner.Called;
            }
        }

        public void After(Action action) {
            _inner.After(ActionAdapter(action));
        }

        public void Before(Action action) {
            _inner.Before(ActionAdapter(action));
        }

        public TResult Invoke() {
            return _inner.Invoke(Unit.Value);
        }

        public Func<TResult> Handler {
            get {
                return Invoke;
            }
        }

        private Func<Unit, TResult> CallAdapter(Func<TResult> func) {
            return t => func();
        }

        private Action<Unit> ActionAdapter(Action action) {
            return t => action();
        }

        public TestCodeDispatchInfo DispatchInfoForCall(int index) {
            return _inner.DispatchInfoForCall(index);
        }

        public Exception ExceptionForCall(int index) {
            return _inner.ExceptionForCall(index);
        }

        public void Reset() {
            _inner.Reset();
        }

        public TResult ResultForCall(int index) {
            return _inner.ResultForCall(index);
        }

        Unit IFuncDispatcher<Unit>.LastArgs {
            get {
                return _inner.LastArgs;
            }
        }

        Unit IFuncDispatcher<Unit>.ArgsForCall(int index) {
            return _inner.ArgsForCall(index);
        }

    }

    public partial class TestActionDispatcher {
        public static TestActionDispatcher<T> Create<T>(Action<T> action) {
            return new TestActionDispatcher<T>(action);
        }

        public static TestActionDispatcher<T> Create<T>() {
            return new TestActionDispatcher<T>();
        }
    }

    public partial class TestActionDispatcher<T> : IFuncDispatcher<T> {

        private readonly HOTestFuncDispatcherState<T, Unit> _inner;

        public TestActionDispatcher() : this(null) {
        }

        public TestActionDispatcher(Action<T> action) {
            if (action == null) {
                action = (T arg1) => {};
            }
            _inner = new HOTestFuncDispatcherState<T, Unit>(CallAdapter(action));
        }

        public IReadOnlyList<CallInfo> Calls {
            get {
                return _inner.Calls.ProjectedTo(c => new CallInfo(c.args, c.dispatchInfo));
            }
        }

        public struct CallInfo {

            public T Args { get; }
            public TestCodeDispatchInfo DispatchInfo { get; }

            public Exception Exception {
                get {
                    return DispatchInfo.Exception;
                }
            }

            internal CallInfo(T args, TestCodeDispatchInfo dispatchInfo) {
                Args = args;
                DispatchInfo = dispatchInfo;
            }
        }

        public int CallCount {
            get {
                return _inner.CallCount;
            }
        }

        public Exception LastException {
            get {
                return _inner.LastException;
            }
        }

        public TestCodeDispatchInfo LastDispatchInfo {
            get {
                return _inner.LastDispatchInfo;
            }
        }

        public bool Called {
            get {
                return _inner.Called;
            }
        }

        public void After(Action<T> action) {
            _inner.After(ActionAdapter(action));
        }

        public void Before(Action<T> action) {
            _inner.Before(ActionAdapter(action));
        }

        public void Invoke(T arg1) {
            _inner.Invoke((arg1));
        }

        public Action<T> Handler {
            get {
                return Invoke;
            }
        }

        private Func<T, Unit> CallAdapter(Action<T> action) {
            return t => {
                action(t);
                return Unit.Value;
            };
        }

        private Action<T> ActionAdapter(Action<T> action) {
            return t => action(t);
        }

        public TestCodeDispatchInfo DispatchInfoForCall(int index) {
            return _inner.DispatchInfoForCall(index);
        }

        public Exception ExceptionForCall(int index) {
            return _inner.ExceptionForCall(index);
        }

        public void Reset() {
            _inner.Reset();
        }

        T IFuncDispatcher<T>.LastArgs {
            get {
                return _inner.LastArgs;
            }
        }

        T IFuncDispatcher<T>.ArgsForCall(int index) {
            return _inner.ArgsForCall(index);
        }


        public T LastArgs {
            get {
                return _inner.LastArgs;
            }
        }

        public T ArgsForCall(int index) {
            return _inner.ArgsForCall(index);
        }


    }


    public partial class TestFuncDispatcher {
        public static TestFuncDispatcher<T, TResult> Create<T, TResult>(Func<T, TResult> func) {
            return new TestFuncDispatcher<T, TResult>(func);
        }

        public static TestFuncDispatcher<T, TResult> Create<T, TResult>() {
            return new TestFuncDispatcher<T, TResult>();
        }
    }


    public partial class TestFuncDispatcher<T, TResult> : IFuncDispatcher<T> {

        private readonly HOTestFuncDispatcherState<T, TResult> _inner;

        public TestFuncDispatcher() : this(null) {
        }

        public TestFuncDispatcher(Func<T, TResult> func) {
            if (func == null) {
                func = (T arg1) => default;
            }
            _inner = new HOTestFuncDispatcherState<T, TResult>(CallAdapter(func));
        }

        public IReadOnlyList<CallInfo> Calls {
            get {
                return _inner.Calls.ProjectedTo(c => new CallInfo(c.args, c.result, c.dispatchInfo));
            }
        }

        public struct CallInfo {

            public T Args { get; }
            public TResult Result { get; }
            public TestCodeDispatchInfo DispatchInfo { get; }

            public Exception Exception {
                get {
                    return DispatchInfo.Exception;
                }
            }

            internal CallInfo(T args, TResult result, TestCodeDispatchInfo dispatchInfo) {
                Args = args;
                Result = result;
                DispatchInfo = dispatchInfo;
            }
        }

        public int CallCount {
            get {
                return _inner.CallCount;
            }
        }

        public Exception LastException {
            get {
                return _inner.LastException;
            }
        }

        public TResult LastResult {
            get {
                return _inner.LastResult;
            }
        }

        public TestCodeDispatchInfo LastDispatchInfo {
            get {
                return _inner.LastDispatchInfo;
            }
        }

        public bool Called {
            get {
                return _inner.Called;
            }
        }

        public void After(Action<T> action) {
            _inner.After(ActionAdapter(action));
        }

        public void Before(Action<T> action) {
            _inner.Before(ActionAdapter(action));
        }

        public TResult Invoke(T arg1) {
            return _inner.Invoke((arg1));
        }

        public Func<T, TResult> Handler {
            get {
                return Invoke;
            }
        }

        private Func<T, TResult> CallAdapter(Func<T, TResult> func) {
            return t => func(t);
        }

        private Action<T> ActionAdapter(Action<T> action) {
            return t => action(t);
        }

        public TestCodeDispatchInfo DispatchInfoForCall(int index) {
            return _inner.DispatchInfoForCall(index);
        }

        public Exception ExceptionForCall(int index) {
            return _inner.ExceptionForCall(index);
        }

        public void Reset() {
            _inner.Reset();
        }

        public TResult ResultForCall(int index) {
            return _inner.ResultForCall(index);
        }

        T IFuncDispatcher<T>.LastArgs {
            get {
                return _inner.LastArgs;
            }
        }

        T IFuncDispatcher<T>.ArgsForCall(int index) {
            return _inner.ArgsForCall(index);
        }


        public T LastArgs {
            get {
                return _inner.LastArgs;
            }
        }

        public T ArgsForCall(int index) {
            return _inner.ArgsForCall(index);
        }
    }

    public partial class TestActionDispatcher {
        public static TestActionDispatcher<T1, T2> Create<T1, T2>(Action<T1, T2> action) {
            return new TestActionDispatcher<T1, T2>(action);
        }

        public static TestActionDispatcher<T1, T2> Create<T1, T2>() {
            return new TestActionDispatcher<T1, T2>();
        }
    }

    public partial class TestActionDispatcher<T1, T2> : IFuncDispatcher<ValueTuple<T1, T2>> {

        private readonly HOTestFuncDispatcherState<ValueTuple<T1, T2>, Unit> _inner;

        public TestActionDispatcher() : this(null) {
        }

        public TestActionDispatcher(Action<T1, T2> action) {
            if (action == null) {
                action = (T1 arg1, T2 arg2) => {};
            }
            _inner = new HOTestFuncDispatcherState<ValueTuple<T1, T2>, Unit>(CallAdapter(action));
        }

        public IReadOnlyList<CallInfo> Calls {
            get {
                return _inner.Calls.ProjectedTo(c => new CallInfo(c.args, c.dispatchInfo));
            }
        }

        public struct CallInfo {

            public ValueTuple<T1, T2> Args { get; }
            public TestCodeDispatchInfo DispatchInfo { get; }

            public Exception Exception {
                get {
                    return DispatchInfo.Exception;
                }
            }

            internal CallInfo(ValueTuple<T1, T2> args, TestCodeDispatchInfo dispatchInfo) {
                Args = args;
                DispatchInfo = dispatchInfo;
            }
        }

        public int CallCount {
            get {
                return _inner.CallCount;
            }
        }

        public Exception LastException {
            get {
                return _inner.LastException;
            }
        }

        public TestCodeDispatchInfo LastDispatchInfo {
            get {
                return _inner.LastDispatchInfo;
            }
        }

        public bool Called {
            get {
                return _inner.Called;
            }
        }

        public void After(Action<T1, T2> action) {
            _inner.After(ActionAdapter(action));
        }

        public void Before(Action<T1, T2> action) {
            _inner.Before(ActionAdapter(action));
        }

        public void Invoke(T1 arg1, T2 arg2) {
            _inner.Invoke((arg1, arg2));
        }

        public Action<T1, T2> Handler {
            get {
                return Invoke;
            }
        }

        private Func<ValueTuple<T1, T2>, Unit> CallAdapter(Action<T1, T2> action) {
            return t => {
                action(t.Item1, t.Item2);
                return Unit.Value;
            };
        }

        private Action<ValueTuple<T1, T2>> ActionAdapter(Action<T1, T2> action) {
            return t => action(t.Item1, t.Item2);
        }

        public TestCodeDispatchInfo DispatchInfoForCall(int index) {
            return _inner.DispatchInfoForCall(index);
        }

        public Exception ExceptionForCall(int index) {
            return _inner.ExceptionForCall(index);
        }

        public void Reset() {
            _inner.Reset();
        }

        ValueTuple<T1, T2> IFuncDispatcher<ValueTuple<T1, T2>>.LastArgs {
            get {
                return _inner.LastArgs;
            }
        }

        ValueTuple<T1, T2> IFuncDispatcher<ValueTuple<T1, T2>>.ArgsForCall(int index) {
            return _inner.ArgsForCall(index);
        }


        public ValueTuple<T1, T2> LastArgs {
            get {
                return _inner.LastArgs;
            }
        }

        public ValueTuple<T1, T2> ArgsForCall(int index) {
            return _inner.ArgsForCall(index);
        }


    }


    public partial class TestFuncDispatcher {
        public static TestFuncDispatcher<T1, T2, TResult> Create<T1, T2, TResult>(Func<T1, T2, TResult> func) {
            return new TestFuncDispatcher<T1, T2, TResult>(func);
        }

        public static TestFuncDispatcher<T1, T2, TResult> Create<T1, T2, TResult>() {
            return new TestFuncDispatcher<T1, T2, TResult>();
        }
    }


    public partial class TestFuncDispatcher<T1, T2, TResult> : IFuncDispatcher<ValueTuple<T1, T2>> {

        private readonly HOTestFuncDispatcherState<ValueTuple<T1, T2>, TResult> _inner;

        public TestFuncDispatcher() : this(null) {
        }

        public TestFuncDispatcher(Func<T1, T2, TResult> func) {
            if (func == null) {
                func = (T1 arg1, T2 arg2) => default;
            }
            _inner = new HOTestFuncDispatcherState<ValueTuple<T1, T2>, TResult>(CallAdapter(func));
        }

        public IReadOnlyList<CallInfo> Calls {
            get {
                return _inner.Calls.ProjectedTo(c => new CallInfo(c.args, c.result, c.dispatchInfo));
            }
        }

        public struct CallInfo {

            public ValueTuple<T1, T2> Args { get; }
            public TResult Result { get; }
            public TestCodeDispatchInfo DispatchInfo { get; }

            public Exception Exception {
                get {
                    return DispatchInfo.Exception;
                }
            }

            internal CallInfo(ValueTuple<T1, T2> args, TResult result, TestCodeDispatchInfo dispatchInfo) {
                Args = args;
                Result = result;
                DispatchInfo = dispatchInfo;
            }
        }

        public int CallCount {
            get {
                return _inner.CallCount;
            }
        }

        public Exception LastException {
            get {
                return _inner.LastException;
            }
        }

        public TResult LastResult {
            get {
                return _inner.LastResult;
            }
        }

        public TestCodeDispatchInfo LastDispatchInfo {
            get {
                return _inner.LastDispatchInfo;
            }
        }

        public bool Called {
            get {
                return _inner.Called;
            }
        }

        public void After(Action<T1, T2> action) {
            _inner.After(ActionAdapter(action));
        }

        public void Before(Action<T1, T2> action) {
            _inner.Before(ActionAdapter(action));
        }

        public TResult Invoke(T1 arg1, T2 arg2) {
            return _inner.Invoke((arg1, arg2));
        }

        public Func<T1, T2, TResult> Handler {
            get {
                return Invoke;
            }
        }

        private Func<ValueTuple<T1, T2>, TResult> CallAdapter(Func<T1, T2, TResult> func) {
            return t => func(t.Item1, t.Item2);
        }

        private Action<ValueTuple<T1, T2>> ActionAdapter(Action<T1, T2> action) {
            return t => action(t.Item1, t.Item2);
        }

        public TestCodeDispatchInfo DispatchInfoForCall(int index) {
            return _inner.DispatchInfoForCall(index);
        }

        public Exception ExceptionForCall(int index) {
            return _inner.ExceptionForCall(index);
        }

        public void Reset() {
            _inner.Reset();
        }

        public TResult ResultForCall(int index) {
            return _inner.ResultForCall(index);
        }

        ValueTuple<T1, T2> IFuncDispatcher<ValueTuple<T1, T2>>.LastArgs {
            get {
                return _inner.LastArgs;
            }
        }

        ValueTuple<T1, T2> IFuncDispatcher<ValueTuple<T1, T2>>.ArgsForCall(int index) {
            return _inner.ArgsForCall(index);
        }


        public ValueTuple<T1, T2> LastArgs {
            get {
                return _inner.LastArgs;
            }
        }

        public ValueTuple<T1, T2> ArgsForCall(int index) {
            return _inner.ArgsForCall(index);
        }
    }

    public partial class TestActionDispatcher {
        public static TestActionDispatcher<T1, T2, T3> Create<T1, T2, T3>(Action<T1, T2, T3> action) {
            return new TestActionDispatcher<T1, T2, T3>(action);
        }

        public static TestActionDispatcher<T1, T2, T3> Create<T1, T2, T3>() {
            return new TestActionDispatcher<T1, T2, T3>();
        }
    }

    public partial class TestActionDispatcher<T1, T2, T3> : IFuncDispatcher<ValueTuple<T1, T2, T3>> {

        private readonly HOTestFuncDispatcherState<ValueTuple<T1, T2, T3>, Unit> _inner;

        public TestActionDispatcher() : this(null) {
        }

        public TestActionDispatcher(Action<T1, T2, T3> action) {
            if (action == null) {
                action = (T1 arg1, T2 arg2, T3 arg3) => {};
            }
            _inner = new HOTestFuncDispatcherState<ValueTuple<T1, T2, T3>, Unit>(CallAdapter(action));
        }

        public IReadOnlyList<CallInfo> Calls {
            get {
                return _inner.Calls.ProjectedTo(c => new CallInfo(c.args, c.dispatchInfo));
            }
        }

        public struct CallInfo {

            public ValueTuple<T1, T2, T3> Args { get; }
            public TestCodeDispatchInfo DispatchInfo { get; }

            public Exception Exception {
                get {
                    return DispatchInfo.Exception;
                }
            }

            internal CallInfo(ValueTuple<T1, T2, T3> args, TestCodeDispatchInfo dispatchInfo) {
                Args = args;
                DispatchInfo = dispatchInfo;
            }
        }

        public int CallCount {
            get {
                return _inner.CallCount;
            }
        }

        public Exception LastException {
            get {
                return _inner.LastException;
            }
        }

        public TestCodeDispatchInfo LastDispatchInfo {
            get {
                return _inner.LastDispatchInfo;
            }
        }

        public bool Called {
            get {
                return _inner.Called;
            }
        }

        public void After(Action<T1, T2, T3> action) {
            _inner.After(ActionAdapter(action));
        }

        public void Before(Action<T1, T2, T3> action) {
            _inner.Before(ActionAdapter(action));
        }

        public void Invoke(T1 arg1, T2 arg2, T3 arg3) {
            _inner.Invoke((arg1, arg2, arg3));
        }

        public Action<T1, T2, T3> Handler {
            get {
                return Invoke;
            }
        }

        private Func<ValueTuple<T1, T2, T3>, Unit> CallAdapter(Action<T1, T2, T3> action) {
            return t => {
                action(t.Item1, t.Item2, t.Item3);
                return Unit.Value;
            };
        }

        private Action<ValueTuple<T1, T2, T3>> ActionAdapter(Action<T1, T2, T3> action) {
            return t => action(t.Item1, t.Item2, t.Item3);
        }

        public TestCodeDispatchInfo DispatchInfoForCall(int index) {
            return _inner.DispatchInfoForCall(index);
        }

        public Exception ExceptionForCall(int index) {
            return _inner.ExceptionForCall(index);
        }

        public void Reset() {
            _inner.Reset();
        }

        ValueTuple<T1, T2, T3> IFuncDispatcher<ValueTuple<T1, T2, T3>>.LastArgs {
            get {
                return _inner.LastArgs;
            }
        }

        ValueTuple<T1, T2, T3> IFuncDispatcher<ValueTuple<T1, T2, T3>>.ArgsForCall(int index) {
            return _inner.ArgsForCall(index);
        }


        public ValueTuple<T1, T2, T3> LastArgs {
            get {
                return _inner.LastArgs;
            }
        }

        public ValueTuple<T1, T2, T3> ArgsForCall(int index) {
            return _inner.ArgsForCall(index);
        }


    }


    public partial class TestFuncDispatcher {
        public static TestFuncDispatcher<T1, T2, T3, TResult> Create<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> func) {
            return new TestFuncDispatcher<T1, T2, T3, TResult>(func);
        }

        public static TestFuncDispatcher<T1, T2, T3, TResult> Create<T1, T2, T3, TResult>() {
            return new TestFuncDispatcher<T1, T2, T3, TResult>();
        }
    }


    public partial class TestFuncDispatcher<T1, T2, T3, TResult> : IFuncDispatcher<ValueTuple<T1, T2, T3>> {

        private readonly HOTestFuncDispatcherState<ValueTuple<T1, T2, T3>, TResult> _inner;

        public TestFuncDispatcher() : this(null) {
        }

        public TestFuncDispatcher(Func<T1, T2, T3, TResult> func) {
            if (func == null) {
                func = (T1 arg1, T2 arg2, T3 arg3) => default;
            }
            _inner = new HOTestFuncDispatcherState<ValueTuple<T1, T2, T3>, TResult>(CallAdapter(func));
        }

        public IReadOnlyList<CallInfo> Calls {
            get {
                return _inner.Calls.ProjectedTo(c => new CallInfo(c.args, c.result, c.dispatchInfo));
            }
        }

        public struct CallInfo {

            public ValueTuple<T1, T2, T3> Args { get; }
            public TResult Result { get; }
            public TestCodeDispatchInfo DispatchInfo { get; }

            public Exception Exception {
                get {
                    return DispatchInfo.Exception;
                }
            }

            internal CallInfo(ValueTuple<T1, T2, T3> args, TResult result, TestCodeDispatchInfo dispatchInfo) {
                Args = args;
                Result = result;
                DispatchInfo = dispatchInfo;
            }
        }

        public int CallCount {
            get {
                return _inner.CallCount;
            }
        }

        public Exception LastException {
            get {
                return _inner.LastException;
            }
        }

        public TResult LastResult {
            get {
                return _inner.LastResult;
            }
        }

        public TestCodeDispatchInfo LastDispatchInfo {
            get {
                return _inner.LastDispatchInfo;
            }
        }

        public bool Called {
            get {
                return _inner.Called;
            }
        }

        public void After(Action<T1, T2, T3> action) {
            _inner.After(ActionAdapter(action));
        }

        public void Before(Action<T1, T2, T3> action) {
            _inner.Before(ActionAdapter(action));
        }

        public TResult Invoke(T1 arg1, T2 arg2, T3 arg3) {
            return _inner.Invoke((arg1, arg2, arg3));
        }

        public Func<T1, T2, T3, TResult> Handler {
            get {
                return Invoke;
            }
        }

        private Func<ValueTuple<T1, T2, T3>, TResult> CallAdapter(Func<T1, T2, T3, TResult> func) {
            return t => func(t.Item1, t.Item2, t.Item3);
        }

        private Action<ValueTuple<T1, T2, T3>> ActionAdapter(Action<T1, T2, T3> action) {
            return t => action(t.Item1, t.Item2, t.Item3);
        }

        public TestCodeDispatchInfo DispatchInfoForCall(int index) {
            return _inner.DispatchInfoForCall(index);
        }

        public Exception ExceptionForCall(int index) {
            return _inner.ExceptionForCall(index);
        }

        public void Reset() {
            _inner.Reset();
        }

        public TResult ResultForCall(int index) {
            return _inner.ResultForCall(index);
        }

        ValueTuple<T1, T2, T3> IFuncDispatcher<ValueTuple<T1, T2, T3>>.LastArgs {
            get {
                return _inner.LastArgs;
            }
        }

        ValueTuple<T1, T2, T3> IFuncDispatcher<ValueTuple<T1, T2, T3>>.ArgsForCall(int index) {
            return _inner.ArgsForCall(index);
        }


        public ValueTuple<T1, T2, T3> LastArgs {
            get {
                return _inner.LastArgs;
            }
        }

        public ValueTuple<T1, T2, T3> ArgsForCall(int index) {
            return _inner.ArgsForCall(index);
        }
    }

    public partial class TestActionDispatcher {
        public static TestActionDispatcher<T1, T2, T3, T4> Create<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action) {
            return new TestActionDispatcher<T1, T2, T3, T4>(action);
        }

        public static TestActionDispatcher<T1, T2, T3, T4> Create<T1, T2, T3, T4>() {
            return new TestActionDispatcher<T1, T2, T3, T4>();
        }
    }

    public partial class TestActionDispatcher<T1, T2, T3, T4> : IFuncDispatcher<ValueTuple<T1, T2, T3, T4>> {

        private readonly HOTestFuncDispatcherState<ValueTuple<T1, T2, T3, T4>, Unit> _inner;

        public TestActionDispatcher() : this(null) {
        }

        public TestActionDispatcher(Action<T1, T2, T3, T4> action) {
            if (action == null) {
                action = (T1 arg1, T2 arg2, T3 arg3, T4 arg4) => {};
            }
            _inner = new HOTestFuncDispatcherState<ValueTuple<T1, T2, T3, T4>, Unit>(CallAdapter(action));
        }

        public IReadOnlyList<CallInfo> Calls {
            get {
                return _inner.Calls.ProjectedTo(c => new CallInfo(c.args, c.dispatchInfo));
            }
        }

        public struct CallInfo {

            public ValueTuple<T1, T2, T3, T4> Args { get; }
            public TestCodeDispatchInfo DispatchInfo { get; }

            public Exception Exception {
                get {
                    return DispatchInfo.Exception;
                }
            }

            internal CallInfo(ValueTuple<T1, T2, T3, T4> args, TestCodeDispatchInfo dispatchInfo) {
                Args = args;
                DispatchInfo = dispatchInfo;
            }
        }

        public int CallCount {
            get {
                return _inner.CallCount;
            }
        }

        public Exception LastException {
            get {
                return _inner.LastException;
            }
        }

        public TestCodeDispatchInfo LastDispatchInfo {
            get {
                return _inner.LastDispatchInfo;
            }
        }

        public bool Called {
            get {
                return _inner.Called;
            }
        }

        public void After(Action<T1, T2, T3, T4> action) {
            _inner.After(ActionAdapter(action));
        }

        public void Before(Action<T1, T2, T3, T4> action) {
            _inner.Before(ActionAdapter(action));
        }

        public void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4) {
            _inner.Invoke((arg1, arg2, arg3, arg4));
        }

        public Action<T1, T2, T3, T4> Handler {
            get {
                return Invoke;
            }
        }

        private Func<ValueTuple<T1, T2, T3, T4>, Unit> CallAdapter(Action<T1, T2, T3, T4> action) {
            return t => {
                action(t.Item1, t.Item2, t.Item3, t.Item4);
                return Unit.Value;
            };
        }

        private Action<ValueTuple<T1, T2, T3, T4>> ActionAdapter(Action<T1, T2, T3, T4> action) {
            return t => action(t.Item1, t.Item2, t.Item3, t.Item4);
        }

        public TestCodeDispatchInfo DispatchInfoForCall(int index) {
            return _inner.DispatchInfoForCall(index);
        }

        public Exception ExceptionForCall(int index) {
            return _inner.ExceptionForCall(index);
        }

        public void Reset() {
            _inner.Reset();
        }

        ValueTuple<T1, T2, T3, T4> IFuncDispatcher<ValueTuple<T1, T2, T3, T4>>.LastArgs {
            get {
                return _inner.LastArgs;
            }
        }

        ValueTuple<T1, T2, T3, T4> IFuncDispatcher<ValueTuple<T1, T2, T3, T4>>.ArgsForCall(int index) {
            return _inner.ArgsForCall(index);
        }


        public ValueTuple<T1, T2, T3, T4> LastArgs {
            get {
                return _inner.LastArgs;
            }
        }

        public ValueTuple<T1, T2, T3, T4> ArgsForCall(int index) {
            return _inner.ArgsForCall(index);
        }


    }


    public partial class TestFuncDispatcher {
        public static TestFuncDispatcher<T1, T2, T3, T4, TResult> Create<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> func) {
            return new TestFuncDispatcher<T1, T2, T3, T4, TResult>(func);
        }

        public static TestFuncDispatcher<T1, T2, T3, T4, TResult> Create<T1, T2, T3, T4, TResult>() {
            return new TestFuncDispatcher<T1, T2, T3, T4, TResult>();
        }
    }


    public partial class TestFuncDispatcher<T1, T2, T3, T4, TResult> : IFuncDispatcher<ValueTuple<T1, T2, T3, T4>> {

        private readonly HOTestFuncDispatcherState<ValueTuple<T1, T2, T3, T4>, TResult> _inner;

        public TestFuncDispatcher() : this(null) {
        }

        public TestFuncDispatcher(Func<T1, T2, T3, T4, TResult> func) {
            if (func == null) {
                func = (T1 arg1, T2 arg2, T3 arg3, T4 arg4) => default;
            }
            _inner = new HOTestFuncDispatcherState<ValueTuple<T1, T2, T3, T4>, TResult>(CallAdapter(func));
        }

        public IReadOnlyList<CallInfo> Calls {
            get {
                return _inner.Calls.ProjectedTo(c => new CallInfo(c.args, c.result, c.dispatchInfo));
            }
        }

        public struct CallInfo {

            public ValueTuple<T1, T2, T3, T4> Args { get; }
            public TResult Result { get; }
            public TestCodeDispatchInfo DispatchInfo { get; }

            public Exception Exception {
                get {
                    return DispatchInfo.Exception;
                }
            }

            internal CallInfo(ValueTuple<T1, T2, T3, T4> args, TResult result, TestCodeDispatchInfo dispatchInfo) {
                Args = args;
                Result = result;
                DispatchInfo = dispatchInfo;
            }
        }

        public int CallCount {
            get {
                return _inner.CallCount;
            }
        }

        public Exception LastException {
            get {
                return _inner.LastException;
            }
        }

        public TResult LastResult {
            get {
                return _inner.LastResult;
            }
        }

        public TestCodeDispatchInfo LastDispatchInfo {
            get {
                return _inner.LastDispatchInfo;
            }
        }

        public bool Called {
            get {
                return _inner.Called;
            }
        }

        public void After(Action<T1, T2, T3, T4> action) {
            _inner.After(ActionAdapter(action));
        }

        public void Before(Action<T1, T2, T3, T4> action) {
            _inner.Before(ActionAdapter(action));
        }

        public TResult Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4) {
            return _inner.Invoke((arg1, arg2, arg3, arg4));
        }

        public Func<T1, T2, T3, T4, TResult> Handler {
            get {
                return Invoke;
            }
        }

        private Func<ValueTuple<T1, T2, T3, T4>, TResult> CallAdapter(Func<T1, T2, T3, T4, TResult> func) {
            return t => func(t.Item1, t.Item2, t.Item3, t.Item4);
        }

        private Action<ValueTuple<T1, T2, T3, T4>> ActionAdapter(Action<T1, T2, T3, T4> action) {
            return t => action(t.Item1, t.Item2, t.Item3, t.Item4);
        }

        public TestCodeDispatchInfo DispatchInfoForCall(int index) {
            return _inner.DispatchInfoForCall(index);
        }

        public Exception ExceptionForCall(int index) {
            return _inner.ExceptionForCall(index);
        }

        public void Reset() {
            _inner.Reset();
        }

        public TResult ResultForCall(int index) {
            return _inner.ResultForCall(index);
        }

        ValueTuple<T1, T2, T3, T4> IFuncDispatcher<ValueTuple<T1, T2, T3, T4>>.LastArgs {
            get {
                return _inner.LastArgs;
            }
        }

        ValueTuple<T1, T2, T3, T4> IFuncDispatcher<ValueTuple<T1, T2, T3, T4>>.ArgsForCall(int index) {
            return _inner.ArgsForCall(index);
        }


        public ValueTuple<T1, T2, T3, T4> LastArgs {
            get {
                return _inner.LastArgs;
            }
        }

        public ValueTuple<T1, T2, T3, T4> ArgsForCall(int index) {
            return _inner.ArgsForCall(index);
        }
    }

    public partial class TestActionDispatcher {
        public static TestActionDispatcher<T1, T2, T3, T4, T5> Create<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> action) {
            return new TestActionDispatcher<T1, T2, T3, T4, T5>(action);
        }

        public static TestActionDispatcher<T1, T2, T3, T4, T5> Create<T1, T2, T3, T4, T5>() {
            return new TestActionDispatcher<T1, T2, T3, T4, T5>();
        }
    }

    public partial class TestActionDispatcher<T1, T2, T3, T4, T5> : IFuncDispatcher<ValueTuple<T1, T2, T3, T4, T5>> {

        private readonly HOTestFuncDispatcherState<ValueTuple<T1, T2, T3, T4, T5>, Unit> _inner;

        public TestActionDispatcher() : this(null) {
        }

        public TestActionDispatcher(Action<T1, T2, T3, T4, T5> action) {
            if (action == null) {
                action = (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) => {};
            }
            _inner = new HOTestFuncDispatcherState<ValueTuple<T1, T2, T3, T4, T5>, Unit>(CallAdapter(action));
        }

        public IReadOnlyList<CallInfo> Calls {
            get {
                return _inner.Calls.ProjectedTo(c => new CallInfo(c.args, c.dispatchInfo));
            }
        }

        public struct CallInfo {

            public ValueTuple<T1, T2, T3, T4, T5> Args { get; }
            public TestCodeDispatchInfo DispatchInfo { get; }

            public Exception Exception {
                get {
                    return DispatchInfo.Exception;
                }
            }

            internal CallInfo(ValueTuple<T1, T2, T3, T4, T5> args, TestCodeDispatchInfo dispatchInfo) {
                Args = args;
                DispatchInfo = dispatchInfo;
            }
        }

        public int CallCount {
            get {
                return _inner.CallCount;
            }
        }

        public Exception LastException {
            get {
                return _inner.LastException;
            }
        }

        public TestCodeDispatchInfo LastDispatchInfo {
            get {
                return _inner.LastDispatchInfo;
            }
        }

        public bool Called {
            get {
                return _inner.Called;
            }
        }

        public void After(Action<T1, T2, T3, T4, T5> action) {
            _inner.After(ActionAdapter(action));
        }

        public void Before(Action<T1, T2, T3, T4, T5> action) {
            _inner.Before(ActionAdapter(action));
        }

        public void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) {
            _inner.Invoke((arg1, arg2, arg3, arg4, arg5));
        }

        public Action<T1, T2, T3, T4, T5> Handler {
            get {
                return Invoke;
            }
        }

        private Func<ValueTuple<T1, T2, T3, T4, T5>, Unit> CallAdapter(Action<T1, T2, T3, T4, T5> action) {
            return t => {
                action(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5);
                return Unit.Value;
            };
        }

        private Action<ValueTuple<T1, T2, T3, T4, T5>> ActionAdapter(Action<T1, T2, T3, T4, T5> action) {
            return t => action(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5);
        }

        public TestCodeDispatchInfo DispatchInfoForCall(int index) {
            return _inner.DispatchInfoForCall(index);
        }

        public Exception ExceptionForCall(int index) {
            return _inner.ExceptionForCall(index);
        }

        public void Reset() {
            _inner.Reset();
        }

        ValueTuple<T1, T2, T3, T4, T5> IFuncDispatcher<ValueTuple<T1, T2, T3, T4, T5>>.LastArgs {
            get {
                return _inner.LastArgs;
            }
        }

        ValueTuple<T1, T2, T3, T4, T5> IFuncDispatcher<ValueTuple<T1, T2, T3, T4, T5>>.ArgsForCall(int index) {
            return _inner.ArgsForCall(index);
        }


        public ValueTuple<T1, T2, T3, T4, T5> LastArgs {
            get {
                return _inner.LastArgs;
            }
        }

        public ValueTuple<T1, T2, T3, T4, T5> ArgsForCall(int index) {
            return _inner.ArgsForCall(index);
        }


    }


    public partial class TestFuncDispatcher {
        public static TestFuncDispatcher<T1, T2, T3, T4, T5, TResult> Create<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> func) {
            return new TestFuncDispatcher<T1, T2, T3, T4, T5, TResult>(func);
        }

        public static TestFuncDispatcher<T1, T2, T3, T4, T5, TResult> Create<T1, T2, T3, T4, T5, TResult>() {
            return new TestFuncDispatcher<T1, T2, T3, T4, T5, TResult>();
        }
    }


    public partial class TestFuncDispatcher<T1, T2, T3, T4, T5, TResult> : IFuncDispatcher<ValueTuple<T1, T2, T3, T4, T5>> {

        private readonly HOTestFuncDispatcherState<ValueTuple<T1, T2, T3, T4, T5>, TResult> _inner;

        public TestFuncDispatcher() : this(null) {
        }

        public TestFuncDispatcher(Func<T1, T2, T3, T4, T5, TResult> func) {
            if (func == null) {
                func = (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) => default;
            }
            _inner = new HOTestFuncDispatcherState<ValueTuple<T1, T2, T3, T4, T5>, TResult>(CallAdapter(func));
        }

        public IReadOnlyList<CallInfo> Calls {
            get {
                return _inner.Calls.ProjectedTo(c => new CallInfo(c.args, c.result, c.dispatchInfo));
            }
        }

        public struct CallInfo {

            public ValueTuple<T1, T2, T3, T4, T5> Args { get; }
            public TResult Result { get; }
            public TestCodeDispatchInfo DispatchInfo { get; }

            public Exception Exception {
                get {
                    return DispatchInfo.Exception;
                }
            }

            internal CallInfo(ValueTuple<T1, T2, T3, T4, T5> args, TResult result, TestCodeDispatchInfo dispatchInfo) {
                Args = args;
                Result = result;
                DispatchInfo = dispatchInfo;
            }
        }

        public int CallCount {
            get {
                return _inner.CallCount;
            }
        }

        public Exception LastException {
            get {
                return _inner.LastException;
            }
        }

        public TResult LastResult {
            get {
                return _inner.LastResult;
            }
        }

        public TestCodeDispatchInfo LastDispatchInfo {
            get {
                return _inner.LastDispatchInfo;
            }
        }

        public bool Called {
            get {
                return _inner.Called;
            }
        }

        public void After(Action<T1, T2, T3, T4, T5> action) {
            _inner.After(ActionAdapter(action));
        }

        public void Before(Action<T1, T2, T3, T4, T5> action) {
            _inner.Before(ActionAdapter(action));
        }

        public TResult Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) {
            return _inner.Invoke((arg1, arg2, arg3, arg4, arg5));
        }

        public Func<T1, T2, T3, T4, T5, TResult> Handler {
            get {
                return Invoke;
            }
        }

        private Func<ValueTuple<T1, T2, T3, T4, T5>, TResult> CallAdapter(Func<T1, T2, T3, T4, T5, TResult> func) {
            return t => func(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5);
        }

        private Action<ValueTuple<T1, T2, T3, T4, T5>> ActionAdapter(Action<T1, T2, T3, T4, T5> action) {
            return t => action(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5);
        }

        public TestCodeDispatchInfo DispatchInfoForCall(int index) {
            return _inner.DispatchInfoForCall(index);
        }

        public Exception ExceptionForCall(int index) {
            return _inner.ExceptionForCall(index);
        }

        public void Reset() {
            _inner.Reset();
        }

        public TResult ResultForCall(int index) {
            return _inner.ResultForCall(index);
        }

        ValueTuple<T1, T2, T3, T4, T5> IFuncDispatcher<ValueTuple<T1, T2, T3, T4, T5>>.LastArgs {
            get {
                return _inner.LastArgs;
            }
        }

        ValueTuple<T1, T2, T3, T4, T5> IFuncDispatcher<ValueTuple<T1, T2, T3, T4, T5>>.ArgsForCall(int index) {
            return _inner.ArgsForCall(index);
        }


        public ValueTuple<T1, T2, T3, T4, T5> LastArgs {
            get {
                return _inner.LastArgs;
            }
        }

        public ValueTuple<T1, T2, T3, T4, T5> ArgsForCall(int index) {
            return _inner.ArgsForCall(index);
        }
    }

    public partial class TestActionDispatcher {
        public static TestActionDispatcher<T1, T2, T3, T4, T5, T6> Create<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> action) {
            return new TestActionDispatcher<T1, T2, T3, T4, T5, T6>(action);
        }

        public static TestActionDispatcher<T1, T2, T3, T4, T5, T6> Create<T1, T2, T3, T4, T5, T6>() {
            return new TestActionDispatcher<T1, T2, T3, T4, T5, T6>();
        }
    }

    public partial class TestActionDispatcher<T1, T2, T3, T4, T5, T6> : IFuncDispatcher<ValueTuple<T1, T2, T3, T4, T5, T6>> {

        private readonly HOTestFuncDispatcherState<ValueTuple<T1, T2, T3, T4, T5, T6>, Unit> _inner;

        public TestActionDispatcher() : this(null) {
        }

        public TestActionDispatcher(Action<T1, T2, T3, T4, T5, T6> action) {
            if (action == null) {
                action = (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) => {};
            }
            _inner = new HOTestFuncDispatcherState<ValueTuple<T1, T2, T3, T4, T5, T6>, Unit>(CallAdapter(action));
        }

        public IReadOnlyList<CallInfo> Calls {
            get {
                return _inner.Calls.ProjectedTo(c => new CallInfo(c.args, c.dispatchInfo));
            }
        }

        public struct CallInfo {

            public ValueTuple<T1, T2, T3, T4, T5, T6> Args { get; }
            public TestCodeDispatchInfo DispatchInfo { get; }

            public Exception Exception {
                get {
                    return DispatchInfo.Exception;
                }
            }

            internal CallInfo(ValueTuple<T1, T2, T3, T4, T5, T6> args, TestCodeDispatchInfo dispatchInfo) {
                Args = args;
                DispatchInfo = dispatchInfo;
            }
        }

        public int CallCount {
            get {
                return _inner.CallCount;
            }
        }

        public Exception LastException {
            get {
                return _inner.LastException;
            }
        }

        public TestCodeDispatchInfo LastDispatchInfo {
            get {
                return _inner.LastDispatchInfo;
            }
        }

        public bool Called {
            get {
                return _inner.Called;
            }
        }

        public void After(Action<T1, T2, T3, T4, T5, T6> action) {
            _inner.After(ActionAdapter(action));
        }

        public void Before(Action<T1, T2, T3, T4, T5, T6> action) {
            _inner.Before(ActionAdapter(action));
        }

        public void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) {
            _inner.Invoke((arg1, arg2, arg3, arg4, arg5, arg6));
        }

        public Action<T1, T2, T3, T4, T5, T6> Handler {
            get {
                return Invoke;
            }
        }

        private Func<ValueTuple<T1, T2, T3, T4, T5, T6>, Unit> CallAdapter(Action<T1, T2, T3, T4, T5, T6> action) {
            return t => {
                action(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, t.Item6);
                return Unit.Value;
            };
        }

        private Action<ValueTuple<T1, T2, T3, T4, T5, T6>> ActionAdapter(Action<T1, T2, T3, T4, T5, T6> action) {
            return t => action(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, t.Item6);
        }

        public TestCodeDispatchInfo DispatchInfoForCall(int index) {
            return _inner.DispatchInfoForCall(index);
        }

        public Exception ExceptionForCall(int index) {
            return _inner.ExceptionForCall(index);
        }

        public void Reset() {
            _inner.Reset();
        }

        ValueTuple<T1, T2, T3, T4, T5, T6> IFuncDispatcher<ValueTuple<T1, T2, T3, T4, T5, T6>>.LastArgs {
            get {
                return _inner.LastArgs;
            }
        }

        ValueTuple<T1, T2, T3, T4, T5, T6> IFuncDispatcher<ValueTuple<T1, T2, T3, T4, T5, T6>>.ArgsForCall(int index) {
            return _inner.ArgsForCall(index);
        }


        public ValueTuple<T1, T2, T3, T4, T5, T6> LastArgs {
            get {
                return _inner.LastArgs;
            }
        }

        public ValueTuple<T1, T2, T3, T4, T5, T6> ArgsForCall(int index) {
            return _inner.ArgsForCall(index);
        }


    }


    public partial class TestFuncDispatcher {
        public static TestFuncDispatcher<T1, T2, T3, T4, T5, T6, TResult> Create<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> func) {
            return new TestFuncDispatcher<T1, T2, T3, T4, T5, T6, TResult>(func);
        }

        public static TestFuncDispatcher<T1, T2, T3, T4, T5, T6, TResult> Create<T1, T2, T3, T4, T5, T6, TResult>() {
            return new TestFuncDispatcher<T1, T2, T3, T4, T5, T6, TResult>();
        }
    }


    public partial class TestFuncDispatcher<T1, T2, T3, T4, T5, T6, TResult> : IFuncDispatcher<ValueTuple<T1, T2, T3, T4, T5, T6>> {

        private readonly HOTestFuncDispatcherState<ValueTuple<T1, T2, T3, T4, T5, T6>, TResult> _inner;

        public TestFuncDispatcher() : this(null) {
        }

        public TestFuncDispatcher(Func<T1, T2, T3, T4, T5, T6, TResult> func) {
            if (func == null) {
                func = (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) => default;
            }
            _inner = new HOTestFuncDispatcherState<ValueTuple<T1, T2, T3, T4, T5, T6>, TResult>(CallAdapter(func));
        }

        public IReadOnlyList<CallInfo> Calls {
            get {
                return _inner.Calls.ProjectedTo(c => new CallInfo(c.args, c.result, c.dispatchInfo));
            }
        }

        public struct CallInfo {

            public ValueTuple<T1, T2, T3, T4, T5, T6> Args { get; }
            public TResult Result { get; }
            public TestCodeDispatchInfo DispatchInfo { get; }

            public Exception Exception {
                get {
                    return DispatchInfo.Exception;
                }
            }

            internal CallInfo(ValueTuple<T1, T2, T3, T4, T5, T6> args, TResult result, TestCodeDispatchInfo dispatchInfo) {
                Args = args;
                Result = result;
                DispatchInfo = dispatchInfo;
            }
        }

        public int CallCount {
            get {
                return _inner.CallCount;
            }
        }

        public Exception LastException {
            get {
                return _inner.LastException;
            }
        }

        public TResult LastResult {
            get {
                return _inner.LastResult;
            }
        }

        public TestCodeDispatchInfo LastDispatchInfo {
            get {
                return _inner.LastDispatchInfo;
            }
        }

        public bool Called {
            get {
                return _inner.Called;
            }
        }

        public void After(Action<T1, T2, T3, T4, T5, T6> action) {
            _inner.After(ActionAdapter(action));
        }

        public void Before(Action<T1, T2, T3, T4, T5, T6> action) {
            _inner.Before(ActionAdapter(action));
        }

        public TResult Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) {
            return _inner.Invoke((arg1, arg2, arg3, arg4, arg5, arg6));
        }

        public Func<T1, T2, T3, T4, T5, T6, TResult> Handler {
            get {
                return Invoke;
            }
        }

        private Func<ValueTuple<T1, T2, T3, T4, T5, T6>, TResult> CallAdapter(Func<T1, T2, T3, T4, T5, T6, TResult> func) {
            return t => func(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, t.Item6);
        }

        private Action<ValueTuple<T1, T2, T3, T4, T5, T6>> ActionAdapter(Action<T1, T2, T3, T4, T5, T6> action) {
            return t => action(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, t.Item6);
        }

        public TestCodeDispatchInfo DispatchInfoForCall(int index) {
            return _inner.DispatchInfoForCall(index);
        }

        public Exception ExceptionForCall(int index) {
            return _inner.ExceptionForCall(index);
        }

        public void Reset() {
            _inner.Reset();
        }

        public TResult ResultForCall(int index) {
            return _inner.ResultForCall(index);
        }

        ValueTuple<T1, T2, T3, T4, T5, T6> IFuncDispatcher<ValueTuple<T1, T2, T3, T4, T5, T6>>.LastArgs {
            get {
                return _inner.LastArgs;
            }
        }

        ValueTuple<T1, T2, T3, T4, T5, T6> IFuncDispatcher<ValueTuple<T1, T2, T3, T4, T5, T6>>.ArgsForCall(int index) {
            return _inner.ArgsForCall(index);
        }


        public ValueTuple<T1, T2, T3, T4, T5, T6> LastArgs {
            get {
                return _inner.LastArgs;
            }
        }

        public ValueTuple<T1, T2, T3, T4, T5, T6> ArgsForCall(int index) {
            return _inner.ArgsForCall(index);
        }
    }

    public partial class TestActionDispatcher {
        public static TestActionDispatcher<T1, T2, T3, T4, T5, T6, T7> Create<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> action) {
            return new TestActionDispatcher<T1, T2, T3, T4, T5, T6, T7>(action);
        }

        public static TestActionDispatcher<T1, T2, T3, T4, T5, T6, T7> Create<T1, T2, T3, T4, T5, T6, T7>() {
            return new TestActionDispatcher<T1, T2, T3, T4, T5, T6, T7>();
        }
    }

    public partial class TestActionDispatcher<T1, T2, T3, T4, T5, T6, T7> : IFuncDispatcher<ValueTuple<T1, T2, T3, T4, T5, T6, T7>> {

        private readonly HOTestFuncDispatcherState<ValueTuple<T1, T2, T3, T4, T5, T6, T7>, Unit> _inner;

        public TestActionDispatcher() : this(null) {
        }

        public TestActionDispatcher(Action<T1, T2, T3, T4, T5, T6, T7> action) {
            if (action == null) {
                action = (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) => {};
            }
            _inner = new HOTestFuncDispatcherState<ValueTuple<T1, T2, T3, T4, T5, T6, T7>, Unit>(CallAdapter(action));
        }

        public IReadOnlyList<CallInfo> Calls {
            get {
                return _inner.Calls.ProjectedTo(c => new CallInfo(c.args, c.dispatchInfo));
            }
        }

        public struct CallInfo {

            public ValueTuple<T1, T2, T3, T4, T5, T6, T7> Args { get; }
            public TestCodeDispatchInfo DispatchInfo { get; }

            public Exception Exception {
                get {
                    return DispatchInfo.Exception;
                }
            }

            internal CallInfo(ValueTuple<T1, T2, T3, T4, T5, T6, T7> args, TestCodeDispatchInfo dispatchInfo) {
                Args = args;
                DispatchInfo = dispatchInfo;
            }
        }

        public int CallCount {
            get {
                return _inner.CallCount;
            }
        }

        public Exception LastException {
            get {
                return _inner.LastException;
            }
        }

        public TestCodeDispatchInfo LastDispatchInfo {
            get {
                return _inner.LastDispatchInfo;
            }
        }

        public bool Called {
            get {
                return _inner.Called;
            }
        }

        public void After(Action<T1, T2, T3, T4, T5, T6, T7> action) {
            _inner.After(ActionAdapter(action));
        }

        public void Before(Action<T1, T2, T3, T4, T5, T6, T7> action) {
            _inner.Before(ActionAdapter(action));
        }

        public void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) {
            _inner.Invoke((arg1, arg2, arg3, arg4, arg5, arg6, arg7));
        }

        public Action<T1, T2, T3, T4, T5, T6, T7> Handler {
            get {
                return Invoke;
            }
        }

        private Func<ValueTuple<T1, T2, T3, T4, T5, T6, T7>, Unit> CallAdapter(Action<T1, T2, T3, T4, T5, T6, T7> action) {
            return t => {
                action(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, t.Item6, t.Item7);
                return Unit.Value;
            };
        }

        private Action<ValueTuple<T1, T2, T3, T4, T5, T6, T7>> ActionAdapter(Action<T1, T2, T3, T4, T5, T6, T7> action) {
            return t => action(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, t.Item6, t.Item7);
        }

        public TestCodeDispatchInfo DispatchInfoForCall(int index) {
            return _inner.DispatchInfoForCall(index);
        }

        public Exception ExceptionForCall(int index) {
            return _inner.ExceptionForCall(index);
        }

        public void Reset() {
            _inner.Reset();
        }

        ValueTuple<T1, T2, T3, T4, T5, T6, T7> IFuncDispatcher<ValueTuple<T1, T2, T3, T4, T5, T6, T7>>.LastArgs {
            get {
                return _inner.LastArgs;
            }
        }

        ValueTuple<T1, T2, T3, T4, T5, T6, T7> IFuncDispatcher<ValueTuple<T1, T2, T3, T4, T5, T6, T7>>.ArgsForCall(int index) {
            return _inner.ArgsForCall(index);
        }


        public ValueTuple<T1, T2, T3, T4, T5, T6, T7> LastArgs {
            get {
                return _inner.LastArgs;
            }
        }

        public ValueTuple<T1, T2, T3, T4, T5, T6, T7> ArgsForCall(int index) {
            return _inner.ArgsForCall(index);
        }


    }


    public partial class TestFuncDispatcher {
        public static TestFuncDispatcher<T1, T2, T3, T4, T5, T6, T7, TResult> Create<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> func) {
            return new TestFuncDispatcher<T1, T2, T3, T4, T5, T6, T7, TResult>(func);
        }

        public static TestFuncDispatcher<T1, T2, T3, T4, T5, T6, T7, TResult> Create<T1, T2, T3, T4, T5, T6, T7, TResult>() {
            return new TestFuncDispatcher<T1, T2, T3, T4, T5, T6, T7, TResult>();
        }
    }


    public partial class TestFuncDispatcher<T1, T2, T3, T4, T5, T6, T7, TResult> : IFuncDispatcher<ValueTuple<T1, T2, T3, T4, T5, T6, T7>> {

        private readonly HOTestFuncDispatcherState<ValueTuple<T1, T2, T3, T4, T5, T6, T7>, TResult> _inner;

        public TestFuncDispatcher() : this(null) {
        }

        public TestFuncDispatcher(Func<T1, T2, T3, T4, T5, T6, T7, TResult> func) {
            if (func == null) {
                func = (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) => default;
            }
            _inner = new HOTestFuncDispatcherState<ValueTuple<T1, T2, T3, T4, T5, T6, T7>, TResult>(CallAdapter(func));
        }

        public IReadOnlyList<CallInfo> Calls {
            get {
                return _inner.Calls.ProjectedTo(c => new CallInfo(c.args, c.result, c.dispatchInfo));
            }
        }

        public struct CallInfo {

            public ValueTuple<T1, T2, T3, T4, T5, T6, T7> Args { get; }
            public TResult Result { get; }
            public TestCodeDispatchInfo DispatchInfo { get; }

            public Exception Exception {
                get {
                    return DispatchInfo.Exception;
                }
            }

            internal CallInfo(ValueTuple<T1, T2, T3, T4, T5, T6, T7> args, TResult result, TestCodeDispatchInfo dispatchInfo) {
                Args = args;
                Result = result;
                DispatchInfo = dispatchInfo;
            }
        }

        public int CallCount {
            get {
                return _inner.CallCount;
            }
        }

        public Exception LastException {
            get {
                return _inner.LastException;
            }
        }

        public TResult LastResult {
            get {
                return _inner.LastResult;
            }
        }

        public TestCodeDispatchInfo LastDispatchInfo {
            get {
                return _inner.LastDispatchInfo;
            }
        }

        public bool Called {
            get {
                return _inner.Called;
            }
        }

        public void After(Action<T1, T2, T3, T4, T5, T6, T7> action) {
            _inner.After(ActionAdapter(action));
        }

        public void Before(Action<T1, T2, T3, T4, T5, T6, T7> action) {
            _inner.Before(ActionAdapter(action));
        }

        public TResult Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) {
            return _inner.Invoke((arg1, arg2, arg3, arg4, arg5, arg6, arg7));
        }

        public Func<T1, T2, T3, T4, T5, T6, T7, TResult> Handler {
            get {
                return Invoke;
            }
        }

        private Func<ValueTuple<T1, T2, T3, T4, T5, T6, T7>, TResult> CallAdapter(Func<T1, T2, T3, T4, T5, T6, T7, TResult> func) {
            return t => func(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, t.Item6, t.Item7);
        }

        private Action<ValueTuple<T1, T2, T3, T4, T5, T6, T7>> ActionAdapter(Action<T1, T2, T3, T4, T5, T6, T7> action) {
            return t => action(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, t.Item6, t.Item7);
        }

        public TestCodeDispatchInfo DispatchInfoForCall(int index) {
            return _inner.DispatchInfoForCall(index);
        }

        public Exception ExceptionForCall(int index) {
            return _inner.ExceptionForCall(index);
        }

        public void Reset() {
            _inner.Reset();
        }

        public TResult ResultForCall(int index) {
            return _inner.ResultForCall(index);
        }

        ValueTuple<T1, T2, T3, T4, T5, T6, T7> IFuncDispatcher<ValueTuple<T1, T2, T3, T4, T5, T6, T7>>.LastArgs {
            get {
                return _inner.LastArgs;
            }
        }

        ValueTuple<T1, T2, T3, T4, T5, T6, T7> IFuncDispatcher<ValueTuple<T1, T2, T3, T4, T5, T6, T7>>.ArgsForCall(int index) {
            return _inner.ArgsForCall(index);
        }


        public ValueTuple<T1, T2, T3, T4, T5, T6, T7> LastArgs {
            get {
                return _inner.LastArgs;
            }
        }

        public ValueTuple<T1, T2, T3, T4, T5, T6, T7> ArgsForCall(int index) {
            return _inner.ArgsForCall(index);
        }
    }
}
