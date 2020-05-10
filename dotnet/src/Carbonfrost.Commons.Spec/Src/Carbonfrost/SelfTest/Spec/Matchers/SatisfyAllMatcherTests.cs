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
using System.Linq;
using System.Reflection;
using Carbonfrost.Commons.Spec;
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.SelfTest.Spec.TestMatchers {

    public class SatisfyAllMatcherTests : TestClass {

        [Fact]
        public void Matches_should_apply_all_nominal() {
            var subj = new SatisfyAllMatcher<int[]>(
                Matchers.BeEmpty(),
                Matchers.HaveCount(0)
            );
            var val = subj.Matches(TestActual.Value(new int[0]));
            Assert.True(val);
        }

        [Fact]
        public void Matches_should_detect_empty_nominal_func() {
            var subj = new SatisfyAllMatcher<int[]>(
                Matchers.BeEmpty(),
                Matchers.HaveCount(1)
            );
            var val = subj.Matches(TestActual.Value(new int[0]));
            Assert.False(val);
        }

        [Fact]
        public void ExpectTo_should_obtain_matcher() {
            Expect(new int[0]).To(Matchers.SatisfyAll(Matchers.HaveCount(0), Matchers.BeEmpty()));
            Assert.IsInstanceOf<SatisfyAllMatcher<int[]>>(Matchers.SatisfyAll<int[]>());
        }

        [Fact]
        public void Expect_ToSatisfy_all_should_obtain_matcher() {
            Expect(new int[0]).ToSatisfy.All(Matchers.HaveCount(0), Matchers.BeEmpty());
        }

        [Fact]
        public void Expect_ToSatisfy_all_should_obtain_matcher_action() {
            Expect(() => { throw new InvalidOperationException(); }).ToSatisfy.All(
                Matchers.Throw());
        }

        [Fact]
        public void Expect_Given_fluent_expression() {
            Given("").Expect<string>(t => t).To(Matchers.SatisfyAll(Matchers.BeEmpty()));
        }

        [Fact]
        public void Expect_Given_fluent_expression_action() {
            // N.B.: Notice the use of TestMatcher.Empty so that overload
            // resolution works to pick ITestMatcher instead of ITestMatcher<object>
            Given().Expect(() => { throw new InvalidOperationException(); })
                .To(Matchers.SatisfyAll(TestMatcher.Anything, Matchers.Throw<InvalidOperationException>()));

        }

        [Fact]
        public void Localizer_should_produce_correct_failure_nested_messages() {
            var subj = new SatisfyAllMatcher<double>(new EqualMatcher<double>(1.0));
            var failure = TestMatcherLocalizer.Failure(subj, 0.0);
            var exception = failure.ToException();

            var lines = exception.Message.Split('\n').Select(t => t.Trim()).Take(2);
            var expected = new [] {
                "Expected to satisfy all:",
                "- be equal to 1",
            };
            Assert.Equal(expected, lines);
        }

        [Fact]
        public void Localizer_should_exclude_matchers_list() {
            var subj = new SatisfyAllMatcher<double>(new EqualMatcher<double>(1.0));
            var failure = TestMatcherLocalizer.Failure(subj, 0.0);
            Assert.DoesNotContain("Matchers", failure.UserData.Keys);
        }
    }
}
#endif
