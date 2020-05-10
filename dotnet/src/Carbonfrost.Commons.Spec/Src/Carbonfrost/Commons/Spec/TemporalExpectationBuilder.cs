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
using System.Collections;
using System.ComponentModel;

namespace Carbonfrost.Commons.Spec {

    struct TemporalExpectationBuilder : ITemporalExpectationBuilder {

        private readonly ExpectationCommand<Unit> _cmd;

        public ITemporalExpectationBuilder Not {
            get {
                return new TemporalExpectationBuilder(_cmd.Negated());
            }
        }

        public ISatisfactionExpectation ToSatisfy {
            get {
                return new SatisfactionExpectation(_cmd);
            }
        }

        public IExceptionExpectation ToThrow {
            get {
                return new ExceptionExpectation(_cmd);
            }
        }

        public IExpectation Will {
            get {
                return new Expectation(_cmd);
            }
        }

        internal TemporalExpectationBuilder(ExpectationCommand<Unit> cmd) {
            _cmd = cmd;
        }

        public void To(ITestMatcher matcher, string message = null, params object[] args) {
            _cmd.Should(matcher, message, args);
        }

        public new bool Equals(object b) {
            throw new InvalidOperationException("TemporalExpectationBuilder.Equals should not be used");
        }

        public void Like(ITestMatcher matcher, string message, params object[] args) {
            _cmd.Should(matcher, message, (object[]) args);
        }
    }

    struct TemporalExpectationBuilder<T> : ITemporalExpectationBuilder<T> {

        private readonly ExpectationCommand<T> _cmd;

        internal TemporalExpectationBuilder(ExpectationCommand<T> cmd) {
            _cmd = cmd;
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

        public ITemporalExpectationBuilder<T> Not {
            get {
                return new TemporalExpectationBuilder<T>(_cmd.Negated());
            }
        }

        public ITemporalExpectationBuilder<TBase> As<TBase>() {
            return new TemporalExpectationBuilder<TBase>(_cmd.As<TBase>());
        }

        public void Like(ITestMatcher matcher, string message, params object[] args) {
            _cmd.Untyped().Should(matcher, message, (object[]) args);
        }

        public void Like(ITestMatcher<T> matcher, string message, params object[] args) {
            _cmd.Should(matcher, message, (object[]) args);
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
            throw new InvalidOperationException("TemporalExpectationBuilder.Equals should not be used");
        }
    }

}
