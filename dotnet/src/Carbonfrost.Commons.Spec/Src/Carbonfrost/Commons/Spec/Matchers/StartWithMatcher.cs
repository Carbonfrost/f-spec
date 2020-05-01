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
using System.Collections.Generic;
using System.Linq;
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.Commons.Spec {

    partial class Matchers {

        public static StartWithMatcher<TSource> StartWith<TSource>(params TSource[] expected) {
            return new StartWithMatcher<TSource>(expected, null);
        }

        public static StartWithMatcher<TSource> StartWith<TSource>(IEnumerable<TSource> expected) {
            return new StartWithMatcher<TSource>(expected, null);
        }

        public static StartWithMatcher<TSource> StartWith<TSource>(IEnumerable<TSource> expected, IEqualityComparer<TSource> comparer) {
            return new StartWithMatcher<TSource>(expected, comparer);
        }

        public static StartWithMatcher<TSource> StartWith<TSource>(IEnumerable<TSource> expected, Comparison<TSource> comparison) {
            return new StartWithMatcher<TSource>(expected, new Assert.EqualityComparisonAdapter<TSource>(comparison));
        }
    }

    static partial class Extensions {

        public static void StartsWith<T>(this Expectation<IEnumerable<T>> e, params T[] expected) {
            Operators.StartWith.Apply<T>(e, expected, (string) null);
        }

        public static void StartsWith<T>(this Expectation<IEnumerable<T>> e, IEnumerable<T> expected, IEqualityComparer<T> comparer) {
            Operators.StartWith.Apply<T>(e, expected, comparer, null);
        }

        public static void StartsWith<T>(this Expectation<IEnumerable<T>> e, IEnumerable<T> expected, Comparison<T> comparison) {
            Operators.StartWith.Apply<T>(e, expected, comparison, null);
        }

        public static void StartsWith<T>(this Expectation<IEnumerable<T>> e, IEnumerable<T> expected) {
            Operators.StartWith.Apply<T>(e, expected, (string) null);
        }

        public static void StartsWith<T>(this Expectation<IEnumerable<T>> e, IEnumerable<T> expected, IEqualityComparer<T> comparer, string message, params object[] args) {
            Operators.StartWith.Apply<T>(e, expected, message, (object[]) args);
        }

        public static void StartsWith<T>(this Expectation<IEnumerable<T>> e, IEnumerable<T> expected, Comparison<T> comparison, string message, params object[] args) {
            Operators.StartWith.Apply<T>(e, expected, comparison, message, (object[]) args);
        }

        public static void StartsWith<T>(this Expectation<IEnumerable<T>> e, IEnumerable<T> expected, string message, params object[] args) {
            Operators.StartWith.Apply<T>(e, expected, message, (object[]) args);
        }
    }

    partial class Asserter {

        public void StartsWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual) {
            StartsWith(expected, actual, (string) null);
        }

        public void StartsWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer) {
            StartsWith(expected, actual, comparer, (string) null);
        }

        public void StartsWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison) {
            StartsWith(expected, actual, comparison, (string) null);
        }

        public void DoesNotStartWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual) {
            DoesNotStartWith(expected, actual, (string) null);
        }

        public void DoesNotStartWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer) {
            DoesNotStartWith(expected, actual, comparer, (string) null);
        }

        public void DoesNotStartWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison) {
            DoesNotStartWith(expected, actual, comparison, (string) null);
        }

        public void StartsWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, string message, params object[] args) {
            That(actual, Matchers.StartWith(expected), message, (object[]) args);
        }

        public void StartsWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer, string message, params object[] args) {
            That(actual, Matchers.StartWith(expected, comparer), message, (object[]) args);
        }

        public void StartsWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison, string message, params object[] args) {
            That(actual, Matchers.StartWith(expected, comparison), message, (object[]) args);
        }

        public void DoesNotStartWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, string message, params object[] args) {
            NotThat(actual, Matchers.StartWith(expected), message, (object[]) args);
        }

        public void DoesNotStartWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer, string message, params object[] args) {
            NotThat(actual, Matchers.StartWith(expected, comparer), message, (object[]) args);
        }

        public void DoesNotStartWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison, string message, params object[] args) {
            NotThat(actual, Matchers.StartWith(expected, comparison), message, (object[]) args);
        }
    }

    partial class Assert {

        public static void StartsWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual) {
            Global.StartsWith<TSource>(expected, actual);
        }

        public static void StartsWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer) {
            Global.StartsWith<TSource>(expected, actual, comparer);
        }

        public static void StartsWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison) {
            Global.StartsWith<TSource>(expected, actual, comparison);
        }

        public static void DoesNotStartWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual) {
            Global.DoesNotStartWith<TSource>(expected, actual);
        }

        public static void DoesNotStartWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer) {
            Global.DoesNotStartWith<TSource>(expected, actual, comparer);
        }

        public static void DoesNotStartWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison) {
            Global.DoesNotStartWith<TSource>(expected, actual, comparison);
        }

        public static void StartsWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, string message, params object[] args) {
            Global.StartsWith<TSource>(expected, actual, message, (object[]) args);
        }

        public static void StartsWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer, string message, params object[] args) {
            Global.StartsWith<TSource>(expected, actual, comparer, message, (object[]) args);
        }

        public static void StartsWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison, string message, params object[] args) {
            Global.StartsWith<TSource>(expected, actual, comparison, message, (object[]) args);
        }

        public static void DoesNotStartWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, string message, params object[] args) {
            Global.DoesNotStartWith<TSource>(expected, actual, message, (object[]) args);
        }

        public static void DoesNotStartWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer, string message, params object[] args) {
            Global.DoesNotStartWith<TSource>(expected, actual, comparer, message, (object[]) args);
        }

        public static void DoesNotStartWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison, string message, params object[] args) {
            Global.DoesNotStartWith<TSource>(expected, actual, comparison, message, (object[]) args);
        }
    }

    partial class Assume {

        public static void StartsWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual) {
            Global.StartsWith<TSource>(expected, actual);
        }

        public static void StartsWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer) {
            Global.StartsWith<TSource>(expected, actual, comparer);
        }

        public static void StartsWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison) {
            Global.StartsWith<TSource>(expected, actual, comparison);
        }

        public static void DoesNotStartWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual) {
            Global.DoesNotStartWith<TSource>(expected, actual);
        }

        public static void DoesNotStartWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer) {
            Global.DoesNotStartWith<TSource>(expected, actual, comparer);
        }

        public static void DoesNotStartWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison) {
            Global.DoesNotStartWith<TSource>(expected, actual, comparison);
        }

        public static void StartsWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, string message, params object[] args) {
            Global.StartsWith<TSource>(expected, actual, message, (object[]) args);
        }

        public static void StartsWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer, string message, params object[] args) {
            Global.StartsWith<TSource>(expected, actual, comparer, message, (object[]) args);
        }

        public static void StartsWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison, string message, params object[] args) {
            Global.StartsWith<TSource>(expected, actual, comparison, message, (object[]) args);
        }

        public static void DoesNotStartWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, string message, params object[] args) {
            Global.DoesNotStartWith<TSource>(expected, actual, message, (object[]) args);
        }

        public static void DoesNotStartWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer, string message, params object[] args) {
            Global.DoesNotStartWith<TSource>(expected, actual, comparer, message, (object[]) args);
        }

        public static void DoesNotStartWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison, string message, params object[] args) {
            Global.DoesNotStartWith<TSource>(expected, actual, comparison, message, (object[]) args);
        }
    }


    namespace TestMatchers {

        public class StartWithMatcher<TSource> : TestMatcher<IEnumerable<TSource>>, ITestMatcherWithEqualityComparerApiConventions<StartWithMatcher<TSource>, TSource> {

            public IEnumerable<TSource> Expected {
                get;
                private set;
            }

            public IEqualityComparer<TSource> Comparer {
                get;
                private set;
            }

            public StartWithMatcher(IEnumerable<TSource> expected, IEqualityComparer<TSource> comparer = null) {
                Expected = expected;
                Comparer = comparer;
            }

            public StartWithMatcher<TSource> WithComparer(IEqualityComparer<TSource> comparer) {
                return new StartWithMatcher<TSource>(Expected, comparer);
            }

            public StartWithMatcher<TSource> WithComparer(IComparer<TSource> comparer) {
                if (comparer == null) {
                    return new StartWithMatcher<TSource>(Expected, null);
                }
                return new StartWithMatcher<TSource>(Expected, new Assert.EqualityComparerAdapter<TSource>(comparer));
            }

            public StartWithMatcher<TSource> WithComparison(Comparison<TSource> comparison) {
                return new StartWithMatcher<TSource>(Expected, new Assert.EqualityComparisonAdapter<TSource>(comparison));
            }

            public override bool Matches(IEnumerable<TSource> actual) {
                var comparer = Comparer;
                if (comparer == null) {
                    comparer = EqualityComparer<TSource>.Default;
                }
                var e = Expected.ToList();
                return actual.Take(e.Count).SequenceEqual(e, comparer);
            }
        }

        class StartWithOperator : SequenceComparisonOperator {

            protected override ITestMatcher<IEnumerable<T>> CreateMatcher<T>(IEnumerable<T> expected) {
                return Matchers.StartWith<T>(expected);
            }

            protected override ITestMatcher<IEnumerable<T>> CreateMatcher<T>(IEnumerable<T> expected, IEqualityComparer<T> comparer) {
                return Matchers.StartWith<T>(expected, comparer);
            }

            protected override ITestMatcher<IEnumerable<T>> CreateMatcher<T>(IEnumerable<T> expected, Comparison<T> comparison) {
                return Matchers.StartWith<T>(expected, comparison);
            }
        }

        partial class Operators {
            internal static readonly ISequenceComparisonOperator StartWith = new StartWithOperator();
        }
    }
}
