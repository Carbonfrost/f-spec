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
using System.Linq;
using Carbonfrost.Commons.Spec.ExecutionModel;
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.Commons.Spec {

    [AttributeUsage(AttributeTargets.ReturnValue | AttributeTargets.Property)]
    public sealed class TestFileContentsAttribute : Attribute, ITestMatcherFactory<string> {

        private readonly TestFileInput _input;

        public string PathPattern {
            get {
                return _input.PathPattern;
            }
        }

        public string Message { get; set; }

        public Uri Url {
            get {
                return _input.Url;
            }
        }

        public string Name { get; set; }

        public TestFileContentsAttribute(string pathPattern) {
            _input = new TestFileInput(pathPattern);
        }

        ITestMatcher<string> ITestMatcherFactory<string>.CreateMatcher(TestContext testContext) {
            if (Url != null) {
                if (Url.IsAbsoluteUri && Url.IsFile) {
                    return Matchers.EqualFileContents(Url.LocalPath);
                }
                return Matchers.EqualDownloadContents(Url);
            }

            var filePath = _input.FindMatchingPaths(testContext).Single();
            return Matchers.EqualFileContents(filePath.FileName);
        }

    }
}
