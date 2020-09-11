//
// Copyright 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    public class TestRunnerState {

        public static readonly TestRunnerState Empty = ReadOnly(new TestRunnerState(default));
        private readonly JTestRunnerState _state;

        private TestRunnerState(JTestRunnerState state) {
            _state = state;
        }

        public TestRunnerState() {
        }

        public TestRunFailureReason FailureReason {
            get {
                return _state.FailureReason;
            }
        }

        public TestRunnerOptions Options {
            get {
                return _state.Options;
            }
        }

        internal static string DefaultDirectory {
            get {
                return Path.Combine(Directory.GetCurrentDirectory(), ".fspec");
            }
        }

        internal static string DefaultFile {
            get {
                return Path.Combine(DefaultDirectory, "results.json");
            }
        }

        public bool IsReadOnly {
            get;
            private set;
        }

        public void ApplyTo(TestRun run) {
            if (_state.Results == null) {
                return;
            }

            var previously = new Dictionary<TestId, TestStatus>();
            foreach (var s in _state.Results) {
                if (s.Id is TestId id) {
                    previously[id] = s.Status;
                }
            }
            foreach (var u in run.DescendantsAndSelf.OfType<TestCaseInfo>()) {
                if (previously.TryGetValue(u.Id, out var state)) {
                    u.Tags.Add(TestTag.Previously(state));
                }
            }
        }

        public void Save() {
            Save(DefaultFile);
        }

        public void Save(string file) {
            if (string.IsNullOrEmpty(file)) {
                throw SpecFailure.EmptyString(nameof(file));
            }
            Directory.CreateDirectory(Path.GetDirectoryName(file));
            File.WriteAllText(file, JsonUtility.ToJson(_state));
        }

        public static TestRunnerState FromFile(string file) {
            if (string.IsNullOrEmpty(file)) {
                throw SpecFailure.EmptyString(nameof(file));
            }
            return new TestRunnerState(JsonUtility.LoadJson<JTestRunnerState>(File.ReadAllText(file)));
        }

        public static TestRunnerState FromResults(TestRunResults results) {
            if (results is null) {
                throw new ArgumentNullException(nameof(results));
            }
            return new TestRunnerState(new JTestRunnerState {
                Results = results.Descendants.Select(r => r.JResult).ToList(),
                FailureReason = results.FailureReason,
                Options = results.RunnerOptions,
            });
        }

        internal static TestRunnerState ReadOnly(TestRunnerState state) {
            state.MakeReadOnly();
            return state;
        }

        private void MakeReadOnly() {
            IsReadOnly = true;
        }
    }
}
