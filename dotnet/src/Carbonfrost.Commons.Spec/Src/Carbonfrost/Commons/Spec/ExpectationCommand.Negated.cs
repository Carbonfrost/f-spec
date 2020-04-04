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

    partial class Extensions {

        internal static ExpectationCommand<T> NegateIfNeeded<T>(this ExpectationCommand<T> cmd, bool negated) {
            if (negated) {
                cmd = cmd.Negated();
            }
            return cmd;
        }

        internal static ExpectationCommand NegateIfNeeded(this ExpectationCommand cmd, bool negated) {
            if (negated) {
                cmd = cmd.Negated();
            }
            return cmd;
        }
    }

    partial class ExpectationCommand {

        class NegationCommand<T> : ExpectationCommand<T> {

            private readonly ExpectationCommand<T> _inner;

            public NegationCommand(ExpectationCommand<T> inner) {
                _inner = inner;
            }

            public override ExpectationCommand<TBase> As<TBase>() {
                return new NegationCommand<TBase>(_inner.As<TBase>());
            }

            public override ExpectationCommand<T> Negated() {
                return _inner;
            }

            public override ExpectationCommand<object> ToAll() {
                return _inner.ToAll().Negated();
            }

            public override ExpectationCommand<object> ToAny() {
                return _inner.ToAny().Negated();
            }

            public override ExpectationCommand<object> Cardinality(int? min, int? max) {
                return _inner.Cardinality(min, max).Negated();
            }

            public override ExpectationCommand<T> Consistently(TimeSpan duration) {
                return _inner.Consistently(duration).Negated();
            }

            public override ExpectationCommand<T> Eventually(TimeSpan duration) {
                return _inner.Eventually(duration).Negated();
            }

            public override void Implies(CommandCondition c) {
                _inner.Implies(c);
            }

            public override TestFailure Should(ITestMatcher<T> matcher) {
                matcher = Matchers.Not(matcher);
                return _inner.Should(matcher);
            }
        }
    }
}
