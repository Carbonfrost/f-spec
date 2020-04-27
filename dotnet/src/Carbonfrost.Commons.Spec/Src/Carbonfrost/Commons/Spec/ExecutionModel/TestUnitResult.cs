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

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    public abstract class TestUnitResult {

        private readonly List<TestMessageEventArgs> _messages = new List<TestMessageEventArgs>();

        public abstract string DisplayName {
            get;
        }

        public abstract TestStatus Status {
            get;
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

        public string Reason {
            get;
            set;
        }

        public ExceptionInfo ExceptionInfo {
            get;
            set;
        }

        public TimeSpan? ExecutionTime {
            get {
                return FinishedAt - StartedAt;
            }
        }

        public abstract DateTime? StartedAt {
            get;
        }

        public abstract DateTime? FinishedAt {
            get;
        }

        // HACK These are the messages that were collected during the test run.
        // Can this be API?
        internal List<TestMessageEventArgs> Messages {
            get {
                return _messages;
            }
        }

        internal virtual void ApplyCounts(TestUnitCounts counts) {}

        internal virtual void SetFailed(Exception ex) {
        }

        internal virtual void Done(TestUnit unit) {
        }
    }
}
