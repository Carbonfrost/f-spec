//
// Copyright 2013, 2016-2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Reflection;

using Carbonfrost.Commons.Spec;
using Carbonfrost.Commons.Spec.ExecutionModel;
using NDesk.Options;
using Carbonfrost.CFSpec.Resources;
using Carbonfrost.Commons.Spec.ExecutionModel.Output;
using System.Text.RegularExpressions;
using System.Linq;

namespace Carbonfrost.CFSpec {

    class ProgramOptions {

        private readonly IConsoleWrapper _console = ConsoleWrapper.Default;

        private TestPlanFilter PlanFilter {
            get {
                return Options.PlanFilter;
            }
        }

        public bool DebugWait;
        public bool Quit;

        private bool ShowWhitespace {
            set {
                if (value) {
                    Options.AssertionMessageFormatMode |= AssertionMessageFormatModes.PrintWhitespace;
                } else {
                    Options.AssertionMessageFormatMode &= ~AssertionMessageFormatModes.PrintWhitespace;
                }
            }
        }

        private bool ShowFullStackTraces {
            set {
                if (value) {
                    Options.AssertionMessageFormatMode |= AssertionMessageFormatModes.FullStackTraces;
                } else {
                    Options.AssertionMessageFormatMode &= ~AssertionMessageFormatModes.FullStackTraces;
                }
            }
        }

        private bool ShowUnifiedDiff {
            set {
                if (value) {
                    Options.AssertionMessageFormatMode |= AssertionMessageFormatModes.UseUnifiedDiff;
                } else {
                    Options.AssertionMessageFormatMode &= ~AssertionMessageFormatModes.UseUnifiedDiff;
                }
            }
        }

        public TestVerificationMode Verify;
        public bool FailOnPending;

        public TestRunnerOptions Options = new TestRunnerOptions();

        private PathCollection FixturePaths {
            get {
                return Options.FixturePaths;
            }
        }

        public PathCollection LoaderPaths {
            get {
                return Options.LoaderPaths;
            }
        }

        private IList<PackageReference> Packages {
            get {
                return Options.PackageReferences;
            }
        }

        private readonly OptionSetExtension OptionSet;

        private ProgramOptions() {
            OptionSet = new OptionSetExtension {
                { "help",          SR.UHelp(),             v => ShowHelp() },
                { "version",       SR.UVersion(),          v => ShowVersion() },

                { "i|fixture=",    SR.UFixture(),          v => FixturePaths.Add(v) },
                { "p|package=",    SR.UPackage(),          v => Packages.Add(SafeParsePackageFormula(v, SR.InvalidPackageReference(), "--package")) },
                { "loader-path=",  SR.ULoaderPath(),       v => LoaderPaths.Add(v) },

                { "exclude=",         SR.UExclude(),        v => PlanFilter.Excludes.AddNew(v) },
                { "exclude-pattern=", SR.UExcludePattern(), v => PlanFilter.Excludes.AddRegex(SafeRegexParse(v, "--exclude-pattern")) },
                { "F|focus=",         SR.UFocus(),          v => PlanFilter.FocusPatterns.AddNew(v) },
                { "no-focus",         SR.UNoFocus(),        v => Options.IgnoreFocus = true },
                { "t|tag=",           SR.UTag(),            v => PlanFilter.Tags.AddNew(v) },
                { "e|include=",          SR.UInclude(),         v => PlanFilter.Includes.AddNew(v) },
                { "E|include-pattern=",  SR.UIncludePattern(),  v => PlanFilter.Includes.AddRegex(SafeRegexParse(v, "--include-pattern")) },

                { "show-whitespace", SR.UShowWhitespace(), v => ShowWhitespace = true },
                { "no-whitespace", SR.UNoWhitespace(),     v => ShowWhitespace = false },
                { "full-stack-traces", SR.UFullStackTraces(), v => ShowFullStackTraces = true },
                { "no-diff",       SR.UNoDiff(),           v => ShowUnifiedDiff = false },
                { "context-lines=",SR.UContextLines(),     v => Options.ContextLines = SafeInt32Parse(v, SR.InvalidContextLines(), "--context-lines") },
                { "no-summary",    SR.UNoSummary(),        v => Options.SuppressSummary = true },
                { "self-test",     SR.USelfTest(),         v => WillSelfTest() },
                { "show-tests",    SR.UShowTestNames(),    v => Options.ShowTestNames = true },
                { "fail-fast",     SR.UFailFast(),         v => Options.FailFast = true },
                { "fail-pending",  SR.UFailOnPending(),    v => FailOnPending = true },

                { "show-pass-explicit", SR.UShowPassExplicit(), v => Options.ShowPassExplicitly = true },
                { "verify=",            SR.UVerify(),           v => Verify = SafeEnumParse<TestVerificationMode>(v, SR.InvalidVerify(), "--verify") },

                { "no-random",     SR.UNoRandomizeSpecs(), v => Options.RandomizeSpecs = false },
                { "random-seed=",  SR.URandom(),           v => Options.RandomSeed = SafeInt32Parse(v, SR.InvalidRandomSeed(), "--random-seed") },

                { "timeout=",      SR.UTimeout(),          v => Options.TestTimeout = SafeTimeSpanParse(v, SR.InvalidTimeSpan(), "--timeout") },
                { "plan-timeout=", SR.UPlanTimeout(),      v => Options.PlanTimeout = SafeTimeSpanParse(v, SR.InvalidTimeSpan(), "--plan-timeout") },
                { "slow-test=",    SR.USlowTest(),         v => Options.SlowTestThreshold = SafeTimeSpanParse(v, SR.InvalidTimeSpan(), "--slow-test") },
                { "pause",         SR.UPause(),            v => DebugWait = true },
            };

            OptionSet.Group(SR.UOutputOptions(), sort: true,
                "context-lines=",
                "no-diff",
                "no-summary",
                "show-whitespace",
                "no-whitespace",
                "show-pass-explicit",
                "show-tests",
                "slow-test=",
                "full-stack-traces"
            );

            OptionSet.Group(SR.UTestSelectionOptions(), sort: true,
                "focus=",
                "no-focus",
                "exclude=",
                "exclude-pattern=",
                "t|tag=",
                "e|include=",
                "E|include-pattern="
            );

            OptionSet.Group(SR.URunnerOptions(), sort: true,
                "plan-timeout=",
                "timeout=",
                "verify=",
                "no-random",
                "random-seed=",
                "self-test",
                "fail-fast",
                "fail-pending",
                "i|fixture=",
                "p|package=",
                "loader-path="
            );

            ShowUnifiedDiff = true;
            ShowFullStackTraces = EnvironmentHelper.Debug;
            FixturePaths.AddAll(EnvironmentHelper.FixturePath);
            LoaderPaths.AddAll(EnvironmentHelper.LoaderPath);
        }

