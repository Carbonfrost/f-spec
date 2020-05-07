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
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.Commons.Spec {

    partial class Matchers {

        public static MatchMatcher Match(string expected) {
            return new MatchMatcher(new Regex(expected));
        }

        public static MatchMatcher Match(string expected, RegexOptions options) {
            return new MatchMatcher(new Regex(expected, options));
        }

        public static MatchMatcher Match(Regex expected) {
            return new MatchMatcher(expected);
        }

    }

    static partial class Extensions {

        public static void Match(this IExpectation<string> e, string expected) {
            Match(e, expected, (string) null);
        }

        public static void Match(this IExpectation<string> e, string expected, RegexOptions options) {
            Match(e, expected, options, (string) null);
        }

        public static void Match(this IExpectation<string> e, Regex expected) {
            Match(e, expected, (string) null);
        }

        public static void Match(this IExpectation<string> e, string expected, string message, params object[] args) {
            e.Like(Matchers.Match(expected), message, (object[]) args);
        }

        public static void Match(this IExpectation<string> e, string expected, RegexOptions options, string message, params object[] args) {
            e.Like(Matchers.Match(expected, options), message, (object[]) args);
        }

        public static void Match(this IExpectation<string> e, Regex expected, string message, params object[] args) {
            e.Like(Matchers.Match(expected), message, (object[]) args);
        }

    }

    partial class Asserter {

        public void Matches(string expected, string actual) {
            Matches(expected, actual, (string) null);
        }

        public void Matches(string expected, RegexOptions options, string actual) {
            Matches(expected, options, actual, (string) null);
        }

        public void Matches(Regex expected, string actual) {
            Matches(expected, actual, (string) null);
        }

        public void Matches(string expected, string actual, string message, params object[] args) {
            That(actual, Matchers.Match(expected), message, (object[]) args);
        }

        public void Matches(string expected, RegexOptions options, string actual, string message, params object[] args) {
            That(actual, Matchers.Match(expected, options), message, (object[]) args);
        }

        public void Matches(Regex expected, string actual, string message, params object[] args) {
            That(actual, Matchers.Match(expected), message, (object[]) args);
        }

        public void DoesNotMatch(string expected, string actual) {
            DoesNotMatch(expected, actual, (string) null);
        }

        public void DoesNotMatch(string expected, RegexOptions options, string actual) {
            DoesNotMatch(expected, options, actual, (string) null);
        }

        public void DoesNotMatch(Regex expected, string actual) {
            DoesNotMatch(expected, actual, (string) null);
        }

        public void DoesNotMatch(string expected, string actual, string message, params object[] args) {
            NotThat(actual, Matchers.Match(expected), message, (object[]) args);
        }

        public void DoesNotMatch(string expected, RegexOptions options, string actual, string message, params object[] args) {
            NotThat(actual, Matchers.Match(expected, options), message, (object[]) args);
        }

        public void DoesNotMatch(Regex expected, string actual, string message, params object[] args) {
            NotThat(actual, Matchers.Match(expected), message, (object[]) args);
        }

    }

    partial class Assert {

        public static void Matches(string expected, string actual) {
            Global.Matches(expected, actual);
        }

        public static void Matches(string expected, RegexOptions options, string actual) {
            Global.Matches(expected, options, actual);
        }

        public static void Matches(Regex expected, string actual) {
            Global.Matches(expected, actual);
        }

        public static void Matches(string expected, string actual, string message, params object[] args) {
            Global.Matches(expected, actual, message, (object[]) args);
        }

        public static void Matches(string expected, RegexOptions options, string actual, string message, params object[] args) {
            Global.Matches(expected, options, actual, message, (object[]) args);
        }

        public static void Matches(Regex expected, string actual, string message, params object[] args) {
            Global.Matches(expected, actual, message, (object[]) args);
        }

        public static void DoesNotMatch(string expected, string actual) {
            Global.DoesNotMatch(expected, actual);
        }

        public static void DoesNotMatch(string expected, RegexOptions options, string actual) {
            Global.DoesNotMatch(expected, options, actual);
        }

        public static void DoesNotMatch(Regex expected, string actual) {
            Global.DoesNotMatch(expected, actual);
        }

        public static void DoesNotMatch(string expected, string actual, string message, params object[] args) {
            Global.DoesNotMatch(expected, actual, message, (object[]) args);
        }

        public static void DoesNotMatch(string expected, RegexOptions options, string actual, string message, params object[] args) {
            Global.DoesNotMatch(expected, options, actual, message, (object[]) args);
        }

        public static void DoesNotMatch(Regex expected, string actual, string message, params object[] args) {
            Global.DoesNotMatch(expected, actual, message, (object[]) args);
        }

    }


    partial class Assume {

        public static void Matches(string expected, string actual) {
            Global.Matches(expected, actual);
        }

        public static void Matches(string expected, RegexOptions options, string actual) {
            Global.Matches(expected, options, actual);
        }

        public static void Matches(Regex expected, string actual) {
            Global.Matches(expected, actual);
        }

        public static void Matches(string expected, string actual, string message, params object[] args) {
            Global.Matches(expected, actual, message, (object[]) args);
        }

        public static void Matches(string expected, RegexOptions options, string actual, string message, params object[] args) {
            Global.Matches(expected, options, actual, message, (object[]) args);
        }

        public static void Matches(Regex expected, string actual, string message, params object[] args) {
            Global.Matches(expected, actual, message, (object[]) args);
        }

        public static void DoesNotMatch(string expected, string actual) {
            Global.DoesNotMatch(expected, actual);
        }

        public static void DoesNotMatch(string expected, RegexOptions options, string actual) {
            Global.DoesNotMatch(expected, options, actual);
        }

        public static void DoesNotMatch(Regex expected, string actual) {
            Global.DoesNotMatch(expected, actual);
        }

        public static void DoesNotMatch(string expected, string actual, string message, params object[] args) {
            Global.DoesNotMatch(expected, actual, message, (object[]) args);
        }

        public static void DoesNotMatch(string expected, RegexOptions options, string actual, string message, params object[] args) {
            Global.DoesNotMatch(expected, options, actual, message, (object[]) args);
        }

        public static void DoesNotMatch(Regex expected, string actual, string message, params object[] args) {
            Global.DoesNotMatch(expected, actual, message, (object[]) args);
        }

    }


    namespace TestMatchers {

        public class MatchMatcher : TestMatcher<string> {

            public Regex Expected { get; private set; }

            public MatchMatcher(Regex expected) {
                if (expected == null) {
                    throw new ArgumentNullException("expected");
                }
                Expected = expected;
            }

            public override bool Matches(string actual) {
                return Expected.IsMatch(actual);
            }

        }
    }

}
