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

        public void DoesNotOverlap<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual) {
            DoesNotOverlap(expected, actual, (string) null);
        }

        public void DoesNotOverlap<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer) {
            DoesNotOverlap(expected, actual, comparer, (string) null);
        }

        public void DoesNotOverlap<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison) {
            DoesNotOverlap(expected, actual, comparison, (string) null);
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

        public void DoesNotOverlap<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, string message, params object[] args) {
            NotThat(actual, Matchers.Overlap(expected), message, (object[]) args);
        }

        public void DoesNotOverlap<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer, string message, params object[] args) {
            NotThat(actual, Matchers.Overlap(expected, comparer), message, (object[]) args);
        }

        public void DoesNotOverlap<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison, string message, params object[] args) {
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

        public static void DoesNotOverlap<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual) {
            Global.DoesNotOverlap<TSource>(expected, actual);
        }

        public static void DoesNotOverlap<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer) {
            Global.DoesNotOverlap<TSource>(expected, actual, comparer);
        }

        public static void DoesNotOverlap<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison) {
            Global.DoesNotOverlap<TSource>(expected, actual, comparison);
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

        public static void DoesNotOverlap<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, string message, params object[] args) {
            Global.DoesNotOverlap<TSource>(expected, actual, message, (object[]) args);
        }

        public static void DoesNotOverlap<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer, string message, params object[] args) {
            Global.DoesNotOverlap<TSource>(expected, actual, comparer, message, (object[]) args);
        }

        public static void DoesNotOverlap<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison, string message, params object[] args) {
            Global.DoesNotOverlap<TSource>(expected, actual, comparison, message, (object[]) args);
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

        public static void DoesNotOverlap<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual) {
            Global.DoesNotOverlap<TSource>(expected, actual);
        }

        public static void DoesNotOverlap<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer) {
            Global.DoesNotOverlap<TSource>(expected, actual, comparer);
        }

        public static void DoesNotOverlap<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison) {
            Global.DoesNotOverlap<TSource>(expected, actual, comparison);
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

        public static void DoesNotOverlap<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, string message, params object[] args) {
            Global.DoesNotOverlap<TSource>(expected, actual, message, (object[]) args);
        }

        public static void DoesNotOverlap<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer, string message, params object[] args) {
            Global.DoesNotOverlap<TSource>(expected, actual, comparer, message, (object[]) args);
        }

        public static void DoesNotOverlap<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison, string message, params object[] args) {
            Global.DoesNotOverlap<TSource>(expected, actual, comparison, message, (object[]) args);
        }
    }

    static partial class Extensions {

        public static void OverlapWith(this EnumerableExpectation e, IEnumerable expected) {
            Operators.Overlap.Apply(e, expected);
        }

        public static void OverlapWith(this EnumerableExpectation e, IEnumerable expected, string message, params object[] args) {
            Operators.Overlap.Apply(e, expected, message, args);
        }

        public static void OverlapWith(this EnumerableExpectation e, IEnumerable expected, Comparison<object> comparison) {
            Operators.Overlap.Apply(e, expected, comparison);
        }

        public static void OverlapWith(this EnumerableExpectation e, IEnumerable expected, Comparison<object> comparison, string message, params object[] args) {
            Operators.Overlap.Apply(e, expected, comparison, message, args);
        }

        public static void OverlapWith(this EnumerableExpectation e, IEnumerable expected, IEqualityComparer comparer) {
            Operators.Overlap.Apply(e, expected, comparer);
        }

        public static void OverlapWith(this EnumerableExpectation e, IEnumerable expected, IEqualityComparer comparer, string message, params object[] args) {
            Operators.Overlap.Apply(e, expected, comparer, message, args);
        }

        public static void OverlapWith<TValue>(this EnumerableExpectation<TValue> e, IEnumerable<TValue> expected) {
            Operators.Overlap.Apply(e, expected);
        }

        public static void OverlapWith<TValue>(this EnumerableExpectation<TValue> e, IEnumerable<TValue> expected, string message, params object[] args) {
            Operators.Overlap.Apply(e, expected, message, args);
        }

        public static void OverlapWith<TValue>(this EnumerableExpectation<TValue> e, IEnumerable<TValue> expected, Comparison<TValue> comparison) {
            Operators.Overlap.Apply(e, expected, comparison);
        }

        public static void OverlapWith<TValue>(this EnumerableExpectation<TValue> e, IEnumerable<TValue> expected, Comparison<TValue> comparison, string message, params object[] args) {
            Operators.Overlap.Apply(e, expected, comparison, message, args);
        }

        public static void OverlapWith<TValue>(this EnumerableExpectation<TValue> e, IEnumerable<TValue> expected, IEqualityComparer<TValue> comparer) {
            Operators.Overlap.Apply(e, expected, comparer);
        }

        public static void OverlapWith<TValue>(this EnumerableExpectation<TValue> e, IEnumerable<TValue> expected, IEqualityComparer<TValue> comparer, string message, params object[] args) {
            Operators.Overlap.Apply(e, expected, comparer, message, args);
        }

        public static void OverlapWith<TValue>(this EnumerableExpectation<TValue> e, params TValue[] expected) {
            Operators.Overlap.Apply(e, expected);
        }
    }

    namespace TestMatchers {

        public class OverlapMatcher<TSource> : TestMatcher<IEnumerable<TSource>>, ITestMatcherWithEqualityComparerApiConventions<OverlapMatcher<TSource>, TSource> {

            public IEnumerable<TSource> Expected {
                get;
                private set;
            }

            public IEqualityComparer<TSource> Comparer {
                get;
                private set;
            }

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

        class OverlapOperator : EnumerableComparisonOperator {

            protected override ITestMatcher<IEnumerable<TValue>> CreateMatcher<TValue>(IEnumerable<TValue> expected) {
                return Matchers.Overlap<TValue>(expected);
            }

            protected override ITestMatcher<IEnumerable<TValue>> CreateMatcher<TValue>(IEnumerable<TValue> expected, IEqualityComparer<TValue> comparer) {
                return Matchers.Overlap<TValue>(expected, comparer);
            }

            protected override ITestMatcher<IEnumerable<TValue>> CreateMatcher<TValue>(IEnumerable<TValue> expected, Comparison<TValue> comparison) {
                return Matchers.Overlap<TValue>(expected, comparison);
            }
        }

        partial class Operators {
            internal static readonly IEnumerableComparisonOperator Overlap = new OverlapOperator();
        }
    }
}
