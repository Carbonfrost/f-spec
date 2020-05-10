//
// Copyright 2017, 2018, 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

namespace Carbonfrost.Commons.Spec {

    static partial class ExpectationCommand {
    }

    partial class Extensions {

        internal static ExpectationCommand<T> NegateIfNeeded<T>(this ExpectationCommand<T> cmd, bool negated) {
            if (negated) {
                cmd = cmd.Negated();
            }
            return cmd;
        }

        internal static void Should<T>(this ExpectationCommand<T> self, ITestMatcher<T> matcher, string message = null, object[] args = null) {
            var failure = self.Should(matcher);
            if (failure != null) {
                IAsserterBehavior behavior = failure.AsserterBehavior;
                behavior.Assert(failure.UpdateTestSubject().UpdateMessage(message, args));
            }
        }

        internal static void Should(this ExpectationCommand<Unit> self, ITestMatcher matcher, string message = null, object[] args = null) {
            var failure = self.Should(
                TestMatcher.UnitWrapper(matcher)
            );

            if (failure != null) {
                IAsserterBehavior behavior = failure.AsserterBehavior;
                behavior.Assert(failure.UpdateTestSubject().UpdateMessage(message, args));
            }
        }
    }

    abstract class ExpectationCommand<T> {

        internal virtual ExpectationCommand<Unit> Untyped() {
            throw new NotSupportedException();
        }

        public abstract TestFailure Should(ITestMatcher<T> matcher);

        public abstract ExpectationCommand<T> Negated();
        public abstract ExpectationCommand<T> Given(string given);

        public virtual ExpectationCommand<TBase> As<TBase>() {
            return new ExpectationCommand.CastCommand<T, TBase>(this);
        }

        // Given that T is IEnumerable -- convert to accumulator commands.
        // We can't know the type of the value (e.g. if T == IEnumerable<TValue>,
        // then TValue is unknown to us).  So we use object here, and implementations
        // should implicitly apply As<TValue>()
        public virtual ExpectationCommand<object> ToAll() {
            throw new NotSupportedException();
        }

        public virtual ExpectationCommand<object> ToAny() {
            throw new NotSupportedException();
        }

        public virtual ExpectationCommand<object> Cardinality(int? min, int? max) {
            throw new NotSupportedException();
        }

        public ExpectationCommand<T> Eventually(TimeSpan duration) {
            return new ExpectationCommand.EventuallyCommand<T>(duration, this);
        }

        public ExpectationCommand<T> Consistently(TimeSpan duration) {
            return new ExpectationCommand.ConsistentlyCommand<T>(duration, this);
        }

        public ExpectationCommand<TResult> Property<TResult>(Func<T, TResult> accessor) {
            return new ExpectationCommand.PropertyCommand<T, TResult>(this, accessor);
        }

        public ExpectationCommand<Exception> CaptureException() {
            return new ExpectationCommand.CaptureExceptionCommand<T>(this);
        }

        internal virtual void Implies(CommandCondition c) {
        }
    }

    enum CommandCondition {
        ExactlyOne,
        NotOneButZeroOrMore,
    }
}
