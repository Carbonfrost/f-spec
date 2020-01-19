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
using System.Reflection;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.SelfTest.Spec {

    public class TestFileDataAttributeTests {

        [Theory]
        [InlineData(typeof(TestFile), typeof(TestFile))]
        [InlineData(typeof(TestFile<string>), typeof(TestFile<string>))]
        [InlineData(typeof(string), typeof(TestFile<string>))]
        public void ToTestData_should_convert_TestFile(Type paramType, Type expected) {
            var testFile = new TestFile("");
            var file = new TestFileDataAttribute(".").ToTestData(
                testFile, paramType.GetTypeInfo());
            Assert.IsInstanceOf(expected, file[0]);
        }

        [Skip("Requires staging of test files")]
        [Theory]
        [TestFileData("hello.txt")]
        public void Acceptance_should_load_test_file_string(string s) {
            Assert.Equal("world", s);
        }


        [Skip("Requires staging of test files")]
        [Theory]
        [TestFileData("hello.txt")]
        public void Acceptance_should_load_test_file(TestFile s) {
            Assert.Equal("hello.txt", s.FileName);
        }

        [Explicit("This test is designed to fail.  Assertions happen on the output")]
        [Theory]
        [TestFileData("hello.txt")]
        public void Acceptance_only_one_argument_permitted(TestFile f, object o) {
        }
    }
}
#endif
