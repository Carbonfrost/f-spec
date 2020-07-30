//
// Copyright 2018 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Linq;
using System.Text.RegularExpressions;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    class TestFileInput {

        public string PathPattern {
            get;
            private set;
        }

        public Uri Url {
            get;
            private set;
        }

        internal GlobTemplate Pattern {
            get;
            private set;
        }

        public TestFileInput(string pathPattern) {
            if (pathPattern == null) {
                throw new ArgumentNullException(nameof(pathPattern));
            }
            PathPattern = pathPattern.Trim();

            // Patterns must not look like URLs
            if (!Regex.IsMatch(pathPattern, "^(data|http|https):")) {
                Pattern = GlobTemplate.Parse(pathPattern);
            }
            if (Pattern == null || Pattern.Variables.Count == 0) {
                // If the URI looks like a file, treat as one
                if (pathPattern.StartsWith("/", StringComparison.Ordinal)) {
                    Url = new Uri("file://" + pathPattern);
                } else {
                    Url = new Uri(pathPattern, UriKind.RelativeOrAbsolute);
                }
            }
        }

        public IEnumerable<GlobTemplateMatch> FindMatchingPaths(TestContext context) {
            if (Pattern == null) {
                return Array.Empty<GlobTemplateMatch>();
            }

            var searchDirectories = new List<string>();
            searchDirectories.Add(".");
            searchDirectories.AddAll(context.TestRunnerOptions.FixturePaths);

            return Pattern.EnumerateFiles(searchDirectories);
        }

        public IEnumerable<T> ReadInputs<T>(TestContext context,
                                            Func<Uri, T> urlReader,
                                            Func<GlobTemplateMatch, T> fileReader) {
            if (Url != null) {
                return new [] { urlReader(Url) };
            }

            return FindMatchingPaths(context).Select(fileReader);
        }
    }

}
