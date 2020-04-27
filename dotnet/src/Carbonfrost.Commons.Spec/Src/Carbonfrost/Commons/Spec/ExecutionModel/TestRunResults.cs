//
// Copyright 2016, 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
using System;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    public class TestRunResults : TestUnitResults {

        private DateTime _startedAt;
        private TestRunProblems _problems;

        public override DateTime? StartedAt {
            get {
                if (Children.Count == 0) {
                    return _startedAt;
                }
                return base.StartedAt;
            }
        }

        public override DateTime? FinishedAt {
            get {
                if (Children.Count == 0) {
                    return _startedAt;
                }
                return base.FinishedAt;
            }
        }

        public TestRunFailureReason FailureReason {
            get {
                if (Failed) {
                    return TestRunFailureReason.Failure;
                }
                if (IsPending) {
                    return TestRunFailureReason.ContainsPendingElements;
                }
                if (ContainsFocusedUnits) {
                    return TestRunFailureReason.ContainsFocusedElements;
                }
                return TestRunFailureReason.Success;
            }
        }

        public TestRunProblems Problems {
            get {
                return _problems ?? (_problems = new TestRunProblems(Descendants));
            }
        }

        public TestRunResults()
            : base("<>") {
        }

        internal void RunStarting() {
            _startedAt = DateTime.Now;
        }
    }
}
