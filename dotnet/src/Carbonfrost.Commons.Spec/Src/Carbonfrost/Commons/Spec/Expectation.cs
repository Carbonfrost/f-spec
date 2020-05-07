//
// Copyright 2016, 2017, 2018, 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

    struct Expectation : IExpectation {

        private readonly ExpectationCommand<Unit> _cmd;

        public IExpectation Not {
            get {
                return new Expectation(_cmd.Negated());
            }
        }

        internal Expectation(ExpectationCommand<Unit> cmd) {
            _cmd = cmd;
        }

        public new bool Equals(object b) {
            throw new InvalidOperationException("Expectation.Equals should not be used");
        }

        public void Like(ITestMatcher matcher, string message = null, object[] args = null) {
            _cmd.Should(matcher, message, args);
        }
    }

    struct Expectation<T> : IExpectation<T> {

        private readonly ExpectationCommand<T> _cmd;

        public IExpectation<T> Not {
            get {
                return new Expectation<T>(_cmd.Negated());
            }
        }

        internal Expectation(ExpectationCommand<T> cmd) {
            _cmd = cmd;
        }

        public IExpectation<T> Approximately<TEpsilon>(TEpsilon epsilon) {
            return new Expectation<T>(ExpectationCommand.Comparer(EpsilonComparer.Create<T, TEpsilon>(epsilon), _cmd));
        }

        public IExpectation<TBase> As<TBase>() {
            return new Expectation<TBase>(_cmd.As<TBase>());
        }

        public void InstanceOf<TExpected>() {
            InstanceOf<TExpected>(null);
        }

        public void InstanceOf<TExpected>(string message, params object[] args) {
            As<object>().Like(Matchers.BeInstanceOf(typeof(TExpected)), message, (object[]) args);
        }

        public void Items() {
            Items(null);
        }

        public void Items(string message, params object[] args) {
            _cmd.Implies(CommandCondition.NotOneButZeroOrMore);
            As<IEnumerable>().Like(TestMatcher<object>.Anything, message, (object[]) args);
        }

        public void Item() {
            Item(null);
        }

        public void Item(string message, params object[] args) {
            _cmd.Implies(CommandCondition.ExactlyOne);
            As<IEnumerable>().Like(TestMatcher<object>.Anything, message, (object[]) args);
        }

        internal Expectation Untyped() {
            return new Expectation(_cmd.Untyped());
        }

        public new bool Equals(object b) {
            throw new InvalidOperationException("Expectation.Equals should not be used");
        }

        public void Like(ITestMatcher<T> matcher, string message = null, params object[] args) {
            _cmd.Should(matcher, message, args);
        }

        public void Like(ITestMatcher matcher, string message = null, params object[] args) {
            _cmd.Untyped().Should(matcher, message, args);
        }
    }
}
