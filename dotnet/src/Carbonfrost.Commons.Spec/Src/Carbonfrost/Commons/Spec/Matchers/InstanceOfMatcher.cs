//
// Copyright 2018-2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Linq;
using System.Reflection;
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.Commons.Spec {

    partial class Matchers {

        public static InstanceOfMatcher BeInstanceOf(Type expected) {
            return new InstanceOfMatcher(expected);
        }

        public static InstanceOfMatcher BeInstanceOf<T>() {
            return new InstanceOfMatcher(typeof(T));
        }

    }

    partial class Asserter {

        public void IsInstanceOf(Type expected, object actual) {
            That(actual, Matchers.BeInstanceOf(expected));
        }

        public void IsInstanceOf(Type expected, object actual, string message, params object[] args) {
            That(actual, Matchers.BeInstanceOf(expected), message, args);
        }

        public void IsInstanceOf<T>(object actual) {
            That(actual, Matchers.BeInstanceOf(typeof(T)));
        }

        public void IsInstanceOf<T>(object actual, string message, params object[] args) {
            That(actual, Matchers.BeInstanceOf(typeof(T)), message, args);
        }

        public void IsNotInstanceOf(Type expected, object actual) {
            NotThat(actual, Matchers.BeInstanceOf(expected));
        }

        public void IsNotInstanceOf(Type expected, object actual, string message, params object[] args) {
            NotThat(actual, Matchers.BeInstanceOf(expected), message, args);
        }

        public void IsNotInstanceOf<T>(object actual) {
            NotThat(actual, Matchers.BeInstanceOf(typeof(T)));
        }

        public void IsNotInstanceOf<T>(object actual, string message, params object[] args) {
            NotThat(actual, Matchers.BeInstanceOf(typeof(T)), message, args);
        }

    }

    partial class Assert {

        public static void IsInstanceOf(Type expected, object actual) {
            Global.IsInstanceOf(expected, actual);
        }

        public static void IsInstanceOf(Type expected, object actual, string message, params object[] args) {
            Global.IsInstanceOf(expected, actual, message, (object[]) args);
        }

        public static void IsInstanceOf<T>(object actual) {
            Global.IsInstanceOf<T>(actual);
        }

        public static void IsInstanceOf<T>(object actual, string message, params object[] args) {
            Global.IsInstanceOf<T>(actual, message, (object[]) args);
        }

        public static void IsNotInstanceOf(Type expected, object actual) {
            Global.IsNotInstanceOf(expected, actual);
        }

        public static void IsNotInstanceOf(Type expected, object actual, string message, params object[] args) {
            Global.IsNotInstanceOf(expected, actual, message, (object[]) args);
        }

        public static void IsNotInstanceOf<T>(object actual) {
            Global.IsNotInstanceOf<T>(actual);
        }

        public static void IsNotInstanceOf<T>(object actual, string message, params object[] args) {
            Global.IsNotInstanceOf<T>(actual, message, (object[]) args);
        }

    }

    partial class Assume {

        public static void IsInstanceOf(Type expected, object actual) {
            Global.IsInstanceOf(expected, actual);
        }

        public static void IsInstanceOf(Type expected, object actual, string message, params object[] args) {
            Global.IsInstanceOf(expected, actual, message, (object[]) args);
        }

        public static void IsInstanceOf<T>(object actual) {
            Global.IsInstanceOf<T>(actual);
        }

        public static void IsInstanceOf<T>(object actual, string message, params object[] args) {
            Global.IsInstanceOf<T>(actual, message, (object[]) args);
        }

        public static void IsNotInstanceOf(Type expected, object actual) {
            Global.IsNotInstanceOf(expected, actual);
        }

        public static void IsNotInstanceOf(Type expected, object actual, string message, params object[] args) {
            Global.IsNotInstanceOf(expected, actual, message, (object[]) args);
        }

        public static void IsNotInstanceOf<T>(object actual) {
            Global.IsNotInstanceOf<T>(actual);
        }

        public static void IsNotInstanceOf<T>(object actual, string message, params object[] args) {
            Global.IsNotInstanceOf<T>(actual, message, (object[]) args);
        }

    }


    partial class Extensions {

        public static void InstanceOf<T>(this IExpectation<T> e, Type expected) {
            e.As<object>().Like(Matchers.BeInstanceOf(expected));
        }

        public static void InstanceOf<T>(this IExpectation<T> e, Type expected, string message, params object[] args) {
            e.As<object>().Like(Matchers.BeInstanceOf(expected), message, args);
        }
    }

    namespace TestMatchers {

        public class InstanceOfMatcher : TestMatcher<object>, ITestMatcherValidations {

            private readonly TestMatcherValidations _validations;

            public Type Expected {
                get;
            }

            [MatcherUserData]
            private bool _ShowActualTypes {
                get {
                    return true;
                }
            }

            object ITestMatcherValidations.AllowingNullActualValue() {
                return AllowingNullActualValue();
            }

            public InstanceOfMatcher AllowingNullActualValue() {
                return new InstanceOfMatcher(Expected, _validations.AllowingNullActualValue());
            }

            public InstanceOfMatcher(Type expected) : this(expected, TestMatcherValidations.None) {
            }

            private InstanceOfMatcher(Type expected, TestMatcherValidations validations) {
                if (expected == null) {
                    throw new ArgumentNullException(nameof(expected));
                }

                Expected = expected;
                _validations = validations;
            }

            public override bool Matches(object actual) {
                _validations.ValidateActual(actual);

                return Expected.GetTypeInfo().IsInstanceOfType(actual);
            }
        }
    }
}