        private void WillSelfTest() {
            if (!TestClass.HasSelfTests) {
                _console.WriteLine("fatal: can't self-test; no tests configured in this fspec build");
                Quit = true;
            }

            Options.IsSelfTest = true;
            ShowFullStackTraces = true;
        }

        public static ProgramOptions Parse(string[] args) {
            var options = new ProgramOptions();
            List<string> unknown = options.ParseInternal(args);

            IEnumerable<string> assemblies;
            string unrecognized;

            if (!CheckForPositionalArgs(unknown, out assemblies, out unrecognized)) {
                throw new NDesk.Options.OptionException("unrecognized option: " + unrecognized, unrecognized);
            }
            options.LoaderPaths.AddAll(assemblies);
            return options;
        }

        private List<string> ParseInternal(string[] args) {
            var myArgs = new List<string>(args.Length);
            foreach (FspecOptionsFile src in FspecOptionsFile.FindAll()) {
                myArgs.AddRange(src.Values);
            }

            foreach (var item in args) {
                myArgs.Add(item);
            }
            return OptionSet.Parse(myArgs);
        }

        static bool CheckForPositionalArgs(IList<string> unknowns,
            out IEnumerable<string> assemblies,
            out string unrecognized
        ) {

            unrecognized = null;
            assemblies = Enumerable.Empty<string>();

            if (unknowns.Count == 0) {
                return true;
            }

            // Determine if there is an unknown option --example
            if (!VerifyNotOptions(unknowns, out unrecognized)) {
                return false;
            }

            assemblies = unknowns;
            return true;
        }

        static bool VerifyNotOptions(IList<string> unknowns, out string unrecognized) {
            unrecognized = null;

            foreach (string s in unknowns) {
                if (s.StartsWith("-", StringComparison.Ordinal)) {
                    unrecognized = s;
                    return false;
                }
            }

            return true;
        }

        public void Usage() {
            var s = new StringWriter();
            OptionSet.WriteUsage(s);
            _console.Write(s.ToString());
        }

        private void ShowHelp() {
            Usage();
            Quit = true;
        }

        static TimeSpan SafeTimeSpanParse(string v, string msg, string optionName) {
            Time result;
            if (Time.TryParse(v, out result)) {
                return result.Value;
            }
            throw ParseFailure(v, msg, optionName);
        }

        static int SafeInt32Parse(string v, string msg, string optionName) {
            int result;
            if (Int32.TryParse(v, out result)) {
                return result;
            }
            throw ParseFailure(v, msg, optionName);
        }

        static T SafeEnumParse<T>(string v, string msg, string optionName) where T : struct {
            T result;
            if (Enum.TryParse(v, true, out result)) {
                return result;
            }
            throw ParseFailure(v, msg, optionName);
        }

        private PackageReference SafeParsePackageFormula(string v, string msg, string optionName) {
            PackageReference result;
            if (PackageReference.TryParse(v, out result)) {
                return result;
            }
            throw ParseFailure(v, msg, optionName);
        }

        static Regex SafeRegexParse(string v, string optionName) {
            try {
                return new Regex(v);
            } catch (Exception ex) {
                throw ParseFailure(v, SR.InvalidRegex(ex.Message), optionName);
            }
        }

        static OptionException ParseFailure(string v, string msg, string optionName) {
            return new OptionException(string.Format("{0}: '{1}'", msg, v), optionName);
        }

        private static DateTime? GetBuildDate() {
            foreach (AssemblyMetadataAttribute meta in typeof(Program).GetTypeInfo().Assembly.GetCustomAttributes(typeof(AssemblyMetadataAttribute))) {
                if (meta.Key == "[share:BuildDate]") {
                    if (DateTime.TryParse(meta.Value, out DateTime result)) {
                        return result;
                    }
                    return null;
                }
            }
            return null;
        }

        private void ShowVersion() {
            var asm = typeof(Program).GetTypeInfo().Assembly;

            string version = TestRunner.Version;

            string registered = " (R)";
            string copy = "(c)";

            if (_console.IsUnicodeEncoding) {
                registered = "®";
                copy = "©";
            }

            DateTime? buildDate = GetBuildDate();
            if (buildDate.GetValueOrDefault().Year < 2016) {
                buildDate = new DateTime(2016, 1, 1, 0, 0, 0);
            }
            string since = buildDate.GetValueOrDefault().Year > 2016 ? "2016 - " : null;
            _console.WriteLine("Carbonfrost{1} Fspec version {0}", version, registered);
            _console.WriteLine("Copyright {0} {2}{1:yyyy} Carbonfrost Systems, Inc.  All rights reserved.", copy, buildDate, since);
            _console.WriteLine();
            Quit = true;
        }
    }
}
