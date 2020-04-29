//
// Copyright 2017, 2018 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using Carbonfrost.Commons.Spec.ExecutionModel;
using Carbonfrost.Commons.Spec.Resources;

namespace Carbonfrost.Commons.Spec {

    partial class ExpectationCommand {

        struct Tally {

            public int Successes;
            public int Failures;
            public bool Stopped;
            public IList<int> Indexes;
            public TestFailure AFailure;

            public static Tally New<T>(
                Func<IEnumerable<T>> thunk,
                ITestMatcher<T> matcher,
                Predicate<Tally> stop,
                TestFailure predicateFailure = null) {

                if (stop == null) {
                    stop = _ => false;
                }
                if (predicateFailure == null) {
                    predicateFailure = TestMatcherLocalizer.FailurePredicate(matcher);
                }

                var result = new Tally();
                result.Indexes = new List<int>();
                int index = 0;
                foreach (var item in thunk()) {
                    if (stop(result)) {
                        result.Stopped = true;
                        break;
                    }

                    var matches = matcher.Matches(TestActual.Value(item));

                    if (matches) {
                        result.Successes++;
                    } else {
                        result.Failures++;
                        result.Indexes.Add(index);
                        result.AFailure = predicateFailure;
                    }

                    index++;
                }
                return result;
            }

            internal TestFailure Lift(string code, string message, bool required = false) {
                if (!required && AFailure == null) {
                    return null;
                }

                var testFailure = new TestFailure(code) {
                    Message = message
                };
                if (AFailure != null) {
                    testFailure.Children.Add(AFailure);
                }
                if (Indexes != null) {
                    testFailure.UserData["Indexes"] = TextUtility.Truncate(Indexes);
                }
                string successes = Successes.ToString();
                if (Stopped) {
                    successes = ">" + successes;
                }
                testFailure.UserData["ActualCount"] = successes;
                return testFailure;
            }
        }

        abstract class SequenceCommandBase<T> : ExpectationCommand<T> {

            protected readonly Func<IEnumerable<T>> thunk;

            protected SequenceCommandBase(Func<IEnumerable<T>> thunk) {
                this.thunk = thunk;
            }

        }

        // All and NotAny give up after 5 failures

        class AllCommand<T> : SequenceCommandBase<T> {

            public AllCommand(Func<IEnumerable<T>> thunk) : base(thunk) {}

            public override ExpectationCommand<T> Negated() {
                return new NotAllCommand<T>(thunk);
            }

            public override TestFailure Should(ITestMatcher<T> matcher) {
                var tally = Tally.New(thunk, matcher, t => (t.Failures == 5));
                return tally.Lift("spec.all", SR.ExpectedAllElementsTo());
            }
        }

        // For the negated commands NotAny and NotAll -- we should tally against
        // the negated matcher; however, in order to get the right (non-double negative)
        // message from the matcher, we should use the bare matcher for the localized message

        class NotAnyCommand<T> : SequenceCommandBase<T> {

            public NotAnyCommand(Func<IEnumerable<T>> thunk) : base(thunk) {}

            public override ExpectationCommand<T> Negated() {
                return new AnyCommand<T>(thunk);
            }

            public override TestFailure Should(ITestMatcher<T> matcher) {
                var tally = Tally.New(thunk,
                                      Matchers.Not(matcher),
                                      t => (t.Failures == 5),
                                      TestMatcherLocalizer.FailurePredicate(matcher));
                return tally.Lift("spec.notAny", SR.ExpectedNotAnyElementTo());
            }
        }

        class NotAllCommand<T> : SequenceCommandBase<T> {

            public NotAllCommand(Func<IEnumerable<T>> thunk) : base(thunk) {}

            public override ExpectationCommand<T> Negated() {
                return new AllCommand<T>(thunk);
            }

