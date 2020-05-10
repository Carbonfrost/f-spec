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

    public partial class TestTagCollection : ICollection<TestTag>, ICollection<string> {

        private readonly Dictionary<string, HashSet<string>> _tags = new Dictionary<string, HashSet<string>>(
            StringComparer.OrdinalIgnoreCase
        );
        private static readonly TestTagCollection Empty = new TestTagCollection(true);

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

        public bool Contains(string name, string value) {
            TestTag.RequireName(name);
            return TryGetGroup(name, false, out var items)
                && (string.IsNullOrEmpty(value) || items.Contains(value));
        }

        public bool Contains(string name) {
            TestTag.RequireName(name);
            return TestTag.TryParse(name, out var item)
                && Contains(item);
        }

        public bool Add(TestTag item) {
            ThrowIfReadOnly();
            return Add(item.Name, item.Value);
        }

        public bool Add(string item) {
            ThrowIfReadOnly();
            return Parse(item, out var tt) && Add(tt);
        }

        void ICollection<string>.Add(string item) {
            Add(item);
        }

        public bool Add(string name, string value) {
            TestTag.RequireName(name);
            return TryGetGroup(name, true, out var items) && (
                items.Add(value ?? string.Empty)
            );
        }

        void ICollection<string>.CopyTo(string[] array, int arrayIndex) {
            All.Select(k => k.ToString()).ToArray().CopyTo(array, arrayIndex);
        }

        public bool Remove(string item) {
            return Parse(item, out var tt) && Remove(tt);
        }

        IEnumerator<string> IEnumerable<string>.GetEnumerator() {
            return All.Select(t => t.ToString()).GetEnumerator();
        }

        void ICollection<TestTag>.Add(TestTag item) {
            Add(item);
        }

        public void Clear() {
            ThrowIfReadOnly();
            _tags.Clear();
        }

        public bool Contains(TestTag item) {
            return Contains(item.Name, item.Value);
        }

        public void CopyTo(TestTag[] array, int arrayIndex) {
            All.ToArray().CopyTo(array, arrayIndex);
        }

        public bool Remove(TestTag item) {
            ThrowIfReadOnly();
            return TryGetGroup(item.Name, false, out var items) && items.Remove(item.Value);
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

        private bool TryGetGroup(string name, bool create, out HashSet<string> result) {
            if (create) {
                result = _tags.GetValueOrCache(
                    name,
                    _ => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                );
                return true;
            }
            return _tags.TryGetValue(name, out result);
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
