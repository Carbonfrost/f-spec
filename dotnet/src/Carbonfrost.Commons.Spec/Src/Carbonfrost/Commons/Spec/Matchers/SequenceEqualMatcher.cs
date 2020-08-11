//
// Copyright 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

        public static SequenceEqualMatcher<TSource> BeSequenceEqualTo<TSource>(IEnumerable<TSource> expected) {
            return new SequenceEqualMatcher<TSource>(expected, EqualityComparer<TSource>.Default);
        }

        public static SequenceEqualMatcher<TSource> BeSequenceEqualTo<TSource>(IEnumerable<TSource> expected, IEqualityComparer<TSource> comparer) {
            return new SequenceEqualMatcher<TSource>(expected, comparer);
        }

        public static SequenceEqualMatcher<TSource> BeSequenceEqualTo<TSource>(IEnumerable<TSource> expected, Comparison<TSource> comparison) {
            return new SequenceEqualMatcher<TSource>(expected, new Assert.EqualityComparisonAdapter<TSource>(comparison));
        }

        public static SequenceEqualMatcher<string> BeSequenceEqualTo(IEnumerable<string> expected, StringComparison comparison) {
            return new SequenceEqualMatcher<string>(expected, comparison.MapComparer());
        }

    }

    static partial class Extensions {

        public static void SequenceEqualTo<T>(this IExpectation<IEnumerable<T>> e, params T[] expected) {
            Operators.SequenceEqual.Apply<T>(e, expected, (string) null);
        }

        public static void SequenceEqualTo<T>(this IExpectation<IEnumerable<T>> e, IEnumerable<T> expected, IEqualityComparer<T> comparer) {
            Operators.SequenceEqual.Apply<T>(e, expected, comparer, null);
        }

        public static void SequenceEqualTo<T>(this IExpectation<IEnumerable<T>> e, IEnumerable<T> expected, Comparison<T> comparison) {
            Operators.SequenceEqual.Apply<T>(e, expected, comparison, null);
        }

        public static void SequenceEqualTo<T>(this IExpectation<IEnumerable<T>> e, IEnumerable<T> expected) {
            Operators.SequenceEqual.Apply<T>(e, expected, (string) null);
        }

        public static void SequenceEqualTo<T>(this IExpectation<IEnumerable<T>> e, IEnumerable<T> expected, IEqualityComparer<T> comparer, string message, params object[] args) {
            Operators.SequenceEqual.Apply<T>(e, expected, message, (object[]) args);
        }

        public static void SequenceEqualTo<T>(this IExpectation<IEnumerable<T>> e, IEnumerable<T> expected, Comparison<T> comparison, string message, params object[] args) {
            Operators.SequenceEqual.Apply<T>(e, expected, comparison, message, (object[]) args);
        }

        public static void SequenceEqualTo<T>(this IExpectation<IEnumerable<T>> e, IEnumerable<T> expected, string message, params object[] args) {
            Operators.SequenceEqual.Apply<T>(e, expected, message, (object[]) args);
        }

        public static void SequenceEqualTo(this IExpectation<IEnumerable<string>> e, IEnumerable<string> expected, StringComparison comparison) {
            SequenceEqualTo(e, expected, comparison, null);
        }

        public static void SequenceEqualTo(this IExpectation<IEnumerable<string>> e, IEnumerable<string> expected, StringComparison comparison, string message, params object[] args) {
            e.As<IEnumerable<string>>().Like(Matchers.BeSequenceEqualTo(expected, comparison), message, (object[]) args);
        }

    }

    partial class Asserter {

        public void SequenceEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual) {
            SequenceEqual(expected, actual, EqualityComparer<TSource>.Default);
        }

        public void SequenceEqual(IEnumerable<string> expected, IEnumerable<string> actual, StringComparison comparison) {
            SequenceEqual<string>(expected, actual, comparison.MapComparer());
        }

        public void SequenceEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison) {
            SequenceEqual<TSource>(expected, actual, new Assert.EqualityComparisonAdapter<TSource>(comparison));
        }

        public void SequenceEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer) {
            That(actual, Matchers.BeSequenceEqualTo(expected, comparer));
        }

        public void NotSequenceEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual) {
            NotSequenceEqual<TSource>(expected, actual, EqualityComparer<TSource>.Default);
        }

        public void NotSequenceEqual(IEnumerable<string> expected, IEnumerable<string> actual, StringComparison comparison) {
            NotSequenceEqual<string>(expected, actual, comparison.MapComparer());
        }

        public void NotSequenceEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison) {
            NotSequenceEqual<TSource>(expected, actual, new Assert.EqualityComparisonAdapter<TSource>(comparison));
        }

        public void NotSequenceEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer) {
            NotThat(actual, Matchers.BeSequenceEqualTo(expected, comparer));
        }

        public void SequenceEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, string message, params object[] args) {
            SequenceEqual(expected, actual, EqualityComparer<TSource>.Default, message, args);
        }

        public void SequenceEqual(IEnumerable<string> expected, IEnumerable<string> actual, StringComparison comparison, string message, params object[] args) {
            SequenceEqual<string>(expected, actual, comparison.MapComparer(), message, args);
        }

        public void SequenceEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison, string message, params object[] args) {
            SequenceEqual<TSource>(expected, actual, new Assert.EqualityComparisonAdapter<TSource>(comparison), message, args);
        }

        public void SequenceEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer, string message, params object[] args) {
            That(actual, Matchers.BeSequenceEqualTo(expected, comparer), message, args);
        }

        public void NotSequenceEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, string message, params object[] args) {
            NotSequenceEqual<TSource>(expected, actual, EqualityComparer<TSource>.Default, message, args);
        }

        public void NotSequenceEqual(IEnumerable<string> expected, IEnumerable<string> actual, StringComparison comparison, string message, params object[] args) {
            NotSequenceEqual<string>(expected, actual, comparison.MapComparer(), message, args);
        }

        public void NotSequenceEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison, string message, params object[] args) {
            NotSequenceEqual<TSource>(expected, actual, new Assert.EqualityComparisonAdapter<TSource>(comparison), message, args);
        }

        public void NotSequenceEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer, string message, params object[] args) {
            NotThat(actual, Matchers.BeSequenceEqualTo(expected, comparer), message, args);
        }
    }

    partial class Assert {

        public static void SequenceEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual) {
            Global.SequenceEqual<TSource>(expected, actual);
        }

        public static void SequenceEqual(IEnumerable<string> expected, IEnumerable<string> actual, StringComparison comparison) {
            Global.SequenceEqual(expected, actual, comparison);
        }

        public static void SequenceEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison) {
            Global.SequenceEqual<TSource>(expected, actual, comparison);
        }

        public static void SequenceEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer) {
            Global.SequenceEqual<TSource>(expected, actual, comparer);
        }

        public static void NotSequenceEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual) {
            Global.NotSequenceEqual<TSource>(expected, actual);
        }

        public static void NotSequenceEqual(IEnumerable<string> expected, IEnumerable<string> actual, StringComparison comparison) {
            Global.NotSequenceEqual(expected, actual, comparison);
        }

        public static void NotSequenceEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison) {
            Global.NotSequenceEqual<TSource>(expected, actual, comparison);
        }

        public static void NotSequenceEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer) {
            Global.NotSequenceEqual<TSource>(expected, actual, comparer);
        }

        public static void SequenceEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, string message, params object[] args) {
            Global.SequenceEqual<TSource>(expected, actual, message, (object[]) args);
        }

        public static void SequenceEqual(IEnumerable<string> expected, IEnumerable<string> actual, StringComparison comparison, string message, params object[] args) {
            Global.SequenceEqual(expected, actual, comparison, message, (object[]) args);
        }

        public static void SequenceEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison, string message, params object[] args) {
            Global.SequenceEqual<TSource>(expected, actual, comparison, message, (object[]) args);
        }

        public static void SequenceEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer, string message, params object[] args) {
            Global.SequenceEqual<TSource>(expected, actual, comparer, message, (object[]) args);
        }

        public static void NotSequenceEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, string message, params object[] args) {
            Global.NotSequenceEqual<TSource>(expected, actual, message, (object[]) args);
        }

        public static void NotSequenceEqual(IEnumerable<string> expected, IEnumerable<string> actual, StringComparison comparison, string message, params object[] args) {
            Global.NotSequenceEqual(expected, actual, comparison, message, (object[]) args);
        }

        public static void NotSequenceEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison, string message, params object[] args) {
            Global.NotSequenceEqual<TSource>(expected, actual, comparison, message, (object[]) args);
        }

        public static void NotSequenceEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer, string message, params object[] args) {
            Global.NotSequenceEqual<TSource>(expected, actual, comparer, message, (object[]) args);
        }
    }

    partial class Assume {

        public static void SequenceEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual) {
            Global.SequenceEqual<TSource>(expected, actual);
        }

        public static void SequenceEqual(IEnumerable<string> expected, IEnumerable<string> actual, StringComparison comparison) {
            Global.SequenceEqual(expected, actual, comparison);
        }

        public static void SequenceEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison) {
            Global.SequenceEqual<TSource>(expected, actual, comparison);
        }

        public static void SequenceEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer) {
            Global.SequenceEqual<TSource>(expected, actual, comparer);
        }

        public static void NotSequenceEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual) {
            Global.NotSequenceEqual<TSource>(expected, actual);
        }

        public static void NotSequenceEqual(IEnumerable<string> expected, IEnumerable<string> actual, StringComparison comparison) {
            Global.NotSequenceEqual(expected, actual, comparison);
        }

        public static void NotSequenceEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison) {
            Global.NotSequenceEqual<TSource>(expected, actual, comparison);
        }

        public static void NotSequenceEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer) {
            Global.NotSequenceEqual<TSource>(expected, actual, comparer);
        }

        public static void SequenceEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, string message, params object[] args) {
            Global.SequenceEqual<TSource>(expected, actual, message, (object[]) args);
        }

        public static void SequenceEqual(IEnumerable<string> expected, IEnumerable<string> actual, StringComparison comparison, string message, params object[] args) {
            Global.SequenceEqual(expected, actual, comparison, message, (object[]) args);
        }

        public static void SequenceEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison, string message, params object[] args) {
            Global.SequenceEqual<TSource>(expected, actual, comparison, message, (object[]) args);
        }

        public static void SequenceEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer, string message, params object[] args) {
            Global.SequenceEqual<TSource>(expected, actual, comparer, message, (object[]) args);
        }

        public static void NotSequenceEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, string message, params object[] args) {
            Global.NotSequenceEqual<TSource>(expected, actual, message, (object[]) args);
        }

        public static void NotSequenceEqual(IEnumerable<string> expected, IEnumerable<string> actual, StringComparison comparison, string message, params object[] args) {
            Global.NotSequenceEqual(expected, actual, comparison, message, (object[]) args);
        }

        public static void NotSequenceEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison, string message, params object[] args) {
            Global.NotSequenceEqual<TSource>(expected, actual, comparison, message, (object[]) args);
        }

        public static void NotSequenceEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer, string message, params object[] args) {
            Global.NotSequenceEqual<TSource>(expected, actual, comparer, message, (object[]) args);
        }
    }

    namespace TestMatchers {

        public class SequenceEqualMatcher<TSource> : TestMatcher<IEnumerable<TSource>>, ITestMatcherWithEqualityComparerApiConventions<SequenceEqualMatcher<TSource>, TSource>, ITestMatcherActualDiff {

            public IEnumerable<TSource> Expected {
                get;
                private set;
            }

            public IEqualityComparer<TSource> Comparer {
                get;
                private set;
            }

            public SequenceEqualMatcher(IEnumerable<TSource> expected) {
                Expected = expected;
            }

            public SequenceEqualMatcher(IEnumerable<TSource> expected, IEqualityComparer<TSource> comparer) {
                Expected = expected;
                Comparer = comparer;
            }

            public SequenceEqualMatcher(IEnumerable<TSource> expected, Comparison<TSource> comparison) {
                Expected = expected;
                if (comparison != null) {
                    Comparer = new Assert.EqualityComparisonAdapter<TSource>(comparison);
                }
            }

            public SequenceEqualMatcher<TSource> WithComparer(IEqualityComparer<TSource> comparer) {
                return new SequenceEqualMatcher<TSource>(Expected, comparer);
            }

            public SequenceEqualMatcher<TSource> WithComparer(IComparer<TSource> comparer) {
                if (comparer == null) {
                    return new SequenceEqualMatcher<TSource>(Expected);
                }
                return new SequenceEqualMatcher<TSource>(Expected, new Assert.EqualityComparerAdapter<TSource>(comparer));
            }

            public SequenceEqualMatcher<TSource> WithComparison(Comparison<TSource> comparison) {
                return new SequenceEqualMatcher<TSource>(Expected, comparison);
            }

            public override bool Matches(IEnumerable<TSource> actual) {
                if (actual == null) {
                    return false;
                }
                var comparer = Comparer ?? EqualityComparer<TSource>.Default;
                if (actual.Count() != Expected.Count()) {
                    return false;
                }
                if (Expected.Zip(actual, (x, y) => comparer.Equals(x, y)).All(b => b)) {
                    return true;
                }
                return false;
            }

            Patch ITestMatcherActualDiff.GetPatch(object actual) {
                return Patch.StandardTextPatch(actual, Expected);
            }
        }

        class SequenceEqualOperator : SequenceComparisonOperator {

            protected override ITestMatcher<IEnumerable<T>> CreateMatcher<T>(IEnumerable<T> expected) {
                return Matchers.BeSequenceEqualTo<T>(expected);
            }

            protected override ITestMatcher<IEnumerable<T>> CreateMatcher<T>(IEnumerable<T> expected, IEqualityComparer<T> comparer) {
                return Matchers.BeSequenceEqualTo<T>(expected, comparer);
            }

            protected override ITestMatcher<IEnumerable<T>> CreateMatcher<T>(IEnumerable<T> expected, Comparison<T> comparison) {
                return Matchers.BeSequenceEqualTo<T>(expected, comparison);
            }
        }

        partial class Operators {
            internal static readonly ISequenceComparisonOperator SequenceEqual = new SequenceEqualOperator();
        }
    }

}
