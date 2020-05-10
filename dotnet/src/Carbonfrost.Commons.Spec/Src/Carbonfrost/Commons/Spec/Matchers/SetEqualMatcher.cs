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
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.Commons.Spec {

    partial class Matchers {

        public static SetEqualMatcher<TSource> BeSetEqualTo<TSource>(IEnumerable<TSource> expected) {
            return new SetEqualMatcher<TSource>(expected, EqualityComparer<TSource>.Default);
        }

        public static SetEqualMatcher<TSource> BeSetEqualTo<TSource>(IEnumerable<TSource> expected, IEqualityComparer<TSource> comparer) {
            return new SetEqualMatcher<TSource>(expected, comparer);
        }

        public static SetEqualMatcher<TSource> BeSetEqualTo<TSource>(IEnumerable<TSource> expected, Comparison<TSource> comparison) {
            return new SetEqualMatcher<TSource>(expected, new Assert.EqualityComparisonAdapter<TSource>(comparison));
        }

        public static SetEqualMatcher<string> BeSetEqualTo(IEnumerable<string> expected, StringComparison comparison) {
            return new SetEqualMatcher<string>(expected, comparison.MapComparer());
        }

    }

    static partial class Extensions {

        public static void SetEqualTo<T>(this IExpectation<IEnumerable<T>> e, params T[] expected) {
            Operators.SetEqual.Apply<T>(e, expected, (string) null);
        }

        public static void SetEqualTo<T>(this IExpectation<IEnumerable<T>> e, IEnumerable<T> expected, IEqualityComparer<T> comparer) {
            Operators.SetEqual.Apply<T>(e, expected, comparer, null);
        }

        public static void SetEqualTo<T>(this IExpectation<IEnumerable<T>> e, IEnumerable<T> expected, Comparison<T> comparison) {
            Operators.SetEqual.Apply<T>(e, expected, comparison, null);
        }

        public static void SetEqualTo<T>(this IExpectation<IEnumerable<T>> e, IEnumerable<T> expected) {
            Operators.SetEqual.Apply<T>(e, expected, (string) null);
        }

        public static void SetEqualTo<T>(this IExpectation<IEnumerable<T>> e, IEnumerable<T> expected, IEqualityComparer<T> comparer, string message, params object[] args) {
            Operators.SetEqual.Apply<T>(e, expected, message, (object[]) args);
        }

        public static void SetEqualTo<T>(this IExpectation<IEnumerable<T>> e, IEnumerable<T> expected, Comparison<T> comparison, string message, params object[] args) {
            Operators.SetEqual.Apply<T>(e, expected, comparison, message, (object[]) args);
        }

        public static void SetEqualTo<T>(this IExpectation<IEnumerable<T>> e, IEnumerable<T> expected, string message, params object[] args) {
            Operators.SetEqual.Apply<T>(e, expected, message, (object[]) args);
        }

        public static void SetEqualTo(this IExpectation<IEnumerable<string>> e, IEnumerable<string> expected, StringComparison comparison) {
            SetEqualTo(e, expected, comparison, null);
        }

        public static void SetEqualTo(this IExpectation<IEnumerable<string>> e, IEnumerable<string> expected, StringComparison comparison, string message, params object[] args) {
            e.As<IEnumerable<string>>().Like(Matchers.BeSetEqualTo(expected, comparison), message, (object[]) args);
        }

    }

    partial class Asserter {

        public void SetEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual) {
            SetEqual(expected, actual, EqualityComparer<TSource>.Default);
        }

        public void SetEqual(IEnumerable<string> expected, IEnumerable<string> actual, StringComparison comparison) {
            SetEqual<string>(expected, actual, comparison.MapComparer());
        }

        public void SetEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison) {
            SetEqual<TSource>(expected, actual, new Assert.EqualityComparisonAdapter<TSource>(comparison));
        }

        public void SetEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer) {
            That(actual, Matchers.BeSetEqualTo(expected, comparer));
        }

        public void NotSetEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual) {
            NotSetEqual<TSource>(expected, actual, EqualityComparer<TSource>.Default);
        }

        public void NotSetEqual(IEnumerable<string> expected, IEnumerable<string> actual, StringComparison comparison) {
            NotSetEqual<string>(expected, actual, comparison.MapComparer());
        }

        public void NotSetEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison) {
            NotSetEqual<TSource>(expected, actual, new Assert.EqualityComparisonAdapter<TSource>(comparison));
        }

        public void NotSetEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer) {
            NotThat(actual, Matchers.BeSetEqualTo(expected, comparer));
        }

        public void SetEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, string message, params object[] args) {
            SetEqual(expected, actual, EqualityComparer<TSource>.Default, message, args);
        }

        public void SetEqual(IEnumerable<string> expected, IEnumerable<string> actual, StringComparison comparison, string message, params object[] args) {
            SetEqual<string>(expected, actual, comparison.MapComparer(), message, args);
        }

        public void SetEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison, string message, params object[] args) {
            SetEqual<TSource>(expected, actual, new Assert.EqualityComparisonAdapter<TSource>(comparison), message, args);
        }

        public void SetEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer, string message, params object[] args) {
            That(actual, Matchers.BeSetEqualTo(expected, comparer), message, args);
        }

        public void NotSetEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, string message, params object[] args) {
            NotSetEqual<TSource>(expected, actual, EqualityComparer<TSource>.Default, message, args);
        }

        public void NotSetEqual(IEnumerable<string> expected, IEnumerable<string> actual, StringComparison comparison, string message, params object[] args) {
            NotSetEqual<string>(expected, actual, comparison.MapComparer(), message, args);
        }

        public void NotSetEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison, string message, params object[] args) {
            NotSetEqual<TSource>(expected, actual, new Assert.EqualityComparisonAdapter<TSource>(comparison), message, args);
        }

        public void NotSetEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer, string message, params object[] args) {
            NotThat(actual, Matchers.BeSetEqualTo(expected, comparer), message, args);
        }
    }

    partial class Assert {

        public static void SetEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual) {
            Global.SetEqual<TSource>(expected, actual);
        }

        public static void SetEqual(IEnumerable<string> expected, IEnumerable<string> actual, StringComparison comparison) {
            Global.SetEqual(expected, actual, comparison);
        }

        public static void SetEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison) {
            Global.SetEqual<TSource>(expected, actual, comparison);
        }

        public static void SetEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer) {
            Global.SetEqual<TSource>(expected, actual, comparer);
        }

        public static void NotSetEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual) {
            Global.NotSetEqual<TSource>(expected, actual);
        }

        public static void NotSetEqual(IEnumerable<string> expected, IEnumerable<string> actual, StringComparison comparison) {
            Global.NotSetEqual(expected, actual, comparison);
        }

        public static void NotSetEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison) {
            Global.NotSetEqual<TSource>(expected, actual, comparison);
        }

        public static void NotSetEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer) {
            Global.NotSetEqual<TSource>(expected, actual, comparer);
        }

        public static void SetEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, string message, params object[] args) {
            Global.SetEqual<TSource>(expected, actual, message, (object[]) args);
        }

        public static void SetEqual(IEnumerable<string> expected, IEnumerable<string> actual, StringComparison comparison, string message, params object[] args) {
            Global.SetEqual(expected, actual, comparison, message, (object[]) args);
        }

        public static void SetEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison, string message, params object[] args) {
            Global.SetEqual<TSource>(expected, actual, comparison, message, (object[]) args);
        }

        public static void SetEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer, string message, params object[] args) {
            Global.SetEqual<TSource>(expected, actual, comparer, message, (object[]) args);
        }

        public static void NotSetEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, string message, params object[] args) {
            Global.NotSetEqual<TSource>(expected, actual, message, (object[]) args);
        }

        public static void NotSetEqual(IEnumerable<string> expected, IEnumerable<string> actual, StringComparison comparison, string message, params object[] args) {
            Global.NotSetEqual(expected, actual, comparison, message, (object[]) args);
        }

        public static void NotSetEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison, string message, params object[] args) {
            Global.NotSetEqual<TSource>(expected, actual, comparison, message, (object[]) args);
        }

        public static void NotSetEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer, string message, params object[] args) {
            Global.NotSetEqual<TSource>(expected, actual, comparer, message, (object[]) args);
        }
    }

    partial class Assume {

        public static void SetEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual) {
            Global.SetEqual<TSource>(expected, actual);
        }

        public static void SetEqual(IEnumerable<string> expected, IEnumerable<string> actual, StringComparison comparison) {
            Global.SetEqual(expected, actual, comparison);
        }

        public static void SetEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison) {
            Global.SetEqual<TSource>(expected, actual, comparison);
        }

        public static void SetEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer) {
            Global.SetEqual<TSource>(expected, actual, comparer);
        }

        public static void NotSetEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual) {
            Global.NotSetEqual<TSource>(expected, actual);
        }

        public static void NotSetEqual(IEnumerable<string> expected, IEnumerable<string> actual, StringComparison comparison) {
            Global.NotSetEqual(expected, actual, comparison);
        }

        public static void NotSetEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison) {
            Global.NotSetEqual<TSource>(expected, actual, comparison);
        }

        public static void NotSetEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer) {
            Global.NotSetEqual<TSource>(expected, actual, comparer);
        }

        public static void SetEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, string message, params object[] args) {
            Global.SetEqual<TSource>(expected, actual, message, (object[]) args);
        }

        public static void SetEqual(IEnumerable<string> expected, IEnumerable<string> actual, StringComparison comparison, string message, params object[] args) {
            Global.SetEqual(expected, actual, comparison, message, (object[]) args);
        }

        public static void SetEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison, string message, params object[] args) {
            Global.SetEqual<TSource>(expected, actual, comparison, message, (object[]) args);
        }

        public static void SetEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer, string message, params object[] args) {
            Global.SetEqual<TSource>(expected, actual, comparer, message, (object[]) args);
        }

        public static void NotSetEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, string message, params object[] args) {
            Global.NotSetEqual<TSource>(expected, actual, message, (object[]) args);
        }

        public static void NotSetEqual(IEnumerable<string> expected, IEnumerable<string> actual, StringComparison comparison, string message, params object[] args) {
            Global.NotSetEqual(expected, actual, comparison, message, (object[]) args);
        }

        public static void NotSetEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison, string message, params object[] args) {
            Global.NotSetEqual<TSource>(expected, actual, comparison, message, (object[]) args);
        }

        public static void NotSetEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer, string message, params object[] args) {
            Global.NotSetEqual<TSource>(expected, actual, comparer, message, (object[]) args);
        }
    }

    namespace TestMatchers {

        public class SetEqualMatcher<TSource> : TestMatcher<IEnumerable<TSource>>, ITestMatcherWithEqualityComparerApiConventions<SetEqualMatcher<TSource>, TSource>, ITestMatcherActualDiff {

            public IEnumerable<TSource> Expected {
                get;
                private set;
            }

            public IEqualityComparer<TSource> Comparer {
                get;
                private set;
            }

            public SetEqualMatcher(IEnumerable<TSource> expected) {
                Expected = expected;
            }

            public SetEqualMatcher(IEnumerable<TSource> expected, IEqualityComparer<TSource> comparer) {
                Expected = expected;
                Comparer = comparer;
            }

            public SetEqualMatcher(IEnumerable<TSource> expected, Comparison<TSource> comparison) {
                Expected = expected;
                if (comparison != null) {
                    Comparer = new Assert.EqualityComparisonAdapter<TSource>(comparison);
                }
            }

            public SetEqualMatcher<TSource> WithComparer(IEqualityComparer<TSource> comparer) {
                return new SetEqualMatcher<TSource>(Expected, comparer);
            }

            public SetEqualMatcher<TSource> WithComparer(IComparer<TSource> comparer) {
                if (comparer == null) {
                    return new SetEqualMatcher<TSource>(Expected);
                }
                return new SetEqualMatcher<TSource>(Expected, new Assert.EqualityComparerAdapter<TSource>(comparer));
            }

            public SetEqualMatcher<TSource> WithComparison(Comparison<TSource> comparison) {
                return new SetEqualMatcher<TSource>(Expected, comparison);
            }

            public override bool Matches(IEnumerable<TSource> actual) {
                var comparer = Comparer ?? EqualityComparer<TSource>.Default;
                var tally = new HashSet<TSource>(Expected, comparer);
                tally.SymmetricExceptWith(actual);
                return tally.Count == 0;
            }

            Patch ITestMatcherActualDiff.GetPatch(object actual) {
                return Patch.StandardTextPatch(actual, Expected);
            }
        }

        class SetEqualOperator : SequenceComparisonOperator {

            protected override ITestMatcher<IEnumerable<T>> CreateMatcher<T>(IEnumerable<T> expected) {
                return Matchers.BeSetEqualTo<T>(expected);
            }

            protected override ITestMatcher<IEnumerable<T>> CreateMatcher<T>(IEnumerable<T> expected, IEqualityComparer<T> comparer) {
                return Matchers.BeSetEqualTo<T>(expected, comparer);
            }

            protected override ITestMatcher<IEnumerable<T>> CreateMatcher<T>(IEnumerable<T> expected, Comparison<T> comparison) {
                return Matchers.BeSetEqualTo<T>(expected, comparison);
            }
        }

        partial class Operators {
            internal static readonly ISequenceComparisonOperator SetEqual = new SetEqualOperator();
        }
    }

}
