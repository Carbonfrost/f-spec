//
// Copyright 2017, 2018-2019 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.Commons.Spec {

    partial class Matchers {

        public static SameMatcher BeSameAs(object value) {
            return new SameMatcher(value);
        }
    }

    partial class Asserter {

        public void Same(object expected, object actual) {
            That(actual, Matchers.BeSameAs(expected));
        }

        public void Same(object expected, object actual, string message, params object[] args) {
            That(actual, Matchers.BeSameAs(expected), message, args);
        }

        public void NotSame(object expected, object actual) {
            NotThat(actual, Matchers.BeSameAs(expected));
        }

        public void NotSame(object expected, object actual, string message, params object[] args) {
            NotThat(actual, Matchers.BeSameAs(expected), message, args);
        }

    }

    partial class Assert {

        public static void Same(object expected, object actual) {
            Global.Same(expected, actual);
        }

        public static void Same(object expected, object actual, string message, params object[] args) {
            Global.Same(expected, actual, message, (object[]) args);
        }

        public static void NotSame(object expected, object actual) {
            Global.NotSame(expected, actual);
        }

        public static void NotSame(object expected, object actual, string message, params object[] args) {
            Global.NotSame(expected, actual, message, (object[]) args);
        }

    }

    partial class Assume {

        public static void Same(object expected, object actual) {
            Global.Same(expected, actual);
        }

        public static void Same(object expected, object actual, string message, params object[] args) {
            Global.Same(expected, actual, message, (object[]) args);
        }

        public static void NotSame(object expected, object actual) {
            Global.NotSame(expected, actual);
        }

        public static void NotSame(object expected, object actual, string message, params object[] args) {
            Global.NotSame(expected, actual, message, (object[]) args);
        }

    }

    partial class Extensions {

        public static void SameAs<T>(this Expectation<T> e, object other) where T : class {
            SameAs(e, other, null);
        }

        public static void SameAs<T>(this Expectation<T> e, object other, string message, params object[] args) where T : class {
            e.Should(Matchers.BeSameAs(other), message, (object[]) args);
        }

    }

    namespace TestMatchers {

        public class SameMatcher : TestMatcher<object> {

            public object Expected { get; private set; }

            public SameMatcher(object expected) {
                Expected = expected;
            }

            public override bool Matches(object actual) {
                return ReferenceEquals(actual, Expected);
            }

        }
    }

}
