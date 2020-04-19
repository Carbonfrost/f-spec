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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.Commons.Spec {

    partial class Matchers {

        public static OverlapMatcher<TSource> Overlap<TSource>(params TSource[] expected) {
            return new OverlapMatcher<TSource>(expected, Assert.GetEqualityComparer<TSource>());
        }

        public static OverlapMatcher<TSource> Overlap<TSource>(IEnumerable<TSource> expected) {
            return new OverlapMatcher<TSource>(expected, Assert.GetEqualityComparer<TSource>());
        }

        public static OverlapMatcher<TSource> Overlap<TSource>(IEnumerable<TSource> expected, IEqualityComparer<TSource> comparer) {
            return new OverlapMatcher<TSource>(expected, comparer);
        }

        public static OverlapMatcher<TSource> Overlap<TSource>(IEnumerable<TSource> expected, Comparison<TSource> comparison) {
            return new OverlapMatcher<TSource>(expected, new Assert.EqualityComparisonAdapter<TSource>(comparison));
        }
    }

    static partial class Extensions {

        public static void OverlapWith<TSource>(this EnumerableExpectation<TSource> e, params TSource[] expected) {
            e.Should(Matchers.Overlap<TSource>(expected));
        }

        public static void OverlapWith<TSource>(this EnumerableExpectation<TSource> e, IEnumerable<TSource> expected, IEqualityComparer<TSource> comparer) {
            OverlapWith<TSource>(e, expected, comparer, null);
        }

        public static void OverlapWith<TSource>(this EnumerableExpectation<TSource> e, IEnumerable<TSource> expected, Comparison<TSource> comparison) {
            OverlapWith<TSource>(e, expected, comparison, null);
        }

        public static void OverlapWith<TSource>(this EnumerableExpectation<TSource> e, IEnumerable<TSource> expected) {
            OverlapWith<TSource>(e, expected, (string) null);
        }

        public static void OverlapWith(this EnumerableExpectation e, params object[] expected) {
            e.Cast<object>().Should(Matchers.Overlap<object>(expected));
        }

        public static void OverlapWith(this EnumerableExpectation e, IEnumerable expected, IEqualityComparer comparer) {
            OverlapWith(e, expected, comparer, null);
        }

        public static void OverlapWith(this EnumerableExpectation e, IEnumerable expected, Comparison<object> comparison) {
            OverlapWith(e, expected, comparison, null);
        }

        public static void OverlapWith(this EnumerableExpectation e, IEnumerable expected) {
            OverlapWith(e, expected, (string) null);
        }

        public static void OverlapWith<TSource>(this EnumerableExpectation<TSource> e, IEnumerable<TSource> expected, IEqualityComparer<TSource> comparer, string message, params object[] args) {
            e.Should(Matchers.Overlap<TSource>(expected, comparer), message, (object[]) args);
        }

        public static void OverlapWith<TSource>(this EnumerableExpectation<TSource> e, IEnumerable<TSource> expected, Comparison<TSource> comparison, string message, params object[] args) {
            e.Should(Matchers.Overlap<TSource>(expected, comparison), message, (object[]) args);
        }

        public static void OverlapWith<TSource>(this EnumerableExpectation<TSource> e, IEnumerable<TSource> expected, string message, params object[] args) {
            e.Should(Matchers.Overlap<TSource>(expected), message, (object[]) args);
        }

        public static void OverlapWith(this EnumerableExpectation e, IEnumerable expected, IEqualityComparer comparer, string message, params object[] args) {
            e.Cast<object>().Should(Matchers.Overlap<object>(expected, comparer), message, (object[]) args);
        }

        public static void OverlapWith(this EnumerableExpectation e, IEnumerable expected, Comparison<object> comparison, string message, params object[] args) {
            e.Cast<object>().Should(Matchers.Overlap<object>(expected, comparison), message, (object[]) args);
        }

        public static void OverlapWith(this EnumerableExpectation e, IEnumerable expected, string message, params object[] args) {
            e.Cast<object>().Should(Matchers.Overlap<object>(expected), message, (object[]) args);
        }
    }

    partial class Asserter {

        public void Overlaps<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual) {
            Overlaps(expected, actual, (string) null);
        }

        public void Overlaps<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer) {
            Overlaps(expected, actual, comparer, (string) null);
        }

        public void Overlaps<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison) {
            Overlaps(expected, actual, comparison, (string) null);
        }

        public void NotOverlaps<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual) {
            NotOverlaps(expected, actual, (string) null);
        }

        public void NotOverlaps<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer) {
            NotOverlaps(expected, actual, comparer, (string) null);
        }

        public void NotOverlaps<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison) {
            NotOverlaps(expected, actual, comparison, (string) null);
        }

        public void Overlaps<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, string message, params object[] args) {
            That(actual, Matchers.Overlap(expected), message, (object[]) args);
        }

        public void Overlaps<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer, string message, params object[] args) {
            That(actual, Matchers.Overlap(expected, comparer), message, (object[]) args);
        }

        public void Overlaps<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison, string message, params object[] args) {
            That(actual, Matchers.Overlap(expected, comparison), message, (object[]) args);
        }

        public void NotOverlaps<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, string message, params object[] args) {
            NotThat(actual, Matchers.Overlap(expected), message, (object[]) args);
        }

        public void NotOverlaps<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer, string message, params object[] args) {
            NotThat(actual, Matchers.Overlap(expected, comparer), message, (object[]) args);
        }

        public void NotOverlaps<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison, string message, params object[] args) {
            NotThat(actual, Matchers.Overlap(expected, comparison), message, (object[]) args);
        }
    }

    partial class Assert {

        public static void Overlaps<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual) {
            Global.Overlaps<TSource>(expected, actual);
        }

        public static void Overlaps<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer) {
            Global.Overlaps<TSource>(expected, actual, comparer);
        }

        public static void Overlaps<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison) {
            Global.Overlaps<TSource>(expected, actual, comparison);
        }

        public static void NotOverlaps<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual) {
            Global.NotOverlaps<TSource>(expected, actual);
        }

        public static void NotOverlaps<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer) {
            Global.NotOverlaps<TSource>(expected, actual, comparer);
        }

        public static void NotOverlaps<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison) {
            Global.NotOverlaps<TSource>(expected, actual, comparison);
        }

        public static void Overlaps<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, string message, params object[] args) {
            Global.Overlaps<TSource>(expected, actual, message, (object[]) args);
        }

        public static void Overlaps<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer, string message, params object[] args) {
            Global.Overlaps<TSource>(expected, actual, comparer, message, (object[]) args);
        }

        public static void Overlaps<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison, string message, params object[] args) {
            Global.Overlaps<TSource>(expected, actual, comparison, message, (object[]) args);
        }

        public static void NotOverlaps<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, string message, params object[] args) {
            Global.NotOverlaps<TSource>(expected, actual, message, (object[]) args);
        }

        public static void NotOverlaps<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer, string message, params object[] args) {
            Global.NotOverlaps<TSource>(expected, actual, comparer, message, (object[]) args);
        }

        public static void NotOverlaps<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison, string message, params object[] args) {
            Global.NotOverlaps<TSource>(expected, actual, comparison, message, (object[]) args);
        }
    }

    partial class Assume {

        public static void Overlaps<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual) {
            Global.Overlaps<TSource>(expected, actual);
        }

        public static void Overlaps<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer) {
            Global.Overlaps<TSource>(expected, actual, comparer);
        }

        public static void Overlaps<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison) {
            Global.Overlaps<TSource>(expected, actual, comparison);
        }

        public static void NotOverlaps<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual) {
            Global.NotOverlaps<TSource>(expected, actual);
        }

        public static void NotOverlaps<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer) {
            Global.NotOverlaps<TSource>(expected, actual, comparer);
        }

        public static void NotOverlaps<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison) {
            Global.NotOverlaps<TSource>(expected, actual, comparison);
        }

        public static void Overlaps<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, string message, params object[] args) {
            Global.Overlaps<TSource>(expected, actual, message, (object[]) args);
        }

        public static void Overlaps<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer, string message, params object[] args) {
            Global.Overlaps<TSource>(expected, actual, comparer, message, (object[]) args);
        }

        public static void Overlaps<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison, string message, params object[] args) {
            Global.Overlaps<TSource>(expected, actual, comparison, message, (object[]) args);
        }

        public static void NotOverlaps<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, string message, params object[] args) {
            Global.NotOverlaps<TSource>(expected, actual, message, (object[]) args);
        }

        public static void NotOverlaps<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer, string message, params object[] args) {
            Global.NotOverlaps<TSource>(expected, actual, comparer, message, (object[]) args);
        }

        public static void NotOverlaps<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison, string message, params object[] args) {
            Global.NotOverlaps<TSource>(expected, actual, comparison, message, (object[]) args);
        }
    }


    namespace TestMatchers {

        public class OverlapMatcher<TSource> : TestMatcher<IEnumerable<TSource>>, ITestMatcherWithEqualityComparerApiConventions<OverlapMatcher<TSource>, TSource> {

            public IEnumerable<TSource> Expected { get; private set; }
            public IEqualityComparer<TSource> Comparer { get; private set; }

            public OverlapMatcher(IEnumerable<TSource> expected, IEqualityComparer<TSource> comparer = null) {
                Expected = expected;
                Comparer = comparer;
            }

            public OverlapMatcher<TSource> WithComparer(IEqualityComparer<TSource> comparer) {
                return new OverlapMatcher<TSource>(Expected, comparer);
            }

            public OverlapMatcher<TSource> WithComparer(IComparer<TSource> comparer) {
                if (comparer == null) {
                    return new OverlapMatcher<TSource>(Expected, null);
                }
                return new OverlapMatcher<TSource>(Expected, new Assert.EqualityComparerAdapter<TSource>(comparer));
            }

            public OverlapMatcher<TSource> WithComparison(Comparison<TSource> comparison) {
                return new OverlapMatcher<TSource>(Expected, new Assert.EqualityComparisonAdapter<TSource>(comparison));
            }

            public override bool Matches(IEnumerable<TSource> actual) {
                var comparer = Comparer;
                if (comparer == null) {
                    comparer = Assert.GetEqualityComparer<TSource>();
                }
                return Expected.Intersect(actual, comparer).Any();
            }
        }
    }
}
