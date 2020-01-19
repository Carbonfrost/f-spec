//
// Copyright 2018-2019 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

        public static BetweenMatcher<T> BeBetween<T>(T low, T high) {
            return new BetweenMatcher<T>(low, high, null);
        }

        public static BetweenMatcher<T> BeBetween<T>(T low, T high, IComparer<T> comparer) {
            return new BetweenMatcher<T>(low, high, comparer);
        }

        public static BetweenMatcher<T> BeBetween<T>(T low, T high, Comparison<T> comparison) {
            return new BetweenMatcher<T>(low, high, Comparer<T>.Create(comparison));
        }

        public static BetweenMatcher<string> BeBetween(string low, string high, StringComparison comparison) {
            return new BetweenMatcher<string>(low, high, comparison.MapComparer());
        }

    }

    static partial class Extensions {

        public static void Between<T>(this Expectation<T> e, T low, T high) {
            Between<T>(e, low, high, (string) null);
        }

        public static void Between<T>(this Expectation<T> e, T low, T high, Comparison<T> comparison) {
            Between<T>(e, low, high, comparison, null);
        }

        public static void Between(this Expectation<string> e, string low, string high, StringComparison comparison) {
            Between(e, low, high, comparison, null);
        }

        public static void Between<T>(this Expectation<T> e, T low, T high, IComparer<T> comparer) {
            Between(e, low, high, comparer, (string) null);
        }

        public static void Between<T>(this Expectation<T> e, T low, T high, string message, params object[] args) {
            e.Should(Matchers.BeBetween(low, high), message, (object[]) args);
        }

        public static void Between<T>(this Expectation<T> e, T low, T high, Comparison<T> comparison, string message, params object[] args) {
            e.Should(Matchers.BeBetween(low, high, comparison), message, (object[]) args);
        }

        public static void Between(this Expectation<string> e, string low, string high, StringComparison comparison, string message, params object[] args) {
            e.Should(Matchers.BeBetween(low, high, comparison), message, (object[]) args);
        }

        public static void Between<T>(this Expectation<T> e, T low, T high, IComparer<T> comparer, string message, params object[] args) {
            e.Should(Matchers.BeBetween(low, high, comparer), message, (object[]) args);
        }

    }

    partial class Asserter {

        public void Between<T>(T low, T high, T actual) {
            That(actual, Matchers.BeBetween(low, high));
        }

        public void Between<T>(T low, T high, T actual, Comparison<T> comparison) {
            That(actual, Matchers.BeBetween<T>(low, high, comparison));
        }

        public void Between<T>(T low, T high, T actual, IComparer<T> comparer) {
            That(actual, Matchers.BeBetween<T>(low, high, comparer));
        }

        public void Between<T>(string low, string high, string actual, StringComparison comparison) {
            That(actual, Matchers.BeBetween(low, high, comparison));
        }

        public void NotBetween<T>(T low, T high, T actual) {
            NotBetween(low, high, actual, (string) null);
        }

        public void NotBetween<T>(T low, T high, T actual, IComparer<T> comparer) {
            NotThat(actual, Matchers.BeBetween(low, high, comparer));
        }

        public void NotBetween<T>(T low, T high, T actual, Comparison<T> comparison) {
            NotThat(actual, Matchers.BeBetween(low, high, comparison));
        }

        public void Between<T>(T low, T high, T actual, string message, params object[] args) {
            That(actual, Matchers.BeBetween(low, high), message, args);
        }

        public void Between<T>(T low, T high, T actual, Comparison<T> comparison, string message, params object[] args) {
            That(actual, Matchers.BeBetween<T>(low, high, comparison), message, args);
        }

        public void Between<T>(T low, T high, T actual, IComparer<T> comparer, string message, params object[] args) {
            That(actual, Matchers.BeBetween<T>(low, high, comparer), message, args);
        }

        public void Between(string low, string high, string actual, StringComparison comparison, string message, params object[] args) {
            That(actual, Matchers.BeBetween(low, high, comparison), message, args);
        }

        public void NotBetween<T>(T low, T high, T actual, string message, params object[] args) {
            NotBetween(low, high, actual, (IComparer<T>) null, message, (object[]) args);
        }

        public void NotBetween<T>(T low, T high, T actual, IComparer<T> comparer, string message, params object[] args) {
            NotThat(actual, Matchers.BeBetween(low, high, comparer), message, (object[]) args);
        }

        public void NotBetween<T>(T low, T high, T actual, Comparison<T> comparison, string message, params object[] args) {
            NotThat(actual, Matchers.BeBetween(low, high, comparison), message, (object[]) args);
        }

    }

    partial class Assert {

        public static void Between<T>(T low, T high, T actual) {
            Global.Between<T>(low, high, actual);
        }

        public static void Between<T>(T low, T high, T actual, Comparison<T> comparison) {
            Global.Between<T>(low, high, actual, comparison);
        }

        public static void Between<T>(T low, T high, T actual, IComparer<T> comparer) {
            Global.Between<T>(low, high, actual, comparer);
        }

        public static void Between<T>(string low, string high, string actual, StringComparison comparison) {
            Global.Between<T>(low, high, actual, comparison);
        }

        public static void NotBetween<T>(T low, T high, T actual) {
            Global.NotBetween<T>(low, high, actual);
        }

        public static void NotBetween<T>(T low, T high, T actual, IComparer<T> comparer) {
            Global.NotBetween<T>(low, high, actual, comparer);
        }

        public static void NotBetween<T>(T low, T high, T actual, Comparison<T> comparison) {
            Global.NotBetween<T>(low, high, actual, comparison);
        }

        public static void Between<T>(T low, T high, T actual, string message, params object[] args) {
            Global.Between<T>(low, high, actual, message, (object[]) args);
        }

        public static void Between<T>(T low, T high, T actual, Comparison<T> comparison, string message, params object[] args) {
            Global.Between<T>(low, high, actual, comparison, message, (object[]) args);
        }

        public static void Between<T>(T low, T high, T actual, IComparer<T> comparer, string message, params object[] args) {
            Global.Between<T>(low, high, actual, comparer, message, (object[]) args);
        }

        public static void Between(string low, string high, string actual, StringComparison comparison, string message, params object[] args) {
            Global.Between(low, high, actual, comparison, message, (object[]) args);
        }

        public static void NotBetween<T>(T low, T high, T actual, string message, params object[] args) {
            Global.NotBetween<T>(low, high, actual, message, (object[]) args);
        }

        public static void NotBetween<T>(T low, T high, T actual, IComparer<T> comparer, string message, params object[] args) {
            Global.NotBetween<T>(low, high, actual, comparer, message, (object[]) args);
        }

        public static void NotBetween<T>(T low, T high, T actual, Comparison<T> comparison, string message, params object[] args) {
            Global.NotBetween<T>(low, high, actual, comparison, message, (object[]) args);
        }

    }

    partial class Assume {

        public static void Between<T>(T low, T high, T actual) {
            Global.Between<T>(low, high, actual);
        }

        public static void Between<T>(T low, T high, T actual, Comparison<T> comparison) {
            Global.Between<T>(low, high, actual, comparison);
        }

        public static void Between<T>(T low, T high, T actual, IComparer<T> comparer) {
            Global.Between<T>(low, high, actual, comparer);
        }

        public static void Between<T>(string low, string high, string actual, StringComparison comparison) {
            Global.Between<T>(low, high, actual, comparison);
        }

        public static void NotBetween<T>(T low, T high, T actual) {
            Global.NotBetween<T>(low, high, actual);
        }

        public static void NotBetween<T>(T low, T high, T actual, IComparer<T> comparer) {
            Global.NotBetween<T>(low, high, actual, comparer);
        }

        public static void NotBetween<T>(T low, T high, T actual, Comparison<T> comparison) {
            Global.NotBetween<T>(low, high, actual, comparison);
        }

        public static void Between<T>(T low, T high, T actual, string message, params object[] args) {
            Global.Between<T>(low, high, actual, message, (object[]) args);
        }

        public static void Between<T>(T low, T high, T actual, Comparison<T> comparison, string message, params object[] args) {
            Global.Between<T>(low, high, actual, comparison, message, (object[]) args);
        }

        public static void Between<T>(T low, T high, T actual, IComparer<T> comparer, string message, params object[] args) {
            Global.Between<T>(low, high, actual, comparer, message, (object[]) args);
        }

        public static void Between(string low, string high, string actual, StringComparison comparison, string message, params object[] args) {
            Global.Between(low, high, actual, comparison, message, (object[]) args);
        }

        public static void NotBetween<T>(T low, T high, T actual, string message, params object[] args) {
            Global.NotBetween<T>(low, high, actual, message, (object[]) args);
        }

        public static void NotBetween<T>(T low, T high, T actual, IComparer<T> comparer, string message, params object[] args) {
            Global.NotBetween<T>(low, high, actual, comparer, message, (object[]) args);
        }

        public static void NotBetween<T>(T low, T high, T actual, Comparison<T> comparison, string message, params object[] args) {
            Global.NotBetween<T>(low, high, actual, comparison, message, (object[]) args);
        }

    }


    namespace TestMatchers {

        public class BetweenMatcher<T> : TestMatcher<T>, ITestMatcherWithComparer<T> {

            public T Low { get; private set; }
            public T High { get; private set; }
            public IComparer<T> Comparer { get; private set; }
            public bool BoundsExclusive { get; private set; }

            [MatcherUserData(Hidden = true)]
            public BetweenMatcher<T> Exclusive {
                get {
                    return new BetweenMatcher<T>(Low, High, Comparer, true);
                }
            }

            [MatcherUserData(Hidden = true)]
            public BetweenMatcher<T> Inclusive {
                get {
                    return new BetweenMatcher<T>(Low, High, Comparer, true);
                }
            }

            public BetweenMatcher(T low, T high, IComparer<T> comparer = null, bool exclusive = false) {
                Low = low;
                High = high;
                Comparer = comparer;
                BoundsExclusive = exclusive;
            }

            public BetweenMatcher<T> WithComparer(IComparer<T> comparer) {
                return new BetweenMatcher<T>(Low, High, comparer);
            }

            public BetweenMatcher<T> WithComparison(Comparison<T> comparison) {
                return new BetweenMatcher<T>(Low, High, Comparer<T>.Create(comparison));
            }

            public BetweenMatcher<T> OrClose(T epsilon) {
                return WithComparer(EpsilonComparer.Create<T>(epsilon));
            }

            public BetweenMatcher<T> OrClose<TEpsilon>(TEpsilon epsilon) {
                return WithComparer(EpsilonComparer.Create<T, TEpsilon>(epsilon));
            }

            public override bool Matches(T actual) {
                var comparer = Comparer ?? Comparer<T>.Default;

                int cmp = comparer.Compare(Low, actual);
                if (cmp > 0 || (cmp == 0 && BoundsExclusive)) {
                    return false;
                }

                cmp = comparer.Compare(actual, High);
                if (cmp > 0 || (cmp == 0 && BoundsExclusive)) {
                    return false;
                }

                return true;
            }

            ITestMatcher<T> ITestMatcherWithComparer<T>.WithComparer(IComparer<T> comparer) {
                return WithComparer(comparer);
            }
        }
    }

}
