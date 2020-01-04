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
using System.Linq;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    public class TestUnitResults : TestUnitResult {

        private readonly TestUnitResultCollection _children;
        private TestUnitCounts _countsCache;
        private readonly string _displayName;

        private TestUnitCounts Counts {
            get {
                if (_countsCache == null) {
                    var all = new TestUnitCounts();
                    ApplyCounts(all);
                    _countsCache = all;
                }

                return _countsCache;
            }
        }

        internal override TestUnitResults ContainerOrSelf {
            get {
                return this;
            }
        }

        public override bool ContainsFocusedUnits {
            get {
                return Children.Any(t => t.ContainsFocusedUnits || t.IsFocused);
            }
        }

        internal int ExecutedCount {
            get {
                return Counts.Total - SkippedCount;
            }
        }

        internal int TotalCount {
            get {
                return Counts.Total;
            }
        }

        public int PassedCount {
            get {
                return Counts.Passed;
            }
        }

        public int SkippedCount {
            get {
                return Counts.Skipped;
            }
        }

        public int FailedCount {
            get {
                return Counts.Failed;
            }
        }

        public int PendingCount {
            get {
                return Counts.Pending;
            }
        }

        public TestUnitResultCollection Children {
            get {
                return _children;
            }
        }

        internal TestUnitResults(string displayName) {
            _displayName = displayName;
            _children = new TestUnitResultCollection(this);
        }

        public override string DisplayName {
            get {
                return _displayName;
            }
        }

        internal override void ApplyCounts(TestUnitCounts counts) {
            foreach (var c in Children) {
                c.ApplyCounts(counts);
            }
        }
    }
}
