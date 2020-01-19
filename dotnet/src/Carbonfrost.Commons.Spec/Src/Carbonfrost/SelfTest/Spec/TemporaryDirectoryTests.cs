#if SELF_TEST

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
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.SelfTest.Spec {

    public class TemporaryDirectoryTests : TestClass {

        [Fact]
        public void CreateFile_should_specify_name() {
            var file = CreateTempDirectory().CreateFile("hello.txt");
            Assert.EndsWith("hello.txt", file.FileName);
        }

        [Fact]
        public void CreateFile_should_read_and_write() {
            var file = CreateTempDirectory().CreateFile("hello");
            file.WriteAllText("world");

            Assert.Equal("world", file.TextContents);
        }

        [Fact]
        public void Disposing_should_dispose_files() {
            TestTemporaryFile file;
            using (var dir = CreateTempDirectory()) {
                file = dir.CreateFile("hello");
            }

            Assert.True(file.IsDisposed);
        }
    }
}
#endif
