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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.SelfTest.Spec.TestMatchers {

    public class ExpectationTests : TestClass {

        [Fact]
        public void As_should_change_type_twice() {
            Expect(new[] { 0, 16, 32 })
                .As<IEnumerable<int>>()
                .As<IEnumerable>().ToHave.All.GreaterThan(-1);
        }

        [Fact]
        public void Items_should_assert_enumerable() {
            Expect(1, 2).ToHave.Exactly(2).Items();
        }

        [Fact]
        [PassExplicitly]
        public void Expect_Range_Items_should_fail_on_too_few() {
            try {
                Expect(1, 2).ToHave.Exactly(4).Items();

            } catch (AssertException ex) {
                var lines = ex.Message.Split('\n').Select(t => t.Trim()).Take(1);
                var expected = new [] {
                    "Expected exactly 4 items",
                };
                Assert.Equal(expected, lines);
                Assert.ContainsKeyWithValue("ActualCount", "2", ex.TestFailure.UserData);
                Assert.Pass();
            }
        }

        [Fact]
        [PassExplicitly]
        public void Expect_Range_Items_should_fail_on_too_many() {
            try {
                Expect(1, 2, 3).ToHave.Exactly(1).Item();

            } catch (AssertException ex) {
                var lines = ex.Message.Split('\n').Select(t => t.Trim()).Take(1);
                var expected = new [] {
                    "Expected exactly 1 items",
                };
                Assert.Equal(expected, lines);
                Assert.ContainsKeyWithValue("ActualCount", ">2", ex.TestFailure.UserData);
                Assert.Pass();
            }
        }

        [Fact]
        [PassExplicitly]
        public void Expect_Range_NotItems_should_fail() {
            try {
                Expect(1, 2, 3, 4).ToHave.Not.Exactly(4).Items();

            } catch (AssertException ex) {
                var lines = ex.Message.Split('\n').Select(t => t.Trim()).Take(1);
                var expected = new [] {
                    "Expected not exactly 4 items",
                };
                Assert.Equal(expected, lines);
                Assert.Pass();
            }
        }
    }
}
#endif
