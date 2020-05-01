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
using Carbonfrost.Commons.Spec;
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.SelfTest.Spec.TestMatchers {

    public class OverlapMatcherTests : TestClass {

        [Fact]
        public void Matches_should_detect_overlap_nominal() {
            var subj = new OverlapMatcher<string>(new [] { "a" });
            Assert.True(subj.Matches(new [] { "a", "b" , "c" }));
            Assert.False(subj.Matches(new [] { "d" }));
        }

        [Fact]
        public void Matches_should_detect_overlap_string_comparison() {
            Comparison<char> comparison = (x, y) => x.CompareTo(y);
            var subj = new OverlapMatcher<string>(new [] { "A" }, StringComparer.OrdinalIgnoreCase);
            Assert.True(subj.Matches(new [] { "a", "b" , "c" }));
            Assert.False(subj.Matches(new [] { "d" }));
        }

        [Fact]
        public void ExpectTo_should_obtain_matcher() {
            Comparison<char> comparison = (x, y) => x.CompareTo(y);
            Expect("hello").To(Matchers.Overlap("eLL", comparison));
            Assert.IsInstanceOf<OverlapMatcher<string>>(Matchers.Overlap("ell"));
        }

        [Fact]
        public void Expect_ToHave_should_have_fluent_expression() {
            Expect(new [] { "a", "b", "c" }).ToHave.OverlapWith(new [] { "a" });
        }

        [Fact]
        public void Expect_ToHave_should_have_fluent_expression_negative() {
            Expect(new [] { "a", "b", "c" }).Not.ToHave.OverlapWith(new [] { "z" });
        }

    }
}
#endif
