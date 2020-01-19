#if SELF_TEST

//
// Copyright 2016-2018 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.SelfTest.Spec.TestMatchers {

    public class ContainsSubstringMatcherTests : TestClass {

        [Fact]
        public void Matches_should_detect_substrings_nominal() {
            var subj = new ContainsSubstringMatcher("a");
            Assert.True(subj.Matches("abc"));
        }

        [Fact]
        public void Matches_should_detect_substrings_string_comparison() {
            var subj = new ContainsSubstringMatcher("A", StringComparison.OrdinalIgnoreCase);
            Assert.True(subj.Matches("abc"));
        }

        [Fact]
        public void IgnoringCase_should_return_clone_and_update_comparison() {
            var subj = new ContainsSubstringMatcher("a");
            Assert.NotSame(subj, subj.IgnoringCase);
            Assert.Equal(StringComparison.OrdinalIgnoreCase, subj.IgnoringCase.Comparison);
        }

        [Fact]
        public void UsingCurrentCulture_should_return_clone_and_update_comparison() {
            var subj = new ContainsSubstringMatcher("a");
            Assert.NotSame(subj, subj.UsingCurrentCulture);
            Assert.Equal(StringComparison.CurrentCulture, subj.UsingCurrentCulture.Comparison);
        }

        [Fact]
        public void ExpectTo_should_obtain_matcher() {
            Expect("hello").To(Matchers.ContainSubstring("eLL", StringComparison.OrdinalIgnoreCase));
            Assert.IsInstanceOf<ContainsSubstringMatcher>(Matchers.ContainSubstring("ell"));
        }

        [Fact]
        public void Expect_ToHave_should_have_fluent_expression() {
            Expect("hello").ToHave.Substring("eLL", StringComparison.OrdinalIgnoreCase);
            Assert.IsInstanceOf<ContainsSubstringMatcher>(Matchers.ContainSubstring("ell"));
        }

        [Fact]
        public void Expect_ToHave_should_have_fluent_expression_negative() {
            Expect("hello").Not.ToHave.Substring("Bye", StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void Expect_ToHave_should_have_fluent_expression_extra_typing() {
            Expect("hello").ToHave.Substring("eLL", StringComparison.OrdinalIgnoreCase);
            Assert.IsInstanceOf<ContainsSubstringMatcher>(Matchers.ContainSubstring("ell"));
        }
    }
}
#endif
