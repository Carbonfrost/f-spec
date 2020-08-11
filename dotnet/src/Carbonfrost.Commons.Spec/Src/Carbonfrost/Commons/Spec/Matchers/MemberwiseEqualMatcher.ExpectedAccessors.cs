//
// Copyright 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.Commons.Spec {

    namespace TestMatchers {

        partial class MemberwiseEqualMatcher {

            internal struct ExpectedAccessorData {
                public readonly object Specified;
                public readonly ITestMatcher<object> Matcher;
                public readonly IMemberAccessor Accessor;
                public readonly string Name;

                public ExpectedAccessorData(string name, object specified, ITestMatcher<object> matcher, IMemberAccessor accessor) {
                    Name = name;
                    Specified = specified;
                    Matcher = matcher;
                    Accessor = accessor;
                }

                public override string ToString() {
                    return $"{Name} {Matcher}";
                }
            }

            internal class ExpectedAccessors : IReadOnlyDictionary<string, object> {
                private readonly Dictionary<string, ExpectedAccessorData> _items = new Dictionary<string, ExpectedAccessorData>();

                public ExpectedAccessors(bool untypedTarget, object expected, TestMemberFilter filter) {
                    if (expected is null) {
                        return;
                    }

                    // If the target is untyped, then we can't use the field or property directly
                    // because it could mismatch, so we have to evaluate it now
                    if (untypedTarget) {
                        LoadFromKvp(GetFieldsAndProperties(expected, filter));
                    } else {
                        LoadFromObject(expected, filter);
                    }
                }

                public ExpectedAccessors(IEnumerable<KeyValuePair<string, object>> expected) {
                    if (expected == null) {
                        return;
                    }
                    LoadFromKvp(expected);
                }

                public IEnumerable<ExpectedAccessorData> Data {
                    get {
                        return _items.Values;
                    }
                }

                public object this[string key] {
                    get {
                        return _items[key];
                    }
                }

                public IEnumerable<string> Keys {
                    get {
                        return _items.Keys;
                    }
                }

                public IEnumerable<object> Values {
                    get {
                        return _items.Values.Select(t => t.Specified);
                    }
                }

                public int Count {
                    get {
                        return _items.Count;
                    }
                }

                public bool ContainsKey(string key) {
                    return _items.ContainsKey(key);
                }

                public IEnumerator<KeyValuePair<string, object>> GetEnumerator() {
                    return _items.Select(
                        k => new KeyValuePair<string, object>(k.Key, k.Value.Specified)
                    ).GetEnumerator();
                }

                public bool TryGetValue(string key, out object value) {
                    if (_items.TryGetValue(key, out var item)) {
                        value = item.Specified;
                        return true;
                    }
                    value = null;
                    return false;
                }

                IEnumerator IEnumerable.GetEnumerator() {
                    return GetEnumerator();
                }

                private static ITestMatcher<object> Matcher(object specified) {
                    if (specified is null) {
                        return Matchers.BeNull();
                    }
                    if (specified is ITestMatcher<object> tm) {
                        return tm;
                    }

                    var iface = specified.GetType().GetInterface(typeof(ITestMatcher<>).FullName);
                    if (iface == null) {
                        return Matchers.Equal(specified);
                    }
                    return (ITestMatcher<object>) TestMatcher.Adapter(
                        iface.GetGenericArguments()[0],
                        typeof(object),
                        specified
                    );
                }

                private IEnumerable<KeyValuePair<string, object>> GetFieldsAndProperties(object expected, TestMemberFilter filter) {
                    foreach (var f in filter.GetMembers(expected.GetType().GetTypeInfo())) {
                        if (f is FieldInfo field) {
                            yield return new KeyValuePair<string, object>(f.Name, field.GetValue(expected));
                        } else {
                            yield return new KeyValuePair<string, object>(f.Name, ((PropertyInfo) f).GetValue(expected));
                        }
                    }
                }

                private void LoadFromKvp(IEnumerable<KeyValuePair<string, object>> expected) {
                    foreach (var kvp in expected) {
                        IMemberAccessor accessor = MemberAccessors.PropertyOrField(kvp.Key);
                        object specified = kvp.Value;
                        _items.Add(kvp.Key, new ExpectedAccessorData(
                            kvp.Key,
                            specified,
                            Matcher(specified),
                            accessor
                        ));
                    }
                }

                private void LoadFromObject(object expected, TestMemberFilter filter) {
                    foreach (var f in filter.GetMembers(expected.GetType().GetTypeInfo())) {
                        IMemberAccessor accessor;
                        accessor = MemberAccessors.PropertyOrField(f);
                        object specified = accessor.GetValue(expected);
                        _items.Add(f.Name, new ExpectedAccessorData(
                            f.Name,
                            specified,
                            Matcher(specified),
                            accessor
                        ));
                    }
                }
            }
        }
    }

}
