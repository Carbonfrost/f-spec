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
using System.Collections.Generic;
using System.Linq;
using Carbonfrost.Commons.Spec;
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.SelfTest.Spec.TestMatchers {

    public class SetEqualMatcherTests : TestClass {

        [Fact]
        public void Matches_should_detect_substrings_nominal() {
            var subj = new SetEqualMatcher<string>(new [] { "a", "b", "c" });
            Assert.True(subj.Matches(new [] { "c", "a", "b" }));
        }

        [Fact]
        public void Matches_should_detect_strings_string_comparison() {
            var subj = new SetEqualMatcher<string>(new [] { "a", "b", "c" }, StringComparer.OrdinalIgnoreCase);
            Assert.True(subj.Matches(new [] { "c", "A", "B" }));
        }

        [Fact]
        public void ExpectTo_should_obtain_matcher() {
            Expect(new [] { "hello", "world" }).To(
                Matchers.BeSetEqualTo(new [] { "hello", "world" }, StringComparer.OrdinalIgnoreCase));
            Assert.IsInstanceOf<SetEqualMatcher<string>>(Matchers.BeSetEqualTo(new [] { "ell" }));
        }

        [Fact]
        public void Assert_SetEqual_should_apply_to_collections() {
            Assert.SetEqual(new [] { 10, 20, 30 }, new List<int> { 10, 20, 30 });
        }

        [Fact]
        public void Assert_NotSetEqual_should_apply_to_collections() {
            Assert.NotSetEqual(new [] { 20, 30 }, new List<int> { 10, 20, 30 });
        }

        [Fact]
        public void Expect_ToBe_should_have_fluent_expression() {
            Expect(new [] { "HELLO" }).ToBe.SetEqualTo(new [] { "HELLO" });
        }

        [Fact]
        public void Expect_ToBe_should_have_fluent_expression_string() {
            Expect(new [] { "HELLO" }).ToBe.SetEqualTo(new [] { "hello" }, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void Expect_Given_fluent_expression() {
            Given().Expect(() => new [] { 420 }).To(Matchers.BeSetEqualTo(new [] { 420 }));
        }

        [Fact]
        public void Expect_Given_fluent_expression_action() {
            Given().Expect(() => { throw new InvalidOperationException(); })
                .To(Matchers.Throw<InvalidOperationException>());

        }
    }
}
#endif
