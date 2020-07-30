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

    abstract class TestContextWrapper : TestContext {

        public TestContext BaseContext {
            get;
        }

        public override TestEnvironment Environment {
            get {
                return BaseContext.Environment;
            }
        }

        internal override TestLoader Loader {
            get {
                return BaseContext.Loader;
            }
        }

        public override TestLog Log {
            get {
                return BaseContext.Log;
            }
        }

        public override ITestRunnerEvents TestRunnerEvents {
            get {
                return BaseContext.TestRunnerEvents;
            }
        }

        public override TestRunnerOptions TestRunnerOptions {
            get {
                return BaseContext.TestRunnerOptions;
            }
        }

        public override Random Random {
            get {
                return BaseContext.Random;
            }
        }

        public override TestUnit TestUnit {
            get {
                return BaseContext.TestUnit;
            }
        }

        protected TestContextWrapper(TestContext baseContext) {
            if (baseContext == null) {
                throw new ArgumentNullException(nameof(baseContext));
            }
            BaseContext = baseContext;
        }

        public override TestTemporaryDirectory CreateTempDirectory(string name) {
            return BaseContext.CreateTempDirectory(name);
        }

        public override TestTemporaryFile CreateTempFile(string name) {
            return BaseContext.CreateTempFile(name);
        }
    }
}
