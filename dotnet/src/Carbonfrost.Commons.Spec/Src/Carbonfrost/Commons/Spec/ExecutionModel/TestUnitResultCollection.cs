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
using System.Collections.ObjectModel;
using System.Linq;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    public class TestUnitResultCollection : Collection<TestUnitResult> {

        internal static readonly TestUnitResultCollection Empty = new TestUnitResultCollection(null) {
            IsReadOnly = true
        };

        private readonly TestUnitResult _owner;
        private TestUnitCounts _countsCache;

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
                foreach (var item in Items.Reverse()) {
                    if (item.FinishedAt.HasValue) {
                        return item.FinishedAt;
                    }
                }
                return null;
            }
        }

        internal TestStatus Status {
            get {
                return Counts.Status;
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

        internal TestUnitResultCollection(TestUnitResult owner) {
            _owner = owner;
        }

        protected void ThrowIfReadOnly() {
            if (IsReadOnly) {
                throw SpecFailure.ReadOnlyCollection();
            }
        }

        protected override void InsertItem(int index, TestUnitResult item) {
            if (item == null) {
                throw new ArgumentNullException(nameof(item));
            }
            if (item.Parent == _owner) {
                return;
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
            if (item == null) {
                throw new ArgumentNullException(nameof(item));
            }

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

        internal void MakeReadOnly() {
            IsReadOnly = true;
        }

        public bool IsReadOnly {
            get;
            private set;
        }

        private void ClearCache() {
            _countsCache = null;
        }
    }
}
