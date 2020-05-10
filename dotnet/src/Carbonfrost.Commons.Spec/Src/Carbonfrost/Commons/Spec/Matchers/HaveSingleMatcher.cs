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
using System.Collections.Generic;
using System.Linq;
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.Commons.Spec {

    partial class Matchers {

        public static HaveSingleMatcher HaveSingle() {
            return new HaveSingleMatcher();
        }

        public static HaveSingleMatcher<TSource> HaveSingle<TSource>(Predicate<TSource> predicate) {
            return new HaveSingleMatcher<TSource>(predicate);
        }

    }

    partial class Asserter {

        public void Single(IEnumerable collection) {
            That(collection, Matchers.HaveSingle());
        }

        public void Single(IEnumerable collection, string message, params object[] args) {
            That(collection, Matchers.HaveSingle(), message, (object[]) args);
        }

        public void NotSingle(IEnumerable collection) {
            NotThat(collection, Matchers.HaveSingle());
        }

        public void NotSingle(IEnumerable collection, string message, params object[] args) {
            NotThat(collection, Matchers.HaveSingle(), message, (object[]) args);
        }

        public void Single<TSource>(Predicate<TSource> predicate, IEnumerable<TSource> collection) {
            That(collection, Matchers.HaveSingle<TSource>(predicate));
        }

        public void Single<TSource>(Predicate<TSource> predicate, IEnumerable<TSource> collection, string message, params object[] args) {
            That(collection, Matchers.HaveSingle<TSource>(predicate), message, (object[]) args);
        }

        public void DoesNotHaveSingle<TSource>(Predicate<TSource> predicate, IEnumerable<TSource> collection) {
            NotThat(collection, Matchers.HaveSingle<TSource>(predicate));
        }

        public void DoesNotHaveSingle<TSource>(Predicate<TSource> predicate, IEnumerable<TSource> collection, string message, params object[] args) {
            NotThat(collection, Matchers.HaveSingle<TSource>(predicate), message, (object[]) args);
        }
    }

    static partial class Extensions {

        public static void Single<TValue>(this IEnumerableExpectation<TValue> e) {
            Operators.Single.Apply(e);
        }

        public static void Single<TValue>(this IEnumerableExpectation<TValue> e, string message, params object[] args) {
            Operators.Single.Apply(e, message, args);
        }

        public static void Single<TValue>(this IEnumerableExpectation<TValue> e, Predicate<TValue> predicate) {
            Operators.Single.Apply(e, predicate);
        }

        public static void Single<TValue>(this IEnumerableExpectation<object> e, Predicate<TValue> predicate) {
            Operators.Single.Apply(e.Cast<TValue>(), predicate);
        }

        public static void Single<TValue>(this IEnumerableExpectation<TValue> e, Predicate<TValue> predicate, string message, params object[] args) {
            Operators.Single.Apply(e, predicate, message, args);
        }

        public static void Single<TValue>(this IEnumerableExpectation<object> e, Predicate<TValue> predicate, string message, params object[] args) {
            Operators.Single.Apply(e.Cast<TValue>(), predicate, message, args);
        }
    }

    partial class Assert {

        public static void Single(IEnumerable collection) {
            Global.Single(collection);
        }

        public static void Single(IEnumerable collection, string message, params object[] args) {
            Global.Single(collection, message, (object[]) args);
        }

        public static void NotSingle(IEnumerable collection) {
            Global.NotSingle(collection);
        }

        public static void NotSingle(IEnumerable collection, string message, params object[] args) {
            Global.NotSingle(collection, message, (object[]) args);
        }

        public static void Single<TSource>(Predicate<TSource> predicate, IEnumerable<TSource> collection) {
            Global.Single<TSource>(predicate, collection);
        }

        public static void Single<TSource>(Predicate<TSource> predicate, IEnumerable<TSource> collection, string message, params object[] args) {
            Global.Single<TSource>(predicate, collection, message, (object[]) args);
        }

        public static void DoesNotHaveSingle<TSource>(Predicate<TSource> predicate, IEnumerable<TSource> collection) {
            Global.DoesNotHaveSingle<TSource>(predicate, collection);
        }

        public static void DoesNotHaveSingle<TSource>(Predicate<TSource> predicate, IEnumerable<TSource> collection, string message, params object[] args) {
            Global.DoesNotHaveSingle<TSource>(predicate, collection, message, (object[]) args);
        }
    }

    partial class Assume {

        public static void Single(IEnumerable collection) {
            Global.Single(collection);
        }

        public static void Single(IEnumerable collection, string message, params object[] args) {
            Global.Single(collection, message, (object[]) args);
        }

        public static void NotSingle(IEnumerable collection) {
            Global.NotSingle(collection);
        }

        public static void NotSingle(IEnumerable collection, string message, params object[] args) {
            Global.NotSingle(collection, message, (object[]) args);
        }

        public static void Single<TSource>(Predicate<TSource> predicate, IEnumerable<TSource> collection) {
            Global.Single<TSource>(predicate, collection);
        }

        public static void Single<TSource>(Predicate<TSource> predicate, IEnumerable<TSource> collection, string message, params object[] args) {
            Global.Single<TSource>(predicate, collection, message, (object[]) args);
        }

        public static void DoesNotHaveSingle<TSource>(Predicate<TSource> predicate, IEnumerable<TSource> collection) {
            Global.DoesNotHaveSingle<TSource>(predicate, collection);
        }

        public static void DoesNotHaveSingle<TSource>(Predicate<TSource> predicate, IEnumerable<TSource> collection, string message, params object[] args) {
            Global.DoesNotHaveSingle<TSource>(predicate, collection, message, (object[]) args);
        }
    }

    namespace TestMatchers {

        public class HaveSingleMatcher : TestMatcher<IEnumerable> {

            public override bool Matches(IEnumerable actual) {
                if (actual == null) {
                    throw new ArgumentNullException(nameof(actual));
                }
                return CountEstimate(actual) == 1;
            }

            static int CountEstimate(IEnumerable actual) {
                var c = actual as ICollection;
                if (c != null) {
                    return c.Count;
                }
                return actual.Cast<object>().Take(2).Count();
            }
        }

        public class HaveSingleMatcher<TSource> : TestMatcher<IEnumerable<TSource>> {

            public Predicate<TSource> Predicate { get; private set; }

            public HaveSingleMatcher(Predicate<TSource> predicate) {
                Predicate = predicate;
            }

            public override bool Matches(IEnumerable<TSource> actual) {
                if (actual == null) {
                    throw new ArgumentNullException(nameof(actual));
                }
                return CountEstimate(actual) == 1;
            }

            int CountEstimate(IEnumerable<TSource> actual) {
                Func<TSource, bool> f = t => Predicate(t);
                return actual.Where(f).Take(2).Count();
            }
        }

        class HaveSingleOperator : PredicateOperator {

            protected override ITestMatcher<IEnumerable> CreateMatcher() {
                return Matchers.HaveSingle();
            }

            protected override ITestMatcher<IEnumerable<T>> CreateMatcher<T>(Predicate<T> predicate) {
                return Matchers.HaveSingle(predicate);
            }
        }

        static partial class Operators {
            internal static readonly IPredicateOperator Single = new HaveSingleOperator();
        }
    }

}
