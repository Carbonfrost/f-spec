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

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    internal struct JTestUnitResult {

        public TestId? Id {
            get;
            set;
        }

        public TestName? TestName {
            get;
            set;
        }

        public TestUnitType Type {
            get;
            set;
        }

        public TestStatus Status {
            get;
            set;
        }

        public string DisplayName {
            get;
            set;
        }

        public TimeSpan? ExecutionTime {
            get;
            set;
        }

        public double? ExecutedPercentage {
            get;
            set;
        }

        public JTestAttributes Attributes {
            get;
            set;
        }

        public int? Ordinal {
            get;
            set;
        }
    }
}
