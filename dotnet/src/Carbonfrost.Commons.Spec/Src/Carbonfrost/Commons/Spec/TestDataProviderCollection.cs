//
// Copyright 2016, 2017, 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
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
using System.Collections.Generic;

namespace Carbonfrost.Commons.Spec {

    public class TestDataProviderCollection : IReadOnlyList<ITestDataProvider> {

        internal static readonly TestDataProviderCollection Empty
            = new TestDataProviderCollection(Array.Empty<ITestDataProvider>());

        private readonly IList<ITestDataProvider> _items;

        public TestDataProviderCollection(IList<ITestDataProvider> items) {
            if (items == null) {
                throw new ArgumentNullException(nameof(items));
            }
            _items = items;
        }

        internal static TestDataProviderCollection Create(IList<ITestDataProvider> items) {
            if (items == null || items.Count == 0) {
                return Empty;
            }
            return new TestDataProviderCollection(items);
        }

        public ITestDataProvider this[int index] {
            get {
                return _items[index];
            }
        }

        public int Count {
            get {
                return _items.Count;
            }
        }

        public IEnumerator<ITestDataProvider> GetEnumerator() {
            return _items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
