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
using System.Collections.Generic;
using System.Linq;
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.Commons.Spec {

    partial class Matchers {

        public static EndWithMatcher<TSource> EndWith<TSource>(params TSource[] expected) {
            return new EndWithMatcher<TSource>(expected, null);
        }

        public static EndWithMatcher<TSource> EndWith<TSource>(IEnumerable<TSource> expected) {
            return new EndWithMatcher<TSource>(expected, null);
        }

        public static EndWithMatcher<TSource> EndWith<TSource>(IEnumerable<TSource> expected, IEqualityComparer<TSource> comparer) {
            return new EndWithMatcher<TSource>(expected, comparer);
        }

        public static EndWithMatcher<TSource> EndWith<TSource>(IEnumerable<TSource> expected, Comparison<TSource> comparison) {
            return new EndWithMatcher<TSource>(expected, new Assert.EqualityComparisonAdapter<TSource>(comparison));
        }

    }

    static partial class Extensions {

        public static void EndsWith<TSource>(this Expectation<IEnumerable<TSource>> e, params TSource[] expected) {
            EndsWith<TSource>(e, expected, (string) null);
        }

        public static void EndsWith<TSource>(this Expectation<IEnumerable<TSource>> e, IEnumerable<TSource> expected, IEqualityComparer<TSource> comparer) {
            EndsWith<TSource>(e, expected, comparer, null);
        }

        public static void EndsWith<TSource>(this Expectation<IEnumerable<TSource>> e, IEnumerable<TSource> expected, Comparison<TSource> comparison) {
            EndsWith<TSource>(e, expected, comparison, null);
        }

        public static void EndsWith<TSource>(this Expectation<IEnumerable<TSource>> e, IEnumerable<TSource> expected) {
            EndsWith<TSource>(e, expected, (string) null);
        }

        public static void EndsWith<TSource>(this Expectation<IEnumerable<TSource>> e, IEnumerable<TSource> expected, IEqualityComparer<TSource> comparer, string message, params object[] args) {
            e.Should(Matchers.EndWith(expected, comparer), message, (object[]) args);
        }

        public static void EndsWith<TSource>(this Expectation<IEnumerable<TSource>> e, IEnumerable<TSource> expected, Comparison<TSource> comparison, string message, params object[] args) {
            e.Should(Matchers.EndWith(expected, comparison), message, (object[]) args);
        }

        public static void EndsWith<TSource>(this Expectation<IEnumerable<TSource>> e, IEnumerable<TSource> expected, string message, params object[] args) {
            e.Should(Matchers.EndWith(expected), message, (object[]) args);
        }
    }

    partial class Asserter {

        public void EndsWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual) {
            EndsWith(expected, actual, (string) null);
        }

        public void EndsWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer) {
            EndsWith(expected, actual, comparer, (string) null);
        }

        public void EndsWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison) {
            EndsWith(expected, actual, comparison, (string) null);
        }

        public void DoesNotEndWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual) {
            DoesNotEndWith(expected, actual, (string) null);
        }

        public void DoesNotEndWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer) {
            DoesNotEndWith(expected, actual, comparer, (string) null);
        }

        public void DoesNotEndWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison) {
            DoesNotEndWith(expected, actual, comparison, (string) null);
        }

        public void EndsWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, string message, params object[] args) {
            That(actual, Matchers.EndWith(expected), message, (object[]) args);
        }

        public void EndsWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer, string message, params object[] args) {
            That(actual, Matchers.EndWith(expected, comparer), message, (object[]) args);
        }

        public void EndsWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison, string message, params object[] args) {
            That(actual, Matchers.EndWith(expected, comparison), message, (object[]) args);
        }

        public void DoesNotEndWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, string message, params object[] args) {
            NotThat(actual, Matchers.EndWith(expected), message, (object[]) args);
        }

        public void DoesNotEndWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer, string message, params object[] args) {
            NotThat(actual, Matchers.EndWith(expected, comparer), message, (object[]) args);
        }

        public void DoesNotEndWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison, string message, params object[] args) {
            NotThat(actual, Matchers.EndWith(expected, comparison), message, (object[]) args);
        }
    }

    partial class Assert {

        public static void EndsWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual) {
            Global.EndsWith<TSource>(expected, actual);
        }

        public static void EndsWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer) {
            Global.EndsWith<TSource>(expected, actual, comparer);
        }

        public static void EndsWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison) {
            Global.EndsWith<TSource>(expected, actual, comparison);
        }

        public static void DoesNotEndWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual) {
            Global.DoesNotEndWith<TSource>(expected, actual);
        }

        public static void DoesNotEndWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer) {
            Global.DoesNotEndWith<TSource>(expected, actual, comparer);
        }

        public static void DoesNotEndWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison) {
            Global.DoesNotEndWith<TSource>(expected, actual, comparison);
        }

        public static void EndsWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, string message, params object[] args) {
            Global.EndsWith<TSource>(expected, actual, message, (object[]) args);
        }

        public static void EndsWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer, string message, params object[] args) {
            Global.EndsWith<TSource>(expected, actual, comparer, message, (object[]) args);
        }

        public static void EndsWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison, string message, params object[] args) {
            Global.EndsWith<TSource>(expected, actual, comparison, message, (object[]) args);
        }

        public static void DoesNotEndWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, string message, params object[] args) {
            Global.DoesNotEndWith<TSource>(expected, actual, message, (object[]) args);
        }

        public static void DoesNotEndWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer, string message, params object[] args) {
            Global.DoesNotEndWith<TSource>(expected, actual, comparer, message, (object[]) args);
        }

        public static void DoesNotEndWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison, string message, params object[] args) {
            Global.DoesNotEndWith<TSource>(expected, actual, comparison, message, (object[]) args);
        }
    }

    partial class Assume {

        public static void EndsWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual) {
            Global.EndsWith<TSource>(expected, actual);
        }

        public static void EndsWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer) {
            Global.EndsWith<TSource>(expected, actual, comparer);
        }

        public static void EndsWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison) {
            Global.EndsWith<TSource>(expected, actual, comparison);
        }

        public static void DoesNotEndWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual) {
            Global.DoesNotEndWith<TSource>(expected, actual);
        }

        public static void DoesNotEndWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer) {
            Global.DoesNotEndWith<TSource>(expected, actual, comparer);
        }

        public static void DoesNotEndWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison) {
            Global.DoesNotEndWith<TSource>(expected, actual, comparison);
        }

        public static void EndsWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, string message, params object[] args) {
            Global.EndsWith<TSource>(expected, actual, message, (object[]) args);
        }

        public static void EndsWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer, string message, params object[] args) {
            Global.EndsWith<TSource>(expected, actual, comparer, message, (object[]) args);
        }

        public static void EndsWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison, string message, params object[] args) {
            Global.EndsWith<TSource>(expected, actual, comparison, message, (object[]) args);
        }

        public static void DoesNotEndWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, string message, params object[] args) {
            Global.DoesNotEndWith<TSource>(expected, actual, message, (object[]) args);
        }

        public static void DoesNotEndWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer, string message, params object[] args) {
            Global.DoesNotEndWith<TSource>(expected, actual, comparer, message, (object[]) args);
        }

        public static void DoesNotEndWith<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison, string message, params object[] args) {
            Global.DoesNotEndWith<TSource>(expected, actual, comparison, message, (object[]) args);
        }
    }


    namespace TestMatchers {

        public class EndWithMatcher<TSource> : TestMatcher<IEnumerable<TSource>> {

            public IEnumerable<TSource> Expected { get; private set; }
            public IEqualityComparer<TSource> Comparer { get; private set; }

            public EndWithMatcher(IEnumerable<TSource> expected, IEqualityComparer<TSource> comparer = null) {
                Expected = expected;
                Comparer = comparer;
            }

            public EndWithMatcher<TSource> WithComparer(IEqualityComparer<TSource> comparer) {
                return new EndWithMatcher<TSource>(Expected, comparer);
            }

            public EndWithMatcher<TSource> WithComparer(IComparer<TSource> comparer) {
                if (comparer == null) {
                    return new EndWithMatcher<TSource>(Expected, null);
                }
                return new EndWithMatcher<TSource>(Expected, new Assert.EqualityComparerAdapter<TSource>(comparer));
            }

            public EndWithMatcher<TSource> WithComparison(Comparison<TSource> comparison) {
                return new EndWithMatcher<TSource>(Expected, new Assert.EqualityComparisonAdapter<TSource>(comparison));
            }

            public override bool Matches(IEnumerable<TSource> actual) {
                var comparer = Comparer;
                if (comparer == null) {
                    comparer = EqualityComparer<TSource>.Default;
                }
                var e = Expected.ToList();
                return actual.Reverse().Take(e.Count).Reverse().SequenceEqual(e, comparer);
            }
        }
    }
}
