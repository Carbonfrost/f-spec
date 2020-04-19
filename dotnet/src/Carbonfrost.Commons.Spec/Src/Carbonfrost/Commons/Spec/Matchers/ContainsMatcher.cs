//
// Copyright 2018-2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
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
using System.Collections.Generic;
using System.Linq;
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.Commons.Spec {

    partial class Matchers {

        public static ContainsMatcher<TSource> Contain<TSource>(TSource expected) {
            return new ContainsMatcher<TSource>(expected);
        }

        public static ContainsMatcher<TSource> Contain<TSource>(TSource expected, IEqualityComparer<TSource> comparer) {
            return new ContainsMatcher<TSource>(expected, comparer);
        }

        public static ContainsMatcher<TSource> Contain<TSource>(TSource expected, Comparison<TSource> comparison) {
            return new ContainsMatcher<TSource>(expected, comparison);
        }

    }

    partial class Extensions {

        public static void Element(this EnumerableExpectation e, object item) {
            Element(e, item, (string) null);
        }

        public static void Element(this EnumerableExpectation e, object item, string message, params object[] args) {
            e.Cast<object>().Should(Matchers.Contain(item), message, (object[]) args);
        }

        public static void Element<TSource>(this EnumerableExpectation<TSource> e, TSource item) {
            Element<TSource>(e, item, (string) null);
        }

        public static void Element<TSource>(this EnumerableExpectation<TSource> e, TSource item, string message, params object[] args) {
            e.Should(Matchers.Contain(item), message, (object[]) args);
        }

        public static void Element<TSource>(this EnumerableExpectation<TSource> e, TSource item, IEqualityComparer<TSource> comparer) {
            Element<TSource>(e, item, comparer, (string) null);
        }

        public static void Element<TSource>(this EnumerableExpectation<TSource> e, TSource item, IEqualityComparer<TSource> comparer, string message, params object[] args) {
            e.Should(Matchers.Contain(item, comparer), message, (object[]) args);
        }

        public static void Element<TSource>(this EnumerableExpectation<TSource> e, TSource item, Comparison<TSource> comparison) {
            Element<TSource>(e, item, comparison, (string) null);
        }

        public static void Element<TSource>(this EnumerableExpectation<TSource> e, TSource item, Comparison<TSource> comparison, string message, params object[] args) {
            e.Should(Matchers.Contain(item, comparison), message, (object[]) args);
        }
    }

    partial class Asserter {

        public void Contains<TSource>(TSource expected, IEnumerable<TSource> actual) {
            Contains(expected, actual, (string) null);
        }

        public void Contains<TSource>(TSource expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer) {
            Contains(expected, actual, comparer, (string) null);
        }

        public void Contains<TSource>(TSource expected, IEnumerable<TSource> actual, Comparison<TSource> comparison) {
            Contains(expected, actual, comparison, (string) null);
        }

        public void Contains<TSource>(TSource expected, IEnumerable<TSource> actual, string message, params object[] args) {
            That(actual, Matchers.Contain<TSource>(expected), message, (object[]) args);
        }

        public void Contains<TSource>(TSource expected, IEnumerable<TSource> actual, Comparison<TSource> comparison, string message, params object[] args) {
            That(actual, Matchers.Contain<TSource>(expected, comparison), message, (object[]) args);
        }

        public void Contains<TSource>(TSource expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer, string message, params object[] args) {
            That(actual, Matchers.Contain<TSource>(expected, comparer), message, (object[]) args);
        }

        public void DoesNotContain<TSource>(TSource expected, IEnumerable<TSource> actual) {
            DoesNotContain(expected, actual, (string) null);
        }

        public void DoesNotContain<TSource>(TSource expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer) {
            DoesNotContain(expected, actual, comparer, (string) null);
        }

        public void DoesNotContain<TSource>(TSource expected, IEnumerable<TSource> actual, Comparison<TSource> comparison) {
            DoesNotContain(expected, actual, comparison, (string) null);
        }

        public void DoesNotContain<TSource>(TSource expected, IEnumerable<TSource> actual, string message, params object[] args) {
            NotThat(actual, Matchers.Contain(expected), message, (object[]) args);
        }

        public void DoesNotContain<TSource>(TSource expected, IEnumerable<TSource> actual, Comparison<TSource> comparison, string message, params object[] args) {
            NotThat(actual, Matchers.Contain(expected, comparison), message, (object[]) args);
        }

        public void DoesNotContain<TSource>(TSource expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer, string message, params object[] args) {
            NotThat(actual, Matchers.Contain(expected, comparer), message, (object[]) args);
        }
    }

    partial class Assert {

        public static void Contains<TSource>(TSource expected, IEnumerable<TSource> actual) {
            Global.Contains<TSource>(expected, actual);
        }

        public static void Contains<TSource>(TSource expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer) {
            Global.Contains<TSource>(expected, actual, comparer);
        }

        public static void Contains<TSource>(TSource expected, IEnumerable<TSource> actual, Comparison<TSource> comparison) {
            Global.Contains<TSource>(expected, actual, comparison);
        }

        public static void Contains<TSource>(TSource expected, IEnumerable<TSource> actual, string message, params object[] args) {
            Global.Contains<TSource>(expected, actual, message, (object[]) args);
        }

        public static void Contains<TSource>(TSource expected, IEnumerable<TSource> actual, Comparison<TSource> comparison, string message, params object[] args) {
            Global.Contains<TSource>(expected, actual, comparison, message, (object[]) args);
        }

        public static void Contains<TSource>(TSource expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer, string message, params object[] args) {
            Global.Contains<TSource>(expected, actual, comparer, message, (object[]) args);
        }

        public static void DoesNotContain<TSource>(TSource expected, IEnumerable<TSource> actual) {
            Global.DoesNotContain<TSource>(expected, actual);
        }

        public static void DoesNotContain<TSource>(TSource expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer) {
            Global.DoesNotContain<TSource>(expected, actual, comparer);
        }

        public static void DoesNotContain<TSource>(TSource expected, IEnumerable<TSource> actual, Comparison<TSource> comparison) {
            Global.DoesNotContain<TSource>(expected, actual, comparison);
        }

        public static void DoesNotContain<TSource>(TSource expected, IEnumerable<TSource> actual, string message, params object[] args) {
            Global.DoesNotContain<TSource>(expected, actual, message, (object[]) args);
        }

        public static void DoesNotContain<TSource>(TSource expected, IEnumerable<TSource> actual, Comparison<TSource> comparison, string message, params object[] args) {
            Global.DoesNotContain<TSource>(expected, actual, comparison, message, (object[]) args);
        }

        public static void DoesNotContain<TSource>(TSource expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer, string message, params object[] args) {
            Global.DoesNotContain<TSource>(expected, actual, comparer, message, (object[]) args);
        }
    }

    partial class Assume {

        public static void Contains<TSource>(TSource expected, IEnumerable<TSource> actual) {
            Global.Contains<TSource>(expected, actual);
        }

        public static void Contains<TSource>(TSource expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer) {
            Global.Contains<TSource>(expected, actual, comparer);
        }

        public static void Contains<TSource>(TSource expected, IEnumerable<TSource> actual, Comparison<TSource> comparison) {
            Global.Contains<TSource>(expected, actual, comparison);
        }

        public static void Contains<TSource>(TSource expected, IEnumerable<TSource> actual, string message, params object[] args) {
            Global.Contains<TSource>(expected, actual, message, (object[]) args);
        }

        public static void Contains<TSource>(TSource expected, IEnumerable<TSource> actual, Comparison<TSource> comparison, string message, params object[] args) {
            Global.Contains<TSource>(expected, actual, comparison, message, (object[]) args);
        }

        public static void Contains<TSource>(TSource expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer, string message, params object[] args) {
            Global.Contains<TSource>(expected, actual, comparer, message, (object[]) args);
        }

        public static void DoesNotContain<TSource>(TSource expected, IEnumerable<TSource> actual) {
            Global.DoesNotContain<TSource>(expected, actual);
        }

        public static void DoesNotContain<TSource>(TSource expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer) {
            Global.DoesNotContain<TSource>(expected, actual, comparer);
        }

        public static void DoesNotContain<TSource>(TSource expected, IEnumerable<TSource> actual, Comparison<TSource> comparison) {
            Global.DoesNotContain<TSource>(expected, actual, comparison);
        }

        public static void DoesNotContain<TSource>(TSource expected, IEnumerable<TSource> actual, string message, params object[] args) {
            Global.DoesNotContain<TSource>(expected, actual, message, (object[]) args);
        }

        public static void DoesNotContain<TSource>(TSource expected, IEnumerable<TSource> actual, Comparison<TSource> comparison, string message, params object[] args) {
            Global.DoesNotContain<TSource>(expected, actual, comparison, message, (object[]) args);
        }

        public static void DoesNotContain<TSource>(TSource expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer, string message, params object[] args) {
            Global.DoesNotContain<TSource>(expected, actual, comparer, message, (object[]) args);
        }
    }


    namespace TestMatchers {

        public class ContainsMatcher<TSource> : TestMatcher<IEnumerable<TSource>>, ITestMatcherWithEqualityComparerApiConventions<ContainsMatcher<TSource>, TSource> {

            public TSource Expected { get; private set; }
            public IEqualityComparer<TSource> Comparer { get; private set; }

            public ContainsMatcher(TSource expected) {
                Expected = expected;
            }

            public ContainsMatcher(TSource expected, IEqualityComparer<TSource> comparer) {
                if (expected == null) {
                    throw new ArgumentNullException("expected");
                }
                Expected = expected;
                Comparer = comparer;
            }

            public ContainsMatcher(TSource expected, Comparison<TSource> comparison) {
                if (expected == null) {
                    throw new ArgumentNullException("expected");
                }
                Expected = expected;
                if (comparison != null) {
                    Comparer = new Assert.EqualityComparisonAdapter<TSource>(comparison);
                }
            }

            public ContainsMatcher<TSource> WithComparison(Comparison<TSource> comparison) {
                return new ContainsMatcher<TSource>(Expected, comparison);
            }

            public ContainsMatcher<TSource> WithComparer(IEqualityComparer<TSource> comparer) {
                return new ContainsMatcher<TSource>(Expected, comparer);
            }

            public override bool Matches(IEnumerable<TSource> actual) {
                if (actual == null) {
                    throw new ArgumentNullException("actual");
                }
                return actual.Contains(Expected, Comparer ?? EqualityComparer<TSource>.Default);
            }
        }
    }

}
