//
// Copyright 2018-2019 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Collections;
using System.Linq;
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.Commons.Spec {

    partial class Matchers {

        public static HaveLengthMatcher HaveLength(int length) {
            return new HaveLengthMatcher(length);
        }

    }

    partial class Asserter {

        public void HasLength(int length, Array array) {
            That(array, Matchers.HaveLength(length));
        }

        public void HasLength(int length, Array array, string message, params object[] args) {
            That(array, Matchers.HaveLength(length), message, args);
        }

        public void HasLength(int length, string str) {
            That(str, Matchers.HaveLength(length));
        }

        public void HasLength(int length, string str, string message, params object[] args) {
            That(str, Matchers.HaveLength(length), message, args);
        }

        public void DoesNotHaveLength(int length, Array array) {
            NotThat(array, Matchers.HaveLength(length));
        }

        public void DoesNotHaveLength(int length, Array array, string message, params object[] args) {
            NotThat(array, Matchers.HaveLength(length), message, args);
        }

        public void DoesNotHaveLength(int length, string str) {
            NotThat(str, Matchers.HaveLength(length));
        }

        public void DoesNotHaveLength(int length, string str, string message, params object[] args) {
            NotThat(str, Matchers.HaveLength(length), message, args);
        }
    }

    partial class Assert {

        public static void HasLength(int length, Array array) {
            Global.HasLength(length, array);
        }

        public static void HasLength(int length, Array array, string message, params object[] args) {
            Global.HasLength(length, array, message, (object[]) args);
        }

        public static void HasLength(int length, string str) {
            Global.HasLength(length, str);
        }

        public static void HasLength(int length, string str, string message, params object[] args) {
            Global.HasLength(length, str, message, (object[]) args);
        }

        public static void DoesNotHaveLength(int length, Array array) {
            Global.DoesNotHaveLength(length, array);
        }

        public static void DoesNotHaveLength(int length, Array array, string message, params object[] args) {
            Global.DoesNotHaveLength(length, array, message, (object[]) args);
        }

        public static void DoesNotHaveLength(int length, string str) {
            Global.DoesNotHaveLength(length, str);
        }

        public static void DoesNotHaveLength(int length, string str, string message, params object[] args) {
            Global.DoesNotHaveLength(length, str, message, (object[]) args);
        }
    }

    partial class Assume {

        public static void HasLength(int length, Array array) {
            Global.HasLength(length, array);
        }

        public static void HasLength(int length, Array array, string message, params object[] args) {
            Global.HasLength(length, array, message, (object[]) args);
        }

        public static void HasLength(int length, string str) {
            Global.HasLength(length, str);
        }

        public static void HasLength(int length, string str, string message, params object[] args) {
            Global.HasLength(length, str, message, (object[]) args);
        }

        public static void DoesNotHaveLength(int length, Array array) {
            Global.DoesNotHaveLength(length, array);
        }

        public static void DoesNotHaveLength(int length, Array array, string message, params object[] args) {
            Global.DoesNotHaveLength(length, array, message, (object[]) args);
        }

        public static void DoesNotHaveLength(int length, string str) {
            Global.DoesNotHaveLength(length, str);
        }

        public static void DoesNotHaveLength(int length, string str, string message, params object[] args) {
            Global.DoesNotHaveLength(length, str, message, (object[]) args);
        }
    }


    partial class Extensions {

        public static void Length<T>(this Expectation<T> e, int length) {
            Length<T>(e, length, null);
        }

        public static void Length(this EnumerableExpectation e, int length) {
            Length(e, length, null);
        }

        public static void Length<TSource>(this EnumerableExpectation<TSource> e, int length) {
            Length<TSource>(e, length, null);
        }

        public static void Length<T>(this Expectation<T> e, int length, string message, params object[] args) {
            e.As<IEnumerable>().Should(Matchers.HaveLength(length), message, (object[]) args);
        }

        public static void Length(this EnumerableExpectation e, int length, string message, params object[] args) {
            e.Self.Should(Matchers.HaveLength(length), message, (object[]) args);
        }

        public static void Length<TSource>(this EnumerableExpectation<TSource> e, int length, string message, params object[] args) {
            e.Self.Should(Matchers.HaveLength(length), message, (object[]) args);
        }

    }

    namespace TestMatchers {

        public class HaveLengthMatcher : TestMatcher<IEnumerable> {

            public int Expected { get; private set; }

            public HaveLengthMatcher(int expected) {
                Expected = expected;
            }

            public override bool Matches(IEnumerable actual) {
                if (actual == null) {
                    throw new ArgumentNullException("actual");
                }
                if (actual is Array) {
                    return ((Array) actual).Length == Expected;
                }
                if (actual is string) {
                    return ((string) actual).Length == Expected;
                }
                throw SpecFailure.HaveLengthWorksWith(actual.GetType());
            }

        }
    }

}
