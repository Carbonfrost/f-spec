//
// Copyright 2016, 2018 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

using System.Collections.Generic;
using System.Collections;

namespace Carbonfrost.Commons.Spec {

    class MakeReadOnlyList<T> : IList<T>, IReadOnlyList<T> {

        private readonly IList<T> _items;

        public MakeReadOnlyList() : this(new List<T>()) {
        }

        public MakeReadOnlyList(IList<T> items) {
            _items = items;
        }

        public int IndexOf(T item) {
            return _items.IndexOf(item);
        }

        public void Insert(int index, T item) {
            ThrowIfReadOnly();
            _items.Insert(index, item);
        }

        public void RemoveAt(int index) {
            ThrowIfReadOnly();
            _items.RemoveAt(index);
        }

        public T this[int index] {
            get {
                return _items[index];
            }
            set {
                ThrowIfReadOnly();
                _items[index] = value;
            }
        }

        public void Add(T item) {
            ThrowIfReadOnly();
            _items.Add(item);
        }

        public void Clear() {
            ThrowIfReadOnly();
            _items.Clear();
        }

        public bool Contains(T item) {
            return _items.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex) {
            _items.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item) {
            ThrowIfReadOnly();
            return _items.Remove(item);
        }

        public int Count {
            get {
                return _items.Count;
            }
        }

        public IEnumerator<T> GetEnumerator() {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public void MakeReadOnly() {
            IsReadOnly = true;
        }

        public bool IsReadOnly {
            get;
            set;
        }

        private void ThrowIfReadOnly() {
            if (IsReadOnly) {
                throw SpecFailure.Sealed();
            }
        }
    }
}
