//
// Copyright 2016-2018, 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Reflection;

namespace Carbonfrost.Commons.Spec.ExecutionModel.Output {

    class ConsoleLogger : TestRunnerLogger {

        private static readonly IConsoleWrapper console = ConsoleWrapper.Default;
        private readonly DisplayFlags _flags;
        private readonly ConsoleOutputParts _parts;
        private readonly RenderContext _renderContext;

        private RenderContext RenderContext {
            get {
                return _renderContext;
            }
        }

        public ConsoleLogger(TestRunnerOptions opts) {
            if (!opts.SuppressSummary) {
                _flags = DisplayFlags.ShowSummary;
            }
            if (opts.ShowTestNames) {
                _flags |= DisplayFlags.ShowCaseStart;
            }
            if (opts.ShowPassExplicitly) {
                _flags |= DisplayFlags.ShowExplicitPasses;
            }
            _parts = new ConsoleOutputParts(opts);
            _renderContext = new RenderContext {
                Console = console,
                Parts = _parts,
            };
        }

        protected override void OnCaseFinished(TestCaseFinishedEventArgs e) {
            _parts.onTestCaseFinished.Render(RenderContext, e.Result);
            EndTestVerbose(e.TestCase);
        }

        protected override void OnTheoryStarted(TestTheoryStartedEventArgs e) {
            ShowTestVerbose(e.TestTheory);
        }

        protected override void OnAssemblyStarted(TestAssemblyStartedEventArgs e) {
            ShowTestVerbose(e.TestAssembly);
        }

        protected override void OnAssemblyFinished(TestAssemblyFinishedEventArgs e) {
            EndTestVerbose(e.TestAssembly);
        }

        protected override void OnNamespaceStarted(TestNamespaceStartedEventArgs e) {
            ShowTestVerbose(e.TestNamespace);
        }

        protected override void OnNamespaceFinished(TestNamespaceFinishedEventArgs e) {
            EndTestVerbose(e.TestNamespace);
        }

        protected override void OnClassStarted(TestClassStartedEventArgs e) {
            ShowTestVerbose(e.TestClass);
        }

        protected override void OnCaseStarted(TestCaseStartedEventArgs e) {
            ShowTestVerbose(e.TestCase);
        }

        protected override void OnClassFinished(TestClassFinishedEventArgs e) {
            EndTestVerbose(e.TestClass);
        }

        protected override void OnTheoryFinished(TestTheoryFinishedEventArgs e) {
            _parts.onTestTheoryFinished.Render(RenderContext, e.Results);
            EndTestVerbose(e.TestTheory);
        }

        protected override void OnRunnerStarting(TestRunnerStartingEventArgs e) {
            if (EnvironmentHelper.Debug) {
                console.White();
                console.WriteLine("Debug mode activated");
                console.ResetColor();
            }
        }

        protected override void OnRunnerStarted(TestRunnerStartedEventArgs e) {
            console.White();
            console.WriteLine(string.Format("Will run {0} tests", e.WillRunTests));
            console.WriteLine(string.Format("Random Seed: {0}", e.Options.RandomSeed));

            var versionString = TestRunner.Version.Replace(" - ", " (\"fspec ") + "\")";
            console.WriteLine(string.Format("Running with: fspec/{0}", versionString));
            console.WriteLine();
            console.ResetColor();
        }

        protected override void OnRunnerFinished(TestRunnerFinishedEventArgs e) {
            var duration = e.Results.ExecutionTime;
            var results = e.Results;
            console.White();
            console.WriteLine();
            console.WriteLine();
            console.Write(
                "Ran {0} of {1} ({2:0.#}%) tests in {3}",
                results.ExecutedCount,
                results.TotalCount,
                100 * results.ExecutedPercentage,
                FormatDuration(duration)
            );
            console.WriteLine();
            console.ResetColor();

            _parts.onTestRunFinished.Render(RenderContext, e.Results);
        }

        protected override void OnMessage(TestMessageEventArgs e) {
            console.ColorFor(e.Severity);
            console.Write(e.Severity.ToString());
            console.ResetColor();
            console.Write(": ");
            console.WriteLine(e.Message);
        }

        private void ShowTestVerbose(TestUnit unit) {
            if (ShouldSkipVerbose(unit)) {
                return;
            }
            if ((_flags & DisplayFlags.ShowCaseStart) > 0) {
                console.HeadlineColorFor(unit);
                if (unit is TestCaseInfo tci) {
                    console.Write($"{unit.Name}");
                    var args = string.Join(
                        ",",
                        tci.TestName.Arguments.Select(s => TextUtility.Truncate(s))
                    );
                    console.Write($"{args}: ");
                } else {
                    console.WriteLine($"{unit.Name} {unit.Type}");
                }

                console.ResetColor();
                console.ResetStyles();
                console.PushIndent();
            }
        }

        private void EndTestVerbose(TestUnit unit) {
            if ((_flags & DisplayFlags.ShowCaseStart) > 0) {
                if (ShouldSkipVerbose(unit)) {
                    return;
                }
                console.WriteLine();
                console.PopIndent();
            }
        }

        private static bool ShouldSkipVerbose(TestUnit unit) {
            if (unit.Skipped) {
                return true;
            }
            if (unit is TestClassInfo || unit is TestNamespace) {
                return unit.Children.Count == 0 || unit.Children.All(c => ShouldSkipVerbose(c));
            }
            if (unit is TestRun) {
                return false;
            }
            return false;
        }

        static string PrettyCodeBase(Assembly assembly, bool makeRelative = false) {
            return Utility.PrettyCodeBase(assembly, makeRelative);
        }

        static string FormatDuration(TimeSpan? duration) {
            if (duration.HasValue) {
                return ((Time) duration.Value).ToString("n");
            }
            return "";
        }

        [Flags]
        enum DisplayFlags {
            ShowSummary = 1,
            ShowCaseStart = 2,
            ShowExplicitPasses = 4,
        }
    }
}
