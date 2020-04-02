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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    public class TestRunnerOptions {

        private int _randomSeed;
        private Flags _flags;
        private TimeSpan? _testTimeout;
        private TimeSpan? _planTimeout;
        private readonly MakeReadOnlyList<string> _focusPatterns = new MakeReadOnlyList<string>();
        private readonly MakeReadOnlyList<string> _skipPatterns = new MakeReadOnlyList<string>();
        private AssertionMessageFormatModes _assertionMessageFormatMode;
        private int _contextLines = -1;
        private readonly PathCollection _fixturePaths = new PathCollection();
        private readonly MakeReadOnlyList<PackageReference> _packageReferences = new MakeReadOnlyList<PackageReference>();
        private readonly AssemblyLoader _loader = new AssemblyLoader();
        private readonly PathCollection _loaderPaths = new PathCollection();

        internal bool IsSelfTest {
            get {
                return (_flags & Flags.SelfTest) > 0;
            }
            set {
                WritePreamble();
                SetFlag(value, Flags.SelfTest);
            }
        }

        public bool FailFast {
            get {
                return (_flags & Flags.FailFast) > 0;
            }
            set {
                WritePreamble();
                SetFlag(value, Flags.FailFast);
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

        public PathCollection FixturePaths {
            get {
                return _fixturePaths;
            }
        }

        public PathCollection LoaderPaths {
            get {
                return _loaderPaths;
            }
        }

        public Func<string, Assembly> LoadAssemblyFromPath {
            get {
                return _loader.LoadAssemblyFromPath;
            }
            set {
                WritePreamble();
                _loader.LoadAssemblyFromPath = value;
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
                SetFlag(value, Flags.RandomizeSpecs);
            }
        }

        public bool IgnoreFocus {
            get {
                return (_flags & Flags.IgnoreFocus) > 0;
            }
            set {
                WritePreamble();
                SetFlag(value, Flags.IgnoreFocus);
            }
        }

        public bool ShowPassExplicitly {
            get {
                return (_flags & Flags.ShowPassExplicitly) > 0;
            }
            set {
                WritePreamble();
                SetFlag(value, Flags.ShowPassExplicitly);
            }
        }

        public bool ShowTestNames {
            get {
                return (_flags & Flags.ShowTestNames) > 0;
            }
            set {
                WritePreamble();
                SetFlag(value, Flags.ShowTestNames);
            }
        }

        public bool SuppressSummary {
            get {
                return (_flags & Flags.SuppressSummary) > 0;
            }
            set {
                WritePreamble();
                SetFlag(value, Flags.SuppressSummary);
            }
        }

        public bool IsReadOnly {
            get;
            private set;
        }

        public IList<PackageReference> PackageReferences {
            get {
                return _packageReferences;
            }
        }

        public TestRunnerOptions() : this(null) {
        }

        public TestRunnerOptions(TestRunnerOptions copyFrom) {
            if (copyFrom == null) {
                return;
            }

            SkipPatterns.AddAll(copyFrom.SkipPatterns);
            IgnoreFocus = copyFrom.IgnoreFocus;
            FocusPatterns.AddAll(copyFrom.FocusPatterns);
            FixturePaths.AddAll(copyFrom.FixturePaths);
            LoaderPaths.AddAll(copyFrom.LoaderPaths);
            LoadAssemblyFromPath = copyFrom.LoadAssemblyFromPath;
            RandomSeed = copyFrom.RandomSeed;
            _flags = copyFrom._flags;
            ContextLines = copyFrom.ContextLines;
            IsSelfTest = copyFrom.IsSelfTest;
            PackageReferences.AddAll(copyFrom.PackageReferences);
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
            _focusPatterns.MakeReadOnly();
            _skipPatterns.MakeReadOnly();
            _fixturePaths.MakeReadOnly();
            _loaderPaths.MakeReadOnly();
            _packageReferences.MakeReadOnly();
        }

        public TestRunnerOptions Clone() {
            return new TestRunnerOptions(this);
        }

        internal IEnumerable<Assembly> LoadAssembliesAndBindLoader() {
            var list = new List<Assembly>();
            list.AddRange(_loader.LoadAssemblies(LoaderPaths));

            _loader.RegisterAssemblyResolve(
                list.Select(t => Path.GetDirectoryName(new Uri(t.CodeBase).LocalPath)).Distinct()
            );

            list.AddRange(
                _loader.LoadAssemblies(PackageReferences.Select(pkg => pkg.ResolveAssembly()))
            );
            return list;
        }

        private void WritePreamble() {
            if (IsReadOnly) {
                throw SpecFailure.Sealed();
            }
        }

        private void SetFlag(bool value, Flags flag) {
            _flags = value ? (_flags | flag) : (_flags & ~flag);
        }

        [Flags]
        enum Flags {
            RandomizeSpecs = 1 << 0,
            IgnoreFocus = 1 << 1,
            ShowPassExplicitly = 1 << 2,
            ShowTestNames = 1 << 3,
            SuppressSummary = 1 << 4,
            FailFast = 1 << 5,
            SelfTest = 1 << 6,
        }
    }
}
