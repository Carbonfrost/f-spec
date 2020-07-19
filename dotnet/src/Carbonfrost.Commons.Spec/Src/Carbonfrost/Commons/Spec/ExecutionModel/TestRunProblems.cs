//
// Copyright 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    public class TestRunProblems : IEnumerable<TestUnitResult> {

        private readonly List<TestUnitResult> _pending = new List<TestUnitResult>();
        private readonly List<TestUnitResult> _failures = new List<TestUnitResult>();
        private readonly List<TestUnitResult> _slow = new List<TestUnitResult>();

        public int Count {
            get {
                return Failures.Count + Pending.Count + Slow.Count;
            }
        }

        public IReadOnlyList<TestUnitResult> Failures {
            get {
                return _failures;
            }
        }

        public IReadOnlyList<TestUnitResult> Pending {
            get {
                return _pending;
            }
        }

        public IReadOnlyList<TestUnitResult> Slow {
            get {
                return _slow;
            }
        }

        internal TestRunProblems(IEnumerable<TestUnitResult> descendants, TestRunnerOptions opts) {
            foreach (var item in descendants) {
                if (item.Children.Count > 0) {
                    // Because statuses rollup into the composite result (e.g. if composite contains
                    // only failed tests, then it rolls up as failed),
                    // only report composite results as a problem if there is a setup error.
                    bool ignoreProblem = item.ExceptionInfo == null && item.Messages.Count == 0;
                    if (ignoreProblem) {
                        continue;
                    }
                }
                if (item.IsPending) {
                    _pending.Add(item);
                } else if (item.Failed) {
                    _failures.Add(item);
                } else if (item.IsSlow) {
                    _slow.Add(item);
                }
            }
        }

        public IEnumerator<TestUnitResult> GetEnumerator() {
            return Failures.Concat(Pending).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
