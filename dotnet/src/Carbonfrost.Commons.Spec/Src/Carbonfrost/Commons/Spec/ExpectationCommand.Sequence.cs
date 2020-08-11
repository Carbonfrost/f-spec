//
// Copyright 2017, 2018, 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
                IEnumerable<T> items,
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
                foreach (var item in items) {
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

        // Assertion on one or more of the items in a list.  Since we use the non-generic
        // IEnumerable, we have no idea what the type of the items could be so, we can only
        // assert on object
        //
        abstract class SequenceCommandBase : ExpectationCommand<object>, ITestMatcher<IEnumerable> {

            private readonly ExpectationCommand<IEnumerable> _inner;
            private ITestMatcher<object> _real;

            // Somewhat hackish, but we share the result with the parent
            public TestFailure Result;

            protected ExpectationCommand<IEnumerable> Inner {
                get {
                    return _inner;
                }
            }

            protected SequenceCommandBase(ExpectationCommand<IEnumerable> inner) {
                _inner = inner;
            }

            public sealed override TestFailure Should(ITestMatcher<object> matcher) {
                _real = matcher;
                var _ = _inner.Should(this);
                return Result;
            }

            protected abstract TestFailure ShouldCore(
                IEnumerable<object> items,
                ITestMatcher<object> baseMatcher
            );

            public bool Matches(ITestActualEvaluation<IEnumerable> actualFactory) {
                var items = actualFactory.Value;
                if (items == null) {
                    throw SpecFailure.SequenceNullConversion();
                }

                Result = ShouldCore(items.Cast<object>(), _real);
                return Result == null;
            }
        }

        // All and NotAny give up after 5 failures
        class AllCommand : SequenceCommandBase {

            public AllCommand(ExpectationCommand<IEnumerable> inner) : base(inner) {}

            public override ExpectationCommand<object> Negated() {
                return new NotAllCommand(Inner);
            }

            public override ExpectationCommand<object> Given(string given) {
                return new AllCommand(Inner.Given(given));
            }

            protected override TestFailure ShouldCore(IEnumerable<object> items,
                ITestMatcher<object> baseMatcher
            ) {
                var tally = Tally.New(items, baseMatcher, t => (t.Failures == 5));
                return tally.Lift("all", SR.ExpectedAllElementsTo());
            }
        }

        // For the negated commands NotAny and NotAll -- we should tally against
        // the negated matcher; however, in order to get the right (non-double negative)
        // message from the matcher, we should use the bare matcher for the localized message

        class NotAnyCommand : SequenceCommandBase {

            public NotAnyCommand(ExpectationCommand<IEnumerable> inner) : base(inner) {}

            public override ExpectationCommand<object> Negated() {
                return new AnyCommand(Inner);
            }

            public override ExpectationCommand<object> Given(string given) {
                return new NotAnyCommand(Inner.Given(given));
            }

            protected override TestFailure ShouldCore(IEnumerable<object> items, ITestMatcher<object> baseMatcher) {
                var tally = Tally.New(items,
                                      Matchers.Not(baseMatcher),
                                      t => (t.Failures == 5),
                                      TestMatcherLocalizer.FailurePredicate(baseMatcher));
                return tally.Lift("notAny", SR.ExpectedNotAnyElementTo());
            }
        }

        class NotAllCommand : SequenceCommandBase {

            public NotAllCommand(ExpectationCommand<IEnumerable> inner) : base(inner) {}

            public override ExpectationCommand<object> Negated() {
                return new AllCommand(Inner);
            }

            public override ExpectationCommand<object> Given(string given) {
                return new NotAllCommand(Inner.Given(given));
            }

            protected override TestFailure ShouldCore(IEnumerable<object> items, ITestMatcher<object> baseMatcher) {
                var tally = Tally.New(items,
                                      Matchers.Not(baseMatcher),
                                      t => (t.Successes > 0),
                                      TestMatcherLocalizer.FailurePredicate(baseMatcher));
                if (tally.Successes > 0) {
                    return null;
                }
                return tally.Lift("notAll", SR.ExpectedNotAllElementsTo());
            }
        }

        class AnyCommand : SequenceCommandBase {

            public AnyCommand(ExpectationCommand<IEnumerable> inner) : base(inner) {}

            public override ExpectationCommand<object> Negated() {
                return new NotAnyCommand(Inner);
            }

            public override ExpectationCommand<object> Given(string given) {
                return new AnyCommand(Inner.Given(given));
            }

            protected override TestFailure ShouldCore(IEnumerable<object> items, ITestMatcher<object> baseMatcher) {
                var tally = Tally.New(items, baseMatcher, t => (t.Successes > 0));
                if (tally.Successes > 0) {
                    return null;
                }
                return tally.Lift("any", SR.ExpectedAnyElementTo());
            }
        }

        class CardinalityCommand : SequenceCommandBase {

            private readonly int? _min;
            private readonly int? _max;
            private readonly bool _outer;

            internal bool ShouldVerify {
                get {
                    return Assert.UseStrictMode;
                }
            }

            public CardinalityCommand(ExpectationCommand<IEnumerable> inner, int? min, int? max, bool outer) : base(inner) {
                _min = min;
                _max = max;
                _outer = outer;
            }

            public override ExpectationCommand<object> Negated() {
                return new CardinalityCommand(Inner, _min, _max, !_outer);
            }

            public override ExpectationCommand<object> Given(string given) {
                return new CardinalityCommand(Inner.Given(given), _min, _max, _outer);
            }

            internal override void Implies(CommandCondition c) {
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

            protected override TestFailure ShouldCore(IEnumerable<object> items, ITestMatcher<object> baseMatcher) {
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

                var tally = Tally.New(items, baseMatcher, stopper);
                bool outOfRange = (_max.HasValue && tally.Successes > _max.Value)
                    || (_min.HasValue && tally.Successes < _min.Value);

                if (_outer) {
                    outOfRange = !outOfRange;
                }

                if (outOfRange) {
                    string message = Message();
                    tally.AFailure = TestMatcherLocalizer.FailurePredicate(baseMatcher);

                    if (tally.AFailure.Message == "") {
                        tally.AFailure = null;
                        message = message.Replace("to:", "items");
                    }

                    return tally.Lift("cardinality", message, true);
                }

                return null;
            }
        }
    }

}
