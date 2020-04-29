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
using System;
using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.Commons.Spec {

    partial class ExpectationCommand {

        class CaptureExceptionCommand : ExpectationCommand<Exception> {

            private readonly Action _thunk;
            private readonly bool _negated;

            public CaptureExceptionCommand(Action thunk, bool negated) {
                _thunk = thunk;
                _negated = negated;
            }

            public override ExpectationCommand<Unit> Untyped() {
                throw new NotImplementedException();
            }

            public override TestFailure Should(ITestMatcher<Exception> matcher) {
                var ex = Record.Exception(_thunk);
                if (_negated) {
                    matcher = Matchers.Not(matcher);
                }

                var matches = matcher.Matches(TestActual.Value(ex));
                if (!matches) {
                    return TestMatcherLocalizer.Failure(matcher, ex);
                }
                return null;
            }

            public override ExpectationCommand<Exception> Negated() {
                return new CaptureExceptionCommand(_thunk, !_negated);
            }

            public override ExpectationCommand<TResult> Property<TResult>(Func<Exception, TResult> accessor) {
                return ExpectationCommand.Of(() => accessor(Record.Exception(_thunk)));
            }
        }

    }

}
