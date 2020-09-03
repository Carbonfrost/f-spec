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
using System;
using System.Reflection;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    public class TestCaseResult : TestUnitResult {

        private readonly TestName _testName;
        private TestStatus _status;
        private DateTime? _finishedAt;
        private DateTime? _startedAt;
        private Flags _flags;

        public TestName TestName {
            get {
                return _testName;
            }
        }

        public override DateTime? StartedAt {
            get {
                return _startedAt;
            }
        }

        public override DateTime? FinishedAt {
            get {
                return _finishedAt;
            }
        }

        public override TestStatus Status {
            get {
                return _status;
            }
        }

        public override TestUnitResultCollection Children {
            get {
                return TestUnitResultCollection.Empty;
            }
        }

        public override bool IsFocused {
            get {
                return _flags.HasFlag(Flags.Focused);
            }
        }

        public override bool IsSlow {
            get {
                return _flags.HasFlag(Flags.Slow);
            }
        }

        internal TestCaseResult(TestCaseInfo testCase, TestStatus status = TestStatus.NotRun) {
            _status = status;
            Reason = testCase.Reason;
            _testName = testCase.TestName;
        }

        internal override void ApplyCounts(TestUnitCounts counts) {
            counts.Apply(Status);
        }

        internal void SetSuccess() {
            _status = TestStatus.Passed;
        }

        internal override void SetFailed(Exception ex) {
            if (ex is TargetInvocationException) {
                ex = ex.InnerException;
            }
            ExceptionInfo = ExceptionInfo.FromException(ex);

            if (ex is PassException) {
                _status = TestStatus.Passed;
                IsStatusExplicit = true;

            } else if (ex is PendingException) {
                _status = TestStatus.Pending;

            } else if (ex is FailException) {
                _status = TestStatus.Failed;
                IsStatusExplicit = true;

            } else {
                _status = TestStatus.Failed;
            }
        }

        internal override void Done(TestUnit unit, TestRunnerOptions opts) {
            _finishedAt = DateTime.Now;
            if (Status == TestStatus.NotRun) {
                SetSuccess();
            }
            if (unit != null && unit.IsFocused) {
                _flags |= Flags.Focused;
            }
            if (ExecutionTime >= opts.SlowTestThreshold.Value) {
                _flags |= Flags.Slow;
            }
        }

        internal void Starting() {
            _startedAt = DateTime.Now;
        }

        public override string ToString() {
            return $"{Status} {DisplayName}";
        }

        public sealed override string DisplayName {
            get {
                return _testName.DisplayName;
            }
        }

        internal override JTestUnitResult JResult {
            get {
                return new JTestUnitResult {
                    Status = Status,
                    DisplayName = DisplayName,
                    TestName = TestName,
                    Attributes = Attributes,
                };
            }
        }

        [Flags]
        enum Flags {
            None = 0,
            Slow = 1,
            Focused = 2,
        }
    }
}
