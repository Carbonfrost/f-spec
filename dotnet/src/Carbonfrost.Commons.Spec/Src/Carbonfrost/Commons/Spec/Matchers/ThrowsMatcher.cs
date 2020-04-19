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

        public static void Throw<TException>(this Expectation e) where TException : Exception{
            Throw<TException>(e, null);
        }

        public static void Throw(this Expectation e) {
            Throw(e, null);
        }

        public static void Throw<TException>(this Expectation e, string message, params object[] args) where TException : Exception{
            e.Should(Matchers.Throw<TException>(), message, (object[]) args);
        }

        public static void Throw(this Expectation e, string message, params object[] args) {
            e.Should(Matchers.Throw(), message, (object[]) args);
        }

        public static void Exception<TException>(this ExceptionExpectation e) where TException : Exception{
            Exception<TException>(e, null);
        }

        public static void Exception(this ExceptionExpectation e) {
            Exception(e, null);
        }

        public static void Exception<TException>(this ExceptionExpectation e, string message, params object[] args) where TException : Exception{
            e.Should(Matchers.Throw<TException>(), message, (object[]) args);
        }

        public static void Exception(this ExceptionExpectation e, string message, params object[] args) {
            e.Should(Matchers.Throw(), message, (object[]) args);
        }

    }

    partial class Asserter {

        public void DoesNotThrow(Action action) {
            NotThat(action, Matchers.Throw());
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

        public void DoesNotThrow(Action action, string message, params object[] args) {
            NotThat(action, Matchers.Throw(), message, args);
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

        public static void DoesNotThrow(Action action, string message, params object[] args) {
            Global.DoesNotThrow(action, message, (object[]) args);
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

        public static void DoesNotThrow(Action action, string message, params object[] args) {
            Global.DoesNotThrow(action, message, (object[]) args);
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

        public class ThrowsMatcher : ITestMatcher, ITestMatcherActualException {

            private readonly RecordExceptionFlags _flags;
            private Exception _actualException;

            public Type Expected { get; private set; }

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

            public bool Matches(Action testCode) {
                var ex = Record.Exception(testCode, _flags);
                _actualException = ex;
                return Expected.GetTypeInfo().IsInstanceOfType(ex);
            }

            // HACK We make this available so that it can be reported.
            // Really, this means that the ThrowsMatcher is no longer reusable
            // because we carry this state.

            Exception ITestMatcherActualException.ActualException {
                get {
                    return _actualException;
                }
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
