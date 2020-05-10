//
// Copyright 2017, 2018 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

namespace Carbonfrost.Commons.Spec {

    struct EnumerableExpectation : IEnumerableExpectation {

        private readonly ExpectationCommand<IEnumerable> _cmd;

        IExpectation<IEnumerable<object>> IEnumerableExpectation<object>.Self {
            get {
                return new Expectation<IEnumerable<object>>(_cmd.Items());
            }
        }

        public IExpectation<IEnumerable> Self {
            get {
                return As<IEnumerable>();
            }
        }

        public IExpectation<object> Any {
            get {
                return new Expectation<object>(_cmd.ToAny());
            }
        }

        public IExpectation<object> All {
            get {
                return new Expectation<object>(_cmd.ToAll());
            }
        }

        public IExpectation<object> Single {
            get {
                return Exactly(1);
            }
        }

        public IExpectation<object> None {
            get {
                return Exactly(0);
            }
        }

        public IExpectation<object> No {
            get {
                return Exactly(0);
            }
        }

        IEnumerableExpectation<object> IEnumerableExpectation<object>.Not {
            get {
                return new EnumerableExpectation(_cmd.Negated());
            }
        }

        public IEnumerableExpectation Not {
            get {
                return new EnumerableExpectation(_cmd.Negated());
            }
        }

        public IExpectation<object> Exactly(int count) {
            return new Expectation<object>(_cmd.Cardinality(count, count));
        }

        public IExpectation<object> Between(int min, int max) {
            return new Expectation<object>(_cmd.Cardinality(min, max));
        }

        public IExpectation<object> AtLeast(int min) {
            return new Expectation<object>(_cmd.Cardinality(min, null));
        }

        public IExpectation<object> AtMost(int max) {
            return new Expectation<object>(_cmd.Cardinality(null, max));
        }

        internal EnumerableExpectation(ExpectationCommand<IEnumerable> cmd) {
            _cmd = cmd;
        }

        public IExpectation<TBase> As<TBase>() {
            return new Expectation<TBase>(_cmd.As<TBase>());
        }

        public IEnumerableExpectation<T> Cast<T>() {
            return new EnumerableExpectation<T>(_cmd.As<IEnumerable<T>>());
        }

        public void Like(ITestMatcher<IEnumerable<object>> matcher, string message = null, object[] args = null) {
            _cmd.Items().Should(matcher, message, (object[]) args);
        }

        public void Like(ITestMatcher<IEnumerable> matcher, string message = null, object[] args = null) {
            _cmd.Should(matcher, message, (object[]) args);
        }

        public void Like(ITestMatcher matcher, string message = null, object[] args = null) {
            _cmd.Untyped().Should(matcher, message, (object[]) args);
        }
    }

    struct EnumerableExpectation<TValue> : IEnumerableExpectation<TValue> {

        private readonly ExpectationCommand<IEnumerable<TValue>> _cmd;

        public IExpectation<IEnumerable<TValue>> Self {
            get {
                return new Expectation<IEnumerable<TValue>>(_cmd);
            }
        }

        public IExpectation<TValue> Any {
            get {
                return new Expectation<TValue>(_cmd.ToAny().As<TValue>());
            }
        }

        public IExpectation<TValue> All {
            get {
                return new Expectation<TValue>(_cmd.ToAll().As<TValue>());
            }
        }

        public IExpectation<TValue> Single {
            get {
                return Exactly(1);
            }
        }

        public IExpectation<TValue> None {
            get {
                return Exactly(0);
            }
        }

        public IExpectation<TValue> No {
            get {
                return Exactly(0);
            }
        }

        public IEnumerableExpectation<TValue> Not {
            get {
                return new EnumerableExpectation<TValue>(_cmd.Negated());
            }
        }

        internal EnumerableExpectation(ExpectationCommand<IEnumerable<TValue>> cmd) {
            _cmd = cmd;
        }

        internal EnumerableExpectation(Func<IEnumerable<TValue>> thunk, bool negated, string given, bool assumption) {
            _cmd = ExpectationCommand.Of(thunk, negated, given, assumption);
        }

        public IExpectation<TValue> Exactly(int count) {
            return new Expectation<TValue>(_cmd.Cardinality(count, count).As<TValue>());
        }

        public IExpectation<TValue> Between(int min, int max) {
            return new Expectation<TValue>(_cmd.Cardinality(min, max).As<TValue>());
        }

        public IExpectation<TValue> AtLeast(int min) {
            return new Expectation<TValue>(_cmd.Cardinality(min, null).As<TValue>());
        }

        public IExpectation<TValue> AtMost(int max) {
            return new Expectation<TValue>(_cmd.Cardinality(null, max).As<TValue>());
        }

        public IExpectation<TBase> As<TBase>() {
            return new Expectation<TBase>(_cmd.As<TBase>());
        }

        public IEnumerableExpectation<TResult> Cast<TResult>() {
            return new EnumerableExpectation<TResult>(_cmd.As<IEnumerable>().Items<TResult>());
        }

        public void Like(ITestMatcher<IEnumerable<TValue>> matcher, string message = null, object[] args = null) {
            Self.Like(matcher, message, (object[]) args);
        }

        public void Like(ITestMatcher matcher, string message = null, object[] args = null) {
            Self.Like(matcher, message, (object[]) args);
        }
    }

    partial class Extensions {

        public static IExpectation<TSource> Any<TSource>(this IEnumerableExpectation e) {
            return e.Cast<TSource>().Any;
        }

        public static IExpectation<TSource> All<TSource>(this IEnumerableExpectation e) {
            return e.Cast<TSource>().All;
        }

        public static IExpectation<TSource> Single<TSource>(this IEnumerableExpectation e) {
            return e.Cast<TSource>().Single;
        }

        public static IExpectation<TSource> AtLeast<TSource>(this IEnumerableExpectation e, int min) {
            return e.Cast<TSource>().AtLeast(min);
        }

        public static IExpectation<TSource> AtMost<TSource>(this IEnumerableExpectation e, int max) {
            return e.Cast<TSource>().AtMost(max);
        }

        public static IExpectation<TSource> Between<TSource>(this IEnumerableExpectation e, int min, int max) {
            return e.Cast<TSource>().Between(min, max);
        }

        public static IExpectation<TSource> No<TSource>(this IEnumerableExpectation e) {
            return e.Cast<TSource>().No;
        }

        public static IExpectation<TSource> None<TSource>(this IEnumerableExpectation e) {
            return e.Cast<TSource>().None;
        }

    }
}
