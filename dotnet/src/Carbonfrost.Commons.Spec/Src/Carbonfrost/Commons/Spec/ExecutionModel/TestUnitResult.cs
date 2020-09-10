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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    public abstract class TestUnitResult {

        private readonly List<TestMessageEventArgs> _messages = new List<TestMessageEventArgs>();
        private TestStatus _status;

        public abstract string DisplayName {
            get;
        }

        public abstract TestUnitType Type {
            get;
        }

        public virtual TestStatus Status {
            get {
                if (ExceptionInfo != null) {
                    return TestStatus.Failed;
                }
                return _status;
            }
        }

        public TestUnitResult Parent {
            get;
            internal set;
        }

        public virtual bool IsFocused {
            get {
                return false;
            }
        }

        public bool ContainsFocusedUnits {
            get {
                return Children.Any(t => t.ContainsFocusedUnits || t.IsFocused);
            }
        }

        public virtual TestUnitResultCollection Children {
            get {
                return TestUnitResultCollection.Empty;
            }
        }

        public IEnumerable<TestUnitResult> Descendants {
            get {
                return Children.SelectMany(c => c.DescendantsAndSelf);
            }
        }

        public IEnumerable<TestUnitResult> DescendantsAndSelf {
            get {
                return new[] { this }.Concat(Descendants);
            }
        }

        // Was the status explicitly set as the result of Assert.Pass() or Assert.Fail()?
        public bool IsStatusExplicit {
            get;
            set;
        }

        public bool IsRunning {
            get {
                return Status == TestStatus.Running;
            }
        }

        public bool Passed {
            get {
                return Status == TestStatus.Passed;
            }
        }

        public bool Failed {
            get {
                return Status == TestStatus.Failed;
            }
        }

        public bool Skipped {
            get {
                return Status == TestStatus.Skipped;
            }
        }

        public bool IsPending {
            get {
                return Status == TestStatus.Pending;
            }
        }

        public virtual bool IsSlow {
            get {
                return false;
            }
        }

        public string Reason {
            get;
            set;
        }

        public virtual ExceptionInfo ExceptionInfo {
            get;
            set;
        }

        public TimeSpan? ExecutionTime {
            get {
                return FinishedAt - StartedAt;
            }
        }

        // HACK These are the messages that were collected during the test run.
        // Can this be API?
        internal List<TestMessageEventArgs> Messages {
            get {
                return _messages;
            }
        }

        public bool StrictlyPassed {
            get {
                return Children.Counts.StrictlyPassed;
            }
        }

        public virtual DateTime? StartedAt {
            get {
                return Children.StartedAt;
            }
        }

        public virtual DateTime? FinishedAt {
            get {
                return Children.FinishedAt;
            }
        }

        private TestUnitCounts Counts {
            get {
                return Children.Counts;
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

        internal abstract JTestUnitResult JResult {
            get;
        }

        internal JTestAttributes Attributes {
            get {
                const JTestAttributes _ = default;
                return (ContainsFocusedUnits ? JTestAttributes.ContainsFocusedUnits : _)
                    | (Failed ? JTestAttributes.Failed : _)
                    | (IsPending ? JTestAttributes.Pending : _)
                    | (IsRunning ? JTestAttributes.Running : _)
                    | (IsStatusExplicit ? JTestAttributes.StatusExplicit : _)
                    | (Passed ? JTestAttributes.Passed : _)
                    | (Skipped ? JTestAttributes.Skipped : _)
                    | (StrictlyPassed ? JTestAttributes.StrictlyPassed : _)
                    | (IsFocused ? JTestAttributes.Focused : _)
                    | (IsSlow ? JTestAttributes.Slow : _)
                ;
            }
        }

        internal virtual void SetFailed(Exception ex) {
            // Problem occured with setup
            if (ex is TargetInvocationException) {
                ex = ex.InnerException;
            }
            ExceptionInfo = ExceptionInfo.FromException(ex);
            Reason = "Problem occurred during setup";
        }

        internal virtual void Done(TestUnit unit, TestRunnerOptions opts) {
            if (Children.Count == 0) {
                _status = TestUnit.ConvertToStatus(unit).GetValueOrDefault(TestStatus.Passed);
            } else {
                _status = Children.Status;
            }
        }

        internal virtual void ApplyCounts(TestUnitCounts counts) {
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
