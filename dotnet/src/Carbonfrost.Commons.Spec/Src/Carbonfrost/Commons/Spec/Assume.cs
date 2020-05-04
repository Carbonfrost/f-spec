//
// Copyright 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
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

namespace Carbonfrost.Commons.Spec {

    public static partial class Assume {

        static Asserter Global {
            get {
                return Asserter.Assumptions;
            }
        }

        public static ExpectationBuilder<IEnumerable> Expect() {
            return Global.Expect();
        }

        public static ExpectationBuilder<T> Expect<T>(T value) {
            return Global.Expect<T>(value);
        }

        public static ExpectationBuilder<IEnumerable<TValue>> Expect<TValue>(params TValue[] value) {
            return Global.Expect<TValue>(value);
        }

        public static ExpectationBuilder Expect(Action value) {
            return Global.Expect(value);
        }

        public static ExpectationBuilder<T> Expect<T>(Func<T> func) {
            return Global.Expect<T>(func);
        }
    }
}
