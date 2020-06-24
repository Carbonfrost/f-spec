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
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Carbonfrost.Commons.Spec {

    struct ExpectationBuilder : IExpectationBuilder {

        private readonly ExpectationCommand<Unit> _cmd;

        internal static TimeSpan DefaultDelay {
            get {
                return TimeSpan.FromSeconds(0.500);
            }
        }

        public ITemporalExpectationBuilder Consistently {
            get {
                return new TemporalExpectationBuilder(_cmd.Consistently(ExpectationBuilder.DefaultDelay));
            }
        }

        public ITemporalExpectationBuilder Eventually {
            get {
                return new TemporalExpectationBuilder(_cmd.Eventually(ExpectationBuilder.DefaultDelay));
            }
        }

        public IExpectation Will {
            get {
                return new Expectation(_cmd);
            }
        }

        public IExceptionExpectation ToThrow {
            get {
                return new ExceptionExpectation(_cmd);
            }
        }

        public ISatisfactionExpectation ToSatisfy {
            get {
                return new SatisfactionExpectation(_cmd);
            }
        }

        public IExpectationBuilder Not {
            get {
                return new ExpectationBuilder(_cmd.Negated());
            }
        }

        internal ExpectationBuilder(ExpectationCommand<Unit> cmd) {
            _cmd = cmd;
        }

        internal ExpectationBuilder(Action thunk, bool negated, string given, bool assumption) {
            _cmd = ExpectationCommand.TestCode(thunk, negated, given, assumption);
        }

        public void To(ITestMatcher matcher, string message = null, params object[] args) {
            _cmd.Should(matcher, message, args);
        }

        public new bool Equals(object b) {
            throw new InvalidOperationException("ExpectationBuilder.Equals should not be used");
        }
    }

    struct ExpectationBuilder<T> : IExpectationBuilder<T> {

        private readonly ExpectationCommand<T> _cmd;

        public IExpectationBuilder<T> Not {
            get {
                return new ExpectationBuilder<T>(_cmd.Negated());
            }
        }

        public ITemporalExpectationBuilder<T> Consistently {
            get {
                return new TemporalExpectationBuilder<T>(_cmd.Consistently(ExpectationBuilder.DefaultDelay));
            }
        }

        public ITemporalExpectationBuilder<T> Eventually {
            get {
                return new TemporalExpectationBuilder<T>(_cmd.Eventually(ExpectationBuilder.DefaultDelay));
            }
        }

        public IExpectation<T> ToBe {
            get {
                return new Expectation<T>(_cmd);
            }
        }

        public IEnumerableExpectation ToHave {
            get {
                return new EnumerableExpectation(_cmd.As<IEnumerable>());
            }
        }

        public ISatisfactionExpectation<T> ToSatisfy {
            get {
                return new SatisfactionExpectation<T>(_cmd);
            }
        }

        internal ExpectationBuilder(Func<T> thunk, bool negated, string given, bool assumption) {
            _cmd = ExpectationCommand.Of(thunk, negated, given, assumption);
        }

        internal ExpectationBuilder(ExpectationCommand<T> cmd) {
            _cmd = cmd;
        }

        public IExpectationBuilder<TBase> As<TBase>() {
            return new ExpectationBuilder<TBase>(_cmd.As<TBase>());
        }

        public IExpectationBuilder<TProperty> Property<TProperty>(Expression<Func<T, TProperty>> property) {
            var name = ((MemberExpression) property.Body).Member.Name;
            return new ExpectationBuilder<TProperty>(_cmd.Property(property.Compile(), name));
        }

        public void To(ITestMatcher matcher, string message = null, params object[] args) {
            _cmd.Untyped().Should(matcher, message, args);
        }

        public void To(ITestMatcher<T> matcher, string message = null, params object[] args) {
            _cmd.Should(matcher, message, args);
        }

        public void To(ITestMatcher<object> matcher, string message = null, params object[] args) {
            _cmd.As<object>().Should(matcher, message, args);
        }

        public new bool Equals(object b) {
            throw new InvalidOperationException("ExpectationBuilder.Equals should not be used");
        }
    }

    struct ExpectationBuilder<TSelf, T> : IExpectationBuilder<TSelf, T>
        where TSelf : IEnumerable<T>
    {

        private readonly ExpectationCommand<TSelf> _cmd;

        IExpectationBuilder<TSelf> IExpectationBuilder<TSelf>.Not {
            get {
                return Not;
            }
        }

        public IExpectationBuilder<TSelf, T> Not {
            get {
                return new ExpectationBuilder<TSelf, T>(_cmd.Negated());
            }
        }

        public ITemporalExpectationBuilder<TSelf> Consistently {
            get {
                return new TemporalExpectationBuilder<TSelf>(_cmd.Consistently(ExpectationBuilder.DefaultDelay));
            }
        }

        public ITemporalExpectationBuilder<TSelf> Eventually {
            get {
                return new TemporalExpectationBuilder<TSelf>(_cmd.Eventually(ExpectationBuilder.DefaultDelay));
            }
        }

        public IExpectation<TSelf> ToBe {
            get {
                return new Expectation<TSelf>(_cmd);
            }
        }

        IEnumerableExpectation IExpectationBuilder<TSelf>.ToHave {
            get {
                return new EnumerableExpectation(_cmd.As<IEnumerable>());
            }
        }

        public IEnumerableExpectation<T> ToHave {
            get {
                return new EnumerableExpectation<T>(_cmd.As<IEnumerable>().Items<T>());
            }
        }

        public ISatisfactionExpectation<TSelf> ToSatisfy {
            get {
                return new SatisfactionExpectation<TSelf>(_cmd);
            }
        }

        internal ExpectationBuilder(Func<TSelf> thunk, bool negated, string given, bool assumption) {
            _cmd = ExpectationCommand.Of(thunk, negated, given, assumption);
        }

        private ExpectationBuilder(ExpectationCommand<TSelf> cmd) {
            _cmd = cmd;
        }

        public IExpectationBuilder<TBase> As<TBase>() {
            return new ExpectationBuilder<TBase>(_cmd.As<TBase>());
        }

        public IExpectationBuilder<TProperty> Property<TProperty>(Expression<Func<TSelf, TProperty>> property) {
            var name = ((MemberExpression) property.Body).Member.Name;
            return new ExpectationBuilder<TProperty>(_cmd.Property(property.Compile(), name));
        }

        public void To(ITestMatcher matcher, string message = null, params object[] args) {
            _cmd.Untyped().Should(matcher, message, args);
        }

        public void To(ITestMatcher<TSelf> matcher, string message = null, params object[] args) {
            _cmd.Should(matcher, message, args);
        }

        public void To(ITestMatcher<object> matcher, string message = null, params object[] args) {
            _cmd.As<object>().Should(matcher, message, args);
        }

        public new bool Equals(object b) {
            throw new InvalidOperationException("ExpectationBuilder.Equals should not be used");
        }
    }

}
