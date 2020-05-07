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
using System.Linq;
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.Commons.Spec {

    partial class Matchers {

        public static EndWithSubstringMatcher EndWithSubstring(string expected, StringComparison comparison) {
            return new EndWithSubstringMatcher(expected, comparison);
        }

        public static EndWithSubstringMatcher EndWithSubstring(string expected) {
            return new EndWithSubstringMatcher(expected);
        }
    }

    static partial class Extensions {

        public static void EndsWith(this IExpectation<string> e, string expected) {
            EndsWith(e, expected, (string) null);
        }

        public static void EndsWith(this IExpectation<string> e, string expected, StringComparison comparison) {
            EndsWith(e, expected, comparison, (string) null);
        }

        public static void EndsWith(this IExpectation<string> e, string expected, string message, params object[] args) {
            e.Like(Matchers.EndWithSubstring(expected), message, (object[]) args);
        }

        public static void EndsWith(this IExpectation<string> e, string expected, StringComparison comparison, string message, params object[] args) {
            e.Like(Matchers.EndWithSubstring(expected, comparison), message, (object[]) args);
        }

    }

        partial class Asserter {

        public void EndsWith(string substring, string actual) {
            EndsWith(substring, actual, null, (object[]) null);
        }

        public void EndsWith(string substring, StringComparison comparison, string actual) {
            EndsWith(substring, comparison, actual, null, (object[]) null);
        }

        public void EndsWith(string substring, string actual, string message, params object[] args) {
            That(actual, Matchers.EndWithSubstring(substring), message, (object[]) args);
        }

        public void EndsWith(string substring, StringComparison comparison, string actual, string message, params object[] args) {
            That(actual, Matchers.EndWithSubstring(substring, comparison), message, (object[]) args);
        }

        public void DoesNotEndWith(string substring, string actual) {
            DoesNotEndWith(substring, actual, null, (object[]) null);
        }

        public void DoesNotEndWith(string substring, StringComparison comparison, string actual) {
            DoesNotEndWith(substring, comparison, actual, null, (object[]) null);
        }

        public void DoesNotEndWith(string substring, string actual, string message, params object[] args) {
            NotThat(actual, Matchers.EndWithSubstring(substring), message, (object[]) args);
        }

        public void DoesNotEndWith(string substring, StringComparison comparison, string actual, string message, params object[] args) {
            NotThat(actual, Matchers.EndWithSubstring(substring, comparison), message, (object[]) args);
        }
    }

    partial class Assert {

        public static void EndsWith(string substring, string actual) {
            Global.EndsWith(substring, actual);
        }

        public static void EndsWith(string substring, StringComparison comparison, string actual) {
            Global.EndsWith(substring, comparison, actual);
        }

        public static void EndsWith(string substring, string actual, string message, params object[] args) {
            Global.EndsWith(substring, actual, message, (object[]) args);
        }

        public static void EndsWith(string substring, StringComparison comparison, string actual, string message, params object[] args) {
            Global.EndsWith(substring, comparison, actual, message, (object[]) args);
        }

        public static void DoesNotEndWith(string substring, string actual) {
            Global.DoesNotEndWith(substring, actual);
        }

        public static void DoesNotEndWith(string substring, StringComparison comparison, string actual) {
            Global.DoesNotEndWith(substring, comparison, actual);
        }

        public static void DoesNotEndWith(string substring, string actual, string message, params object[] args) {
            Global.DoesNotEndWith(substring, actual, message, (object[]) args);
        }

        public static void DoesNotEndWith(string substring, StringComparison comparison, string actual, string message, params object[] args) {
            Global.DoesNotEndWith(substring, comparison, actual, message, (object[]) args);
        }
    }

    partial class Assume {

        public static void EndsWith(string substring, string actual) {
            Global.EndsWith(substring, actual);
        }

        public static void EndsWith(string substring, StringComparison comparison, string actual) {
            Global.EndsWith(substring, comparison, actual);
        }

        public static void EndsWith(string substring, string actual, string message, params object[] args) {
            Global.EndsWith(substring, actual, message, (object[]) args);
        }

        public static void EndsWith(string substring, StringComparison comparison, string actual, string message, params object[] args) {
            Global.EndsWith(substring, comparison, actual, message, (object[]) args);
        }

        public static void DoesNotEndWith(string substring, string actual) {
            Global.DoesNotEndWith(substring, actual);
        }

        public static void DoesNotEndWith(string substring, StringComparison comparison, string actual) {
            Global.DoesNotEndWith(substring, comparison, actual);
        }

        public static void DoesNotEndWith(string substring, string actual, string message, params object[] args) {
            Global.DoesNotEndWith(substring, actual, message, (object[]) args);
        }

        public static void DoesNotEndWith(string substring, StringComparison comparison, string actual, string message, params object[] args) {
            Global.DoesNotEndWith(substring, comparison, actual, message, (object[]) args);
        }
    }


    namespace TestMatchers {

        public class EndWithSubstringMatcher : TestMatcher<string> {

            public string Expected { get; private set; }
            public StringComparison Comparison { get; private set; }

            [MatcherUserData(Hidden = true)]
            public EndWithSubstringMatcher IgnoringCase {
                get {
                    return new EndWithSubstringMatcher(Expected, Comparison.MakeIgnoreCase());
                }
            }

            [MatcherUserData(Hidden = true)]
            public EndWithSubstringMatcher UsingCurrentCulture {
                get {
                    return new EndWithSubstringMatcher(Expected, Comparison.MakeCurrentCulture());
                }
            }

            public EndWithSubstringMatcher(string expected, StringComparison comparison = StringComparison.Ordinal) {
                if (expected == null) {
                    throw new ArgumentNullException("expected");
                }
                Expected = expected;
                Comparison = comparison;
            }

            public EndWithSubstringMatcher WithComparison(StringComparison comparison) {
                return new EndWithSubstringMatcher(Expected, comparison);
            }

            public EndWithSubstringMatcher WithComparer(StringComparer comparer) {
                return new EndWithSubstringMatcher(Expected, TextUtility.ToComparison(comparer));
            }

            public override bool Matches(string actual) {
                if (actual == null) {
                    throw new ArgumentNullException("actual");
                }
                return actual.EndsWith(Expected, Comparison);
            }
        }
    }
}
