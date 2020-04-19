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

        public static ReferenceTypeMatcher BeReferenceType() {
            return new ReferenceTypeMatcher();
        }

    }

    partial class Asserter {

        public void IsReferenceType(object actual) {
            That(actual, Matchers.BeReferenceType());
        }

        public void IsReferenceType(object actual, string message, params object[] args) {
            That(actual, Matchers.BeReferenceType(), message, args);
        }

        public void IsNotReferenceType(object actual) {
            NotThat(actual, Matchers.BeReferenceType());
        }

        public void IsNotReferenceType(object actual, string message, params object[] args) {
            NotThat(actual, Matchers.BeReferenceType(), message, args);
        }

    }

    partial class Assert {

        public static void IsReferenceType(object actual) {
            Global.IsReferenceType(actual);
        }

        public static void IsReferenceType(object actual, string message, params object[] args) {
            Global.IsReferenceType(actual, message, (object[]) args);
        }

        public static void IsNotReferenceType(object actual) {
            Global.IsNotReferenceType(actual);
        }

        public static void IsNotReferenceType(object actual, string message, params object[] args) {
            Global.IsNotReferenceType(actual, message, (object[]) args);
        }

    }

    partial class Assume {

        public static void IsReferenceType(object actual) {
            Global.IsReferenceType(actual);
        }

        public static void IsReferenceType(object actual, string message, params object[] args) {
            Global.IsReferenceType(actual, message, (object[]) args);
        }

        public static void IsNotReferenceType(object actual) {
            Global.IsNotReferenceType(actual);
        }

        public static void IsNotReferenceType(object actual, string message, params object[] args) {
            Global.IsNotReferenceType(actual, message, (object[]) args);
        }

    }


    partial class Extensions {

        public static void ReferenceType<T>(this Expectation<T> e) {
            e.As<object>().Should(Matchers.BeReferenceType());
        }

        public static void ReferenceType<T>(this Expectation<T> e, string message, params object[] args) {
            e.As<object>().Should(Matchers.BeReferenceType(), message, args);
        }

    }

    namespace TestMatchers {

        public class ReferenceTypeMatcher : TestMatcher<object> {

            public override bool Matches(object actual) {
                if (actual == null) {
                    return true;
                }

                return !actual.GetType().IsValueType;
            }
        }
    }

}