            public override TestFailure Should(ITestMatcher<T> matcher) {
                var tally = Tally.New(thunk,
                                      Matchers.Not(matcher),
                                      t => (t.Successes > 0),
                                      TestMatcherLocalizer.FailurePredicate(matcher));
                if (tally.Successes > 0) {
                    return null;
                }
                return tally.Lift("spec.notAll", SR.ExpectedNotAllElementsTo());
            }
        }

        class AnyCommand<T> : SequenceCommandBase<T> {

            public AnyCommand(Func<IEnumerable<T>> thunk) : base(thunk) {}

            public override ExpectationCommand<T> Negated() {
                return new NotAnyCommand<T>(thunk);
            }

            public override TestFailure Should(ITestMatcher<T> matcher) {
                var tally = Tally.New(thunk, matcher, t => (t.Successes > 0));
                if (tally.Successes > 0) {
                    return null;
                }
                return tally.Lift("spec.any", SR.ExpectedAnyElementTo());
            }
        }

        class CardinalityCommand<T> : SequenceCommandBase<T> {

            private readonly int? _min;
            private readonly int? _max;
            private readonly bool _outer;

            internal bool ShouldVerify {
                get {
                    return Assert.UseStrictMode;
                }
            }

            public CardinalityCommand(Func<IEnumerable<T>> thunk, int? min, int? max, bool outer) : base(thunk) {
                _min = min;
                _max = max;
                _outer = outer;
            }

            public override ExpectationCommand<T> Negated() {
                return new CardinalityCommand<T>(thunk, _min, _max, !_outer);
            }

            public override void Implies(CommandCondition c) {
                if (!ShouldVerify) {
                    return;
                }
                if (c == CommandCondition.ExactlyOne) {
                    if (_min.GetValueOrDefault(1) != 1 || _max.GetValueOrDefault(1) != 1) {
                        throw SpecFailure.ExactlyOnePlural();
                    }

                } else if (c == CommandCondition.NotOneButZeroOrMore) {
                    if (_min.GetValueOrDefault(1) == 1 && _max.GetValueOrDefault(1) == 1) {
                        throw SpecFailure.ExactlyOneSingular();
                    }
                }
            }

            private string Message() {
                string message;
                if (_min == _max) {
                    message = SR.ExpectedExactly(_min);
                } else if (_min.HasValue && _max.HasValue) {
                    message = SR.ExpectedBetweenTo(_min, _max);
                } else if (_min.HasValue) {
                    message = SR.ExpectedAtLeastTo(_min);
                } else {
                    message = SR.ExpectedAtMostTo(_max);
                }

                // TODO Here and below -- use localized strings, don't rely on this
                // being in English
                if (_outer) {
                    message = message.Replace("Expected", "Expected not");
                }
                return message;
            }

            public override TestFailure Should(ITestMatcher<T> matcher) {
                if (ShouldVerify) {
                    if (_max < _min) {
                        throw SpecFailure.CardinalityMinGreaterThanMax();
                    }
                    if (_min < 0 || _max < 0) {
                        throw SpecFailure.NegativeCardinality();
                    }
                }
                if (!_min.HasValue && !_max.HasValue) {
                    return null;
                }

                Predicate<Tally> stopper = null;
                if (_max.HasValue) {
                    stopper = t => t.Successes > _max;
                }

                var tally = Tally.New(thunk, matcher, stopper);
                bool outOfRange = (_max.HasValue && tally.Successes > _max.Value)
                    || (_min.HasValue && tally.Successes < _min.Value);

                if (_outer) {
                    outOfRange = !outOfRange;
                }

                if (outOfRange) {
                    string message = Message();
                    tally.AFailure = TestMatcherLocalizer.FailurePredicate(matcher);

                    if (tally.AFailure.Message == "") {
                        tally.AFailure = null;
                        message = message.Replace("to:", "items");
                    }

                    return tally.Lift("spec.cardinality", message, true);
                }

                return null;
            }
        }
    }

}
