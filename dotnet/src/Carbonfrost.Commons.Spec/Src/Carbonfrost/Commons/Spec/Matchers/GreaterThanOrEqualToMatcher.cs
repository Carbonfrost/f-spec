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
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.Commons.Spec {

    partial class Matchers {

        public static GreaterThanOrEqualToMatcher<T> BeGreaterThanOrEqualTo<T>(T expected) {
            return new GreaterThanOrEqualToMatcher<T>(expected);
        }

        public static GreaterThanOrEqualToMatcher<T> BeGreaterThanOrEqualTo<T>(T expected, IComparer<T> comparer) {
            return new GreaterThanOrEqualToMatcher<T>(expected, comparer);
        }

        public static GreaterThanOrEqualToMatcher<T> BeGreaterThanOrEqualTo<T>(T expected, Comparison<T> comparison) {
            return new GreaterThanOrEqualToMatcher<T>(expected, Comparer<T>.Create(comparison));
        }

    }

    static partial class Extensions {

        public static void GreaterThanOrEqualTo<T>(this Expectation<T> e, T expected) {
            Operators.GreaterThanOrEqualTo.Apply(e, expected, (string) null);
        }

        public static void GreaterThanOrEqualTo<T>(this Expectation<T> e, T expected, IComparer<T> comparer) {
            Operators.GreaterThanOrEqualTo.Apply(e, expected, comparer, null);
        }

        public static void GreaterThanOrEqualTo<T>(this Expectation<T> e, T expected, Comparison<T> comparison) {
            Operators.GreaterThanOrEqualTo.Apply(e, expected, comparison, null);
        }

        public static void GreaterThanOrEqualTo<T>(this Expectation<T> e, T expected, string message, params object[] args) {
            Operators.GreaterThanOrEqualTo.Apply(e, expected, message, (object[]) args);
        }

        public static void GreaterThanOrEqualTo<T>(this Expectation<T> e, T expected, IComparer<T> comparer, string message, params object[] args) {
            Operators.GreaterThanOrEqualTo.Apply(e, expected, comparer, message, (object[]) args);
        }

        public static void GreaterThanOrEqualTo<T>(this Expectation<T> e, T expected, Comparison<T> comparison, string message, params object[] args) {
            Operators.GreaterThanOrEqualTo.Apply(e, expected, comparison, message, (object[]) args);
        }
    }

    partial class Asserter {

        public void GreaterThanOrEqualTo<T>(T expected, T actual) {
            GreaterThanOrEqualTo<T>(expected, actual, Comparer<T>.Default);
        }

        public void GreaterThanOrEqualTo<T>(T expected, T actual, IComparer<T> comparer) {
            That(actual, Matchers.BeGreaterThanOrEqualTo(expected, comparer));
        }

        public void GreaterThanOrEqualTo<T>(T expected, T actual, Comparison<T> comparison) {
            That(actual, Matchers.BeGreaterThanOrEqualTo(expected, comparison));
        }

        public void GreaterThanOrEqualTo<T>(T expected, T actual, string message, params object[] args) {
            GreaterThanOrEqualTo<T>(expected, actual, Comparer<T>.Default, message, args);
        }

        public void GreaterThanOrEqualTo<T>(T expected, T actual, IComparer<T> comparer, string message, params object[] args) {
            That(actual, Matchers.BeGreaterThanOrEqualTo(expected, comparer), message, args);
        }

        public void GreaterThanOrEqualTo<T>(T expected, T actual, Comparison<T> comparison, string message, params object[] args) {
            That(actual, Matchers.BeGreaterThanOrEqualTo(expected, comparison), message, args);
        }

    }

    partial class Assert {

        public static void GreaterThanOrEqualTo<T>(T expected, T actual) {
            Global.GreaterThanOrEqualTo<T>(expected, actual);
        }

        public static void GreaterThanOrEqualTo<T>(T expected, T actual, IComparer<T> comparer) {
            Global.GreaterThanOrEqualTo<T>(expected, actual, comparer);
        }

        public static void GreaterThanOrEqualTo<T>(T expected, T actual, Comparison<T> comparison) {
            Global.GreaterThanOrEqualTo<T>(expected, actual, comparison);
        }

        public static void GreaterThanOrEqualTo<T>(T expected, T actual, string message, params object[] args) {
            Global.GreaterThanOrEqualTo<T>(expected, actual, message, (object[]) args);
        }

        public static void GreaterThanOrEqualTo<T>(T expected, T actual, IComparer<T> comparer, string message, params object[] args) {
            Global.GreaterThanOrEqualTo<T>(expected, actual, comparer, message, (object[]) args);
        }

        public static void GreaterThanOrEqualTo<T>(T expected, T actual, Comparison<T> comparison, string message, params object[] args) {
            Global.GreaterThanOrEqualTo<T>(expected, actual, comparison, message, (object[]) args);
        }

    }

    partial class Assume {

        public static void GreaterThanOrEqualTo<T>(T expected, T actual) {
            Global.GreaterThanOrEqualTo<T>(expected, actual);
        }

        public static void GreaterThanOrEqualTo<T>(T expected, T actual, IComparer<T> comparer) {
            Global.GreaterThanOrEqualTo<T>(expected, actual, comparer);
        }

        public static void GreaterThanOrEqualTo<T>(T expected, T actual, Comparison<T> comparison) {
            Global.GreaterThanOrEqualTo<T>(expected, actual, comparison);
        }

        public static void GreaterThanOrEqualTo<T>(T expected, T actual, string message, params object[] args) {
            Global.GreaterThanOrEqualTo<T>(expected, actual, message, (object[]) args);
        }

        public static void GreaterThanOrEqualTo<T>(T expected, T actual, IComparer<T> comparer, string message, params object[] args) {
            Global.GreaterThanOrEqualTo<T>(expected, actual, comparer, message, (object[]) args);
        }

        public static void GreaterThanOrEqualTo<T>(T expected, T actual, Comparison<T> comparison, string message, params object[] args) {
            Global.GreaterThanOrEqualTo<T>(expected, actual, comparison, message, (object[]) args);
        }

    }


    namespace TestMatchers {

        public class GreaterThanOrEqualToMatcher<T> : TestMatcher<T>, ITestMatcherWithComparer<T> {

            public T Expected {
                get;
                private set;
            }

            public IComparer<T> Comparer {
                get;
                private set;
            }

            public GreaterThanOrEqualToMatcher(T expected, IComparer<T> comparer = null) {
                Expected = expected;
                Comparer = comparer ?? Comparer<T>.Default;
            }

            public GreaterThanOrEqualToMatcher<T> WithComparer(IComparer<T> comparer) {
                return new GreaterThanOrEqualToMatcher<T>(Expected, comparer);
            }

            public GreaterThanOrEqualToMatcher<T> WithComparison(Comparison<T> comparison) {
                return new GreaterThanOrEqualToMatcher<T>(Expected, Comparer<T>.Create(comparison));
            }

            public GreaterThanOrEqualToMatcher<T> OrClose(T epsilon) {
                return WithComparer(EpsilonComparer.Create(epsilon));
            }

            public GreaterThanOrEqualToMatcher<T> OrClose<TEpsilon>(TEpsilon epsilon) {
                return WithComparer(EpsilonComparer.Create<T, TEpsilon>(epsilon));
            }

            public override bool Matches(T actual) {
                return CompareSafely(Comparer, actual, Expected) >= 0;
            }

            ITestMatcher<T> ITestMatcherWithComparer<T>.WithComparer(IComparer<T> comparer) {
                return WithComparer(comparer);
            }
        }

        class GreaterThanOrEqualToOperator : ComparisonOperator {

            protected override ITestMatcher<T> CreateMatcher<T>(T expected) {
                return Matchers.BeGreaterThanOrEqualTo(expected);
            }

            protected override ITestMatcher<T> CreateMatcher<T>(T expected, IComparer<T> comparer) {
                return Matchers.BeGreaterThanOrEqualTo(expected, comparer);
            }

            protected override ITestMatcher<T> CreateMatcher<T>(T expected, Comparison<T> comparison) {
                return Matchers.BeGreaterThanOrEqualTo(expected, comparison);
            }
        }

        static partial class Operators {
            internal static readonly IComparisonOperator GreaterThanOrEqualTo = new GreaterThanOrEqualToOperator();
        }
    }
}
