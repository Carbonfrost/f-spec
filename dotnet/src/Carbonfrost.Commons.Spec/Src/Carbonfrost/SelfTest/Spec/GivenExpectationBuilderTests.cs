#if SELF_TEST

//
// Copyright 2017 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

    public class GivenExpectationBuilderTests : TestClass {

        [Fact]
        public void Given_thunk_should_implement_expectation() {
            Given("hello", "world").Expect((x, y) => x + y)
                .NotTo(Matchers.BeNull());
            Given("hello", "world").Expect((x, y) => x + y)
                .Not.ToBe.Null();
        }

        [Fact]
        public void Given_thunk_should_implement_enumerable_expectation() {
            Given("hello", "world").Expect((x, y) => x + y)
                .To(Matchers.ContainSubstring("lowo"));
            Given("hello", "world").Expect((x, y) => x + y)
                .ToHave.Substring("lowo");
        }

        [Fact]
        [PassExplicitly]
        public void Given_message_should_contain_arguments() {
            try {
                Given("hello", "world").Expect((x, y) => x + y)
                    .To(Matchers.Equal("hello~~world"));
            } catch (AssertException ex) {
                Expect(ex.TestFailure.UserData["Given"]).ToBe.EqualTo("hello, world");
                Assert.Pass();
            }
        }

        [Fact]
        [PassExplicitly]
        public void Given_message_should_contain_arguments_value() {
            try {
                Given("hello", "world").Expect((x, y) => x + y)
                    .ToBe.EqualTo("goodbye, earth!");

            } catch (AssertException ex) {
                Expect(ex.TestFailure.UserData["Given"]).ToBe.EqualTo("hello, world");
                Assert.Pass();
            }
        }

        [Fact]
        [PassExplicitly]
        public void Given_message_should_contain_arguments_enumerable() {
            try {
                Given("hello", "world").Expect((x, y) => x + y)
                    .ToHave.Count(600);

                Assert.Fail("Assertion should have failed");

            } catch (AssertException ex) {
                Expect(ex.TestFailure.UserData["Given"]).ToBe.EqualTo("hello, world");
                Assert.Pass();
            }
        }

        [Fact]
        [PassExplicitly]
        public void Given_message_should_contain_arguments_satisfaction() {
            try {
                Given("hello", "world").Expect((x, y) => x + y)
                    .ToSatisfy.All(Matchers.BeEmpty());

                Assert.Fail("Assertion should have failed");

            } catch (AssertException ex) {
                Expect(ex.TestFailure.UserData["Given"]).ToBe.EqualTo("hello, world");
                Assert.Pass();
            }
        }
    }
}
#endif
