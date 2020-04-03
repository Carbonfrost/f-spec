#if SELF_TEST

//
// Copyright 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

using Carbonfrost.Commons.Spec;
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.SelfTest.Spec.TestMatchers {

    public class DistinctMatcherTests : TestClass {

        [Fact]
        public void Matches_should_detect_distinct_nominal() {
            var subj = new DistinctMatcher<string>();
            Assert.True(subj.Matches(new [] { "c", "a", "b" }));
        }

        [Fact]
        public void Matches_should_detect_strings_string_comparison() {
            var subj = new DistinctMatcher<string>(StringComparer.OrdinalIgnoreCase);
            Assert.True(subj.Matches(new [] { "c", "A", "B" }));
        }

        [Fact]
        public void ExpectTo_should_obtain_matcher() {
            Expect(new [] { "hello", "world" }).To(
                Matchers.BeDistinct(StringComparer.OrdinalIgnoreCase));
            Assert.IsInstanceOf<DistinctMatcher<string>>(Matchers.BeDistinct<string>());
            Assert.IsInstanceOf<DistinctMatcher<string>>(Matchers.BeDistinct(StringComparer.OrdinalIgnoreCase));
        }

        [Fact]
        public void Assert_Distinct_should_apply_to_collections() {
            Assert.Distinct(new [] { 10, 20, 30 });
            Assert.Distinct(new List<int> { 10, 20, 30 });
        }

        [Fact]
        public void Assert_NotDistinct_should_apply_to_collections() {
            Assert.NotDistinct(new [] { 10, 10, 30 });
            Assert.NotDistinct(new List<int> { 10, 10, 30 });
        }

        [Fact]
        public void Expect_ToBe_should_have_fluent_expression() {
            Expect(new [] { "HELLO" }).ToBe.Distinct();
        }

        [Fact]
        public void Expect_ToBe_should_have_fluent_expression_string() {
            Expect(new [] { "HELLO" }).ToBe.Distinct(StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void Expect_Given_fluent_expression() {
            // TODO It shouldn't be necessary to specify type arguments
            Given().Expect(() => new [] { 420 }).To(Matchers.BeDistinct<int>());
        }
    }
}
#endif
