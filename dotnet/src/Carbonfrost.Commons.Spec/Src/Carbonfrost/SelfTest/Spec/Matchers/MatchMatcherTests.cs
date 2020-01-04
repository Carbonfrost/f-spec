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
using System.Linq;
using System.Text.RegularExpressions;
using Carbonfrost.Commons.Spec;
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.SelfTest.Spec.TestMatchers {

    public class MatchMatcherTests : TestClass {

        [Fact]
        public void Matches_should_detect_substrings_nominal() {
            var subj = new MatchMatcher(new Regex("^a+"));
            Assert.True(subj.Matches("aaaa"));
        }

        [Fact]
        public void Matches_should_detect_contra() {
            var subj = new MatchMatcher(new Regex("^a+"));
            Assert.False(subj.Matches("b"));
        }

        [Fact]
        public void ExpectTo_should_obtain_matcher() {
            Expect("hello").To(Matchers.Match("heLLo", RegexOptions.IgnoreCase));
            Assert.IsInstanceOf<MatchMatcher>(Matchers.Match("ell"));
        }

        [Fact]
        public void Expect_ToBe_should_have_fluent_expression() {
            Expect("now").ToBe.Match("now");
        }

        [Fact]
        public void Expect_ToHave_should_have_fluent_expression() {
            Expect(new [] { "now", "here", "this" }).ToHave.All<string>().Match("\\w+");
        }

        [Fact]
        public void Expect_Given_fluent_expression() {
            Given().Expect(() => "420").To(Matchers.Match("\\d+"));
        }
    }
}
#endif
