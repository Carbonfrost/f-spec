#if SELF_TEST

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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Carbonfrost.Commons.Spec;
using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.SelfTest.Spec.ExecutionModel {

    public class PathCollectionTests {

        [Fact]
        public void GetFullPath_should_provide_rooted_path_when_relative_directory_is_Added() {
            string dotNetDir = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            string[] files = { "fileA", "fileB" };

            var pc = new PathCollection {
                FileSystem = new TestFileSystem(),
             };
             pc.Add("relativeDirectory");

            var path = pc.GetFullPath("fileB");
            Assert.Equal("/working/relativeDirectory/fileB", path);
        }

        [Theory]
        [InlineData("/usr/local/bin:/usr/bin", "l", Name = "Linux")]
        [InlineData("/usr/local/bin;/usr/bin", "w", Name = "Windows")]
        [InlineData("/usr/local/bin\n/usr/bin", "r", Name = "Roundtrip")]
        [InlineData("/usr/local/bin\n/usr/bin", "g", Name = "General (Roundtrip)")]
        public void ParseExact_should_generate_correct_values(string input, string format) {
            var result = PathCollection.ParseExact(input, format);
            Assert.Contains("/usr/local/bin", result);
            Assert.Contains("/usr/bin", result);
        }

        [Fact]
        public void ParseExact_should_parse_empty_string_into_current_directory() {
            var result = PathCollection.ParseExact("/usr/local/bin:/usr/bin::", "l");
            Assert.Contains(Environment.CurrentDirectory, result);
        }

        [Theory]
        [InlineData("l", "/usr:/usr/local")]
        [InlineData("w", "/usr;/usr/local")]
        [InlineData("r", "/usr\n/usr/local")]
        [InlineData("g", "/usr\n/usr/local")]
        public void ToString_should_generate_correct_format(string format, string expected) {
            var result = new PathCollection("/usr", "/usr/local");
            Assert.Equal(expected, result.ToString(format));
        }

         class TestFileSystem : FileSystem {

            public override bool IsDirectory(string path) {
                return path == "relativeDirectory" || path == "/working/relativeDirectory";
            }

            public override IEnumerable<string> DirectoryEnumerateFiles(string path) {
                if (path == "relativeDirectory" || path == "/working/relativeDirectory") {
                    return new[] { path + "/fileA", path + "/fileB" };
                }
                return Array.Empty<string>();
            }

            public override IEnumerable<string> DirectoryEnumerateFiles(string item, string searchPattern, SearchOption searchOption) {
                return DirectoryEnumerateFiles(item);
            }

            public override bool Exists(string path) {
                return true;
            }

            public override string WorkingDirectory {
                get {
                    return "/working";
                }
            }
        }
    }
}
#endif
