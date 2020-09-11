#if SELF_TEST

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
using Carbonfrost.Commons.Spec;
using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.SelfTest.Spec.ExecutionModel {

    public class TestIdTests {

        [Fact]
        public void Operator_Equals_will_test_value_equality() {
            var left = TestId.Parse("9121e62543b5881d5ff2e04a9dd6ae231c55da3122a98dc4379177617955af10");
            var right = TestId.Parse("9121e62543b5881d5ff2e04a9dd6ae231c55da3122a98dc4379177617955af10");
            Assert.True(left == right);
        }

        [Fact]
        public void JsonUtility_LoadJson_should_parse_String() {
            Assert.Equal(
                TestId.Parse("1234"),
                JsonUtility.LoadJson<TestId>("\"1234\"")
            );
        }

        [Fact]
        public void JsonUtility_ToJson_should_generate_String() {
            Assert.Equal(
                "\"1234\"",
                JsonUtility.ToJson(TestId.Parse("1234"))
            );
        }

        [Fact]
        public void FromTestName_will_encode_bytes() {
            var name = new TestName(
                "assembly",
                "ns",
                "class",
                "subjectClassBinding",
                "method",
                1,
                "dataName",
                new [] { "arg1", "arg2" }
            );
            string expected = "9121e62543b5881d5ff2e04a9dd6ae231c55da3122a98dc4379177617955af10";
            Assert.Equal(
                expected,
                TestId.FromTestName(name).ToString()
            );
        }
    }
}

#endif
