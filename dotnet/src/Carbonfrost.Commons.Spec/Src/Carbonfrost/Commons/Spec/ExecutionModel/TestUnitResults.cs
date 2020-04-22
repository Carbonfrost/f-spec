//
// Copyright 2016, 2018, 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Reflection;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    public class TestUnitResults : TestUnitResult {

        private readonly TestUnitResultCollection _children;
        private readonly string _displayName;
        private TestStatus _status;

        public override TestStatus Status {
            get {
                if (ExceptionInfo != null) {
                    return TestStatus.Failed;
                }
                return _status;
            }
        }

        public bool StrictlyPassed {
            get {
                return _children.Counts.StrictlyPassed;
            }
        }

        public override DateTime? StartedAt {
            get {
                return _children.StartedAt;
            }
        }

        public override DateTime? FinishedAt {
            get {
                return _children.FinishedAt;
            }
        }

        private TestUnitCounts Counts {
            get {
                return _children.Counts;
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

        internal double ExecutedPercentage {
            get {
                if (TotalCount == 0) {
                    return 0;
                }
                return (double) ExecutedCount / (double) TotalCount;
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

        internal override void SetFailed(Exception ex) {
            // Problem occured with setup
            if (ex is TargetInvocationException) {
                ex = ex.InnerException;
            }
            ExceptionInfo = ExceptionInfo.FromException(ex);
            Reason = "Problem occurred during setup";
        }

        internal override void Done(TestUnit unit) {
            if (Children.Count == 0) {
                _status = TestUnit.ConvertToStatus(unit).GetValueOrDefault(TestStatus.Passed);
            } else {
                _status = Children.Status;
            }
        }

        internal override void ApplyCounts(TestUnitCounts counts) {
            // If no children, then count self
            if (Children.Count == 0) {
                counts.Apply(_status);
                return;
            }

            foreach (var c in Children) {
                c.ApplyCounts(counts);
            }
        }
    }
}
