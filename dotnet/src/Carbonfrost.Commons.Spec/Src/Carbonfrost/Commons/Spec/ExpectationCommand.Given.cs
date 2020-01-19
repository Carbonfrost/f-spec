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
using System.Linq;
using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.Commons.Spec {

    partial class ExpectationCommand {

        public static ExpectationCommand<T> Given<T>(this ExpectationCommand<T> inner, string given) {
            if (given != null) {
                return new GivenCommand<T>(inner, given);
            }
            return inner;
        }

        public static IExpectationCommand Given(this IExpectationCommand inner, string given) {
            if (given != null) {
                return new GivenCommand(inner, given);
            }
            return inner;
        }

        class GivenCommand : IExpectationCommand {

            private readonly IExpectationCommand _inner;
            private readonly string _given;

            public GivenCommand(IExpectationCommand inner, string given) {
                _inner = inner;
                _given = given;
            }

            public TestFailure Should(ITestMatcher matcher) {
                var failure = _inner.Should(matcher);
                if (failure == null) {
                    return null;
                }

                return failure.UpdateGiven(_given);
            }

            public IExpectationCommand Negated() {
                return new GivenCommand(_inner.Negated(), _given);
            }

            public IExpectationCommand Eventually(TimeSpan delay) {
                return new GivenCommand(_inner.Eventually(delay), _given);
            }

            public IExpectationCommand Consistently(TimeSpan delay) {
                return new GivenCommand(_inner.Consistently(delay), _given);
            }
        }

        class GivenCommand<T> : ExpectationCommand<T> {

            private readonly ExpectationCommand<T> _inner;
            private readonly string _given;

            public GivenCommand(ExpectationCommand<T> inner, string given) {
                _inner = inner;
                _given = given;
            }

            public override ExpectationCommand<TBase> As<TBase>() {
                return new GivenCommand<TBase>(_inner.As<TBase>(), _given);
            }

            public override ExpectationCommand<T> Negated() {
                return new GivenCommand<T>(_inner.Negated(), _given);
            }

            public override IExpectationCommand Untyped() {
                return Given(_inner.Untyped(), _given);
            }

            public override ExpectationCommand<object> ToAll() {
                return _inner.ToAll();
            }

            public override ExpectationCommand<object> ToAny() {
                return _inner.ToAny();
            }

            public override ExpectationCommand<object> Cardinality(int? min, int? max) {
                return _inner.Cardinality(min, max);
            }

            public override ExpectationCommand<T> Consistently(TimeSpan duration) {
                return Given(_inner.Consistently(duration), _given);
            }

            public override ExpectationCommand<T> Eventually(TimeSpan duration) {
                return Given(_inner.Eventually(duration), _given);
            }

            public override void Implies(CommandCondition c) {
                _inner.Implies(c);
            }

            public override TestFailure Should(ITestMatcher<T> matcher) {
                var failure = _inner.Should(matcher);
                if (failure == null) {
                    return null;
                }

                return failure.UpdateGiven(_given);
            }
        }
    }
}
