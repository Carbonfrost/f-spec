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

    public class AssumeTests : TestClass {

        [Fact]
        [PassExplicitly]
        public void Assume_does_not_throw_on_error() {
            try {
                Assume.True(false, "Assumption failed");
            } catch (AssertException) {
                Assert.Fail("Not expected to have an assertion failure on Assume");
            }

            Assert.Pass();
        }

        [Fact]
        [PassExplicitly]
        public void Assume_Expect_does_not_throw_on_expectation() {
            try {
                Assume.Expect(true).ToBe.False();
            } catch (AssertException) {
                Assert.Fail("Not expected to have an assertion failure on Assume");
            }

            Assert.Pass();
        }

    }
}
#endif
