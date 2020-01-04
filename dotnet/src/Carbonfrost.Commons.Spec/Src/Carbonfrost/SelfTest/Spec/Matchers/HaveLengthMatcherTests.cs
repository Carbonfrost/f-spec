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
using System.Collections.Generic;
using System.Linq;
using Carbonfrost.Commons.Spec;
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.SelfTest.Spec.TestMatchers {

    public class HaveLengthMatcherTests : TestClass {

        [Fact]
        public void Matches_should_detect_array_length_nominal() {
            var subj = new HaveLengthMatcher(2);
            Assert.True(subj.Matches(new [] { 1, 2, }));
        }

        [Fact]
        public void Matches_should_detect_string_length_nominal() {
            var subj = new HaveLengthMatcher(2);
            Assert.True(subj.Matches("ab"));
        }

        [Fact]
        public void Via_Assert_should_match() {
            Assert.HasLength(2, "tt");
            Assert.HasLength(2, new [] { 1, 2 });
        }

        [Fact]
        public void Via_Assert_should_match_negated() {
            Assert.DoesNotHaveLength(0, "tt");
            Assert.DoesNotHaveLength(0, new [] { 1, 2 });
        }

        [Fact]
        public void ExpectTo_should_obtain_matcher() {
            Expect("tt").To(Matchers.HaveLength(2));
            Assert.IsInstanceOf<HaveLengthMatcher>(Matchers.HaveLength(2));

            Expect(new [] { 1, 2 }).To(Matchers.HaveLength(2));
            Assert.IsInstanceOf<HaveLengthMatcher>(Matchers.HaveLength(2));
        }

        [Fact]
        public void Expect_Given_fluent_expression() {
            Given().Expect(() => "hello").To(Matchers.HaveLength(5));
        }

        [Fact]
        public void Expect_ToHave_should_have_fluent_expression_string() {
            Expect("hello").ToHave.Length(5);
        }

        [Fact]
        public void Expect_ToBe_should_have_fluent_expression_string() {
            Expect("hello").ToBe.Length(5);
        }

        [Fact]
        public void Expect_ToHave_should_have_fluent_expression_Array() {
            Expect(new [] { 1, 2, 3, 4, 5}).ToHave.Length(5);
        }

        [Fact]
        public void Expect_ToBe_should_have_fluent_expression_Array() {
            Expect(new [] { 1, 2, 3, 4, 5}).ToBe.Length(5);
        }

        [Fact]
        public void Expect_should_throw_error_on_unsupported_type() {
            try {
                // You have to use Count - not Length - to assert this
                Expect(new List<int>()).ToHave.Length(0);

            } catch (AssertException e) {
                Assert.StartsWith("Can't assert length on type `System.Collections.Generic.List`1[System.Int32]'", e.Message);
            }
        }

        [Fact]
        public void Expect_via_enumerable_erasure_should_have_comprehensible_message() {
            // see comments in EmptyMatcher for this test case
            try {
                Expect(new [] { 4 }).ToHave.All.Length(3);

            } catch (AssertException e) {
                Assert.StartsWith("Invalid cast required by `spec.haveLength'.  This conversion may have been implicit", e.Message);
            }
        }
    }
}
#endif
