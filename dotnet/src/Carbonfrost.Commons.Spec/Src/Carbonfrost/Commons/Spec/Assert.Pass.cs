//
// Copyright 2016 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Linq;

namespace Carbonfrost.Commons.Spec {

    partial class Assert {

        public static void Pass(string message) {
            throw SpecFailure.Pass(message);
        }

        public static void Pass() {
            throw SpecFailure.Pass();
        }

        public static void Pass(IFormatProvider formatProvider, string format, params object[] args) {
            throw SpecFailure.Pass(string.Format(formatProvider, format, args));
        }

        public static void Pass(string format, params object[] args) {
            throw SpecFailure.Pass(string.Format(format, args));
        }

        public static void Fail(IFormatProvider formatProvider, string format, params object[] args) {
            throw SpecFailure.Fail(string.Format(formatProvider, format, args));
        }

        public static void Fail(string format, params object[] args) {
            throw SpecFailure.Fail(string.Format(format, args));
        }

        public static void Fail(string message) {
            throw SpecFailure.Fail(message);
        }

        public static void Fail() {
            throw SpecFailure.Fail();
        }

        public static void Pending(string message) {
            throw SpecFailure.Pending(message);
        }

        public static void Pending() {
            throw SpecFailure.Pending();
        }

        public static void Pending(IFormatProvider formatProvider, string format, params object[] args) {
            throw SpecFailure.Pending(string.Format(formatProvider, format, args));
        }

        public static void Pending(string format, params object[] args) {
            throw SpecFailure.Pending(string.Format(format, args));
        }
    }
}
