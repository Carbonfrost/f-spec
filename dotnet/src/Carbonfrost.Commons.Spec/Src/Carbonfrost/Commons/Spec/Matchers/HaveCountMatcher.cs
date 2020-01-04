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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.Commons.Spec {

    partial class Matchers {

        public static HaveCountMatcher HaveCount(int count) {
            return new HaveCountMatcher(count);
        }

        public static HaveCountMatcher<TSource> HaveCount<TSource>(int count, Predicate<TSource> predicate) {
            return new HaveCountMatcher<TSource>(count, predicate);
        }

    }

    partial class Asserter {

        public void HasCount(int count, IEnumerable collection) {
            That(collection, Matchers.HaveCount(count));
        }

        public void HasCount(int count, IEnumerable collection, string message, params object[] args) {
            That(collection, Matchers.HaveCount(count), message, (object[]) args);
        }

        public void DoesNotHaveCount(int count, IEnumerable collection) {
            NotThat(collection, Matchers.HaveCount(count));
        }

        public void DoesNotHaveCount(int count, IEnumerable collection, string message, params object[] args) {
            NotThat(collection, Matchers.HaveCount(count), message, (object[]) args);
        }

        public void HasCount<TSource>(int count, Predicate<TSource> predicate, IEnumerable<TSource> collection) {
            That(collection, Matchers.HaveCount<TSource>(count, predicate));
        }

        public void HasCount<TSource>(int count, Predicate<TSource> predicate, IEnumerable<TSource> collection, string message, params object[] args) {
            That(collection, Matchers.HaveCount<TSource>(count, predicate), message, (object[]) args);
        }

        public void DoesNotHaveCount<TSource>(int count, Predicate<TSource> predicate, IEnumerable<TSource> collection) {
            NotThat(collection, Matchers.HaveCount<TSource>(count, predicate));
        }

        public void DoesNotHaveCount<TSource>(int count, Predicate<TSource> predicate, IEnumerable<TSource> collection, string message, params object[] args) {
            NotThat(collection, Matchers.HaveCount<TSource>(count, predicate), message, (object[]) args);
        }
    }

    partial class Assert {

        public static void HasCount(int count, IEnumerable collection) {
            Global.HasCount(count, collection);
        }

        public static void HasCount(int count, IEnumerable collection, string message, params object[] args) {
            Global.HasCount(count, collection, message, (object[]) args);
        }

        public static void DoesNotHaveCount(int count, IEnumerable collection) {
            Global.DoesNotHaveCount(count, collection);
        }

        public static void DoesNotHaveCount(int count, IEnumerable collection, string message, params object[] args) {
            Global.DoesNotHaveCount(count, collection, message, (object[]) args);
        }

		public static void HasCount<TSource>(int count, Predicate<TSource> predicate, IEnumerable<TSource> collection) {
            Global.HasCount<TSource>(count, predicate, collection);
        }

        public static void HasCount<TSource>(int count, Predicate<TSource> predicate, IEnumerable<TSource> collection, string message, params object[] args) {
            Global.HasCount<TSource>(count, predicate, collection, message, (object[]) args);
        }

        public static void DoesNotHaveCount<TSource>(int count, Predicate<TSource> predicate, IEnumerable<TSource> collection) {
            Global.DoesNotHaveCount<TSource>(count, predicate, collection);
        }

        public static void DoesNotHaveCount<TSource>(int count, Predicate<TSource> predicate, IEnumerable<TSource> collection, string message, params object[] args) {
            Global.DoesNotHaveCount<TSource>(count, predicate, collection, message, (object[]) args);
        }
    }

    partial class Assume {

        public static void HasCount(int count, IEnumerable collection) {
            Global.HasCount(count, collection);
        }

        public static void HasCount(int count, IEnumerable collection, string message, params object[] args) {
            Global.HasCount(count, collection, message, (object[]) args);
        }

        public static void DoesNotHaveCount(int count, IEnumerable collection) {
            Global.DoesNotHaveCount(count, collection);
        }

        public static void DoesNotHaveCount(int count, IEnumerable collection, string message, params object[] args) {
            Global.DoesNotHaveCount(count, collection, message, (object[]) args);
        }

		public static void HasCount<TSource>(int count, Predicate<TSource> predicate, IEnumerable<TSource> collection) {
            Global.HasCount<TSource>(count, predicate, collection);
        }

        public static void HasCount<TSource>(int count, Predicate<TSource> predicate, IEnumerable<TSource> collection, string message, params object[] args) {
            Global.HasCount<TSource>(count, predicate, collection, message, (object[]) args);
        }

        public static void DoesNotHaveCount<TSource>(int count, Predicate<TSource> predicate, IEnumerable<TSource> collection) {
            Global.DoesNotHaveCount<TSource>(count, predicate, collection);
        }

        public static void DoesNotHaveCount<TSource>(int count, Predicate<TSource> predicate, IEnumerable<TSource> collection, string message, params object[] args) {
            Global.DoesNotHaveCount<TSource>(count, predicate, collection, message, (object[]) args);
        }
    }


    partial class Extensions {

        public static void Count<TSource>(this EnumerableExpectation<TSource> e, int count) {
            Count<TSource>(e, count, (string) null);
        }

        public static void Count(this EnumerableExpectation e, int count) {
            Count(e, count, (string) null);
        }

        public static void Count<TSource>(this EnumerableExpectation<TSource> e, int count, string message, params object[] args) {
            e.As<IEnumerable>().Should(Matchers.HaveCount(count), message, (object[]) args);
        }

        public static void Count(this EnumerableExpectation e, int count, string message, params object[] args) {
            e.As<IEnumerable>().Should(Matchers.HaveCount(count), message, (object[]) args);
        }

		public static void Count<TSource>(this EnumerableExpectation<TSource> e, int count, Predicate<TSource> predicate) {
            Count<TSource>(e, count, predicate, null);
        }

        public static void Count<TSource>(this EnumerableExpectation e, int count, Predicate<TSource> predicate) {
            Count(e, count, predicate, null);
        }

        public static void Count<TSource>(this EnumerableExpectation<TSource> e, int count, Predicate<TSource> predicate, string message, params object[] args) {
            e.Should(Matchers.HaveCount(count, predicate), message, (object[]) args);
        }

        public static void Count<TSource>(this EnumerableExpectation e, int count, Predicate<TSource> predicate, string message, params object[] args) {
            e.Cast<TSource>().Should(Matchers.HaveCount(count, predicate), message, (object[]) args);
        }
    }

    namespace TestMatchers {

        public class HaveCountMatcher : TestMatcher<IEnumerable> {

            public int Expected { get; private set; }

            public HaveCountMatcher(int expected) {
                Expected = expected;
            }

            public override bool Matches(IEnumerable actual) {
                if (actual == null) {
                    throw new ArgumentNullException("actual");
                }
                return ActualCount(actual) == Expected;
            }

            static int ActualCount(IEnumerable actual) {
                var c = actual as ICollection;
                if (c != null) {
                    return c.Count;
                }
                return actual.Cast<object>().Count();
            }
        }

        public class HaveCountMatcher<TSource> : TestMatcher<IEnumerable<TSource>> {

            public int Expected { get; private set; }
            public Predicate<TSource> Predicate { get; private set; }

            public HaveCountMatcher(int expected, Predicate<TSource> predicate) {
                Expected = expected;
                Predicate = predicate;
            }

            public override bool Matches(IEnumerable<TSource> actual) {
                if (actual == null) {
                    throw new ArgumentNullException("actual");
                }
                return ActualCount(actual) == Expected;
            }

            int ActualCount(IEnumerable<TSource> actual) {
                Func<TSource, bool> f = t => Predicate(t);
                return actual.Count(f);
            }
        }
    }

}
