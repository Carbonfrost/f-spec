#if SELF_TEST

//
// Copyright 2018, 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

    public class InlineDataAttributeTests {

        public string State;

        [Explicit("This test is designed to fail.  Assertions happen on the output")]
        [Theory]
        [InlineData("wrong", "number", "of", "arguments")]
        [CLSCompliant(false)]
        public void Acceptance_mismatch_arguments(string onlyOneArgument) {
        }

        [Fact]
        public void TestData_can_initialize_from_null() {
            var inline = new InlineDataAttribute(null, null);
            Assert.Equal(new object[] { null, null }, inline.Data);
            Assert.Equal(new[] {
                new TestData(new object[] { null, null })
            }, ((ITestDataProvider) inline).GetData(null));
        }

        [Fact]
        public void Reason_propagates_to_test_data() {
            var inline = new InlineDataAttribute("a") { Reason = "r" };
            Assert.Equal("r", ((ITestDataProvider) inline).GetData(null).First().Reason);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("2")]
        public void Create_new_instance_on_test_cases(string name) {
            Assert.Null(State);

            // Detect shared state by setting something
            State = name;
        }
    }
}
#endif
