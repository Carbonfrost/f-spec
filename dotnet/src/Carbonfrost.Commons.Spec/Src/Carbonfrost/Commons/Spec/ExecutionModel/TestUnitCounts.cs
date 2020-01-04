//
// Copyright 2016 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

    internal class TestUnitCounts {

        private readonly int[] _counts = new int[(int)TestStatus.Pending + 1];

        public int Total {
            get {
                return _counts.Sum();
            }
        }

        public int Passed {
            get {
                return _counts[(int)TestStatus.Passed];
            }
        }

        public int Skipped {
            get {
                return _counts[(int)TestStatus.Skipped];
            }
        }

        public int Failed {
            get {
                return _counts[(int)TestStatus.Failed];
            }
        }

        public int Pending {
            get {
                return _counts[(int)TestStatus.Pending];
            }
        }

        public void Apply(TestStatus status) {
            _counts[(int)status]++;
        }
    }
}
