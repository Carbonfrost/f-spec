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
using System.Diagnostics;

using Carbonfrost.Commons.Spec.ExecutionModel;
using Carbonfrost.Commons.Spec.Resources;

namespace Carbonfrost.Commons.Spec {

    partial class ExpectationCommand {

        class ConsistentlyCommand : ExpectationCommand {

            private readonly Action _thunk;
            private readonly TimeSpan _duration;
            private readonly bool _negated;

            public ConsistentlyCommand(TimeSpan duration,
                                       Action thunk,
                                       bool negated = false) {
                _duration = duration;
                _thunk = thunk;
                _negated = negated;
            }

            public override TestFailure Should(ITestMatcher matcher) {
                var s = Stopwatch.StartNew();
                var durationMS = (int) _duration.TotalMilliseconds;

                do {
                    if (matcher.Matches(_thunk) == _negated) {
                        var aFailure = TestMatcherLocalizer.FailurePredicate(matcher);
                        return new TestFailure("spec.consistently") {
                            Message = SR.ConsistentlyElapsedBefore(TextUtility.FormatDuration(_duration)),
                            Children = { aFailure },
                        };
                    }

                } while (s.ElapsedMilliseconds <= durationMS);

                return null;
            }

            public override ExpectationCommand Negated() {
                return new ConsistentlyCommand(_duration, _thunk, !_negated);
            }
        }

        class ConsistentlyCommand<T> : ExpectationCommand<T> {

            private readonly Func<T> _thunk;
            private readonly TimeSpan _duration;
            private readonly bool _negated;

            public ConsistentlyCommand(TimeSpan duration,
                                       Func<T> thunk,
                                       bool negated = false) {
                _duration = duration;
                _thunk = thunk;
                _negated = negated;
            }

            public override TestFailure Should(ITestMatcher<T> matcher) {
                var s = Stopwatch.StartNew();
                var durationMS = (int) _duration.TotalMilliseconds;

                do {
                    if (matcher.Matches(_thunk) == _negated) {
                        var actual = _thunk();
                        var aFailure = TestMatcherLocalizer.FailurePredicate(matcher);
                        var result = new TestFailure("spec.consistently") {
                            Message = SR.ConsistentlyElapsedBefore(TextUtility.FormatDuration(_duration)),
                            Children = { aFailure },
                        };
                        result.UserData["Actual"] = TextUtility.ConvertToString(actual);
                        return result;
                    }

                } while (s.ElapsedMilliseconds <= durationMS);

                return null;
            }

            public override ExpectationCommand<T> Negated() {
                return new ConsistentlyCommand<T>(_duration, _thunk, !_negated);
            }
        }
    }

}
