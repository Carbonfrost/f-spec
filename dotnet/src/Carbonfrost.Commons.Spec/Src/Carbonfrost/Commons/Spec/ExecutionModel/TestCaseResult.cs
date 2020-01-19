//
// Copyright 2016-2018 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    public class TestCaseResult : TestUnitResult {

        private readonly string _displayName;

        public string Output {
            get; set;
        }

        internal TestCaseResult(TestCase testCase) {
            _displayName = testCase.DisplayName;
            Reason = testCase.Reason;
        }

        internal TestCaseResult(string displayName) {
            _displayName = displayName;
        }

        public override string DisplayName {
            get {
                return _displayName;
            }
        }

        internal override void ApplyCounts(TestUnitCounts counts) {
            counts.Apply(Status);
        }

    }
}
