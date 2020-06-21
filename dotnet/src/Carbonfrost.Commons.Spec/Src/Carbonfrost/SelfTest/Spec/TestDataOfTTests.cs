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

namespace Carbonfrost.SelfTest.Spec {

    public class TestDataOfTTests {

        [Fact]
        public void FCreate_should_create_focused_test_data() {
            Assert.True(TestData.FCreate<string>("arg").Focus().IsFocused);
        }

        [Fact]
        public void XCreate_should_create_pending_test_data() {
            Assert.True(TestData.XCreate<string>("arg").Pending().IsPending);
        }

        [Fact]
        public void FTestData_conversion_should_create_focused_test_data() {
            Assert.True(((TestData) new FTestData<string>("arg")).IsFocused);
        }

        [Fact]
        public void FTestData_conversion_empty_should_create_focused_test_data() {
            Assert.True(((TestData) new FTestData<string>()).IsFocused);
        }

        [Fact]
        public void XTestData_conversion_should_create_focused_test_data() {
            Assert.True(((TestData) new XTestData<string>("arg")).IsPending);
        }

        [Fact]
        public void XTestData_conversion_empty_should_create_focused_test_data() {
            Assert.True(((TestData) new XTestData<string>()).IsPending);
        }

        [Fact]
        public void TestData_use_cases() {
            // Simply set up a value
            var simple = new object[] {
                new TestData<string>("hello"),
                TestData<string>.Create("hello"),
                TestData.Create<string>("hello"),
            };

            // Prepend X or F to change pending/focus
            var pending = new object[] {
                new XTestData<string>("hello"),
                XTestData<string>.Create("hello"),
                XTestData.Create<string>("hello"),
                TestData.XCreate<string>("hello")
            };

            var focus = new object[] {
                new FTestData<string>("hello"),
                FTestData<string>.Create("hello"),
                FTestData.Create<string>("hello"),
                TestData.FCreate<string>("hello")
            };
        }

    }
}
#endif
