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
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.SelfTest.Spec.TestMatchers {

    public class HaveCountMatcherTests : TestClass {

        [Fact]
        public void Matches_should_detect_count_nominal() {
            var subj = new HaveCountMatcher(1);
            Assert.True(subj.Matches(new[] { "1", }));
        }

        [Fact]
        public void Matches_should_detect_non_matching_count_nominal() {
            var subj = new HaveCountMatcher(1);
            Assert.False(subj.Matches(new[] { "1", "2", }));
        }

        [Fact]
        public void ExpectTo_should_obtain_matcher() {
            Assert.IsInstanceOf<HaveCountMatcher>(Matchers.HaveCount(2));
        }

        [Fact]
        public void Expect_ToHave_fluent_expression() {
            Expect(new int[2]).ToHave.Count(2);
        }

        [Fact]
        public void Expect_using_params_ToHave_Count_predicate() {
            Expect(new [] { "a", "b", "c" }).ToHave.Count(1, (string t) => t == "a");
            Expect("a", "b", "c").ToHave.Count(1, (string t) => t == "a");
        }
    }
}
#endif
