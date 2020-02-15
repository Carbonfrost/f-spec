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
using System.Collections.Generic;
using System.Linq;
using Carbonfrost.Commons.Spec;
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.SelfTest.Spec.TestMatchers {

    public class EqualMatcherTests : TestClass {

        [Fact]
        public void Matches_should_detect_substrings_nominal() {
            var subj = new EqualMatcher<string>("a");
            Assert.True(subj.Matches("a"));
        }

        [Fact]
        public void Matches_should_detect_strings_string_comparison() {
            var subj = new EqualMatcher<string>("A", StringComparer.OrdinalIgnoreCase);
            Assert.True(subj.Matches("a"));
        }

        [Fact]
        public void Matches_should_report_string_comparer() {
            var subj = new EqualMatcher<string>("A", StringComparer.OrdinalIgnoreCase);
            var failure = TestMatcherLocalizer.Failure(subj, "A");
            Assert.ContainsKeyWithValue("Comparer", "ordinal (ignore case)", failure.UserData);
        }

        [Fact]
        public void Matches_should_report_fuzzy_comparer() {
            var subj = new EqualMatcher<double>(1.0).OrClose(0.2);
            var failure = TestMatcherLocalizer.Failure(subj, 0.0);
            Assert.ContainsKeyWithValue("Comparer", "close by 0.2", failure.UserData);
        }

        [Fact]
        public void Matches_should_apply_approximation() {
            var subj = new EqualMatcher<double>(5.0).OrClose(0.03);
            Assert.True(subj.Matches(5.02));
        }

        [Fact]
        public void ExpectTo_should_obtain_matcher() {
            Expect("hello").To(Matchers.Equal("heLLo", StringComparer.OrdinalIgnoreCase));
            Assert.IsInstanceOf<EqualMatcher<string>>(Matchers.Equal("ell"));
        }

        [Fact]
        public void OrClose_should_return_different_instance() {
            var matcher = Matchers.Equal(5.0);
            Assert.NotSame(matcher, matcher.OrClose(0.03));
            Assert.IsInstanceOf<EqualMatcher<double>>(matcher);
        }

        [Fact]
        public void ExpectTo_OrClose_should_obtain_matcher_approximation() {
            Expect(5.02).To(Matchers.Equal(5.0).OrClose(0.03));
        }

        [Fact]
        public void ExpectTo_OrClose_should_obtain_matcher_approximation_timespans() {
            Expect(DateTime.Now).To(
                Matchers.Equal(DateTime.Now).OrClose(TimeSpan.FromMilliseconds(500)));
            Assert.IsInstanceOf<EqualMatcher<string>>(Matchers.Equal("ell"));
        }

        [Fact]
        public void Expect_ToBe_Approximately_should_have_fluent_expression() {
            Expect(5.02).ToBe.Approximately(0.3).EqualTo(5);
        }

        [Fact]
        public void Expect_ToBe_Approximately_should_have_fluent_expression_timespan() {
            Expect(DateTime.Now).ToBe.Approximately(TimeSpan.FromMilliseconds(500))
                .EqualTo(DateTime.Now);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData(420, 420)]
        public void AssertEqual_nullable_should_work(int? a, int? b) {
            Assert.Equal(a, b);
        }

        [Fact]
        public void Assert_Equal_should_apply_to_collections() {
            Assert.Equal(new [] { 10, 20, 30 }, new List<int> { 10, 20, 30 });
        }

        [Fact]
        public void Assert_Equal_should_apply_to_values() {
            Assert.Equal(235, 235);
        }

        [Fact]
        public void Assert_NotEqual_should_apply_to_collections() {
            Assert.NotEqual(new [] { 20, 30 }, new List<int> { 10, 20, 30 });
        }

        [Fact]
        public void Assert_NotEqual_should_apply_to_values() {
            Assert.NotEqual(143, 69);
        }

        [Fact]
        public void Expect_ToBe_should_have_fluent_expression() {
            Expect("HELLO").ToBe.EqualTo("HELLO");
        }

        [Fact]
        public void Expect_ToBe_should_have_fluent_expression_list() {
            Expect(new [] { "HELLO", "WORLD" }).ToBe.EqualTo(new [] { "HELLO", "WORLD" });
        }

        [Fact]
        public void Expect_Given_fluent_expression() {
            Given().Expect(() => 420).To(Matchers.Equal(420));
        }

        [Fact]
        public void TestFailure_from_short_string_should_not_produce_diff() {
            var subj = new EqualMatcher<string>("and");
            var failure = TestMatcherLocalizer.Failure(subj, "bool");

            Assert.Null(failure.UserData.Diff);
        }

        [Fact]
        public void TestFailure_from_long_string_should_produce_diff() {
            var subj = new EqualMatcher<string>(@"and
bool
count
double
epsilon");
            var failure = TestMatcherLocalizer.Failure(subj, @"and
bool
c
d
e");
            Assert.NotNull(failure.UserData.Diff);
            var diff = failure.UserData.Diff.ToString().Split(new [] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            Assert.Equal(new [] { "@@@ -1,6 +1,6",
                             " and",
                             " bool",
                             "-count",
                             "-double",
                             "-epsilon",
                             "+c",
                             "+d",
                             "+e" }, diff);

        }
    }
}
#endif
