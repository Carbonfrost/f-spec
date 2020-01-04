//
// Copyright 2016-2018 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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


namespace Carbonfrost.Commons.Spec.ExecutionModel.Output {

    class ConsoleLogger : TestRunnerLogger {

        private static readonly IConsoleWrapper console = ConsoleWrapper.Default;
        private readonly IList<TestUnitResult> _problems = new List<TestUnitResult>();
        private static readonly string RULE = new string('-', 24);
        private readonly DisplayFlags _flags;
        private readonly IList<TestMessageEventArgs> _bufferLog = new List<TestMessageEventArgs>();

        private readonly ConsoleOutputPart<TestCaseResult>[] _onTestCaseFinished;
        private readonly ConsoleOutputPart<TestRunResults>[] _onTestRunFinished;
        private readonly ConsoleOutputPart<IList<TestUnitResult>>[] _onTestRunFinishedWithProblems;

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

            _onTestCaseFinished = new[] {
                new ConsoleTestCaseStatus(console),
            };
            _onTestRunFinished = new[] {
                new ConsoleTestRunResults(console),
            };
            _onTestRunFinishedWithProblems = new[] {
                new ConsoleTestRunProblems(console),
            };
        }

        protected override void OnTestClassFinished(TestClassFinishedEventArgs e) {
            if (_bufferLog.Count > 0) {
                console.PushIndent();
                console.WriteLine();
                console.WriteLine();
                console.WriteLine(e.Class.Name);

                foreach (var m in _bufferLog) {
                    PrintMessage(m);
                }
                console.PopIndent();
                console.WriteLine();
                _bufferLog.Clear();
            }

            // Test class failed in a special way, so we want to display this
            if (!e.Result.Passed) {
                console.ColorFor(e.Result);
                console.WriteLine();
                console.Write(string.Format("  {0} (setup): ", e.Class));
                console.ResetColor();
                DisplayResult(e.Result);
            }
        }

        protected override void OnTestNamespaceStarted(TestNamespaceStartedEventArgs e) {
            console.Gray();
            console.Write("{0}: ", e.Namespace);
            console.ResetColor();
            base.OnTestNamespaceStarted(e);
        }

        protected override void OnTestNamespaceFinished(TestNamespaceFinishedEventArgs e) {
            console.Gray();
            console.Write(" ({0} of {1} tests, {2})",
                          e.Results.ExecutedCount,
                          e.Results.TotalCount,
                          FormatDuration(e.Results.ExecutionTime));
            console.WriteLine();
            console.ResetColor();
            base.OnTestNamespaceFinished(e);
        }

        protected override void OnTestAssemblyStarted(TestAssemblyStartedEventArgs e) {
            console.Gray();
            console.WriteLine(PrettyCodeBase(e.Assembly, true));
            console.ResetColor();
        }

        protected override void OnTestCaseStarted(TestCaseStartedEventArgs e) {
            // We only show this once for a theory (hence on position 0)
            if (e.Position == 0 && (_flags & DisplayFlags.ShowCaseStart) > 0) {
                console.Gray();
                console.WriteLine();
                console.Write("  " + e.MethodName + ": ");
                console.ResetColor();
            }
        }

        protected override void OnTestCaseFinished(TestCaseFinishedEventArgs e) {
            e.Result.Messages = _bufferLog.ToArray();
            _bufferLog.Clear();

            if (IsProblem(e.Result)) {
                _problems.Add(e.Result);
            }

            RenderResults(_onTestCaseFinished, e.Result);
        }

        protected override void OnTestRunnerStarting(TestRunnerStartingEventArgs e) {
            if (EnvironmentHelper.Debug) {
                console.White();
                console.WriteLine("Debug mode activated");
                console.ResetColor();
            }
        }

