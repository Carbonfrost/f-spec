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

        public static StartWithSubstringMatcher StartWithSubstring(string expected, StringComparison comparison) {
            return new StartWithSubstringMatcher(expected, comparison);
        }

        public static StartWithSubstringMatcher StartWithSubstring(string expected) {
            return new StartWithSubstringMatcher(expected);
        }
    }

    static partial class Extensions {

        public static void StartsWith(this Expectation<string> e, string expected) {
            StartsWith(e, expected, (string) null);
        }

        public static void StartsWith(this Expectation<string> e, string expected, StringComparison comparison) {
            StartsWith(e, expected, comparison, (string) null);
        }

        public static void StartsWith(this Expectation<string> e, string expected, string message, params object[] args) {
            e.Should(Matchers.StartWithSubstring(expected), message, (object[]) args);
        }

        public static void StartsWith(this Expectation<string> e, string expected, StringComparison comparison, string message, params object[] args) {
            e.Should(Matchers.StartWithSubstring(expected, comparison), message, (object[]) args);
        }

    }

    partial class Asserter {

        public void StartsWith(string substring, string actual) {
            StartsWith(substring, actual, null, (object[]) null);
        }

        public void StartsWith(string substring, StringComparison comparison, string actual) {
            StartsWith(substring, comparison, actual, null, (object[]) null);
        }

        public void StartsWith(string substring, string actual, string message, params object[] args) {
            That(actual, Matchers.StartWithSubstring(substring), message, (object[]) args);
        }

        public void StartsWith(string substring, StringComparison comparison, string actual, string message, params object[] args) {
            That(actual, Matchers.StartWithSubstring(substring, comparison), message, (object[]) args);
        }

        public void DoesNotStartWith(string substring, string actual) {
            DoesNotStartWith(substring, actual, null, (object[]) null);
        }

        public void DoesNotStartWith(string substring, StringComparison comparison, string actual) {
            DoesNotStartWith(substring, comparison, actual, null, (object[]) null);
        }

        public void DoesNotStartWith(string substring, string actual, string message, params object[] args) {
            NotThat(actual, Matchers.StartWithSubstring(substring), message, (object[]) args);
        }

        public void DoesNotStartWith(string substring, StringComparison comparison, string actual, string message, params object[] args) {
            NotThat(actual, Matchers.StartWithSubstring(substring, comparison), message, (object[]) args);
        }
    }

    partial class Assert {

        public static void StartsWith(string substring, string actual) {
            Global.StartsWith(substring, actual);
        }

        public static void StartsWith(string substring, StringComparison comparison, string actual) {
            Global.StartsWith(substring, comparison, actual);
        }

        public static void StartsWith(string substring, string actual, string message, params object[] args) {
            Global.StartsWith(substring, actual, message, (object[]) args);
        }

        public static void StartsWith(string substring, StringComparison comparison, string actual, string message, params object[] args) {
            Global.StartsWith(substring, comparison, actual, message, (object[]) args);
        }

        public static void DoesNotStartWith(string substring, string actual) {
            Global.DoesNotStartWith(substring, actual);
        }

        public static void DoesNotStartWith(string substring, StringComparison comparison, string actual) {
            Global.DoesNotStartWith(substring, comparison, actual);
        }

        public static void DoesNotStartWith(string substring, string actual, string message, params object[] args) {
            Global.DoesNotStartWith(substring, actual, message, (object[]) args);
        }

        public static void DoesNotStartWith(string substring, StringComparison comparison, string actual, string message, params object[] args) {
            Global.DoesNotStartWith(substring, comparison, actual, message, (object[]) args);
        }
    }

    partial class Assume {

        public static void StartsWith(string substring, string actual) {
            Global.StartsWith(substring, actual);
        }

        public static void StartsWith(string substring, StringComparison comparison, string actual) {
            Global.StartsWith(substring, comparison, actual);
        }

        public static void StartsWith(string substring, string actual, string message, params object[] args) {
            Global.StartsWith(substring, actual, message, (object[]) args);
        }

        public static void StartsWith(string substring, StringComparison comparison, string actual, string message, params object[] args) {
            Global.StartsWith(substring, comparison, actual, message, (object[]) args);
        }

        public static void DoesNotStartWith(string substring, string actual) {
            Global.DoesNotStartWith(substring, actual);
        }

        public static void DoesNotStartWith(string substring, StringComparison comparison, string actual) {
            Global.DoesNotStartWith(substring, comparison, actual);
        }

        public static void DoesNotStartWith(string substring, string actual, string message, params object[] args) {
            Global.DoesNotStartWith(substring, actual, message, (object[]) args);
        }

        public static void DoesNotStartWith(string substring, StringComparison comparison, string actual, string message, params object[] args) {
            Global.DoesNotStartWith(substring, comparison, actual, message, (object[]) args);
        }
    }

    namespace TestMatchers {

        public class StartWithSubstringMatcher : TestMatcher<string> {

            public string Expected { get; private set; }
            public StringComparison Comparison { get; private set; }

            [MatcherUserData(Hidden = true)]
            public StartWithSubstringMatcher IgnoringCase {
                get {
                    return new StartWithSubstringMatcher(Expected, Comparison.MakeIgnoreCase());
                }
            }

            [MatcherUserData(Hidden = true)]
            public StartWithSubstringMatcher UsingCurrentCulture {
                get {
                    return new StartWithSubstringMatcher(Expected, Comparison.MakeCurrentCulture());
                }
            }

            public StartWithSubstringMatcher(string expected, StringComparison comparison = StringComparison.Ordinal) {
                if (expected == null) {
                    throw new ArgumentNullException("expected");
                }
                Expected = expected;
                Comparison = comparison;
            }

            public StartWithSubstringMatcher WithComparison(StringComparison comparison) {
                return new StartWithSubstringMatcher(Expected, comparison);
            }

            public StartWithSubstringMatcher WithComparer(StringComparer comparer) {
                return new StartWithSubstringMatcher(Expected, TextUtility.ToComparison(comparer));
            }

            public override bool Matches(string actual) {
                if (actual == null) {
                    throw new ArgumentNullException("actual");
                }
                return actual.StartsWith(Expected, Comparison);
            }
        }
    }
}
