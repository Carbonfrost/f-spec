//
// Copyright 2018-2019 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Collections.Generic;
using System.Linq;
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.Commons.Spec {

    partial class Matchers {

        public static HaveKeyWithValueMatcher<TKey, TValue> HaveKeyWithValue<TKey, TValue>(TKey key, TValue value) {
            return new HaveKeyWithValueMatcher<TKey, TValue>(key, value);
        }

    }

    partial class Asserter {

        public void ContainsKeyWithValue<TKey, TValue>(TKey key, TValue value, IEnumerable<KeyValuePair<TKey, TValue>> collection) {
            ContainsKeyWithValue(key, value, collection, null);
        }

        public void ContainsKeyWithValue<TKey, TValue>(TKey key, TValue value, IEnumerable<KeyValuePair<TKey, TValue>> collection, string message, params object[] args) {
            That(collection, Matchers.HaveKeyWithValue<TKey, TValue>(key, value), message, args);
        }

        public void DoesNotContainKeyWithValue<TKey, TValue>(TKey key, TValue value, IEnumerable<KeyValuePair<TKey, TValue>> collection) {
            DoesNotContainKeyWithValue(key, value, collection, null);
        }

        public void DoesNotContainKeyWithValue<TKey, TValue>(TKey key, TValue value, IEnumerable<KeyValuePair<TKey, TValue>> collection, string message, params object[] args) {
            NotThat(collection, Matchers.HaveKeyWithValue<TKey, TValue>(key, value), message, args);
        }
    }

    partial class Assert {

        public static void ContainsKeyWithValue<TKey, TValue>(TKey key, TValue value, IEnumerable<KeyValuePair<TKey, TValue>> collection) {
            Global.ContainsKeyWithValue<TKey, TValue>(key, value, collection);
        }

        public static void ContainsKeyWithValue<TKey, TValue>(TKey key, TValue value, IEnumerable<KeyValuePair<TKey, TValue>> collection, string message, params object[] args) {
            Global.ContainsKeyWithValue<TKey, TValue>(key, value, collection, message, (object[]) args);
        }

        public static void DoesNotContainKeyWithValue<TKey, TValue>(TKey key, TValue value, IEnumerable<KeyValuePair<TKey, TValue>> collection) {
            Global.DoesNotContainKeyWithValue<TKey, TValue>(key, value, collection);
        }

        public static void DoesNotContainKeyWithValue<TKey, TValue>(TKey key, TValue value, IEnumerable<KeyValuePair<TKey, TValue>> collection, string message, params object[] args) {
            Global.DoesNotContainKeyWithValue<TKey, TValue>(key, value, collection, message, (object[]) args);
        }
    }

    partial class Assume {

        public static void ContainsKeyWithValue<TKey, TValue>(TKey key, TValue value, IEnumerable<KeyValuePair<TKey, TValue>> collection) {
            Global.ContainsKeyWithValue<TKey, TValue>(key, value, collection);
        }

        public static void ContainsKeyWithValue<TKey, TValue>(TKey key, TValue value, IEnumerable<KeyValuePair<TKey, TValue>> collection, string message, params object[] args) {
            Global.ContainsKeyWithValue<TKey, TValue>(key, value, collection, message, (object[]) args);
        }

        public static void DoesNotContainKeyWithValue<TKey, TValue>(TKey key, TValue value, IEnumerable<KeyValuePair<TKey, TValue>> collection) {
            Global.DoesNotContainKeyWithValue<TKey, TValue>(key, value, collection);
        }

        public static void DoesNotContainKeyWithValue<TKey, TValue>(TKey key, TValue value, IEnumerable<KeyValuePair<TKey, TValue>> collection, string message, params object[] args) {
            Global.DoesNotContainKeyWithValue<TKey, TValue>(key, value, collection, message, (object[]) args);
        }
    }


    partial class Extensions {

        public static void KeyWithValue<TKey, TValue>(this EnumerableExpectation e, TKey key, TValue value) {
            KeyWithValue(e, key, value, null);
        }

        public static void KeyWithValue<TKey, TValue>(this EnumerableExpectation e, TKey key, TValue value, string message, params object[] args) {
            e.As<IEnumerable<KeyValuePair<TKey, TValue>>>().Should(Matchers.HaveKeyWithValue<TKey, TValue>(key, value), message, (object[]) args);
        }

        public static void KeyWithValue<TKey, TValue>(this EnumerableExpectation<KeyValuePair<TKey, TValue>> e, TKey key, TValue value) {
            KeyWithValue(e, key, value, null);
        }

        public static void KeyWithValue<TKey, TValue>(this EnumerableExpectation<KeyValuePair<TKey, TValue>> e, TKey key, TValue value, string message, params object[] args) {
            e.As<IEnumerable<KeyValuePair<TKey, TValue>>>().Should(Matchers.HaveKeyWithValue<TKey, TValue>(key, value), message, (object[]) args);
        }
    }

    namespace TestMatchers {

        public class HaveKeyWithValueMatcher<TKey, TValue>
            : TestMatcher<IEnumerable<KeyValuePair<TKey, TValue>>>, ITestMatcher<IEnumerable<IGrouping<TKey, TValue>>> {

            private readonly TKey _key;
            private readonly TValue _value;

            public HaveKeyWithValueMatcher(TKey key, TValue value) {
                _key = key;
                _value = value;
            }

            public override bool Matches(IEnumerable<KeyValuePair<TKey, TValue>> actual) {
                if (actual == null) {
                    throw new ArgumentNullException("actual");
                }
                var c = actual as IDictionary<TKey, TValue>;
                if (c != null) {
                    TValue value;
                    return c.TryGetValue(_key, out value) && Equals(value, _value);
                }
                var expected = new KeyValuePair<TKey, TValue>(_key, _value);
                return actual.Any(kvp => kvp.Equals(expected));
            }

            public bool Matches(IEnumerable<IGrouping<TKey, TValue>> actual) {
                if (actual == null) {
                    throw new ArgumentNullException("actual");
                }
                var expected = new KeyValuePair<TKey, TValue>(_key, _value);
                return actual.Any(kvp => kvp.Key.Equals(_key) && kvp.Contains(_value));
            }

            public bool Matches(ITestActualEvaluation<IEnumerable<IGrouping<TKey, TValue>>> actualFactory) {
                if (actualFactory == null) {
                    throw new ArgumentNullException("actualFactory");
                }
                return Matches(actualFactory.Value);
            }
        }
    }

}
