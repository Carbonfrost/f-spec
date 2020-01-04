#if SELF_TEST

//
// Copyright 2017, 2018 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

    public class EnumerableExpectationTests : TestClass {

        [Fact]
        public void Expect_ToHave_All_should_apply_matcher() {
            Expect(new [] { 0, 16, 32, }).ToHave.All.GreaterThan(-1);
        }

        [Fact]
        public void Expect_ToHave_All_via_cast_should_apply_matcher() {
            // All is untyped
            Expect(new [] { "", "", }).ToHave.All.Empty();
        }

        [Fact]
        public void Expect_ToHave_All_Not_expression_should_apply_matcher() {
            Expect(new [] { "a", "b", }).ToHave.All.Not.Empty();
        }

        [Fact]
        public void Expect_NotToHave_All_should_apply_matcher() {
            Expect(new [] { 0, 16, 32, }).Not.ToHave.All.GreaterThan(10);
        }

        [Fact]
        [PassExplicitly]
        public void Expect_ToHave_All_should_fail_with_correct_message() {
            try {
                Expect(new [] { 1, 0, 16, 32, }).ToHave.All.GreaterThan(0);

            } catch (AssertException ex) {
                var lines = ex.Message.Split('\n').Select(t => t.Trim()).Take(2);
                Assert.Equal(new [] {
                    "Expected all elements to:",
                    "- be > 0",
                }, lines);
                Assert.Pass();
            }
        }

        [Fact]
        [PassExplicitly]
        public void Expect_ToHave_All_should_fail_with_correct_message_multi() {
            try {
                Expect(new [] { 1, 0, 16, 32, -1, -2, -3, -4 }).ToHave.All.GreaterThan(0);

            } catch (AssertException ex) {
                Assert.Equal("1, 4, 5, 6, ...", ex.TestFailure.UserData["Indexes"]);
                Assert.Pass();
            }
        }

        [Fact]
        [PassExplicitly]
        public void Expect_NotToHave_All_should_fail_with_correct_message() {
            try {
                Expect(new [] { 1, 0, 16, 32, }).Not.ToHave.All.GreaterThan(-1);

            } catch (AssertException ex) {
                var lines = ex.Message.Split('\n').Select(t => t.Trim()).Take(2);
                Assert.Equal(new [] {
                    "Expected not all elements to:",
                    "- be > -1",
                }, lines);
                Assert.Pass();
            }
        }

        [Fact]
        public void Expect_ToHave_Any_should_apply_matcher() {
            Expect(new[] {
                       0,
                       16,
                       32,
                   }).ToHave.Any.EqualTo(32);
        }

        [Fact]
        public void Expect_NotToHave_Any_should_apply_matcher() {
            Expect(new[] {
                       0,
                       16,
                       32,
                   }).Not.ToHave.Any.EqualTo(60);
        }

        [Fact]
        [PassExplicitly]
        public void Expect_ToHave_Any_should_fail_with_correct_message() {
            try {
                Expect(new[] {
                           0,
                           60,
                           16,
                           32,
                       }).ToHave.Any.EqualTo(30);
            }
            catch (AssertException ex) {
                var lines = ex.Message.Split('\n').Select(t => t.Trim()).Take(2);
                Assert.Equal(new [] {
                    "Expected any element to:",
                    "- be equal to 30",
                }, lines);
                Assert.Pass();
            }
        }

        [Fact]
        [PassExplicitly]
        public void Expect_NotToHave_Any_should_fail_with_correct_message() {
            try {
                Expect(new[] {
                           0,
                           60,
                           16,
                           32,
                       }).Not.ToHave.Any.EqualTo(60);
            }
            catch (AssertException ex) {
                Assert.Equal("spec.notAny", ex.TestFailure.Name);
                Assert.Equal("1", ex.TestFailure.UserData["Indexes"]);

                var lines = ex.Message.Split('\n').Select(t => t.Trim()).Take(2);
                Assert.Equal(new [] {
                    "Expected not any element to:",
                    "- be equal to 60",
                }, lines);
                Assert.Pass();
            }
        }

        [Fact]
        [PassExplicitly]
        public void Expect_NotToHave_Any_should_fail_with_correct_message_multi() {
            try {
                Expect(new[] {
                           0,
                           60,
                           60,
                           16,
                           32,
                       }).Not.ToHave.Any.EqualTo(60);
            }
            catch (AssertException ex) {
                Assert.Equal("1, 2", ex.TestFailure.UserData["Indexes"]);
                Assert.Pass();
            }
        }

        [Fact]
        public void Self_should_apply_expectations_to_value() {
            Expect(new [] { 0 }).ToHave.Self.Not.Null();
        }

        [Fact]
        public void Any_All_extension_syntax() {
            Expect(0, 1, 2).ToHave.Any<int>().GreaterThan(1);
            Expect(0, 1, 2).ToHave.All<int>().GreaterThan(-1);
        }

        [Fact]
        public void Expect_Single_should_pass() {
            Expect(0, 1, 2).ToHave.Single.EqualTo(2);
        }

        [Fact]
        public void Expect_Single_should_fail() {
            try {
                Expect(0, 1, 0).ToHave.Single.EqualTo(0);

            } catch (AssertException ex) {
                var lines = ex.Message.Split('\n').Select(t => t.Trim()).Take(2);
                var expected = new [] {
                    "Expected exactly 1 to:",
                    "- be equal to 0",
                };
                Assert.Equal(expected, lines);
            }
        }

        [Fact]
        [PassExplicitly]
        public void Expect_Range_should_fail() {
            try {
                Expect(0, 1, 0).ToHave.Between(5, 8).EqualTo(0);

            } catch (AssertException ex) {
                var lines = ex.Message.Split('\n').Select(t => t.Trim()).Take(2);
                var expected = new [] {
                    "Expected between 5 and 8 to:",
                    "- be equal to 0",
                };
                Assert.Equal(expected, lines);
                Assert.Pass();
            }
        }

        [Fact]
        [PassExplicitly]
        public void Expect_Range_should_fail_on_too_few() {
            try {
                Expect(0, 1, 0).ToHave.Between(10, 20).GreaterThan(-1);

            } catch (AssertException ex) {
                var lines = ex.Message.Split('\n').Select(t => t.Trim()).Take(2);
                var expected = new [] {
                    "Expected between 10 and 20 to:",
                    "- be > -1",
                };
                Assert.Equal(expected, lines);
                Assert.ContainsKeyWithValue("ActualCount", "3", ex.TestFailure.UserData);
                Assert.Pass();
            }
        }

        [Fact]
        public void Expect_None_should_pass() {
            Expect(0, 1, 0).ToHave.None.EqualTo(20);
            Expect(0, 0, 0).ToHave.None.Not.EqualTo(0);
        }

        [Fact]
        public void Expect_No_Items_should_pass() {
            Expect().ToHave.No.Items();
        }
    }
}
#endif
