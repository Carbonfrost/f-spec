//
// Copyright 2018 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Collections.ObjectModel;
using System.Linq;

using System.Reflection;


namespace Carbonfrost.Commons.Spec.ExecutionModel {

    public class TestUnitCollection : Collection<TestUnit> {

        internal static readonly TestUnitCollection Empty =
             new TestUnitCollection(false);

        private readonly TestUnit _owner;

        internal TestUnitCollection(bool _) {
            IsReadOnly = true;
        }

        internal TestUnitCollection(TestUnit owner) {
            if (owner == null) {
                throw new ArgumentNullException("owner");
            }
            _owner = owner;
        }

        protected void ThrowIfReadOnly() {
            if (IsReadOnly) {
                throw SpecFailure.ReadOnlyCollection();
            }
        }

        protected override void InsertItem(int index, TestUnit item) {
            if (item == null) {
                throw new ArgumentNullException("item");
            }
            if (item.Parent != null) {
                throw new NotImplementedException();
            }

            ThrowIfReadOnly();
            item.SetParent(_owner);
            Items.Insert(index, item);
        }

        protected override void ClearItems() {
            ThrowIfReadOnly();
            foreach (var item in Items) {
                item.SetParent(null);
            }

            base.ClearItems();
        }

        protected override void SetItem(int index, TestUnit item) {
            if (item == null) {
                throw new ArgumentNullException("item");
            }

            ThrowIfReadOnly();
            this[index].SetParent(null);
            item.SetParent(_owner);
            Items[index] = item;
        }

        protected override void RemoveItem(int index) {
            ThrowIfReadOnly();
            this[index].SetParent(null);
            Items.RemoveAt(index);
        }

        internal void MakeReadOnly() {
            IsReadOnly = true;
        }

        public bool IsReadOnly { get; private set; }

        public void AddAssembly(Assembly assembly) {
            if (assembly == null) {
                throw new ArgumentNullException("assembly");
            }
            Add(TestAssembly.Create(assembly));
        }

    }
}
