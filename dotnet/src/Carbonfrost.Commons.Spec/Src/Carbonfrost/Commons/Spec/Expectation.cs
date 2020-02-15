//
// Copyright 2016, 2017, 2018 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Collections;
using System.ComponentModel;
using System.Linq;
using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.Commons.Spec {

    public struct Expectation {

        private readonly IExpectationCommand _cmd;

        public Expectation Not {
            get {
                return new Expectation(_cmd.Negated());
            }
        }

        internal Expectation(IExpectationCommand cmd) {
            _cmd = cmd;
        }

        internal void Should(ITestMatcher matcher, string message = null, params object[] args) {
            var failure = _cmd.Should(matcher);

            if (failure != null) {
                throw failure.UpdateTestSubject().UpdateMessage(message, args).ToException();
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This is an override of Object.Equals(). Call Assert.Equal() instead.", true)]
        public new bool Equals(object b) {
            throw new InvalidOperationException("Expectation.Equals should not be used");
        }
    }

    public struct Expectation<T> {

        private readonly ExpectationCommand<T> _cmd;

        public Expectation<T> Not {
            get {
                return new Expectation<T>(_cmd.Negated());
            }
        }

        internal Expectation(ExpectationCommand<T> cmd) {
            _cmd = cmd;
        }

        public Expectation<T> Approximately(T epsilon) {
            return new Expectation<T>(ExpectationCommand.Comparer(EpsilonComparer.Create(epsilon), _cmd));
        }

        public Expectation<T> Approximately<TEpsilon>(TEpsilon epsilon) {
            return new Expectation<T>(ExpectationCommand.Comparer(EpsilonComparer.Create<T, TEpsilon>(epsilon), _cmd));
        }

        public Expectation<TBase> As<TBase>() {
            return new Expectation<TBase>(_cmd.As<TBase>());
        }

        public void InstanceOf<TExpected>() {
            InstanceOf<TExpected>(null);
        }

        public void InstanceOf<TExpected>(string message, params object[] args) {
            As<object>().Should(Matchers.BeInstanceOf(typeof(TExpected)), message, (object[]) args);
        }

        public void Items() {
            Items(null);
        }

        public void Items(string message, params object[] args) {
            _cmd.Implies(CommandCondition.NotOneButZeroOrMore);
            As<IEnumerable>().Should(TestMatcher<object>.Anything, message, (object[]) args);
        }

        public void Item() {
            Item(null);
        }

        public void Item(string message, params object[] args) {
            _cmd.Implies(CommandCondition.ExactlyOne);
            As<IEnumerable>().Should(TestMatcher<object>.Anything, message, (object[]) args);
        }

        internal Expectation Untyped() {
            return new Expectation(_cmd.Untyped());
        }

        internal void Should(ITestMatcher<T> matcher, string message = null, params object[] args) {
            var failure = _cmd.Should(matcher);

            if (failure != null) {
                throw failure.UpdateTestSubject().UpdateMessage(message, args).ToException();
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This is an override of Object.Equals(). Call Assert.Equal() instead.", true)]
        public new bool Equals(object b) {
            throw new InvalidOperationException("Expectation.Equals should not be used");
        }
    }
}
