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

    public struct EnumerableExpectation {

        private readonly ExpectationCommand<IEnumerable> _cmd;

        public Expectation<IEnumerable> Self {
            get {
                return As<IEnumerable>();
            }
        }

        public Expectation<object> Any {
            get {
                return new Expectation<object>(_cmd.ToAny());
            }
        }

        public Expectation<object> All {
            get {
                return new Expectation<object>(_cmd.ToAll());
            }
        }

        public Expectation<object> Single {
            get {
                return Exactly(1);
            }
        }

        public Expectation<object> None {
            get {
                return Exactly(0);
            }
        }

        public Expectation<object> No {
            get {
                return Exactly(0);
            }
        }

        public EnumerableExpectation Not {
            get {
                return new EnumerableExpectation(_cmd.Negated());
            }
        }

        public Expectation<object> Exactly(int count) {
            return new Expectation<object>(_cmd.Cardinality(count, count));
        }

        public Expectation<object> Between(int min, int max) {
            return new Expectation<object>(_cmd.Cardinality(min, max));
        }

        public Expectation<object> AtLeast(int min) {
            return new Expectation<object>(_cmd.Cardinality(min, null));
        }

        public Expectation<object> AtMost(int max) {
            return new Expectation<object>(_cmd.Cardinality(null, max));
        }

        internal EnumerableExpectation(ExpectationCommand<IEnumerable> cmd) {
            _cmd = cmd;
        }

        public Expectation<TBase> As<TBase>() {
            return new Expectation<TBase>(_cmd.As<TBase>());
        }

        public EnumerableExpectation<T> Cast<T>() {
            return new EnumerableExpectation<T>(_cmd.As<IEnumerable<T>>());
        }
    }

    public struct EnumerableExpectation<TValue> : IExpectation<IEnumerable<TValue>> {

        private readonly ExpectationCommand<IEnumerable<TValue>> _cmd;

        public Expectation<IEnumerable<TValue>> Self {
            get {
                return new Expectation<IEnumerable<TValue>>(_cmd);
            }
        }

        public Expectation<TValue> Any {
            get {
                return new Expectation<TValue>(_cmd.ToAny().As<TValue>());
            }
        }

        public Expectation<TValue> All {
            get {
                return new Expectation<TValue>(_cmd.ToAll().As<TValue>());
            }
        }

        public Expectation<TValue> Single {
            get {
                return Exactly(1);
            }
        }

        public Expectation<TValue> None {
            get {
                return Exactly(0);
            }
        }

        public Expectation<TValue> No {
            get {
                return Exactly(0);
            }
        }

        public EnumerableExpectation<TValue> Not {
            get {
                return new EnumerableExpectation<TValue>(_cmd.Negated());
            }
        }

        internal EnumerableExpectation(ExpectationCommand<IEnumerable<TValue>> cmd) {
            _cmd = cmd;
        }

        internal EnumerableExpectation(Func<IEnumerable<TValue>> thunk, bool negated, string given) {
            _cmd = ExpectationCommand.Of(thunk).NegateIfNeeded(negated).Given(given);
        }

        public Expectation<TValue> Exactly(int count) {
            return new Expectation<TValue>(_cmd.Cardinality(count, count).As<TValue>());
        }

        public Expectation<TValue> Between(int min, int max) {
            return new Expectation<TValue>(_cmd.Cardinality(min, max).As<TValue>());
        }

        public Expectation<TValue> AtLeast(int min) {
            return new Expectation<TValue>(_cmd.Cardinality(min, null).As<TValue>());
        }

        public Expectation<TValue> AtMost(int max) {
            return new Expectation<TValue>(_cmd.Cardinality(null, max).As<TValue>());
        }

        public Expectation<TBase> As<TBase>() {
            return new Expectation<TBase>(_cmd.As<TBase>());
        }

        ExpectationCommand<IEnumerable<TValue>> IExpectation<IEnumerable<TValue>>.ToCommand() {
            return ((IExpectation<IEnumerable<TValue>>) Self).ToCommand();
        }
    }

    partial class Extensions {

        public static Expectation<TSource> Any<TSource>(this EnumerableExpectation e) {
            return e.Cast<TSource>().Any;
        }

        public static Expectation<TSource> All<TSource>(this EnumerableExpectation e) {
            return e.Cast<TSource>().All;
        }

        public static Expectation<TSource> Single<TSource>(this EnumerableExpectation e) {
            return e.Cast<TSource>().Single;
        }

        public static Expectation<TSource> AtLeast<TSource>(this EnumerableExpectation e, int min) {
            return e.Cast<TSource>().AtLeast(min);
        }

        public static Expectation<TSource> AtMost<TSource>(this EnumerableExpectation e, int max) {
            return e.Cast<TSource>().AtMost(max);
        }

        public static Expectation<TSource> Between<TSource>(this EnumerableExpectation e, int min, int max) {
            return e.Cast<TSource>().Between(min, max);
        }

        public static Expectation<TSource> No<TSource>(this EnumerableExpectation e) {
            return e.Cast<TSource>().No;
        }

        public static Expectation<TSource> None<TSource>(this EnumerableExpectation e) {
            return e.Cast<TSource>().None;
        }

    }
}
