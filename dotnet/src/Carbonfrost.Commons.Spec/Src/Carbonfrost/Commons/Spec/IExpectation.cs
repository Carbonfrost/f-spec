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

namespace Carbonfrost.Commons.Spec {

    interface IExpectation {
        IExpectationCommand ToCommand();
    }

    interface IExpectation<T> {
        ExpectationCommand<T> ToCommand();
    }

    partial class Extensions {

        internal static void Should<T>(this IExpectation<T> self, ITestMatcher<T> matcher, string message = null, params object[] args) {
            var failure = self.ToCommand().Should(matcher);

            if (failure != null) {
                throw failure.UpdateTestSubject().UpdateMessage(message, args).ToException();
            }
        }

        internal static void Should(this IExpectation self, ITestMatcher matcher, string message = null, params object[] args) {
            var failure = self.ToCommand().Should(matcher);

            if (failure != null) {
                throw failure.UpdateTestSubject().UpdateMessage(message, args).ToException();
            }
        }
    }
}
