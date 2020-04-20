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
using System.Reflection;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    public abstract class TestUnitResult {

        private DateTime _finishedAt;
        private DateTime _startedAt;
        private readonly List<TestMessageEventArgs> _messages = new List<TestMessageEventArgs>();

        public abstract string DisplayName {
            get;
        }

        public TestStatus Status {
            get;
            set;
        }

        public TestUnitResults Parent {
            get;
            internal set;
        }

        public bool IsFocused {
            get;
            internal set;
        }

        public virtual bool ContainsFocusedUnits {
            get {
                return false;
            }
        }

        internal virtual TestUnitResults ContainerOrSelf {
            get {
                if (Parent == null) {
                    return null;
                }
                return Parent.ContainerOrSelf;
            }
        }

        // Was the status explicitly set as the result of Assert.Pass() or Assert.Fail()?
        public bool IsStatusExplicit {
            get; set;
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

        public string Reason {
            get;
            set;
        }

        public ExceptionInfo ExceptionInfo {
            get;
            set;
        }

        public TimeSpan ExecutionTime {
            get {
                return FinishedAt - StartedAt;
            }
        }

        public DateTime StartedAt {
            get {
                return _startedAt;
            }
        }

        public DateTime FinishedAt {
            get {
                return _finishedAt;
            }
        }

        // HACK These are the messages that were collected during the test run.
        // Can this be API?
        internal List<TestMessageEventArgs> Messages {
            get {
                return _messages;
            }
        }

        internal virtual void ApplyCounts(TestUnitCounts counts) {}

        internal void SetSuccess() {
            Status = TestStatus.Passed;
        }

        internal void SetFailed(Exception ex) {
            if (ex is TargetInvocationException) {
                ex = ex.InnerException;
            }
            ExceptionInfo = ExceptionInfo.FromException(ex);
            Reason = Reason;

            if (ex is PassException) {
                Status = TestStatus.Passed;
                IsStatusExplicit = true;

            } else if (ex is PendingException) {
                Status = TestStatus.Pending;

            } else if (ex is FailException) {
                Status = TestStatus.Failed;
                IsStatusExplicit = true;

            } else {
                Status = TestStatus.Failed;
            }
        }

        internal void Starting() {
            _startedAt = DateTime.Now;
        }

        internal void Done() {
            _finishedAt = DateTime.Now;
            if (Status == TestStatus.NotRun) {
                SetSuccess();
            }
        }

        internal void Done(DateTime startedAt) {
            Done();
            _startedAt = startedAt;
        }

    }
}
