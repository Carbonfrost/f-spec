//
// Copyright 2018, 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Collections.ObjectModel;
using System.Linq;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    public class TestUnitResultCollection : Collection<TestUnitResult> {

        private readonly TestUnitResults _owner;
        private TestUnitCounts _countsCache;
        private TestStatus? _statusCache;

        internal DateTime? StartedAt {
            get {
                if (Count == 0) {
                    return null;
                }
                return Items[0].StartedAt;
            }
        }

        internal DateTime? FinishedAt {
            get {
                if (Count == 0) {
                    return null;
                }
                return Items.Last().FinishedAt;
            }
        }

        internal TestStatus Status {
            get {
                if (_statusCache == null) {
                    _statusCache = ComputeStatusSlow();
                }
                return _statusCache.Value;
            }
        }

        internal TestUnitCounts Counts {
            get {
                if (_countsCache == null) {
                    _countsCache = new TestUnitCounts();
                    foreach (var c in Items) {
                        c.ApplyCounts(_countsCache);
                    }
                }

                return _countsCache;
            }
        }

        internal TestUnitResultCollection(TestUnitResults owner) {
            _owner = owner;
        }

        protected void ThrowIfReadOnly() {
            if (IsReadOnly) {
                throw SpecFailure.ReadOnlyCollection();
            }
        }

        protected override void InsertItem(int index, TestUnitResult item) {
            if (item == null) {
                throw new ArgumentNullException("item");
            }
            if (item.Parent != null) {
                throw new NotImplementedException();
            }

            ThrowIfReadOnly();
            item.Parent = _owner;
            base.InsertItem(index, item);
            ClearCache();
        }

        protected override void ClearItems() {
            ThrowIfReadOnly();
            foreach (var item in Items) {
                item.Parent = null;
            }

            base.ClearItems();
            ClearCache();
        }

        protected override void SetItem(int index, TestUnitResult item) {
            if (item == null)
                throw new ArgumentNullException("item");

            ThrowIfReadOnly();
            this[index].Parent = null;
            item.Parent = _owner;
            base.SetItem(index, item);
            ClearCache();
        }

        protected override void RemoveItem(int index) {
            ThrowIfReadOnly();
            this[index].Parent = null;
            base.RemoveItem(index);
            ClearCache();
        }

        public void MakeReadOnly() {
            IsReadOnly = true;
        }

        public bool IsReadOnly { get; private set; }

        private void ClearCache() {
            _countsCache = null;
            _statusCache = null;
        }

        private TestStatus ComputeStatusSlow() {
            if (Items.Count == 0) {
                return TestStatus.Passed;
            }
            if (Items.Any(c => c.Status == TestStatus.Failed)) {
                return TestStatus.Failed;
            }
            return TestStatus.Passed;
        }
    }
}
