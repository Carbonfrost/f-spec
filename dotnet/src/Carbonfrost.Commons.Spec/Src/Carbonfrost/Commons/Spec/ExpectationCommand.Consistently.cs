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

        internal class ConsistentlyCommand<T> : ExpectationCommand<T> {

            private readonly ExpectationCommand<T> _inner;
            private readonly TimeSpan _duration;

            public ConsistentlyCommand(TimeSpan duration,
                                       ExpectationCommand<T> inner) {
                _duration = duration;
                _inner = inner;
            }

            public override ExpectationCommand<T> Given(string given) {
                return new ConsistentlyCommand<T>(_duration, _inner.Given(given));
            }

            public override TestFailure Should(ITestMatcher<T> matcher) {
                var s = Stopwatch.StartNew();
                var durationMS = (int) _duration.TotalMilliseconds;

                do {
                    var aFailure = _inner.Should(matcher);
                    if (aFailure != null) {
                        var result = new TestFailure("spec.consistently") {
                            Message = SR.ConsistentlyElapsedBefore(((Time) _duration).ToString("n")),
                            Children = {
                                TestMatcherLocalizer.FailurePredicate(matcher)
                             },
                        };
                        result.UserData.CopyActuals(aFailure.UserData);
                        return result;
                    }

                } while (s.ElapsedMilliseconds <= durationMS);

                return null;
            }

            public override ExpectationCommand<T> Negated() {
                return new ConsistentlyCommand<T>(_duration, _inner.Negated());
            }
        }
    }

}
