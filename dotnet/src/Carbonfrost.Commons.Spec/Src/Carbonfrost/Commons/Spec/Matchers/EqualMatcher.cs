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
using System.ComponentModel;
using System.Linq;
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.Commons.Spec {

    partial class Matchers {

        public static EqualMatcher<T> Equal<T>(T expected) {
            return new EqualMatcher<T>(expected, Assert.GetEqualityComparer<T>());
        }

        public static EqualMatcher<T> Equal<T>(T expected, IEqualityComparer<T> comparer) {
            return new EqualMatcher<T>(expected, comparer);
        }

        public static EqualMatcher<T> Equal<T>(T expected, Comparison<T> comparison) {
            return new EqualMatcher<T>(expected, new Assert.EqualityComparisonAdapter<T>(comparison));
        }

        public static EqualMatcher<string> Equal(string expected, StringComparison comparison) {
            return new EqualMatcher<string>(expected, comparison.MapComparer());
        }

    }

    static partial class Extensions {

        public static void EqualTo<T>(this Expectation<T> e, T expected) {
            EqualTo<T>(e, expected, (string) null);
        }

        public static void EqualTo<T>(this Expectation<T> e, T expected, Comparison<T> comparison) {
            EqualTo<T>(e, expected, comparison, null);
        }

        public static void EqualTo(this Expectation<string> e, string expected, StringComparison comparison) {
            EqualTo(e, expected, comparison, null);
        }

        public static void EqualTo<T>(this Expectation<T> e, T expected, IEqualityComparer<T> comparer) {
            EqualTo(e, expected, comparer, null);
        }

        public static void EqualTo<T>(this Expectation<T> e, T expected, string message, params object[] args) {
            e.Should(Matchers.Equal(expected));
        }

        public static void EqualTo<T>(this Expectation<T> e, T expected, Comparison<T> comparison, string message, params object[] args) {
            e.Should(Matchers.Equal(expected, comparison));
        }

        public static void EqualTo(this Expectation<string> e, string expected, StringComparison comparison, string message, params object[] args) {
            e.Should(Matchers.Equal(expected, comparison));
        }

        public static void EqualTo<T>(this Expectation<T> e, T expected, IEqualityComparer<T> comparer, string message, params object[] args) {
            e.Should(Matchers.Equal(expected, comparer));
        }

    }

    partial class Asserter {

        public void Equal<T>(T expected, T actual) {
            That(actual, Matchers.Equal(expected));
        }

        public void Equal<T>(T expected, T actual, Comparison<T> comparison) {
            That(actual, Matchers.Equal<T>(expected, comparison));
        }

        public void Equal<T>(T expected, T actual, IEqualityComparer<T> comparer) {
            That(actual, Matchers.Equal<T>(expected, comparer));
        }

        public void Equal(string expected, string actual, StringComparison comparison) {
            That(actual, Matchers.Equal(expected, comparison));
        }

        public void Equal<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual) {
            Equal<IEnumerable<TSource>>(expected, actual, Assert.GetEqualityComparer<IEnumerable<TSource>>(true));
        }

        public void Equal<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer) {
            Equal<IEnumerable<TSource>>(expected, actual, Assert.GetEqualityComparer<IEnumerable<TSource>>(true, new Assert.AssertEqualityComparerAdapter<TSource>(comparer)));
        }

        public void Equal<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison) {
            Equal<IEnumerable<TSource>>(expected, actual, Assert.GetEqualityComparer<IEnumerable<TSource>>(true, new Assert.EqualityComparisonAdapter<TSource>(comparison)));
        }

        public void NotEqual<T>(T expected, T actual) {
            NotEqual(expected, actual, Assert.GetEqualityComparer<T>());
        }

        public void NotEqual<T>(T expected, T actual, IEqualityComparer<T> comparer) {
            NotThat(actual, Matchers.Equal(expected, comparer));
        }

        public void NotEqual<T>(T expected, T actual, Comparison<T> comparison) {
            NotThat(actual, Matchers.Equal(expected, comparison));
        }

        public void NotEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual) {
            NotEqual<IEnumerable<TSource>>(expected, actual, Assert.GetEqualityComparer<IEnumerable<TSource>>(true));
        }

        public void NotEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer) {
            NotEqual<IEnumerable<TSource>>(expected, actual, Assert.GetEqualityComparer<IEnumerable<TSource>>(true, new Assert.AssertEqualityComparerAdapter<TSource>(comparer)));
        }

        public void Equal<T>(T expected, T actual, string message, params object[] args) {
            That(actual, Matchers.Equal(expected), message, args);
        }

        public void Equal<T>(T expected, T actual, Comparison<T> comparison, string message, params object[] args) {
            That(actual, Matchers.Equal<T>(expected, comparison), message, args);
        }

        public void Equal<T>(T expected, T actual, IEqualityComparer<T> comparer, string message, params object[] args) {
            That(actual, Matchers.Equal<T>(expected, comparer), message, args);
        }

        public void Equal(string expected, string actual, StringComparison comparison, string message, params object[] args) {
            That(actual, Matchers.Equal(expected, comparison), message, args);
        }

        public void Equal<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, string message, params object[] args) {
            Equal<IEnumerable<TSource>>(expected, actual, Assert.GetEqualityComparer<IEnumerable<TSource>>(true), message, args);
        }

        public void Equal<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer, string message, params object[] args) {
            Equal<IEnumerable<TSource>>(expected, actual, Assert.GetEqualityComparer<IEnumerable<TSource>>(true, new Assert.AssertEqualityComparerAdapter<TSource>(comparer)), message, args);
        }

        public void Equal<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison, string message, params object[] args) {
            Equal<IEnumerable<TSource>>(expected, actual, Assert.GetEqualityComparer<IEnumerable<TSource>>(true, new Assert.EqualityComparisonAdapter<TSource>(comparison)), message, args);
        }

        public void NotEqual<T>(T expected, T actual, string message, params object[] args) {
            NotEqual(expected, actual, Assert.GetEqualityComparer<T>(), message, args);
        }

        public void NotEqual<T>(T expected, T actual, IEqualityComparer<T> comparer, string message, params object[] args) {
            NotThat(actual, Matchers.Equal(expected, comparer), message, args);
        }

        public void NotEqual<T>(T expected, T actual, Comparison<T> comparison, string message, params object[] args) {
            NotThat(actual, Matchers.Equal(expected, comparison), message, args);
        }

        public void NotEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, string message, params object[] args) {
            NotEqual<IEnumerable<TSource>>(expected, actual, Assert.GetEqualityComparer<IEnumerable<TSource>>(true), message, args);
        }

        public void NotEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer, string message, params object[] args) {
            NotEqual<IEnumerable<TSource>>(expected, actual, Assert.GetEqualityComparer<IEnumerable<TSource>>(true, new Assert.AssertEqualityComparerAdapter<TSource>(comparer)), message, args);
        }
    }

    partial class Assert {

        public static void Equal<T>(T expected, T actual) {
            Global.Equal<T>(expected, actual);
        }

        public static void Equal<T>(T expected, T actual, Comparison<T> comparison) {
            Global.Equal<T>(expected, actual, comparison);
        }

        public static void Equal<T>(T expected, T actual, IEqualityComparer<T> comparer) {
            Global.Equal<T>(expected, actual, comparer);
        }

        public static void Equal(string expected, string actual, StringComparison comparison) {
            Global.Equal(expected, actual, comparison);
        }

        public static void Equal<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual) {
            Global.Equal<TSource>(expected, actual);
        }

        public static void Equal<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer) {
            Global.Equal<TSource>(expected, actual, comparer);
        }

        public static void Equal<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison) {
            Global.Equal<TSource>(expected, actual, comparison);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This is an override of Object.Equals(). Call Assert.Equal() instead.", true)]
        public new static bool Equals(object a, object b) {
            throw new InvalidOperationException("Assert.Equals should not be used");
        }

        public static void NotEqual<T>(T expected, T actual) {
            Global.NotEqual<T>(expected, actual);
        }

        public static void NotEqual<T>(T expected, T actual, IEqualityComparer<T> comparer) {
            Global.NotEqual<T>(expected, actual, comparer);
        }

        public static void NotEqual<T>(T expected, T actual, Comparison<T> comparison) {
            Global.NotEqual<T>(expected, actual, comparison);
        }

        public static void NotEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual) {
            Global.NotEqual<TSource>(expected, actual);
        }

        public static void NotEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer) {
            Global.NotEqual<TSource>(expected, actual, comparer);
        }

        public static void Equal<T>(T expected, T actual, string message, params object[] args) {
            Global.Equal<T>(expected, actual, message, (object[]) args);
        }

        public static void Equal<T>(T expected, T actual, Comparison<T> comparison, string message, params object[] args) {
            Global.Equal<T>(expected, actual, comparison, message, (object[]) args);
        }

        public static void Equal<T>(T expected, T actual, IEqualityComparer<T> comparer, string message, params object[] args) {
            Global.Equal<T>(expected, actual, comparer, message, (object[]) args);
        }

        public static void Equal(string expected, string actual, StringComparison comparison, string message, params object[] args) {
            Global.Equal(expected, actual, comparison, message, (object[]) args);
        }

        public static void Equal<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, string message, params object[] args) {
            Global.Equal<TSource>(expected, actual, message, (object[]) args);
        }

        public static void Equal<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer, string message, params object[] args) {
            Global.Equal<TSource>(expected, actual, comparer, message, (object[]) args);
        }

        public static void Equal<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison, string message, params object[] args) {
            Global.Equal<TSource>(expected, actual, comparison, message, (object[]) args);
        }

        public static void NotEqual<T>(T expected, T actual, string message, params object[] args) {
            Global.NotEqual<T>(expected, actual, message, (object[]) args);
        }

        public static void NotEqual<T>(T expected, T actual, IEqualityComparer<T> comparer, string message, params object[] args) {
            Global.NotEqual<T>(expected, actual, comparer, message, (object[]) args);
        }

        public static void NotEqual<T>(T expected, T actual, Comparison<T> comparison, string message, params object[] args) {
            Global.NotEqual<T>(expected, actual, comparison, message, (object[]) args);
        }

        public static void NotEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, string message, params object[] args) {
            Global.NotEqual<TSource>(expected, actual, message, (object[]) args);
        }

        public static void NotEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer, string message, params object[] args) {
            Global.NotEqual<TSource>(expected, actual, comparer, message, (object[]) args);
        }
    }

    partial class Assume {

        public static void Equal<T>(T expected, T actual) {
            Global.Equal<T>(expected, actual);
        }

        public static void Equal<T>(T expected, T actual, Comparison<T> comparison) {
            Global.Equal<T>(expected, actual, comparison);
        }

        public static void Equal<T>(T expected, T actual, IEqualityComparer<T> comparer) {
            Global.Equal<T>(expected, actual, comparer);
        }

        public static void Equal(string expected, string actual, StringComparison comparison) {
            Global.Equal(expected, actual, comparison);
        }

        public static void Equal<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual) {
            Global.Equal<TSource>(expected, actual);
        }

        public static void Equal<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer) {
            Global.Equal<TSource>(expected, actual, comparer);
        }

        public static void Equal<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison) {
            Global.Equal<TSource>(expected, actual, comparison);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This is an override of Object.Equals(). Call Assert.Equal() instead.", true)]
        public new static bool Equals(object a, object b) {
            throw new InvalidOperationException("Assert.Equals should not be used");
        }

        public static void NotEqual<T>(T expected, T actual) {
            Global.NotEqual<T>(expected, actual);
        }

        public static void NotEqual<T>(T expected, T actual, IEqualityComparer<T> comparer) {
            Global.NotEqual<T>(expected, actual, comparer);
        }

        public static void NotEqual<T>(T expected, T actual, Comparison<T> comparison) {
            Global.NotEqual<T>(expected, actual, comparison);
        }

        public static void NotEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual) {
            Global.NotEqual<TSource>(expected, actual);
        }

        public static void NotEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer) {
            Global.NotEqual<TSource>(expected, actual, comparer);
        }

        public static void Equal<T>(T expected, T actual, string message, params object[] args) {
            Global.Equal<T>(expected, actual, message, (object[]) args);
        }

        public static void Equal<T>(T expected, T actual, Comparison<T> comparison, string message, params object[] args) {
            Global.Equal<T>(expected, actual, comparison, message, (object[]) args);
        }

        public static void Equal<T>(T expected, T actual, IEqualityComparer<T> comparer, string message, params object[] args) {
            Global.Equal<T>(expected, actual, comparer, message, (object[]) args);
        }

        public static void Equal(string expected, string actual, StringComparison comparison, string message, params object[] args) {
            Global.Equal(expected, actual, comparison, message, (object[]) args);
        }

        public static void Equal<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, string message, params object[] args) {
            Global.Equal<TSource>(expected, actual, message, (object[]) args);
        }

        public static void Equal<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer, string message, params object[] args) {
            Global.Equal<TSource>(expected, actual, comparer, message, (object[]) args);
        }

        public static void Equal<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, Comparison<TSource> comparison, string message, params object[] args) {
            Global.Equal<TSource>(expected, actual, comparison, message, (object[]) args);
        }

        public static void NotEqual<T>(T expected, T actual, string message, params object[] args) {
            Global.NotEqual<T>(expected, actual, message, (object[]) args);
        }

        public static void NotEqual<T>(T expected, T actual, IEqualityComparer<T> comparer, string message, params object[] args) {
            Global.NotEqual<T>(expected, actual, comparer, message, (object[]) args);
        }

        public static void NotEqual<T>(T expected, T actual, Comparison<T> comparison, string message, params object[] args) {
            Global.NotEqual<T>(expected, actual, comparison, message, (object[]) args);
        }

        public static void NotEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, string message, params object[] args) {
            Global.NotEqual<TSource>(expected, actual, message, (object[]) args);
        }

        public static void NotEqual<TSource>(IEnumerable<TSource> expected, IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer, string message, params object[] args) {
            Global.NotEqual<TSource>(expected, actual, comparer, message, (object[]) args);
        }
    }



    namespace TestMatchers {

        public class EqualMatcher<T> : TestMatcher<T>, ITestMatcherWithComparer<T> {

            public T Expected { get; private set; }
            public IEqualityComparer<T> Comparer { get; private set; }

            public EqualMatcher(T expected, IEqualityComparer<T> comparer = null) {
                Expected = expected;
                Comparer = comparer;
            }

            public EqualMatcher<T> WithComparer(IEqualityComparer<T> comparer) {
                return new EqualMatcher<T>(Expected, comparer);
            }

            public EqualMatcher<T> WithComparer(IComparer<T> comparer) {
                if (comparer == null) {
                    return new EqualMatcher<T>(Expected, null);
                }
                return new EqualMatcher<T>(Expected, new Assert.EqualityComparerAdapter<T>(comparer));
            }

            public EqualMatcher<T> WithComparison(Comparison<T> comparison) {
                return new EqualMatcher<T>(Expected, new Assert.EqualityComparisonAdapter<T>(comparison));
            }

            public EqualMatcher<T> OrClose(T epsilon) {
                return WithComparer(EpsilonComparer.Create<T>(epsilon));
            }

            public EqualMatcher<T> OrClose<TEpsilon>(TEpsilon epsilon) {
                return WithComparer(EpsilonComparer.Create<T, TEpsilon>(epsilon));
            }

            public override bool Matches(T actual) {
                var comparer = Comparer;
                if (comparer == null) {
                    comparer = Assert.GetEqualityComparer<T>();
                }
                return comparer.Equals(Expected, actual);
            }

            ITestMatcher<T> ITestMatcherWithComparer<T>.WithComparer(IComparer<T> comparer) {
                return WithComparer(comparer);
            }
        }
    }

}
