//
// Copyright 2017, 2019 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

        public static GreaterThanMatcher<T> BeGreaterThan<T>(T expected) {
            return new GreaterThanMatcher<T>(expected);
        }

        public static GreaterThanMatcher<T> BeGreaterThan<T>(T expected, IComparer<T> comparer) {
            return new GreaterThanMatcher<T>(expected, comparer);
        }

        public static GreaterThanMatcher<T> BeGreaterThan<T>(T expected, Comparison<T> comparison) {
            return new GreaterThanMatcher<T>(expected, Comparer<T>.Create(comparison));
        }

    }

    static partial class Extensions {

        public static void GreaterThan<T>(this IExpectation<T> e, T expected) {
            Operators.GreaterThan.Apply(e, expected, (string) null);
        }

        public static void GreaterThan<T>(this IExpectation<T> e, T expected, IComparer<T> comparer) {
            Operators.GreaterThan.Apply(e, expected, comparer, null);
        }

        public static void GreaterThan<T>(this IExpectation<T> e, T expected, Comparison<T> comparison) {
            Operators.GreaterThan.Apply(e, expected, comparison, null);
        }

        public static void GreaterThan<T>(this IExpectation<T> e, T expected, string message, params object[] args) {
            Operators.GreaterThan.Apply(e, expected, message, (object[]) args);
        }

        public static void GreaterThan<T>(this IExpectation<T> e, T expected, IComparer<T> comparer, string message, params object[] args) {
            Operators.GreaterThan.Apply(e, expected, comparer, message, (object[]) args);
        }

        public static void GreaterThan<T>(this IExpectation<T> e, T expected, Comparison<T> comparison, string message, params object[] args) {
            Operators.GreaterThan.Apply(e, expected, comparison, message, (object[]) args);
        }
    }

    partial class Asserter {

        public void GreaterThan<T>(T expected, T actual) {
            GreaterThan<T>(expected, actual, Comparer<T>.Default);
        }

        public void GreaterThan<T>(T expected, T actual, IComparer<T> comparer) {
            That(actual, Matchers.BeGreaterThan(expected, comparer));
        }

        public void GreaterThan<T>(T expected, T actual, Comparison<T> comparison) {
            That(actual, Matchers.BeGreaterThan(expected, comparison));
        }

        public void GreaterThan<T>(T expected, T actual, string message, params object[] args) {
            GreaterThan<T>(expected, actual, Comparer<T>.Default, message, args);
        }

        public void GreaterThan<T>(T expected, T actual, IComparer<T> comparer, string message, params object[] args) {
            That(actual, Matchers.BeGreaterThan(expected, comparer), message, args);
        }

        public void GreaterThan<T>(T expected, T actual, Comparison<T> comparison, string message, params object[] args) {
            That(actual, Matchers.BeGreaterThan(expected, comparison), message, args);
        }
    }

    partial class Assert {

        public static void GreaterThan<T>(T expected, T actual) {
            Global.GreaterThan<T>(expected, actual);
        }

        public static void GreaterThan<T>(T expected, T actual, IComparer<T> comparer) {
            Global.GreaterThan<T>(expected, actual, comparer);
        }

        public static void GreaterThan<T>(T expected, T actual, Comparison<T> comparison) {
            Global.GreaterThan<T>(expected, actual, comparison);
        }

        public static void GreaterThan<T>(T expected, T actual, string message, params object[] args) {
            Global.GreaterThan<T>(expected, actual, message, (object[]) args);
        }

        public static void GreaterThan<T>(T expected, T actual, IComparer<T> comparer, string message, params object[] args) {
            Global.GreaterThan<T>(expected, actual, comparer, message, (object[]) args);
        }

        public static void GreaterThan<T>(T expected, T actual, Comparison<T> comparison, string message, params object[] args) {
            Global.GreaterThan<T>(expected, actual, comparison, message, (object[]) args);
        }
    }

    partial class Assume {

        public static void GreaterThan<T>(T expected, T actual) {
            Global.GreaterThan<T>(expected, actual);
        }

        public static void GreaterThan<T>(T expected, T actual, IComparer<T> comparer) {
            Global.GreaterThan<T>(expected, actual, comparer);
        }

        public static void GreaterThan<T>(T expected, T actual, Comparison<T> comparison) {
            Global.GreaterThan<T>(expected, actual, comparison);
        }

        public static void GreaterThan<T>(T expected, T actual, string message, params object[] args) {
            Global.GreaterThan<T>(expected, actual, message, (object[]) args);
        }

        public static void GreaterThan<T>(T expected, T actual, IComparer<T> comparer, string message, params object[] args) {
            Global.GreaterThan<T>(expected, actual, comparer, message, (object[]) args);
        }

        public static void GreaterThan<T>(T expected, T actual, Comparison<T> comparison, string message, params object[] args) {
            Global.GreaterThan<T>(expected, actual, comparison, message, (object[]) args);
        }
    }


    namespace TestMatchers {

        public class GreaterThanMatcher<T> : TestMatcher<T>, ITestMatcherWithComparer<T> {

            public T Expected { get; private set; }
            public IComparer<T> Comparer { get; private set; }

            public GreaterThanMatcher(T expected, IComparer<T> comparer = null) {
                Expected = expected;
                Comparer = comparer ?? Comparer<T>.Default;
            }

            public GreaterThanMatcher<T> WithComparer(IComparer<T> comparer) {
                return new GreaterThanMatcher<T>(Expected, comparer);
            }

            public GreaterThanMatcher<T> WithComparison(Comparison<T> comparison) {
                return new GreaterThanMatcher<T>(Expected, Comparer<T>.Create(comparison));
            }

            public GreaterThanMatcher<T> OrClose(T epsilon) {
                return WithComparer(EpsilonComparer.Create(epsilon));
            }

            public GreaterThanMatcher<T> OrClose<TEpsilon>(TEpsilon epsilon) {
                return WithComparer(EpsilonComparer.Create<T, TEpsilon>(epsilon));
            }

            public override bool Matches(T actual) {
                return CompareSafely(Comparer, actual, Expected) > 0;
            }

            ITestMatcher<T> ITestMatcherWithComparer<T>.WithComparer(IComparer<T> comparer) {
                return WithComparer(comparer);
            }
        }

        class GreaterThanOperator : ComparisonOperator {

            protected override ITestMatcher<T> CreateMatcher<T>(T expected) {
                return Matchers.BeGreaterThan(expected);
            }

            protected override ITestMatcher<T> CreateMatcher<T>(T expected, IComparer<T> comparer) {
                return Matchers.BeGreaterThan(expected, comparer);
            }

            protected override ITestMatcher<T> CreateMatcher<T>(T expected, Comparison<T> comparison) {
                return Matchers.BeGreaterThan(expected, comparison);
            }
        }

        static partial class Operators {
            internal static readonly IComparisonOperator GreaterThan = new GreaterThanOperator();
        }
    }
}
