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

    public class HaveSingleMatcherTests : TestClass {

        [Fact]
        public void Matches_should_detect_count_nominal() {
            var subj = new HaveSingleMatcher();
            Assert.True(subj.Matches(new[] { "1", }));
        }

        [Fact]
        public void Matches_should_detect_non_matching_count_nominal() {
            var subj = new HaveSingleMatcher();
            Assert.False(subj.Matches(new[] { "1", "2", }));
        }

        [Fact]
        public void ExpectTo_should_obtain_matcher() {
            Assert.IsInstanceOf<HaveSingleMatcher>(Matchers.HaveSingle());
        }

        [Fact]
        public void Expect_ToHave_fluent_expression() {
            Expect(new int[1]).ToHave.Single();
        }

        [Fact]
        public void Expect_using_params_ToHave_Single_predicate() {
            Expect(new [] { "a", "b", "c" }).ToHave.Single((string t) => t == "a");
            Expect("a", "b", "c").ToHave.Single((string t) => t == "a");
        }

        [Fact]
        public void Expect_using_params_ToHave_Single_predicate_inner_item() {
            Expect(new [] { "a", "b", "c" }).ToHave.Single((string t) => t == "b");
            Expect("a", "b", "c").ToHave.Single((string t) => t == "b");
        }

        [Fact]
        public void Expect_ToHave_Single_item() {
            Expect(new [] { "z" }).ToHave.Single.Item();
        }

        [Fact]
        public void Single_with_predicate_will_detect_single_item() {
            Assert.Single(r => r == "b", new [] { "a", "b", "a" });
        }
    }
}
#endif
