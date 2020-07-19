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
using System;

using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.Commons.Spec {

    class RootTestContext : TestContext {

        private readonly TestUnit _self;
        private readonly TestRunner _runner;
        private readonly TestLoader _loader;
        private readonly TestLog _log;
        private TestTemporaryDirectory _defaultTemp;
        private readonly TestEnvironment _environment = new TestEnvironment();

        public override TestEnvironment Environment {
            get {
                return _environment;
            }
        }

        internal override TestLoader Loader {
            get {
                return _loader;
            }
        }

        public override TestLog Log {
            get {
                return _log;
            }
        }

        public override ITestRunnerEvents TestRunnerEvents {
            get {
                return _runner;
            }
        }

        public override TestRunnerOptions TestRunnerOptions {
            get {
                return _runner.Options;
            }
        }

        public override Random Random {
            get {
                return _runner.RandomCache;
            }
        }

        public override TestUnit TestUnit {
            get {
                return _self;
            }
        }

        public RootTestContext(TestUnit self, TestRunner runner) {
            _self = self;
            _runner = runner;
            _log = new TestLog(runner);
            _loader = new TestLoader(_runner.Options, this);
        }

        public override TestTemporaryDirectory CreateTempDirectory(string name) {
            return RegisterDisposable(new TestTemporaryDirectory(_runner.SessionId, name));
        }

        public override TestTemporaryFile CreateTempFile(string name) {
            return DefaultTempDirectory().CreateFile(name);
        }

        private TestTemporaryDirectory DefaultTempDirectory() {
            if (_defaultTemp == null) {
                _defaultTemp = CreateTempDirectory();
            }
            return _defaultTemp;
        }

    }
}
