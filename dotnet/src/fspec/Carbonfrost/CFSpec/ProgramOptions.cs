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

namespace Carbonfrost.CFSpec {

    class ProgramOptions {

        private readonly IConsoleWrapper _console = ConsoleWrapper.Default;
        public readonly List<string> Assemblies = new List<string>();
        public readonly List<string> FixturePaths = new List<string>();
        public readonly List<string> FocusPatterns = new List<string>();
        public readonly List<string> SkipPatterns = new List<string>();
        public int RandomSeed;
        public bool DebugWait;
        public bool DontRandomizeSpecs;
        public bool Quit;
        public bool ShowTestNames;
        public bool NoFocus;
        public bool NoSummary;
        public bool NoWhitespace;
        public bool NoUnifiedDiff;
        public bool ShowPassExplicitly;
        public int ContextLines = -1;
        public bool SelfTest;
        public TestVerificationMode Verify;

        public TimeSpan? TestTimeout;
        public TimeSpan? PlanTimeout;

        private readonly OptionSet OptionSet;

        public ProgramOptions() {
            this.OptionSet = new OptionSet {
                { "help",          SR.UHelp(),             v => ShowHelp() },
                { "version",       SR.UVersion(),          v => ShowVersion() },

                { "i|fixture=",    SR.UFixture(),          v => FixturePaths.Add(v) },

                { "skip=",         SR.USkip(),             v => SkipPatterns.Add(v) },
                { "focus=",        SR.UFocus(),            v => FocusPatterns.Add(v) },
                { "no-focus",      SR.UNoFocus(),          v => NoFocus = true },

                { "no-whitespace", SR.UNoWhitespace(),     v => NoWhitespace = true },
                { "no-diff",       SR.UNoDiff(),           v => NoUnifiedDiff = true },
                { "context-lines=",SR.UContextLines(),     v => ContextLines = SafeInt32Parse(v, SR.InvalidContextLines(), "--context-lines") },
                { "no-summary",    SR.UNoSummary(),        v => NoSummary = true },
                { "self-test",     SR.USelfTest(),         v => WillSelfTest() },
                { "show-tests",    SR.UShowTestNames(),    v => ShowTestNames = true },

                { "show-pass-explicit", SR.UShowPassExplicit(), v => ShowPassExplicitly = true },
                { "verify=",            SR.UVerify(),           v => Verify = SafeEnumParse<TestVerificationMode>(v, SR.InvalidVerify(), "--verify") },

                { "no-random",     SR.UNoRandomizeSpecs(), v => DontRandomizeSpecs = true },
                { "random-seed=",  SR.URandom(),           v => RandomSeed = SafeInt32Parse(v, SR.InvalidRandomSeed(), "--random-seed") },

                { "timeout=",      SR.UTimeout(),          v => TestTimeout = SafeTimeSpanParse(v, SR.InvalidTimeSpan(), "--timeout") },
                { "plan-timeout=", SR.UPlanTimeout(),      v => PlanTimeout = SafeTimeSpanParse(v, SR.InvalidTimeSpan(), "--plan-timeout") },
                { "pause",         SR.UPause(),            v => DebugWait = true },
            };
        }

        private void WillSelfTest() {
            if (!TestClass.HasSelfTests) {
                _console.WriteLine("fatal: can't self-test; no tests configured in this fspec build");
                Quit = true;
            }

            SelfTest = true;
        }

        public List<string> Parse(string[] args) {
            var myArgs = new List<string>(args.Length);
            foreach (var item in args) {
                myArgs.Add(item);
            }
            return OptionSet.Parse(myArgs);
        }

        public void Usage() {
            _console.WriteLine("Usage:  spec [OPTION]... [ASSEMBLY]...");
            _console.WriteLine("Run tests in the specified assemblies");
            _console.WriteLine();

            var s = new StringWriter();
            OptionSet.WriteOptionDescriptions(s);
            _console.Write(s.ToString());
        }

        private void ShowHelp() {
            Usage();
            Quit = true;
        }

        static TimeSpan SafeTimeSpanParse(string v, string msg, string optionName) {
            TimeSpan result;
            if (ParseUtility.TryParseTimeSpan(v, out result)) {
                return result;
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
            _console.WriteLine("Carbonfrost{1} Spec version {0}", version, registered);
            _console.WriteLine("Copyright {0} {2}{1:yyyy} Carbonfrost Systems, Inc.  All rights reserved.", copy, buildDate, since);
            _console.WriteLine();
            Quit = true;
        }
    }
}
