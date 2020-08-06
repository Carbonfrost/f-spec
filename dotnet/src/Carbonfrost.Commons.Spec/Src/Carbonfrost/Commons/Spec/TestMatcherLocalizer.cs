//
// Copyright 2018, 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using Carbonfrost.Commons.Spec.ExecutionModel;
using Carbonfrost.Commons.Spec.Resources;
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.Commons.Spec {

    static class TestMatcherLocalizer {

        public static TestFailure FailurePredicate(object matcher) {
            return FailureMessageCore(matcher, true);
        }

        public static TestFailure Failure(object matcher, object actual) {
            TestFailure failure = FailureMessageCore(matcher, false);
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
            TestMatcherName matcherName, UserDataCollection data) {

            if (matcherName.IsInvariant) {
                return "";
            }
            if (matcherName.IsAndOr) {
                return matcherName.IsNegated ? SR.NotExpectedTo() : SR.ExpectedTo();
            }

            var msgCode = matcherName.ToSRKey();
            var msg = SR.ResourceManager.GetString(msgCode);
            if (msg == null) {
                return MissingLocalization(msgCode);
            }

            var fill = FillableMessage.Fill(msg, data);
            data.ExpectedConsumedInMessage = fill.Keys.Contains("Expected");
            return fill.ToString();
        }

        private static object ActualMatcher(object matcher) {
            if (matcher is ISupportTestMatcher support) {
                return ActualMatcher(support.RealMatcher);
            }
            if (matcher is INotMatcher nt) {
                return ActualMatcher(nt.InnerMatcher);
            }
            return matcher;
        }

        static TestFailure FailureMessageCore(object matcher, bool predicate) {
            var matcherName = TestMatcherName.For(matcher);
            if (predicate) {
                matcherName = matcherName.Predicate();
            }

            // We use the actual matcher to get the user data for the failure
            var failure = new TestFailure(matcherName, ActualMatcher(matcher));

            string message = ExpectedMessage(matcherName, failure.UserData);
            failure.Message = message;
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
                var f = FailureMessageCore(child, true);
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
