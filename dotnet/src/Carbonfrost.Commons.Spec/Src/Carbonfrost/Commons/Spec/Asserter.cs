//
// Copyright 2019, 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
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
using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.Commons.Spec {

    internal partial class Asserter {

        internal static readonly Asserter Default = new Asserter(false);
        internal static readonly Asserter Assumptions = new Asserter(true);
        private readonly AsserterBehavior _defaultBehavior;
        private bool _disabled;

        private AsserterBehavior Behavior {
            get {
                return _disabled ? AsserterBehavior.Disabled : _defaultBehavior;
            }
        }

        internal Asserter(bool assumption) {
            _defaultBehavior = assumption ? AsserterBehavior.Assumption : AsserterBehavior.Default;
        }

        public IExpectationBuilder<IEnumerable> Expect() {
            return new ExpectationBuilder<IEnumerable>(() => Array.Empty<object>(), false, null, Behavior);
        }

        public IExpectationBuilder<T> Expect<T>(T value) {
            return new ExpectationBuilder<T>(() => value, false, null, Behavior);
        }

        public IExpectationBuilder<TEnumerable, T> Expect<TEnumerable, T>(TEnumerable value) where TEnumerable : IEnumerable<T> {
            return new ExpectationBuilder<TEnumerable, T>(() => value, false, null, Behavior);
        }

        public IExpectationBuilder<TValue[], TValue> Expect<TValue>(params TValue[] value) {
            return Expect<TValue[], TValue>(value);
        }

        public IExpectationBuilder Expect(Action value) {
            return new ExpectationBuilder(value, false, null, Behavior);
        }

        public IExpectationBuilder<T> Expect<T>(Func<T> func) {
            return Given().Expect(func);
        }

        public void Pass(string message) {
            RaiseException(SpecFailure.Pass(message));
        }

        public void Pass() {
            RaiseException(SpecFailure.Pass());
        }

        public void Pass(IFormatProvider formatProvider, string format, params object[] args) {
            RaiseException(SpecFailure.Pass(string.Format(formatProvider, format, args)));
        }

        public void Pass(string format, params object[] args) {
            RaiseException(SpecFailure.Pass(string.Format(format, args)));
        }

        public void Fail(IFormatProvider formatProvider, string format, params object[] args) {
            RaiseException(SpecFailure.Fail(string.Format(formatProvider, format, args)));
        }

        public void Fail(string format, params object[] args) {
            RaiseException(SpecFailure.Fail(string.Format(format, args)));
        }

        public void Fail(string message) {
            RaiseException(SpecFailure.Fail(message));
        }

        public void Fail() {
            RaiseException(SpecFailure.Fail());
        }

        public void Pending(string message) {
            RaiseException(SpecFailure.Pending(message));
        }

        public void Pending() {
            RaiseException(SpecFailure.Pending());
        }

        public void Pending(IFormatProvider formatProvider, string format, params object[] args) {
            RaiseException(SpecFailure.Pending(string.Format(formatProvider, format, args)));
        }

        public void Pending(string format, params object[] args) {
            RaiseException(SpecFailure.Pending(string.Format(format, args)));
        }

        private void RaiseException(Exception ex) {
            Behavior.Fail(ex);
        }

        public IDisposable Disabled() {
            _disabled = true;
            return new Disposer(() => _disabled = false);
        }

        internal void That(Action actual, ITestMatcher matcher, string message = null, params object[] args) {
            Expect(actual).To(matcher, message, (object[]) args);
        }

        internal void That<T>(T actual, ITestMatcher<T> matcher, string message = null, params object[] args) {
            Expect(actual).To(matcher, message, (object[]) args);
        }

        internal void NotThat(Action actual, ITestMatcher matcher, string message = null, params object[] args) {
            That(actual, Matchers.Not(matcher), message, (object[]) args);
        }

        internal void NotThat<T>(T actual, ITestMatcher<T> matcher, string message = null, params object[] args) {
            That(actual, Matchers.Not(matcher), message, (object[]) args);
        }
    }
}
