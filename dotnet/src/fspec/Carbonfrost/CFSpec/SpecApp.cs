//
// Copyright 2016, 2017, 2019 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.IO;
using System.Linq;
using System.Reflection;

using Carbonfrost.Commons.Spec;
using Carbonfrost.Commons.Spec.ExecutionModel;
using Carbonfrost.Commons.Spec.ExecutionModel.Output;
using Carbonfrost.CFSpec.Resources;

namespace Carbonfrost.CFSpec {

    class SpecApp {

        public ProgramOptions Options;

        public int Run() {
            try {
                return RunCore();

            } catch (Exception ex) {
                // TODO Display this appropriately
                ConsoleWrapper.Default.WriteLine(ex.ToString());
            }

            return 1;
        }

        private int RunCore() {
            var testRunnerOptions = new TestRunnerOptions {
                RandomSeed = Options.RandomSeed,
                RandomizeSpecs = !Options.DontRandomizeSpecs,
                PlanTimeout = Options.PlanTimeout,
                TestTimeout = Options.TestTimeout,
                SuppressSummary = Options.NoSummary,
                ShowTestNames = Options.ShowTestNames,
                ContextLines = Options.ContextLines,
                ShowPassExplicitly = Options.ShowPassExplicitly,
                Verification = Options.Verify,
            };
            if (!Options.NoWhitespace) {
                testRunnerOptions.AssertionMessageFormatMode |= AssertionMessageFormatModes.PrintWhitespace;
            }
            if (!Options.NoUnifiedDiff) {
                testRunnerOptions.AssertionMessageFormatMode |= AssertionMessageFormatModes.UseUnifiedDiff;
            }
            foreach (var s in Options.FocusPatterns) {
                testRunnerOptions.FocusPatterns.Add(s);
            }
            testRunnerOptions.IgnoreFocus = Options.NoFocus;
            foreach (var s in Options.SkipPatterns) {
                testRunnerOptions.SkipPatterns.Add(s);
            }
            foreach (var s in Options.FixturePaths) {
                testRunnerOptions.FixtureDirectories.Add(s);
            }
            SpecLog.DidFinalizeOptions(Options.ToString());

            string message;
            var list = LoadAssemblies(out message);
            if (message != null) {
                Console.WriteLine("spec: " + message);
                return 1;
            }
            foreach (var asm in list) {
                testRunnerOptions.TestRun.AddAssembly(asm);
            }
            if (Options.SelfTest) {
                testRunnerOptions.TestRun.AddAssembly(typeof(TestMatcher).GetTypeInfo().Assembly);
                testRunnerOptions.IsSelfTest = true;
            }
            var runner = TestRunner.Create(testRunnerOptions);
            SpecLog.DidCreateTestRunner(runner);
            var result = runner.RunTests();

            return (int) result.FailureReason;
        }

        private List<Assembly> LoadAssemblies(out string message) {
            var list = new List<Assembly>();
            message = null;

            foreach (var asmPath in Options.Assemblies) {
                string fullPath = Path.GetFullPath(asmPath);
                if (!File.Exists(fullPath)) {
                    message = SR.FailedToLoadAssemblyPath(asmPath);
                    break;
                }
                try {
                    SpecLog.LoadAssembly(fullPath);

                    var asmInfo = System.Runtime.Loader.AssemblyLoadContext.Default.LoadFromAssemblyPath(
                        fullPath
                    );
                    list.Add(asmInfo);

                } catch (BadImageFormatException) {
                    message = SR.FailedToLoadAssembly(asmPath);
                    break;
                } catch (FileNotFoundException ex) {
                    message = SR.FailedToLoadAssemblyPath(asmPath + " -> " + ex.FileName);
                    break;
                } catch (IOException ex) {
                    message = SR.FailedToLoadAssemblyGeneralIO(asmPath, ex.Message);
                    break;
                }
            }
            return list;
        }
    }
}
