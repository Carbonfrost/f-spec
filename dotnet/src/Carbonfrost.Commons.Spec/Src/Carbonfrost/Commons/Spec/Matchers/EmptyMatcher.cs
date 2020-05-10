//
// Copyright 2017, 2019-2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
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
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.Commons.Spec {

    partial class Matchers {

        public static EmptyMatcher BeEmpty() {
            return new EmptyMatcher();
        }

    }

    partial class Extensions {

        public static void Empty<T>(this IExpectation<T> e) {
            Empty(e, null);
        }

        public static void Empty<T>(this IExpectation<T> e, string message, params object[] args) {
            // We have to enforce the constraint at runtime because Expectation<object>
            // is possible and it is meant to represent type erasure caused
            // with EnumerableExpectation.All() or Any()
            e.As<IEnumerable>().Like(Matchers.BeEmpty(), message, (object[]) args);
        }

    }

    partial class Asserter {

        public void Empty(IEnumerable instance) {
            That(instance, Matchers.BeEmpty());
        }

        public void Empty(IEnumerable instance, string message, params object[] args) {
            That(instance, Matchers.BeEmpty(), message, args);
        }

        public void NotEmpty(IEnumerable instance) {
            NotThat(instance, Matchers.BeEmpty());
        }

        public void NotEmpty(IEnumerable instance, string message, params object[] args) {
            NotThat(instance, Matchers.BeEmpty(), message, args);
        }
    }

    partial class Assert {

        public static void Empty(IEnumerable instance) {
            Global.Empty(instance);
        }

        public static void Empty(IEnumerable instance, string message, params object[] args) {
            Global.Empty(instance, message, (object[]) args);
        }

        public static void NotEmpty(IEnumerable instance) {
            Global.NotEmpty(instance);
        }

        public static void NotEmpty(IEnumerable instance, string message, params object[] args) {
            Global.NotEmpty(instance, message, (object[]) args);
        }
    }

    partial class Assume {

        public static void Empty(IEnumerable instance) {
            Global.Empty(instance);
        }

        public static void Empty(IEnumerable instance, string message, params object[] args) {
            Global.Empty(instance, message, (object[]) args);
        }

        public static void NotEmpty(IEnumerable instance) {
            Global.NotEmpty(instance);
        }

        public static void NotEmpty(IEnumerable instance, string message, params object[] args) {
            Global.NotEmpty(instance, message, (object[]) args);
        }
    }


    namespace TestMatchers {

        public class EmptyMatcher : TestMatcher<IEnumerable> {

            public override bool Matches(IEnumerable actual) {
                if (actual == null) {
                    throw new ArgumentNullException(nameof(actual));
                }

                var enumerator = actual.GetEnumerator();
                try {
                    if (enumerator.MoveNext()) {
                        return false;
                    }
                    return true;
                } finally {
                    Safely.Dispose(enumerator);
                }
            }
        }
    }

}
