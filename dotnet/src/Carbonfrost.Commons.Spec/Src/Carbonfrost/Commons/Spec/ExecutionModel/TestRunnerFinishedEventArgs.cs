//
// Copyright 2016, 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

    public class TestRunnerFinishedEventArgs : EventArgs {

        private readonly TestRunnerOptions _opts;

        public TestRunResults Results {
            get;
            private set;
        }

        public TestRunnerOptions Options {
            get {
                return _opts;
            }
        }

        public TestRun TestRun {
            get;
            private set;
        }

        internal TestRunnerFinishedEventArgs(TestRun testRun, TestRunResults results, TestRunnerOptions opts) {
            Results = results;
            TestRun = testRun;
            _opts = opts;
        }
    }
}
