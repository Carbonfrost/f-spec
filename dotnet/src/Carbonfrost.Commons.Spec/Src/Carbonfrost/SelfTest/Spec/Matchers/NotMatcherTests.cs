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

    public class NotMatcherTests : TestClass {

        [Fact]
        public void Matches_should_apply_nominal() {
            var subj = new NotMatcher<int>(
                Matchers.Equal(20)
            );
            var val = subj.Matches(TestActual.Value(10));
            Assert.True(val);
        }

        [Fact]
        public void Matches_should_detect_error() {
            var subj = new NotMatcher<int>(
                Matchers.Equal(20)
            );
            var val = subj.Matches(TestActual.Value(20));
            Assert.False(val);
        }

        [Fact]
        public void ExpectTo_should_obtain_matcher() {
            Expect(new [] { 90 }).To(Matchers.Not(Matchers.HaveCount(0)));
            Assert.IsInstanceOf<NotMatcher<int[]>>(Matchers.Not<int[]>(Matchers.HaveCount(0)));
        }

        [Fact]
        public void Expect_Given_fluent_expression() {
            Given("hey").Expect<string>(t => t).To(Matchers.Not(Matchers.BeEmpty()));
        }

        [Fact]
        public void Failure_Name_should_get_expected_notation() {
            var subj = new NotMatcher<int>(
                Matchers.Equal(20)
            );
            Assert.Equal("spec.equal.not", TestMatcherLocalizer.Failure(subj, "").Name.ToString());
        }
    }
}
#endif
