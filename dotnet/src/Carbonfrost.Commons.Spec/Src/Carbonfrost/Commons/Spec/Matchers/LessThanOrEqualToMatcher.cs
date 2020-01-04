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
using System.Linq;
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.Commons.Spec {

    partial class Matchers {

        public static LessThanOrEqualToMatcher<T> BeLessThanOrEqualTo<T>(T expected) {
            return new LessThanOrEqualToMatcher<T>(expected);
        }

        public static LessThanOrEqualToMatcher<T> BeLessThanOrEqualTo<T>(T expected, IComparer<T> comparer) {
            return new LessThanOrEqualToMatcher<T>(expected, comparer);
        }

        public static LessThanOrEqualToMatcher<T> BeLessThanOrEqualTo<T>(T expected, Comparison<T> comparison) {
            return new LessThanOrEqualToMatcher<T>(expected, Comparer<T>.Create(comparison));
        }

    }

    static partial class Extensions {

        public static void LessThanOrEqualTo<T>(this Expectation<T> e, T expected) {
            LessThanOrEqualTo(e, expected, (string) null);
        }

        public static void LessThanOrEqualTo<T>(this Expectation<T> e, T expected, IComparer<T> comparer) {
            LessThanOrEqualTo(e, expected, comparer, null);
        }

        public static void LessThanOrEqualTo<T>(this Expectation<T> e, T expected, Comparison<T> comparison) {
            LessThanOrEqualTo(e, expected, comparison, null);
        }

		public static void LessThanOrEqualTo<T>(this Expectation<T> e, T expected, string message, params object[] args) {
            e.Should(Matchers.BeLessThanOrEqualTo<T>(expected), message, (object[]) args);
        }

        public static void LessThanOrEqualTo<T>(this Expectation<T> e, T expected, IComparer<T> comparer, string message, params object[] args) {
            e.Should(Matchers.BeLessThanOrEqualTo<T>(expected, comparer), message, (object[]) args);
        }

        public static void LessThanOrEqualTo<T>(this Expectation<T> e, T expected, Comparison<T> comparison, string message, params object[] args) {
            e.Should(Matchers.BeLessThanOrEqualTo<T>(expected, comparison), message, (object[]) args);
        }
    }

    partial class Asserter {

        public void LessThanOrEqualTo<T>(T expected, T actual) {
            LessThanOrEqualTo<T>(expected, actual, Comparer<T>.Default);
        }

        public void LessThanOrEqualTo<T>(T expected, T actual, IComparer<T> comparer) {
            That(actual, Matchers.BeLessThanOrEqualTo(expected, comparer));
        }

        public void LessThanOrEqualTo<T>(T expected, T actual, Comparison<T> comparison) {
            That(actual, Matchers.BeLessThanOrEqualTo(expected, comparison));
        }

        public void LessThanOrEqualTo<T>(T expected, T actual, string message, params object[] args) {
            LessThanOrEqualTo<T>(expected, actual, Comparer<T>.Default, message, args);
        }

        public void LessThanOrEqualTo<T>(T expected, T actual, IComparer<T> comparer, string message, params object[] args) {
            That(actual, Matchers.BeLessThanOrEqualTo(expected, comparer), message, args);
        }

        public void LessThanOrEqualTo<T>(T expected, T actual, Comparison<T> comparison, string message, params object[] args) {
            That(actual, Matchers.BeLessThanOrEqualTo(expected, comparison), message, args);
        }

    }

    partial class Assert {

        public static void LessThanOrEqualTo<T>(T expected, T actual) {
            Global.LessThanOrEqualTo<T>(expected, actual);
        }

        public static void LessThanOrEqualTo<T>(T expected, T actual, IComparer<T> comparer) {
            Global.LessThanOrEqualTo<T>(expected, actual, comparer);
        }

        public static void LessThanOrEqualTo<T>(T expected, T actual, Comparison<T> comparison) {
            Global.LessThanOrEqualTo<T>(expected, actual, comparison);
        }

        public static void LessThanOrEqualTo<T>(T expected, T actual, string message, params object[] args) {
            Global.LessThanOrEqualTo<T>(expected, actual, message, (object[]) args);
        }

        public static void LessThanOrEqualTo<T>(T expected, T actual, IComparer<T> comparer, string message, params object[] args) {
            Global.LessThanOrEqualTo<T>(expected, actual, comparer, message, (object[]) args);
        }

        public static void LessThanOrEqualTo<T>(T expected, T actual, Comparison<T> comparison, string message, params object[] args) {
            Global.LessThanOrEqualTo<T>(expected, actual, comparison, message, (object[]) args);
        }

    }

    partial class Assume {

        public static void LessThanOrEqualTo<T>(T expected, T actual) {
            Global.LessThanOrEqualTo<T>(expected, actual);
        }

        public static void LessThanOrEqualTo<T>(T expected, T actual, IComparer<T> comparer) {
            Global.LessThanOrEqualTo<T>(expected, actual, comparer);
        }

        public static void LessThanOrEqualTo<T>(T expected, T actual, Comparison<T> comparison) {
            Global.LessThanOrEqualTo<T>(expected, actual, comparison);
        }

        public static void LessThanOrEqualTo<T>(T expected, T actual, string message, params object[] args) {
            Global.LessThanOrEqualTo<T>(expected, actual, message, (object[]) args);
        }

        public static void LessThanOrEqualTo<T>(T expected, T actual, IComparer<T> comparer, string message, params object[] args) {
            Global.LessThanOrEqualTo<T>(expected, actual, comparer, message, (object[]) args);
        }

        public static void LessThanOrEqualTo<T>(T expected, T actual, Comparison<T> comparison, string message, params object[] args) {
            Global.LessThanOrEqualTo<T>(expected, actual, comparison, message, (object[]) args);
        }

    }


    namespace TestMatchers {

        public class LessThanOrEqualToMatcher<T> : TestMatcher<T>, ITestMatcherWithComparer<T> {

            public T Expected { get; private set; }
            public IComparer<T> Comparer { get; private set; }

            public LessThanOrEqualToMatcher(T expected, IComparer<T> comparer = null) {
                Expected = expected;
                Comparer = comparer ?? Comparer<T>.Default;
            }

            public LessThanOrEqualToMatcher<T> WithComparer(IComparer<T> comparer) {
                return new LessThanOrEqualToMatcher<T>(Expected, comparer);
            }

            public LessThanOrEqualToMatcher<T> WithComparison(Comparison<T> comparison) {
                return new LessThanOrEqualToMatcher<T>(Expected, Comparer<T>.Create(comparison));
            }

            public LessThanOrEqualToMatcher<T> OrClose(T epsilon) {
                return WithComparer(EpsilonComparer.Create(epsilon));
            }

            public LessThanOrEqualToMatcher<T> OrClose<TEpsilon>(TEpsilon epsilon) {
                return WithComparer(EpsilonComparer.Create<T, TEpsilon>(epsilon));
            }

            public override bool Matches(T actual) {
                return CompareSafely(Comparer, actual, Expected) <= 0;
            }

            ITestMatcher<T> ITestMatcherWithComparer<T>.WithComparer(IComparer<T> comparer) {
                return WithComparer(comparer);
            }
        }
    }
}
