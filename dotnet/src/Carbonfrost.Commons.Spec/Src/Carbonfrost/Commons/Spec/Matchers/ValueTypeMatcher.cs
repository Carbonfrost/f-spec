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
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.Commons.Spec {

    partial class Matchers {

        public static ValueTypeMatcher BeValueType() {
            return new ValueTypeMatcher();
        }

    }

    partial class Asserter {

        public void IsValueType(object actual) {
            That(actual, Matchers.BeValueType());
        }

        public void IsValueType(object actual, string message, params object[] args) {
            That(actual, Matchers.BeValueType(), message, args);
        }

        public void IsNotValueType(object actual) {
            NotThat(actual, Matchers.BeValueType());
        }

        public void IsNotValueType(object actual, string message, params object[] args) {
            NotThat(actual, Matchers.BeValueType(), message, args);
        }

    }

    partial class Assert {

        public static void IsValueType(object actual) {
            Global.IsValueType(actual);
        }

        public static void IsValueType(object actual, string message, params object[] args) {
            Global.IsValueType(actual, message, (object[]) args);
        }

        public static void IsNotValueType(object actual) {
            Global.IsNotValueType(actual);
        }

        public static void IsNotValueType(object actual, string message, params object[] args) {
            Global.IsNotValueType(actual, message, (object[]) args);
        }

    }

    partial class Assume {

        public static void IsValueType(object actual) {
            Global.IsValueType(actual);
        }

        public static void IsValueType(object actual, string message, params object[] args) {
            Global.IsValueType(actual, message, (object[]) args);
        }

        public static void IsNotValueType(object actual) {
            Global.IsNotValueType(actual);
        }

        public static void IsNotValueType(object actual, string message, params object[] args) {
            Global.IsNotValueType(actual, message, (object[]) args);
        }

    }


    partial class Extensions {

        public static void ValueType<T>(this Expectation<T> e) {
            e.As<object>().Should(Matchers.BeValueType());
        }

        public static void ValueType<T>(this Expectation<T> e, string message, params object[] args) {
            e.As<object>().Should(Matchers.BeValueType(), message, args);
        }

    }

    namespace TestMatchers {

        public class ValueTypeMatcher : TestMatcher<object> {

            public override bool Matches(object actual) {
                if (actual == null) {
                    return false;
                }

                return actual.GetType().IsValueType;
            }
        }
    }

}
