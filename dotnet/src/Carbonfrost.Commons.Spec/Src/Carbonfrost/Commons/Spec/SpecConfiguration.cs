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
using System.IO;
using System.Text.Json;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    public class SpecConfiguration {

        public TestRunnerState Results {
            get;
            private set;
        }

        public string ResultsJsonFile {
             get{
                return Path.Combine(FspecCacheDirectory, "results.json");
            }
        }

        public string ProjectDirectory {
            get;
        }

        public string FspecCacheDirectory {
            get {
                return Path.Combine(ProjectDirectory, ".fspec");
            }
        }

        public static SpecConfiguration Create(string projectDirectory) {
            return new SpecConfiguration(projectDirectory).Load();
        }

        public static SpecConfiguration Create() {
            return Create(null);
        }

        internal void CopyToOptions(TestRunnerOptions testRunnerOptions) {
            testRunnerOptions.PreviousRun = Results;
        }

        private SpecConfiguration(string projectDirectory) {
            if (projectDirectory == null) {
                ProjectDirectory = Directory.GetCurrentDirectory();
            } else {
                ProjectDirectory = Path.GetFullPath(projectDirectory);
            }
        }

        private SpecConfiguration Load() {
            try {
                if (File.Exists(ResultsJsonFile)) {
                    Results = TestRunnerState.FromFile(ResultsJsonFile);
                }
            } catch (JsonException ex) {
                Console.Error.WriteLine("warning: parsing previous run cache file: " + ex.Message);
            }
            return this;
        }

        internal void Save(TestRunResults result) {
            Results = TestRunnerState.FromResults(result);
            Results.Save(ResultsJsonFile);
        }

    }
}
