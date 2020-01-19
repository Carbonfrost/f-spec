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
using System;
using System.Collections.Generic;
using System.Linq;

namespace Carbonfrost.Commons.Spec {

    public class TestFixtureDataCollection : IList<TestFixtureData> {

        private readonly List<TestFixtureData> _items = new List<TestFixtureData>();

        public int IndexOf(TestFixtureData item) {
            return _items.IndexOf(item);
        }

        public void Insert(int index, TestFixtureData item) {
            _items.Insert(index, item);
        }

        public void RemoveAt(int index) {
            _items.RemoveAt(index);
        }

        public TestFixtureData this[int index] {
            get {
                return _items[index];
            }
            set {
                _items[index] = value;
            }
        }

        public void Add(TestFixtureData item) {
            _items.Add(item);
        }

        public void Clear() {
            _items.Clear();
        }

        public bool Contains(TestFixtureData item) {
            return _items.Contains(item);
        }

        public void CopyTo(TestFixtureData[] array, int arrayIndex) {
            _items.CopyTo(array, arrayIndex);
        }

        public bool Remove(TestFixtureData item) {
            return _items.Remove(item);
        }

        public int Count {
            get {
                return _items.Count;
            }
        }

        public bool IsReadOnly {
            get {
                return false;
            }
        }

        public IEnumerator<TestFixtureData> GetEnumerator() {
            return _items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
