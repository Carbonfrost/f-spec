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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.Commons.Spec {

    partial class Matchers {

        public static HaveKeyMatcher<TKey> HaveKey<TKey>(TKey key) {
            return new HaveKeyMatcher<TKey>(key);
        }

    }

    partial class Asserter {

        public void ContainsKey<TKey>(TKey key, IEnumerable collection) {
            ContainsKey(key, collection, null);
        }

        public void ContainsKey<TKey>(TKey key, IEnumerable collection, string message, params object[] args) {
            That(collection, Matchers.HaveKey(key), message, args);
        }

        public void DoesNotContainKey<TKey>(TKey key, IEnumerable collection) {
            DoesNotContainKey(key, collection, null);
        }

        public void DoesNotContainKey<TKey>(TKey key, IEnumerable collection, string message, params object[] args) {
            NotThat(collection, Matchers.HaveKey(key), message, args);
        }
    }

    partial class Assert {

        public static void ContainsKey<TKey>(TKey key, IEnumerable collection) {
            Global.ContainsKey<TKey>(key, collection);
        }

        public static void ContainsKey<TKey>(TKey key, IEnumerable collection, string message, params object[] args) {
            Global.ContainsKey<TKey>(key, collection, message, (object[]) args);
        }

        public static void DoesNotContainKey<TKey>(TKey key, IEnumerable collection) {
            Global.DoesNotContainKey<TKey>(key, collection);
        }

        public static void DoesNotContainKey<TKey>(TKey key, IEnumerable collection, string message, params object[] args) {
            Global.DoesNotContainKey<TKey>(key, collection, message, (object[]) args);
        }
    }

    partial class Assume {

        public static void ContainsKey<TKey>(TKey key, IEnumerable collection) {
            Global.ContainsKey<TKey>(key, collection);
        }

        public static void ContainsKey<TKey>(TKey key, IEnumerable collection, string message, params object[] args) {
            Global.ContainsKey<TKey>(key, collection, message, (object[]) args);
        }

        public static void DoesNotContainKey<TKey>(TKey key, IEnumerable collection) {
            Global.DoesNotContainKey<TKey>(key, collection);
        }

        public static void DoesNotContainKey<TKey>(TKey key, IEnumerable collection, string message, params object[] args) {
            Global.DoesNotContainKey<TKey>(key, collection, message, (object[]) args);
        }
    }


    partial class Extensions {

        // Not logical to have EnumerableExpectation<> because there is no type
        // for TValue -- e.g it would have to be EnumerableExpectation<KeyValuePair<TKey, ?>>

        [IgnoreEnumerableExpectationAttribute]
        public static void Key<TKey>(this EnumerableExpectation e, TKey key) {
            Key(e, key, null);
        }

        public static void Key<TKey>(this EnumerableExpectation e, TKey key, string message, params object[] args) {
            e.As<IEnumerable>().Should(Matchers.HaveKey(key), message, (object[]) args);
        }

    }

    namespace TestMatchers {

        public class HaveKeyMatcher<TKey> : TestMatcher<IEnumerable> {

            private readonly TKey _key;

            public TKey Expected { get { return _key; } }

            public HaveKeyMatcher(TKey key) {
                _key = key;
            }

            public override bool Matches(IEnumerable actual) {
                if (actual == null) {
                    throw new ArgumentNullException("actual");
                }

                return Keys(actual).Contains(_key);
            }

            static IEnumerable<TKey> Keys(IEnumerable actual) {
                var ng = actual as IDictionary;
                if (ng != null) {
                    return ng.Keys.Cast<TKey>();
                }

                // Look for IDictionary<>, IReadOnlyDictionary<> using reflection
                var interfaces = actual.GetType().GetTypeInfo().GetInterfaces()
                    .Where(t => t.GetTypeInfo().IsGenericType);

                {
                    var iface = interfaces.FirstOrDefault(IsGenericDictionary);
                    if (iface != null) {
                        return ((IEnumerable) iface.GetTypeInfo().GetProperty("Keys").GetValue(actual)).Cast<TKey>();
                    }
                }

                // Look for IEnumerable<IGrouping<TKey, TValue>> and
                // IEnumerable<KeyValuePair<TKey, TValue>>
                foreach (var iface in interfaces) {
                    PropertyInfo kp;
                    if (IsKVP(iface, out kp)) {
                        var enumMethod = iface.GetTypeInfo().GetMethod("GetEnumerator");
                        return LateBoundKVP(actual, enumMethod, kp);
                    }
                }

                throw SpecFailure.CannotTreatAsDictionaryOrGroupings(actual.GetType());
            }

            static bool IsKVP(Type iface, out PropertyInfo keyProperty) {
                var def = iface.GetTypeInfo().GetGenericTypeDefinition();
                Type kvpType, keyType;

                if (IsClosedGenericOf(iface, typeof(IEnumerable<>), out kvpType)
                    && (IsClosedGenericOf(kvpType, typeof(KeyValuePair<,>), out keyType)
                        || IsClosedGenericOf(kvpType, typeof(IGrouping<,>), out keyType))) {
                    keyProperty = kvpType.GetProperty("Key");
                    return true;;
                }

                keyProperty = null;
                return false;
            }

            static IEnumerable<TKey> LateBoundKVP(object actual, MethodInfo enumeratorMethod, PropertyInfo pi) {
                // We have to invoke the enumerator correspoding to the interface
                // we selected.  It isn't sufficient to just enumerate `actual' directly
                // because it could return some other object type.
                var e = (IEnumerator) enumeratorMethod.Invoke(actual, null);
                while (e.MoveNext()) {
                    yield return (TKey) pi.GetValue(e.Current);
                }
                Safely.Dispose(e);
            }

            static bool IsGenericDictionary(Type iface) {
                var def = iface.GetTypeInfo().GetGenericTypeDefinition();
                return def == typeof(IReadOnlyDictionary<,>)
                    || def == typeof(IDictionary<,>);
            }

            static bool IsClosedGenericOf(Type iface, Type openGeneric, out Type firstArg) {
                firstArg = null;
                if (!iface.GetTypeInfo().IsGenericType) {
                    return false;
                }
                var def = iface.GetTypeInfo().GetGenericTypeDefinition();

                if (openGeneric == def){
                    firstArg = iface.GenericTypeArguments[0];
                    return true;
                }
                return false;
            }

        }
    }

}
