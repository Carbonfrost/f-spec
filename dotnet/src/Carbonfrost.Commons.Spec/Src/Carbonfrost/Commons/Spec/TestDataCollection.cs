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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Carbonfrost.Commons.Spec {

    public class TestDataCollection : Collection<TestData>, ITestDataProvider {

        public TestDataCollection() {
        }

        public TestDataCollection(IEnumerable<TestData> items) {
            if (items == null) {
                return;
            }
            Items.AddAll(items);
        }

        IEnumerable<TestData> ITestDataProvider.GetData(TestContext context) {
            return this;
        }
    }

    public class TestDataCollection<T> : Collection<TestData<T>>, ITestDataProvider {

        public TestDataCollection() {
        }

        public TestDataCollection(IEnumerable<T> items) {
            if (items == null) {
                return;
            }
            Items.AddAll(items.Select(d => TestData.Create(d)));
        }

        public TestDataCollection(IEnumerable<TestData<T>> items) {
            if (items == null) {
                return;
            }
            Items.AddAll(items);
        }

        IEnumerable<TestData> ITestDataProvider.GetData(TestContext context) {
            return this.Select(d => d.Untyped());
        }
    }
}
