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
using Carbonfrost.Commons.Spec;
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.SelfTest.Spec.TestMatchers {

    public class BetweenMatcherTests : TestClass {

        [Fact]
        public void Matches_should_detect_strings_nominal() {
            var subj = new BetweenMatcher<string>("a", "z");
            Assert.True(subj.Matches("a"));
        }

        [Fact]
        public void Exclusive_Matches_should_detect_nominal() {
            var subj = new BetweenMatcher<string>("a", "z");
            Assert.True(subj.Exclusive.Matches("b"));
            Assert.False(subj.Exclusive.Matches("a"));
        }

        [Theory]
        [InlineData("a")]
        [InlineData("B")]
        public void Matches_should_detect_strings_string_comparison(string s) {
            var subj = new BetweenMatcher<string>("A", "x", StringComparer.OrdinalIgnoreCase);
            Assert.True(subj.Matches(s));
        }

        [Fact]
        public void Matches_should_report_string_comparer() {
            var subj = new BetweenMatcher<string>("A", "z", StringComparer.OrdinalIgnoreCase);
            var failure = TestMatcherLocalizer.Failure(subj, "A");
            Assert.ContainsKeyWithValue("Comparer", "ordinal (ignore case)", failure.UserData);
        }

        [Fact]
        public void Matches_should_report_fuzzy_comparer() {
            var subj = new BetweenMatcher<double>(1.0, 2.0).OrClose(0.2);
            var failure = TestMatcherLocalizer.Failure(subj, 0.0);
            Assert.ContainsKeyWithValue("Comparer", "close by 0.2", failure.UserData);
        }

        [Fact]
        public void Matches_should_apply_approximation() {
            var subj = new BetweenMatcher<double>(5.0, 5.5).OrClose(0.03);
            Assert.True(subj.Matches(4.99));
        }

        [Fact]
        public void Matchers_returns_instance_of_correct_type() {
            Assert.IsInstanceOf<BetweenMatcher<string>>(Matchers.BeBetween("ell", "f"));
        }

        [Fact]
        public void OrClose_should_return_different_instance() {
            var matcher = Matchers.BeBetween(5.0, 6.0);
            Assert.NotSame(matcher, matcher.OrClose(0.03));
            Assert.IsInstanceOf<BetweenMatcher<double>>(matcher);
        }

        [Fact]
        public void ExpectTo_OrClose_should_obtain_matcher_approximation() {
            Expect(4.99).To(Matchers.BeBetween(5.0, 6.0).OrClose(0.03));
        }

        [Fact]
        public void Expect_ToBe_Approximately_should_have_fluent_expression() {
            Expect(5.02).ToBe.Approximately(0.3).Between(5, 6);
        }

        [Fact]
        public void Assert_Between_should_apply_to_values() {
            Assert.Between(200, 300, 235);
        }

        [Fact]
        public void Assert_NotBetween_should_apply_to_values() {
            Assert.NotBetween(100, 200, 69);
        }

        [Fact]
        public void Expect_ToBe_should_have_fluent_expression() {
            Expect("HELLO").ToBe.Between("HELLO", "HF");
        }

        [Fact]
        public void Expect_Given_fluent_expression() {
            Given().Expect(() => 420).To(Matchers.BeBetween(0, 600));
        }
    }
}
#endif
