#if SELF_TEST

//
// Copyright 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
using System;
using System.Collections.Generic;
using Carbonfrost.Commons.Spec;
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.SelfTest.Spec.TestMatchers {

    public class SequenceEqualMatcherTests : TestClass {

        [Fact]
        public void Matches_should_detect_substrings_nominal() {
            var subj = new SequenceEqualMatcher<string>(new [] { "a", "b", "c" });
            Assert.True(subj.Matches(new [] { "a", "b", "c" }));
        }

        [Fact]
        public void Matches_should_detect_strings_string_comparison() {
            var subj = new SequenceEqualMatcher<string>(new [] { "a", "b", "c" }, StringComparer.OrdinalIgnoreCase);
            Assert.True(subj.Matches(new [] { "A", "B", "c" }));
        }

        [Fact]
        public void ExpectTo_should_obtain_matcher() {
            Expect(new [] { "hello", "world" }).To(
                Matchers.BeSequenceEqualTo(new [] { "hello", "world" }, StringComparer.OrdinalIgnoreCase));
            Assert.IsInstanceOf<SequenceEqualMatcher<string>>(Matchers.BeSequenceEqualTo(new [] { "ell" }));
        }

        [Fact]
        public void Assert_SequenceEqual_should_apply_to_collections() {
            Assert.SequenceEqual(new [] { 10, 20, 30 }, new List<int> { 10, 20, 30 });
        }

        [Fact]
        public void Assert_NotSequenceEqual_should_apply_to_collections() {
            Assert.NotSequenceEqual(new [] { 10, 20, 30 }, new List<int> { 10, 20 });
        }

        [Fact]
        public void Assert_SequenceEqual_nominal() {
            Assert.SequenceEqual(new [] { 1, 2, 3 }, new [] { 1, 2, 3 });
        }

        [Fact]
        public void Assert_SequenceEqual_converse() {
            var ex = Record.Exception(() => Assert.SequenceEqual(new [] { 1, 2 }, new [] { 3 }));
            Assert.NotNull(ex);

            ex = Record.Exception(() => Assert.SequenceEqual(new int[] { }, new [] { 3, 2, 1 }));
            Assert.NotNull(ex);

            ex = Record.Exception(() => Assert.SequenceEqual(new [] { 1, 2, 3, 5 }, new [] { 1, 2 }));
            Assert.NotNull(ex);
        }

        [Fact]
        public void Expect_ToBe_should_have_fluent_expression() {
            Expect(new [] { "HELLO" }).ToBe.SequenceEqualTo(new [] { "HELLO" });
        }

        [Fact]
        public void Expect_ToBe_should_have_fluent_expression_string() {
            Expect(new [] { "HELLO" }).ToBe.SequenceEqualTo(new [] { "hello" }, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void Expect_Given_fluent_expression() {
            Given().Expect(() => new [] { 420 }).To(Matchers.BeSequenceEqualTo(new [] { 420 }));
        }

        [Fact]
        public void Expect_Given_fluent_expression_action() {
            Given().Expect(() => { throw new InvalidOperationException(); })
                .To(Matchers.Throw<InvalidOperationException>());

        }
    }
}
#endif
