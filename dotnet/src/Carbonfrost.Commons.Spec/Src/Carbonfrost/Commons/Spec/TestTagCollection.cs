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
using System.Linq;

namespace Carbonfrost.Commons.Spec {

    public partial class TestTagCollection : ICollection<TestTag> {

        private readonly Dictionary<TestTagType, HashSet<string>> _tags = new Dictionary<TestTagType, HashSet<string>>();
        internal static readonly TestTagCollection Empty = new TestTagCollection(true);

        public int Count {
            get {
                return _tags.Values.Sum(v => v.Count);
            }
        }

        public bool IsReadOnly {
            get;
            private set;
        }

        private IEnumerable<TestTag> All {
            get {
                return _tags.SelectMany(
                    kvp => kvp.Value.Select(v => new TestTag(kvp.Key, v))
                );
            }
        }

        public IndexCollection ByName {
            get {
                return new IndexCollection(_tags);
            }
        }

        internal bool HasUserTags {
            get {
                if (_tags.Count == 0) {
                    return false;
                }
                return _tags.All(
                    kvp => kvp.Key.Automatic
                );
            }
        }

        public TestTagCollection() {
        }

        private TestTagCollection(bool dummy) {
            MakeReadOnly();
        }

        public TestTagCollection(IEnumerable<TestTag> items) {
            if (items == null) {
                throw new ArgumentNullException(nameof(items));
            }
            foreach (var i in items) {
                Add(i);
            }
        }

        internal static TestTagCollection Create(IEnumerable<TestTag> items) {
            if (items == null) {
                return Empty;
            }
            if (items.Any()) {
                return new TestTagCollection(items);
            }
            return Empty;
        }

        public bool Contains(TestTagType type, string value) {
            TestTag.RequireType(type);
            return TryGetGroup(type, false, out var items)
                && (string.IsNullOrEmpty(value) || items.Contains(value));
        }

        public bool Contains(TestTagType type) {
            TestTag.RequireType(type);
            return TestTag.TryParse(type.ToString(), out var item)
                && Contains(item);
        }

        public bool Add(TestTag item) {
            ThrowIfReadOnly();
            return Add(item.Type, item.Value);
        }

        public bool Add(string item) {
            ThrowIfReadOnly();
            return Parse(item, out var tt) && Add(tt);
        }

        public bool Add(TestTagType type, string value) {
            TestTag.RequireType(type);
            return TryGetGroup(type, true, out var items) && (
                items.Add(value ?? string.Empty)
            );
        }

        public bool Remove(string item) {
            return Parse(item, out var tt) && Remove(tt);
        }

        void ICollection<TestTag>.Add(TestTag item) {
            Add(item);
        }

        public void Clear() {
            ThrowIfReadOnly();
            _tags.Clear();
        }

        public bool Contains(TestTag item) {
            return Contains(item.Type, item.Value);
        }

        public void CopyTo(TestTag[] array, int arrayIndex) {
            All.ToArray().CopyTo(array, arrayIndex);
        }

        public bool Remove(TestTag item) {
            ThrowIfReadOnly();
            return TryGetGroup(item.Type, false, out var items) && items.Remove(item.Value);
        }

        public IEnumerator<TestTag> GetEnumerator() {
            return All.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        internal void MakeReadOnly() {
            IsReadOnly = true;
        }

        private bool TryGetGroup(TestTagType type, bool create, out HashSet<string> result) {
            if (create) {
                result = _tags.GetValueOrCache(
                    type,
                    _ => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                );
                return true;
            }
            return _tags.TryGetValue(type, out result);
        }

        static bool Parse(string item, out TestTag tag) {
            tag = default(TestTag);
            if (string.IsNullOrEmpty(item)) {
                throw SpecFailure.EmptyString(nameof(item));
            }
            return TestTag.TryParse(item, out tag);
        }

        private void ThrowIfReadOnly() {
            if (IsReadOnly) {
                throw SpecFailure.Sealed();
            }
        }
    }

}
