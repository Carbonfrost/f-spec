//
// Copyright 2016, 2017, 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

namespace Carbonfrost.Commons.Spec {

    public struct ExpectationBuilder : IExpectationBuilder {

        private readonly ExpectationCommand _cmd;

        internal static TimeSpan DefaultDelay {
            get {
                return TimeSpan.FromSeconds(0.500);
            }
        }

        public TemporalExpectationBuilder Consistently {
            get {
                return new TemporalExpectationBuilder(_cmd.Consistently(ExpectationBuilder.DefaultDelay));
            }
        }

        public TemporalExpectationBuilder Eventually {
            get {
                return new TemporalExpectationBuilder(_cmd.Eventually(ExpectationBuilder.DefaultDelay));
            }
        }

        public Expectation Will {
            get {
                return new Expectation(_cmd);
            }
        }

        public ExceptionExpectation ToThrow {
            get {
                return new ExceptionExpectation(_cmd);
            }
        }

        public SatisfactionExpectation ToSatisfy {
            get {
                return new SatisfactionExpectation(_cmd);
            }
        }

        public ExpectationBuilder Not {
            get {
                return new ExpectationBuilder(_cmd.Negated());
            }
        }

        internal ExpectationBuilder(ExpectationCommand cmd) {
            _cmd = cmd;
        }

        internal ExpectationBuilder(Action thunk, bool negated, string given) {
            _cmd = ExpectationCommand.TestCode(thunk).NegateIfNeeded(negated).Given(given);
        }

        public void To(ITestMatcher matcher, string message = null, params object[] args) {
            ToExpectation().Should(matcher, message, args);
        }

        public void NotTo(ITestMatcher matcher, string message = null, params object[] args) {
            ToExpectation().Should(Matchers.Not(matcher), message, args);
        }

        public void ToNot(ITestMatcher matcher, string message = null, params object[] args) {
            ToExpectation().Should(Matchers.Not(matcher), message, args);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This is an override of Object.Equals(). Call Assert.Equal() instead.", true)]
        public new bool Equals(object b) {
            throw new InvalidOperationException("ExpectationBuilder.Equals should not be used");
        }

        private Expectation ToExpectation() {
            return new Expectation(_cmd);
        }
    }

    public struct ExpectationBuilder<T> : IExpectationBuilder<T> {

        private readonly ExpectationCommand<T> _cmd;

        public ExpectationBuilder<T> Not {
            get {
                return new ExpectationBuilder<T>(_cmd.Negated());
            }
        }

        public TemporalExpectationBuilder<T> Consistently {
            get {
                return new TemporalExpectationBuilder<T>(_cmd.Consistently(ExpectationBuilder.DefaultDelay));
            }
        }

        public TemporalExpectationBuilder<T> Eventually {
            get {
                return new TemporalExpectationBuilder<T>(_cmd.Eventually(ExpectationBuilder.DefaultDelay));
            }
        }

        public Expectation<T> ToBe {
            get {
                return new Expectation<T>(_cmd);
            }
        }

        public EnumerableExpectation ToHave {
            get {
                return new EnumerableExpectation(_cmd.As<IEnumerable>());
            }
        }

        public SatisfactionExpectation<T> ToSatisfy {
            get {
                return new SatisfactionExpectation<T>(_cmd);
            }
        }

        internal ExpectationBuilder(Func<T> thunk, bool negated, string given) {
            _cmd = ExpectationCommand.Of(thunk).NegateIfNeeded(negated).Given(given);
        }

        internal ExpectationBuilder(ExpectationCommand<T> cmd) {
            _cmd = cmd;
        }

        public ExpectationBuilder<TBase> As<TBase>() {
            return new ExpectationBuilder<TBase>(_cmd.As<TBase>());
        }

        public void To(ITestMatcher matcher, string message = null, params object[] args) {
            ToExpectation(false).Untyped().Should(matcher, message, args);
        }

        public void To(ITestMatcher<T> matcher, string message = null, params object[] args) {
            ToExpectation(false).Should(matcher, message, args);
        }

        public void NotTo(ITestMatcher matcher, string message = null, params object[] args) {
            ToExpectation(true).Untyped().Should(matcher, message, args);
        }

        public void NotTo(ITestMatcher<T> matcher, string message = null, params object[] args) {
            ToExpectation(true).Should(matcher, message, args);
        }

        public void ToNot(ITestMatcher matcher, string message = null, params object[] args) {
            ToExpectation(true).Untyped().Should(matcher, message, args);
        }

        public void ToNot(ITestMatcher<T> matcher, string message = null, params object[] args) {
            ToExpectation(true).Should(matcher, message, args);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This is an override of Object.Equals(). Call Assert.Equal() instead.", true)]
        public new bool Equals(object b) {
            throw new InvalidOperationException("ExpectationBuilder.Equals should not be used");
        }

        private Expectation<T> ToExpectation(bool negated) {
            return new Expectation<T>(_cmd.NegateIfNeeded(negated));
        }
    }

}
