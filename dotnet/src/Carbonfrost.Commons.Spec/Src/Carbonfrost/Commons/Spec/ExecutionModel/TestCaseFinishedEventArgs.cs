//
// Copyright 2016, 2017, 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

    public class TestCaseFinishedEventArgs : EventArgs {

        public TestCaseInfo TestCase {
            get {
                return (TestCaseInfo) _inner.TestUnit;
            }
        }

        public TestCaseResult Result {
            get {
                return (TestCaseResult) _inner.Result;
            }
        }

        private readonly TestUnitFinishedEventArgs _inner;

        internal TestCaseFinishedEventArgs(TestUnitFinishedEventArgs inner) {
            _inner = inner;
        }
    }
}