        protected override void OnTestRunnerStarted(TestRunnerStartedEventArgs e) {
            console.White();
            console.WriteLine(string.Format("Will run {0} tests", e.WillRunTests));
            console.WriteLine(string.Format("Random Seed: {0}", e.Options.RandomSeed));

            var versionString = TestRunner.Version.Replace(" - ", " (\"spec ") + "\")";
            console.WriteLine(string.Format("Running with: spec/{0}", versionString));
            console.WriteLine();
            console.ResetColor();
        }

        protected override void OnTestRunnerFinished(TestRunnerFinishedEventArgs e) {
            var duration = e.Results.ExecutionTime;
            var results = e.Results;
            console.White();
            console.WriteLine();
            console.Write("Ran {0} of {1} tests in {2}", results.ExecutedCount, results.TotalCount, FormatDuration(duration));
            console.WriteLine();
            console.ResetColor();

            RenderResults(_onTestRunFinishedWithProblems, _problems);
            RenderResults(_onTestRunFinished, e.Results);
        }

        protected override void OnMessage(TestMessageEventArgs e) {
            _bufferLog.Add(e);
        }

        static void PrintMessage(TestMessageEventArgs e) {
            switch (e.Severity) {
                case TestMessageSeverity.Debug:
                    console.Gray();
                    break;
                case TestMessageSeverity.Trace:
                case TestMessageSeverity.Information:
                    console.White();
                    break;
                case TestMessageSeverity.Warning:
                    console.Yellow();
                    break;
                case TestMessageSeverity.Error:
                case TestMessageSeverity.Fatal:
                    console.Red();
                    break;
            }
            console.Write(e.Severity.ToString().ToLowerInvariant());
            console.Write(": ");
            console.WriteLine(e.Message);
            console.ResetColor();
        }

        static string PrettyCodeBase(Assembly assembly, bool makeRelative = false) {
            return Utility.PrettyCodeBase(assembly, makeRelative);
        }

        private void DisplayResult(TestUnitResult result) {
            var shouldShowDetails = result.Failed || result.IsPending || result.Messages.Length > 0;
            if ((_flags & DisplayFlags.ShowExplicitPasses) > 0
                && result.Passed
                && result.ExceptionInfo != null
                && !string.IsNullOrEmpty(result.ExceptionInfo.Message)) {
                shouldShowDetails = true;
            }
            if (shouldShowDetails) {
                DisplayResultDetails(result);
            }
            console.ResetColor();
        }

        private bool IsProblem(TestUnitResult result) {
            if ((_flags & DisplayFlags.ShowExplicitPasses) > 0
                && result.Passed
                && result.ExceptionInfo != null
                && !string.IsNullOrEmpty(result.ExceptionInfo.Message)) {

                return true;
            }
            return result.IsPending || result.Skipped || result.Failed;
        }

        internal static void DisplayResultDetails(TestUnitResult result) {
            console.ColorFor(result);
            console.WriteLine();
            console.WriteLine(RULE);
            if (result.Failed) {
                console.WriteLine("Failure");
                console.WriteLine(result.Reason);
            } else if (result.IsPending) {
                console.WriteLine("Pending");
                console.WriteLine(result.Reason);
            }

            console.PushIndent();
            console.WriteLine();
            console.WriteLine(result.DisplayName);

            if (result.ExceptionInfo != null) {
                console.WriteLine();
                console.WriteLine(result.ExceptionInfo.Message.TrimEnd('\r', '\n'));
                console.WriteLine();

                console.DarkGray();
                console.WriteLine(result.ExceptionInfo.StackTrace.TrimEnd('\r', '\n'));
            }

            console.ResetColor();
            foreach (var m in result.Messages) {
                PrintMessage(m);
            }
            console.PopIndent();
            console.WriteLine();
        }

        private static void RenderResults<T>(IEnumerable<ConsoleOutputPart<T>> items, T result) {
            foreach (var r in items) {
                r.Render(result);
            }
        }

        static string FormatDuration(TimeSpan duration) {
            return TextUtility.FormatDuration(duration);
        }

        [Flags]
        enum DisplayFlags {
            ShowSummary = 1,
            ShowCaseStart = 2,
            ShowExplicitPasses = 4,
        }
    }
}
