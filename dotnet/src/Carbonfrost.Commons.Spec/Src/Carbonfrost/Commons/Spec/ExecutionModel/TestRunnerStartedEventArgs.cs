//
// Copyright 2018  Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

    public class TestRunnerStartedEventArgs : EventArgs {

        private readonly int _willRunTests;
        private readonly TestRunnerOptions _options;

        public int WillRunTests {
            get {
                return _willRunTests;
            }
        }

        public TestRunnerOptions Options {
            get {
                return _options;
            }
        }

        public TestRun TestRun {
            get {
                return _options.TestRun;
            }
        }

        internal TestRunnerStartedEventArgs(TestRunnerOptions options, int willRunTests) {
            _options = options;
            _willRunTests = willRunTests;
        }
    }
}
