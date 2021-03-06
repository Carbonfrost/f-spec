//
// Copyright 2017, 2018-2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Reflection;
using System.Threading.Tasks;
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.Commons.Spec {

    partial class Matchers {

        public static ThrowsMatcher Throw<T>() where T : Exception {
            return new ThrowsMatcher(typeof(T));
        }

        public static ThrowsMatcher Throw() {
            return new ThrowsMatcher(typeof(Exception));
        }

        public static ThrowsMatcher Throw(Type type) {
            return new ThrowsMatcher(type);
        }

    }

    partial class Extensions {

        public static void Throw<TException>(this IExpectation e) where TException : Exception{
            Throw<TException>(e, null);
        }

        public static void Throw(this IExpectation e) {
            Throw(e, null);
        }

        public static void Throw<TException>(this IExpectation e, string message, params object[] args) where TException : Exception{
            e.Like(Matchers.Throw<TException>(), message, (object[]) args);
        }

        public static void Throw(this IExpectation e, string message, params object[] args) {
            e.Like(Matchers.Throw(), message, (object[]) args);
        }

        public static void Exception<TException>(this IExceptionExpectation e) where TException : Exception{
            Exception<TException>(e, null);
        }

        public static void Exception(this IExceptionExpectation e) {
            Exception(e, null);
        }

        public static void Exception<TException>(this IExceptionExpectation e, string message, params object[] args) where TException : Exception{
            e.Like(Matchers.Throw<TException>(), message, (object[]) args);
        }

        public static void Exception(this IExceptionExpectation e, string message, params object[] args) {
            e.Like(Matchers.Throw(), message, (object[]) args);
        }

    }

    partial class Asserter {

        public void DoesNotThrow(Action action) {
            NotThat(action, Matchers.Throw());
        }

        public void DoesNotThrow(Action action, string message, params object[] args) {
            NotThat(action, Matchers.Throw(), message, args);
        }

        public void DoesNotThrow<T>(Task task) where T : Exception {
            DoesNotThrow(typeof(T), task);
        }

        public void DoesNotThrow<T>(Action action) where T : Exception {
            DoesNotThrow(typeof(T), action);
        }

        public void DoesNotThrow<T>(Func<object> func) where T : Exception {
            Action act = () => func();
            DoesNotThrow(typeof(T), act);
        }

        public void DoesNotThrow(Type exceptionType, Func<object> func) {
            Action act = () => func();
            DoesNotThrow(exceptionType, act);
        }

        public void DoesNotThrow(Type exceptionType, Task task) {
            Action act = task.Wait;
            DoesNotThrow(exceptionType, act);
        }

        public void DoesNotThrow(Type exceptionType, Action action) {
            NotThat(action, Matchers.Throw(exceptionType));
        }

        public void DoesNotThrow<T>(Task task, string message, params object[] args) where T : Exception {
            DoesNotThrow(typeof(T), task, message, args);
        }

        public void DoesNotThrow<T>(Action action, string message, params object[] args) where T : Exception {
            DoesNotThrow(typeof(T), action, message, args);
        }

        public void DoesNotThrow<T>(Func<object> func, string message, params object[] args) where T : Exception {
            Action act = () => func();
            DoesNotThrow(typeof(T), act, message, args);
        }

        public void DoesNotThrow(Type exceptionType, Func<object> func, string message, params object[] args) {
            Action act = () => func();
            DoesNotThrow(exceptionType, act, message, args);
        }

        public void DoesNotThrow(Type exceptionType, Task task, string message, params object[] args) {
            Action act = task.Wait;
            DoesNotThrow(exceptionType, act, message, args);
        }

        public void DoesNotThrow(Type exceptionType, Action action, string message, params object[] args) {
            NotThat(action, Matchers.Throw(exceptionType), message, args);
        }

        public void Throws<T>(Task task) where T : Exception {
            Throws(typeof(T), task);
        }

        public void Throws<T>(Action action) where T : Exception {
            Throws(typeof(T), action);
        }

        public void Throws<T>(Func<object> func) where T : Exception {
            Action act = () => func();
            Throws(typeof(T), act);
        }

        public void Throws(Type exceptionType, Func<object> func) {
            Action act = () => func();
            Throws(exceptionType, act);
        }

        public void Throws(Type exceptionType, Task task) {
            Action act = task.Wait;
            Throws(exceptionType, act);
        }

        public void Throws(Type exceptionType, Action action) {
            That(action, Matchers.Throw(exceptionType));
        }

        public void Throws<T>(Task task, string message, params object[] args) where T : Exception {
            Throws(typeof(T), task, message, args);
        }

        public void Throws<T>(Action action, string message, params object[] args) where T : Exception {
            Throws(typeof(T), action, message, args);
        }

        public void Throws<T>(Func<object> func, string message, params object[] args) where T : Exception {
            Action act = () => func();
            Throws(typeof(T), act, message, args);
        }

        public void Throws(Type exceptionType, Func<object> func, string message, params object[] args) {
            Action act = () => func();
            Throws(exceptionType, act, message, args);
        }

        public void Throws(Type exceptionType, Task task, string message, params object[] args) {
            Action act = task.Wait;
            Throws(exceptionType, act, message, args);
        }

        public void Throws(Type exceptionType, Action action, string message, params object[] args) {
            That(action, Matchers.Throw(exceptionType), message, args);
        }
    }

    partial class Assert {

        public static void DoesNotThrow(Action action) {
            Global.DoesNotThrow(action);
        }

        public static void DoesNotThrow(Action action, string message, params object[] args) {
            Global.DoesNotThrow(action, message, (object[]) args);
        }

        public static void DoesNotThrow<T>(Task task) where T : Exception {
            Global.DoesNotThrow<T>(task);
        }

        public static void DoesNotThrow<T>(Action action) where T : Exception {
            Global.DoesNotThrow<T>(action);
        }

        public static void DoesNotThrow<T>(Func<object> func) where T : Exception {
            Global.DoesNotThrow<T>(func);
        }

        public static void DoesNotThrow(Type exceptionType, Func<object> func) {
            Global.DoesNotThrow(exceptionType, func);
        }

        public static void DoesNotThrow(Type exceptionType, Task task) {
            Global.DoesNotThrow(exceptionType, task);
        }

        public static void DoesNotThrow(Type exceptionType, Action action) {
            Global.DoesNotThrow(exceptionType, action);
        }

        public static void DoesNotThrow<T>(Task task, string message, params object[] args) where T : Exception {
            Global.DoesNotThrow<T>(task, message, (object[]) args);
        }

        public static void DoesNotThrow<T>(Action action, string message, params object[] args) where T : Exception {
            Global.DoesNotThrow<T>(action, message, (object[]) args);
        }

        public static void DoesNotThrow<T>(Func<object> func, string message, params object[] args) where T : Exception {
            Global.DoesNotThrow<T>(func, message, (object[]) args);
        }

        public static void DoesNotThrow(Type exceptionType, Func<object> func, string message, params object[] args) {
            Global.DoesNotThrow(exceptionType, func, message, (object[]) args);
        }

        public static void DoesNotThrow(Type exceptionType, Task task, string message, params object[] args) {
            Global.DoesNotThrow(exceptionType, task, message, (object[]) args);
        }

        public static void DoesNotThrow(Type exceptionType, Action action, string message, params object[] args) {
            Global.DoesNotThrow(exceptionType, action, message, (object[]) args);
        }

        public static void Throws<T>(Task task) where T : Exception {
            Global.Throws<T>(task);
        }

        public static void Throws<T>(Action action) where T : Exception {
            Global.Throws<T>(action);
        }

        public static void Throws<T>(Func<object> func) where T : Exception {
            Global.Throws<T>(func);
        }

        public static void Throws(Type exceptionType, Func<object> func) {
            Global.Throws(exceptionType, func);
        }

        public static void Throws(Type exceptionType, Task task) {
            Global.Throws(exceptionType, task);
        }

        public static void Throws(Type exceptionType, Action action) {
            Global.Throws(exceptionType, action);
        }

        public static void Throws<T>(Task task, string message, params object[] args) where T : Exception {
            Global.Throws<T>(task, message, (object[]) args);
        }

        public static void Throws<T>(Action action, string message, params object[] args) where T : Exception {
            Global.Throws<T>(action, message, (object[]) args);
        }

        public static void Throws<T>(Func<object> func, string message, params object[] args) where T : Exception {
            Global.Throws<T>(func, message, (object[]) args);
        }

        public static void Throws(Type exceptionType, Func<object> func, string message, params object[] args) {
            Global.Throws(exceptionType, func, message, (object[]) args);
        }

        public static void Throws(Type exceptionType, Task task, string message, params object[] args) {
            Global.Throws(exceptionType, task, message, (object[]) args);
        }

        public static void Throws(Type exceptionType, Action action, string message, params object[] args) {
            Global.Throws(exceptionType, action, message, (object[]) args);
        }
    }

    partial class Assume {

        public static void DoesNotThrow(Action action) {
            Global.DoesNotThrow(action);
        }

        public static void DoesNotThrow(Action action, string message, params object[] args) {
            Global.DoesNotThrow(action, message, (object[]) args);
        }

        public static void DoesNotThrow<T>(Task task) where T : Exception {
            Global.DoesNotThrow<T>(task);
        }

        public static void DoesNotThrow<T>(Action action) where T : Exception {
            Global.DoesNotThrow<T>(action);
        }

        public static void DoesNotThrow<T>(Func<object> func) where T : Exception {
            Global.DoesNotThrow<T>(func);
        }

        public static void DoesNotThrow(Type exceptionType, Func<object> func) {
            Global.DoesNotThrow(exceptionType, func);
        }

        public static void DoesNotThrow(Type exceptionType, Task task) {
            Global.DoesNotThrow(exceptionType, task);
        }

        public static void DoesNotThrow(Type exceptionType, Action action) {
            Global.DoesNotThrow(exceptionType, action);
        }

        public static void DoesNotThrow<T>(Task task, string message, params object[] args) where T : Exception {
            Global.DoesNotThrow<T>(task, message, (object[]) args);
        }

        public static void DoesNotThrow<T>(Action action, string message, params object[] args) where T : Exception {
            Global.DoesNotThrow<T>(action, message, (object[]) args);
        }

        public static void DoesNotThrow<T>(Func<object> func, string message, params object[] args) where T : Exception {
            Global.DoesNotThrow<T>(func, message, (object[]) args);
        }

        public static void DoesNotThrow(Type exceptionType, Func<object> func, string message, params object[] args) {
            Global.DoesNotThrow(exceptionType, func, message, (object[]) args);
        }

        public static void DoesNotThrow(Type exceptionType, Task task, string message, params object[] args) {
            Global.DoesNotThrow(exceptionType, task, message, (object[]) args);
        }

        public static void DoesNotThrow(Type exceptionType, Action action, string message, params object[] args) {
            Global.DoesNotThrow(exceptionType, action, message, (object[]) args);
        }

        public static void Throws<T>(Task task) where T : Exception {
            Global.Throws<T>(task);
        }

        public static void Throws<T>(Action action) where T : Exception {
            Global.Throws<T>(action);
        }

        public static void Throws<T>(Func<object> func) where T : Exception {
            Global.Throws<T>(func);
        }

        public static void Throws(Type exceptionType, Func<object> func) {
            Global.Throws(exceptionType, func);
        }

        public static void Throws(Type exceptionType, Task task) {
            Global.Throws(exceptionType, task);
        }

        public static void Throws(Type exceptionType, Action action) {
            Global.Throws(exceptionType, action);
        }

        public static void Throws<T>(Task task, string message, params object[] args) where T : Exception {
            Global.Throws<T>(task, message, (object[]) args);
        }

        public static void Throws<T>(Action action, string message, params object[] args) where T : Exception {
            Global.Throws<T>(action, message, (object[]) args);
        }

        public static void Throws<T>(Func<object> func, string message, params object[] args) where T : Exception {
            Global.Throws<T>(func, message, (object[]) args);
        }

        public static void Throws(Type exceptionType, Func<object> func, string message, params object[] args) {
            Global.Throws(exceptionType, func, message, (object[]) args);
        }

        public static void Throws(Type exceptionType, Task task, string message, params object[] args) {
            Global.Throws(exceptionType, task, message, (object[]) args);
        }

        public static void Throws(Type exceptionType, Action action, string message, params object[] args) {
            Global.Throws(exceptionType, action, message, (object[]) args);
        }
    }

    namespace TestMatchers {

        public class ThrowsMatcher : ITestMatcher {

            private readonly RecordExceptionFlags _flags;

            public Type Expected {
                get;
                private set;
            }

            [MatcherUserData(Hidden = true)]
            public ThrowsMatcher UnwindingTargetExceptions {
                get {
                    return WithFlags(_flags | RecordExceptionFlags.UnwindTargetExceptions);
                }
            }

            public RecordExceptionFlags Flags {
                get {
                    return _flags;
                }
            }

            public ThrowsMatcher(Type expected = null) : this(expected, RecordExceptionFlags.None) {}

            private ThrowsMatcher(Type expected, RecordExceptionFlags flags) {
                Expected = expected ?? typeof(Exception);
                _flags = flags;
            }

            public bool Matches(ITestActualEvaluation actual) {
                if (actual == null) {
                    throw new ArgumentNullException(nameof(actual));
                }
                var ex = Record.ApplyFlags(actual.Exception, _flags);
                return Expected.GetTypeInfo().IsInstanceOfType(ex);
            }

            public ThrowsMatcher WithFlags(RecordExceptionFlags flags) {
                return new ThrowsMatcher(Expected, flags);
            }
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class ThrowsAttribute : Attribute, ITestMatcherFactory {

        private readonly Type _exceptionType;

        public string Message { get; set; }

        public Type ExceptionType {
            get {
                return _exceptionType;
            }
        }

        public ThrowsAttribute(Type exceptionType) {
            _exceptionType = exceptionType;
        }

        ITestMatcher ITestMatcherFactory.CreateMatcher(TestContext testContext) {
            var flags = RecordExceptionFlags.UnwindTargetExceptions;

            if (testContext.ShouldVerify) {
                flags |= RecordExceptionFlags.StrictVerification;
            }
            return  Matchers.Throw(ExceptionType).WithFlags(flags);
        }
    }
}
