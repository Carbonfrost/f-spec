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
using System;
using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.Commons.Spec {

    partial class ExpectationCommand {

        public static ExpectationCommand TestCode(Action action) {
            return new TestCodeCommand(action, false);
        }

        class TestCodeCommand : ExpectationCommand {

            private readonly Action _thunk;
            private readonly bool _negated;

            public TestCodeCommand(Action thunk, bool negated) {
                _thunk = thunk;
                _negated = negated;
            }

            public override TestFailure Should(ITestMatcher matcher) {
                if (_negated) {
                    matcher = Matchers.Not(matcher);
                }

                var matches = matcher.Matches(_thunk);
                object actual = _thunk;
;
                if (TestMatcher.Innermost(matcher) is ITestMatcherActualException throwsMatcher) {
                    actual = throwsMatcher.ActualException;
                }

                if (!matches) {
                    var failure = TestMatcherLocalizer.Failure(matcher, actual);
                    throw failure.ToException();
                }
                return null;
            }

            public override ExpectationCommand<Exception> CaptureException() {
                return new CaptureExceptionCommand(_thunk, _negated);
            }

            public override ExpectationCommand Negated() {
                return new TestCodeCommand(_thunk, !_negated);
            }

            public override ExpectationCommand Eventually(TimeSpan delay) {
                return new EventuallyCommand(delay, _thunk, _negated);
            }

            public override ExpectationCommand Consistently(TimeSpan delay) {
                return new ConsistentlyCommand(delay, _thunk, _negated);
            }
        }
    }
}
