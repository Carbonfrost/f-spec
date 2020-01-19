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

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    public class TestRunnerOptions {

        private TestRun _testRun;
        private int _randomSeed;
        private Flags _flags;
        private TimeSpan? _testTimeout;
        private TimeSpan? _planTimeout;
        private readonly MakeReadOnlyList<string> _focusPatterns = new MakeReadOnlyList<string>();
        private readonly MakeReadOnlyList<string> _skipPatterns = new MakeReadOnlyList<string>();
        private readonly MakeReadOnlyList<string> _fixtureDirectories = new MakeReadOnlyList<string>();
        private AssertionMessageFormatModes _assertionMessageFormatMode;
        private int _contextLines = -1;
        private TestVerificationMode _verification;

        internal bool IsSelfTest {
            get;
            set;
        }

        public TestVerificationMode Verification {
            get {
                return _verification;
            }
            set {
                WritePreamble();
                _verification = value;
            }
        }

        public int ContextLines {
            get {
                return _contextLines;
            }
            set {
                WritePreamble();
                _contextLines = value;
            }
        }

        public TimeSpan? PlanTimeout {
            get {
                return _planTimeout;
            }
            set {
                WritePreamble();
                _planTimeout = value;
            }
        }

        public AssertionMessageFormatModes AssertionMessageFormatMode {
            get {
                return _assertionMessageFormatMode;
            }
            set {
                WritePreamble();
                _assertionMessageFormatMode = value;
            }
        }

        public TimeSpan? TestTimeout {
            get {
                return _testTimeout;
            }
            set {
                WritePreamble();
                _testTimeout = value;
            }
        }

        public int RandomSeed {
            get {
                return _randomSeed;
            }
            set {
                WritePreamble();
                _randomSeed = value;
            }
        }

        public IList<string> FixtureDirectories {
            get {
                return _fixtureDirectories;
            }
        }

        public TestRun TestRun {
            get {
                if (_testRun == null) {
                    _testRun = new TestRun();
                }
                return _testRun;
            }
            set {
                WritePreamble();
                _testRun = value;
            }
        }

        public IList<string> SkipPatterns {
            get {
                return _skipPatterns;
            }
        }

        public IList<string> FocusPatterns {
            get {
                return _focusPatterns;
            }
        }

        public bool RandomizeSpecs {
            get {
                return (_flags & Flags.RandomizeSpecs) > 0;
            }
            set {
                WritePreamble();
                _flags = value ? (_flags | Flags.RandomizeSpecs) : (_flags & ~Flags.RandomizeSpecs);
            }
        }

        public bool IgnoreFocus {
            get {
                return (_flags & Flags.IgnoreFocus) > 0;
            }
            set {
                WritePreamble();
                _flags = value ? (_flags | Flags.IgnoreFocus) : (_flags & ~Flags.IgnoreFocus);
            }
        }

        public bool ShowPassExplicitly {
            get {
                return (_flags & Flags.ShowPassExplicitly) > 0;
            }
            set {
                WritePreamble();
                _flags = value ? (_flags | Flags.ShowPassExplicitly) : (_flags & ~Flags.ShowPassExplicitly);
            }
        }

        public bool ShowTestNames {
            get {
                return (_flags & Flags.ShowTestNames) > 0;
            }
            set {
                WritePreamble();
                _flags = value ? (_flags | Flags.ShowTestNames) : (_flags & ~Flags.ShowTestNames);
            }
        }

        public bool SuppressSummary {
            get {
                return (_flags & Flags.SuppressSummary) > 0;
            }
            set {
                WritePreamble();
                _flags = value ? (_flags | Flags.SuppressSummary) : (_flags & ~Flags.SuppressSummary);
            }
        }

        public bool IsReadOnly {
            get;
            private set;
        }

        public TestRunnerOptions() {}

        public TestRunnerOptions(TestRunnerOptions copyFrom) {
            if (copyFrom == null) {
                return;
            }

            TestRun = copyFrom.TestRun;
            SkipPatterns.AddAll(copyFrom.SkipPatterns);
            IgnoreFocus = copyFrom.IgnoreFocus;
            FocusPatterns.AddAll(copyFrom.FocusPatterns);
            FixtureDirectories.AddAll(copyFrom.FixtureDirectories);
            RandomSeed = copyFrom.RandomSeed;
            _flags = copyFrom._flags;
            ContextLines = copyFrom.ContextLines;
            Verification = copyFrom.Verification;
            IsSelfTest = copyFrom.IsSelfTest;
        }

        internal TestRunnerOptions Normalize() {
            var result = new TestRunnerOptions(this);
            if (result.RandomSeed <= 0) {
                result.RandomSeed = Environment.TickCount;
            }
            if (result.ContextLines < 0) {
                result.ContextLines = 3; // default
            }
            result.MakeReadOnly();
            return result;
        }

        public void MakeReadOnly() {
            IsReadOnly = true;
            TestRun.Children.MakeReadOnly();
            _focusPatterns.MakeReadOnly();
            _skipPatterns.MakeReadOnly();
            _fixtureDirectories.MakeReadOnly();
        }

        public TestRunnerOptions Clone() {
            return new TestRunnerOptions(this);
        }

        private void WritePreamble() {
            if (IsReadOnly) {
                throw SpecFailure.Sealed();
            }
        }

        [Flags]
        enum Flags {
            RandomizeSpecs,
            IgnoreFocus,
            ShowPassExplicitly,
            ShowTestNames,
            SuppressSummary,
        }
    }
}
