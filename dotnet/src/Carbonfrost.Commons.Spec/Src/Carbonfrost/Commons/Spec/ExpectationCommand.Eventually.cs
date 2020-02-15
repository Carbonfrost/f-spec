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
using System.Diagnostics;
using System.Linq;
using Carbonfrost.Commons.Spec;
using Carbonfrost.Commons.Spec.ExecutionModel;
using Carbonfrost.Commons.Spec.Resources;

namespace Carbonfrost.Commons.Spec {

    partial class ExpectationCommand {

        class EventuallyCommand : IExpectationCommand {

            private readonly Action _thunk;
            private readonly TimeSpan _duration;
            private readonly bool _negated;

            public EventuallyCommand(TimeSpan duration,
                                     Action thunk,
                                     bool negated = false) {
                _duration = duration;
                _thunk = thunk;
                _negated = negated;
            }

            public TestFailure Should(ITestMatcher matcher) {
                var s = Stopwatch.StartNew();
                var durationMS = (int) _duration.TotalMilliseconds;

                while (s.ElapsedMilliseconds <= durationMS) {
                    if (matcher.Matches(_thunk) != _negated) {
                        return null;
                    }
                }

                return new TestFailure("spec.eventually") {
                    Message = SR.EventuallyTimedOutAfter(TextUtility.FormatDuration(_duration)),
                    Children = { TestMatcherLocalizer.FailurePredicate(matcher) },
                };
            }

            public IExpectationCommand Negated() {
                return new EventuallyCommand(_duration, _thunk, !_negated);
            }

            public IExpectationCommand Eventually(TimeSpan delay) {
                throw new NotImplementedException();
            }

            public IExpectationCommand Consistently(TimeSpan delay) {
                throw new NotImplementedException();
            }
        }

        class EventuallyCommand<T> : ExpectationCommand<T> {

            private readonly Func<T> _thunk;
            private readonly TimeSpan _duration;
            private readonly bool _negated;

            public EventuallyCommand(TimeSpan duration,
                                     Func<T> thunk,
                                     bool negated = false) {
                _duration = duration;
                _thunk = thunk;
                _negated = negated;
            }

            public override TestFailure Should(ITestMatcher<T> matcher) {
                var s = Stopwatch.StartNew();
                var durationMS = (int) _duration.TotalMilliseconds;

                while (s.ElapsedMilliseconds <= durationMS) {
                    if (matcher.Matches(_thunk) != _negated) {
                        return null;
                    }
                }

                var actual = _thunk();
                var aFailure = TestMatcherLocalizer.FailurePredicate(matcher);
                var result = new TestFailure("spec.eventually") {
                    Message = SR.EventuallyTimedOutAfter(TextUtility.FormatDuration(_duration)),
                    Children = { aFailure },
                };
                result.UserData["Actual"] = TextUtility.ConvertToString(actual);
                return result;
            }

            public override ExpectationCommand<TBase> As<TBase>() {
                return new CastCommand<T, TBase>(this);
            }

            public override ExpectationCommand<T> Negated() {
                return new EventuallyCommand<T>(_duration, _thunk, !_negated);
            }
        }

    }

}
