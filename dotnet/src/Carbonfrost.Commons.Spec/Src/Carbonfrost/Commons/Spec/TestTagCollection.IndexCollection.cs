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

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Carbonfrost.Commons.Spec {

    public partial class TestTagCollection {

        public struct IndexCollection : IReadOnlyDictionary<TestTagType, ICollection<string>> {
            private readonly IDictionary<TestTagType, HashSet<string>> _keys;

            internal IndexCollection(IDictionary<TestTagType, HashSet<string>> keys) {
                _keys = keys;
            }

            public ICollection<string> this[TestTagType key] {
                get {
                    return _keys[key];
                }
            }

            public IEnumerable<TestTagType> Keys {
                get {
                    return _keys.Keys;
                }
            }

            public IEnumerable<ICollection<string>> Values {
                get {
                    return _keys.Values;
                }
            }

            public int Count {
                get {
                    return _keys.Count;
                }
            }

            public bool ContainsKey(TestTagType key) {
                return _keys.ContainsKey(key);
            }

            public IEnumerator<KeyValuePair<TestTagType, ICollection<string>>> GetEnumerator() {
                return _keys.Select(
                    kvp => new KeyValuePair<TestTagType, ICollection<string>>(kvp.Key, kvp.Value)
                ).GetEnumerator();
            }

            public bool TryGetValue(TestTagType key, out ICollection<string> value) {
                if (_keys.TryGetValue(key, out var result)) {
                    value = result;
                    return true;
                }
                value = null;
                return false;
            }

            IEnumerator IEnumerable.GetEnumerator() {
                return GetEnumerator();
            }
        }
    }

}
