//
// Copyright 2016, 2017, 2019-2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Diagnostics;
using System.Linq;
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.Commons.Spec {

    partial class Matchers {

        public static ContainsSubstringMatcher ContainSubstring(string substring) {
            return new ContainsSubstringMatcher(substring);
        }

        public static ContainsSubstringMatcher ContainSubstring(string substring, StringComparison comparison) {
            return new ContainsSubstringMatcher(substring, comparison);
        }

    }

    partial class Extensions {

        // Expect.ToHave.Substring - is a special case where we test string
        // as a value instead of as an enumerable in order to use its built-in semantics

        [IgnoreEnumerableExpectationAttribute]
        public static void Substring(this IEnumerableExpectation e, string substring) {
            Substring(e, substring, (string) null);
        }

        public static void Substring(this IEnumerableExpectation e, string substring, StringComparison comparison) {
            Substring(e, substring, comparison, null);
        }

        public static void Substring(this IEnumerableExpectation e, string substring, string message, params object[] args) {
            e.As<string>().Like(Matchers.ContainSubstring(substring), message, (object[]) args);
        }

        public static void Substring(this IEnumerableExpectation e, string substring, StringComparison comparison, string message, params object[] args) {
            e.As<string>().Like(Matchers.ContainSubstring(substring, comparison), message, (object[]) args);
        }

    }

    partial class Asserter {

        public void Contains(string expectedSubstring, string actualString) {
            Contains(expectedSubstring, actualString, StringComparison.Ordinal);
        }

        public void Contains(string expectedSubstring, string actualString, StringComparison comparison) {
            That(actualString, Matchers.ContainSubstring(expectedSubstring, comparison));
        }

        public void Contains(string expectedSubstring, string actualString, string message, params object[] args) {
            Contains(expectedSubstring, actualString, StringComparison.Ordinal, message, args);
        }

        public void Contains(string expectedSubstring, string actualString, StringComparison comparison, string message, params object[] args) {
            That(actualString, Matchers.ContainSubstring(expectedSubstring, comparison), message, args);
        }

        public void DoesNotContain(string expectedSubstring, string actualString) {
            DoesNotContain(expectedSubstring, actualString, StringComparison.Ordinal);
        }

        public void DoesNotContain(string expectedSubstring, string actualString, StringComparison comparison) {
            NotThat(actualString, Matchers.ContainSubstring(expectedSubstring, comparison));
        }

        public void DoesNotContain(string expectedSubstring, string actualString, string message, params object[] args) {
            DoesNotContain(expectedSubstring, actualString, StringComparison.Ordinal, message, args);
        }

        public void DoesNotContain(string expectedSubstring, string actualString, StringComparison comparison, string message, params object[] args) {
            NotThat(actualString, Matchers.ContainSubstring(expectedSubstring, comparison), message, args);
        }
    }

    partial class Assert {

        public static void Contains(string expectedSubstring, string actualString) {
            Global.Contains(expectedSubstring, actualString);
        }

        public static void Contains(string expectedSubstring, string actualString, StringComparison comparison) {
            Global.Contains(expectedSubstring, actualString, comparison);
        }

        public static void Contains(string expectedSubstring, string actualString, string message, params object[] args) {
            Global.Contains(expectedSubstring, actualString, message, (object[]) args);
        }

        public static void Contains(string expectedSubstring, string actualString, StringComparison comparison, string message, params object[] args) {
            Global.Contains(expectedSubstring, actualString, comparison, message, (object[]) args);
        }

        public static void DoesNotContain(string expectedSubstring, string actualString) {
            Global.DoesNotContain(expectedSubstring, actualString);
        }

        public static void DoesNotContain(string expectedSubstring, string actualString, StringComparison comparison) {
            Global.DoesNotContain(expectedSubstring, actualString, comparison);
        }

        public static void DoesNotContain(string expectedSubstring, string actualString, string message, params object[] args) {
            Global.DoesNotContain(expectedSubstring, actualString, message, (object[]) args);
        }

        public static void DoesNotContain(string expectedSubstring, string actualString, StringComparison comparison, string message, params object[] args) {
            Global.DoesNotContain(expectedSubstring, actualString, comparison, message, (object[]) args);
        }
    }

    partial class Assume {

        public static void Contains(string expectedSubstring, string actualString) {
            Global.Contains(expectedSubstring, actualString);
        }

        public static void Contains(string expectedSubstring, string actualString, StringComparison comparison) {
            Global.Contains(expectedSubstring, actualString, comparison);
        }

        public static void Contains(string expectedSubstring, string actualString, string message, params object[] args) {
            Global.Contains(expectedSubstring, actualString, message, (object[]) args);
        }

        public static void Contains(string expectedSubstring, string actualString, StringComparison comparison, string message, params object[] args) {
            Global.Contains(expectedSubstring, actualString, comparison, message, (object[]) args);
        }

        public static void DoesNotContain(string expectedSubstring, string actualString) {
            Global.DoesNotContain(expectedSubstring, actualString);
        }

        public static void DoesNotContain(string expectedSubstring, string actualString, StringComparison comparison) {
            Global.DoesNotContain(expectedSubstring, actualString, comparison);
        }

        public static void DoesNotContain(string expectedSubstring, string actualString, string message, params object[] args) {
            Global.DoesNotContain(expectedSubstring, actualString, message, (object[]) args);
        }

        public static void DoesNotContain(string expectedSubstring, string actualString, StringComparison comparison, string message, params object[] args) {
            Global.DoesNotContain(expectedSubstring, actualString, comparison, message, (object[]) args);
        }
    }


    namespace TestMatchers {

        public class ContainsSubstringMatcher : TestMatcher<string> {

            public string Expected {
                get;
            }

            public StringComparison Comparison {
                get;
            }

            public ContainsSubstringMatcher(string expected, StringComparison comparison = StringComparison.Ordinal) {
                if (expected == null) {
                    throw new ArgumentNullException(nameof(expected));
                }
                Expected = expected;
                Comparison = comparison;
            }

            [MatcherUserData(Hidden = true)]
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public ContainsSubstringMatcher IgnoringCase {
                get {
                    return new ContainsSubstringMatcher(Expected, Comparison.MakeIgnoreCase());
                }
            }

            [MatcherUserData(Hidden = true)]
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public ContainsSubstringMatcher UsingCurrentCulture {
                get {
                    return new ContainsSubstringMatcher(Expected, Comparison.MakeCurrentCulture());
                }
            }

            public ContainsSubstringMatcher WithComparison(StringComparison comparison) {
                return new ContainsSubstringMatcher(Expected, comparison);
            }

            public EndWithSubstringMatcher WithComparer(StringComparer comparer) {
                return new EndWithSubstringMatcher(Expected, TextUtility.ToComparison(comparer));
            }

            public override bool Matches(string actual) {
                if (actual == null) {
                    return false;
                }
                return actual.IndexOf(Expected, Comparison) >= 0;
            }
        }
    }

}
