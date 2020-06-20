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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Carbonfrost.Commons.Spec.ExecutionModel;
using Carbonfrost.Commons.Spec.Resources;
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.Commons.Spec {

    static class TestMatcherLocalizer {

        public static TestFailure FailurePredicate(object matcher) {
            return FailureMessageCore(false, matcher, true);
        }

        public static TestFailure Failure(object matcher, object actual) {
            TestFailure failure = FailureMessageCore(false, matcher, false);
            failure.UserData.Add("Actual", actual);

            if (matcher is ITestMatcherActualDiff diff) {
                var patch = diff.GetPatch(actual);
                if (patch != null) {
                    if (patch.ALineCount > 1 || patch.BLineCount > 1) {
                        failure.UserData.Diff = patch;
                    }
                }
            }
            return failure;
        }

        internal static string MissingLocalization(string key) {
            return $"FAILED TO LOCALIZE ({key})";
        }

        private static string ExpectedMessage(
            bool negated, UserDataCollection data, TypeInfo type, bool predicate) {

            if (type.Name == "InvariantMatcher") {
                return "";
            }

            var typeName = TestMatcherName.NAME.Replace(type.Name, "$1");
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
                return MissingLocalization(msgCode);
            }

            var fill = FillableMessage.Fill(msg, data);
            data.ExpectedConsumedInMessage = fill.Keys.Contains("Expected");
            return fill.ToString();
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

            var matcherName = TestMatcherName.FromType(matcherType);
            if (negated) {
                matcherName = matcherName.Negated();
            }
            var failure = new TestFailure(matcherName, matcher);
            failure.Message = ExpectedMessage(negated, failure.UserData, matcherType, predicate);
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
    }
}
