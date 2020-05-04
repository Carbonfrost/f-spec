//
// Copyright 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.Commons.Spec {

    interface IExpectation<T> {
        ExpectationCommand<T> ToCommand();
    }

    partial class Extensions {

        internal static void Should<T>(this IExpectation<T> self, ITestMatcher<T> matcher, string message = null, object[] args = null) {
            var failure = self.ToCommand().Should(matcher);
            if (failure != null) {
                IAsserterBehavior behavior = failure.AsserterBehavior;
                behavior.Assert(failure.UpdateTestSubject().UpdateMessage(message, args));
            }
        }

        internal static void Should(this IExpectation<Unit> self, ITestMatcher matcher, string message = null, object[] args = null) {
            var failure = self.ToCommand().Should(
                TestMatcher.UnitWrapper(matcher)
            );

            if (failure != null) {
                IAsserterBehavior behavior = failure.AsserterBehavior;
                behavior.Assert(failure.UpdateTestSubject().UpdateMessage(message, args));
            }
        }
    }
}
