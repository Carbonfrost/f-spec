//
// Copyright 2017, 2018, 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
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

namespace Carbonfrost.Commons.Spec {

    public static partial class Matchers {

        public static IExpectationBuilder<T> Expect<T>(T value) {
            return Assert.Expect(value);
        }

        public static IExpectationBuilder Expect(Action value) {
            return Assert.Expect(value);
        }

        public static GivenExpectationBuilder<T1, T2, T3, T4, T5, T6, T7, T8> Given<T1, T2, T3, T4, T5, T6, T7, T8>(
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) {
            return Assert.Given<T1, T2, T3, T4, T5, T6, T7, T8>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }

        public static GivenExpectationBuilder<T1, T2, T3, T4, T5, T6, T7> Given<T1, T2, T3, T4, T5, T6, T7>(
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) {
            return Assert.Given<T1, T2, T3, T4, T5, T6, T7>(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }

        public static GivenExpectationBuilder<T1, T2, T3, T4, T5, T6> Given<T1, T2, T3, T4, T5, T6>(
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) {
            return Assert.Given<T1, T2, T3, T4, T5, T6>(arg1, arg2, arg3, arg4, arg5, arg6);
        }

        public static GivenExpectationBuilder<T1, T2, T3, T4, T5> Given<T1, T2, T3, T4, T5>(
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) {
            return Assert.Given<T1, T2, T3, T4, T5>(arg1, arg2, arg3, arg4, arg5);
        }

        public static GivenExpectationBuilder<T1, T2, T3, T4> Given<T1, T2, T3, T4>(
            T1 arg1, T2 arg2, T3 arg3, T4 arg4) {
            return Assert.Given<T1, T2, T3, T4>(arg1, arg2, arg3, arg4);
        }

        public static GivenExpectationBuilder<T1, T2, T3> Given<T1, T2, T3>(
            T1 arg1, T2 arg2, T3 arg3) {
            return Assert.Given<T1, T2, T3>(arg1, arg2, arg3);
        }

        public static GivenExpectationBuilder<T1, T2> Given<T1, T2>(
            T1 arg1, T2 arg2) {
            return Assert.Given<T1, T2>(arg1, arg2);
        }

        public static GivenExpectationBuilder<T> Given<T>(T arg) {
            return Assert.Given<T>(arg);
        }

        public static GivenExpectationBuilder Given() {
            return Assert.Given();
        }
    }
}
