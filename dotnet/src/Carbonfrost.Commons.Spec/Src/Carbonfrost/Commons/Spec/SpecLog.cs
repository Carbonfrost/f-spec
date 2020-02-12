//
// Copyright 2017 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.Commons.Spec {

    internal static class SpecLog {

        public static void DidFinalizeOptions(string text) {
            Debug("Options: " + text);
        }

        public static void LoadAssembly(string fullPath) {
            Debug("Assembly loading: " + fullPath);
        }

        public static void AssemblyResolved(string assembly) {
            Debug("Assembly resolved: " + assembly);
        }

        public static void DidCreateTestRunner(TestRunner runner) {
            Debug("Created test runner: " + runner);
        }

        public static void DidSetupLogger(ITestRunnerLogger logger) {
            Debug("Logger: " + logger);
        }

        public static void DiscoveredTests(IEnumerable<TestUnit> tests) {
            foreach (var t in tests) {
                Debug("Discovered test: " + t.DisplayName);
            }
        }

        static void Debug(string text) {
            if (EnvironmentHelper.Debug) {
                Console.Error.WriteLine(text);
            }
        }

    }

}
