//
// Copyright 2018 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Reflection;
using System.Text.RegularExpressions;
using Carbonfrost.Commons.Spec.ExecutionModel;
using Carbonfrost.Commons.Spec.Resources;
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.Commons.Spec {

    interface ISupportTestMatcher {
        object RealMatcher { get; }
    }

    static class TestMatcherLocalizer {

        private static readonly Regex NAME = new Regex(@"^(.+)Matcher(`\d+)?$");

        public static TestFailure FailurePredicate(object matcher) {
            return FailureMessageCore(false, matcher, true);
        }

        public static TestFailure Failure(object matcher, object actual) {
            TestFailure failure = FailureMessageCore(false, matcher, false);
            var strActual = TextUtility.ConvertToString(actual, ShowWS);
            failure.UserData["Actual"] = strActual;

            var strMatcher = matcher as EqualMatcher<string>;
            if (strMatcher != null) {
                string strExpected = strMatcher.Expected;
                var patch = new Patch(strExpected, strActual);
                if (patch.ALineCount > 1 || patch.BLineCount > 1) {
                    failure.UserData["Diff"] = patch.ToString();
                }
            }
            return failure;
        }

        internal static string Code(TypeInfo type) {
            string name = NAME.Replace(type.Name, "$1");
            return "spec." + char.ToLowerInvariant(name[0]) + name.Substring(1);
        }

        internal static string Caption(string caption) {
            var cap = "Label" + caption;
            return SR.ResourceManager.GetString(cap)
                ?? "FAILED TO LOCALIZE " + caption;
        }

        private static string ExpectedMessage(
            bool negated, IDictionary<string, string> data, TypeInfo type, bool predicate) {

            if (type.Name == "InvariantMatcher") {
                return "";
            }

            var typeName = NAME.Replace(type.Name, "$1");
            if (typeName == "And" || typeName == "Or") {
                return negated ? SR.NotExpectedTo() : SR.ExpectedTo();
            }

            var prefix = "Expected";
            if (predicate) {
                prefix = "Predicate";
            }

            var msgCode = prefix + typeName;
            if (negated) {
                msgCode = "Not" + msgCode;
            }

            var msg = SR.ResourceManager.GetString(msgCode);
            if (msg == null) {
                return "FAILED TO LOCALIZE " + msgCode;
            }

            return TextUtility.Fill(msg, data);
        }

        internal static Dictionary<string, string> ExtractUserData(object matcher) {
            var props = matcher.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(t => t.CanRead && t.GetIndexParameters().Length == 0);

            var result = new Dictionary<string, string>();
            var defaultShowWS = ShowWS;
            foreach (var pi in props) {
                var showWS = defaultShowWS;
                var name = pi.Name;
                string value = null;
                object val = pi.GetValue(matcher);
                var attr = (MatcherUserDataAttribute) pi.GetCustomAttribute(typeof(MatcherUserDataAttribute));
                if (attr != null) {
                    if (attr.Hidden) {
                        continue;
                    }
                }

                if (val is StringComparer) {
                    value = GetStringComparerText(val);
                } else {
                    value = TextUtility.ConvertToString(val, showWS);
                }
                result[name] = value;
            }

            return result;
        }

        static TestFailure FailureMessageCore(bool negated, object matcher, bool predicate) {
            var support = matcher as ISupportTestMatcher;
            if (support != null) {
                return FailureMessageCore(negated, support.RealMatcher, predicate);
            }

            var nt = matcher as INotMatcher;
            if (nt != null) {
                return FailureMessageCore(true, nt.InnerMatcher, predicate);
            }

            var matcherType = matcher.GetType().GetTypeInfo();
            if (matcherType.IsGenericType) {
                matcherType = matcherType.GetGenericTypeDefinition().GetTypeInfo();
            }

            string code = Code(matcherType) + (negated ? ".not" : "");
            var dict = ExtractUserData(matcher);

            var failure = new TestFailure(code) {
                Message = ExpectedMessage(negated, dict, matcherType, predicate),
            };
            failure.UserData.AddAll(dict);
            ChildrenMessages(failure, matcher);
            return failure;
        }

        static void ChildrenMessages(TestFailure failure, object matcher) {
            var cm = matcher as ICompositeTestMatcher;
            if (cm == null) {
                return;
            }

            var result = new List<string>();
            foreach (var child in cm.Matchers) {
                if (child == null) {
                    continue;
                }
                var f = FailureMessageCore(false, child, true);
                failure.Children.Add(f);
            }

            // Set operator text
            if (failure.Children.Count > 1) {
                var last = failure.Children[failure.Children.Count - 1];
                last.Message = cm.Operator + " " + last.Message;
            }
        }

        static string GetStringComparerText(object comparison) {
            if (comparison == null) {
                return "<null>";
            }

            var str = comparison.ToString();

            if (comparison == StringComparer.OrdinalIgnoreCase) {
                str = "ordinal (ignore case)";

            } else if (comparison == StringComparer.Ordinal) {
                str = "ordinal";
            }

            else if (comparison == StringComparer.InvariantCulture) {
                str = "invariant culture";
            }
            else if (comparison == StringComparer.InvariantCultureIgnoreCase) {
                str = "invariant culture (ignore case)";
            }

            return str;
        }

        internal static bool ShowWS {
            get {
                return TestRunner.Current != null && TestRunner.Current
                    .Options.AssertionMessageFormatMode.HasFlag(AssertionMessageFormatModes.PrintWhitespace);
            }
        }

    }
}
