#if SELF_TEST

//
// Copyright 2018, 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using Carbonfrost.Commons.Spec;
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.SelfTest.Spec.TestMatchers {

    public class InstanceOfMatcherTests : TestClass {

        [Fact]
        public void Matches_should_detect_string_nominal() {
            var subj = new InstanceOfMatcher(typeof(string));
            Assert.True(subj.Matches("ab"));
        }

        [Fact]
        [PassExplicitly]
        public void Matches_should_fail_on_null() {
            var subj = new InstanceOfMatcher(typeof(string));
            try {
                subj.Matches((string) null);

                Assert.Fail("Expected to fail with AssertException -- can't match null");
            } catch (AssertVerificationException) {
                Assert.Pass();
            }
        }

        [Fact]
        [PassExplicitly]
        public void Matches_should_fail_on_null_thunk() {
            var subj = new InstanceOfMatcher(typeof(string));
            try {
                subj.Matches(TestActual.Value((string) null));

                Assert.Fail("Expected to fail with AssertException -- can't match null");
            } catch (AssertVerificationException) {
                Assert.Pass();
            }
        }

        [Fact]
        public void Matches_will_disable_validation_errors_inside_SatisfyAnyMatcher() {
            var matcher = Matchers.SatisfyAny(
                new InstanceOfMatcher(typeof(string)),
                new NullMatcher()
            );

            // Normally, BeInstanceOf() when the argument is null throws an exception, but since
            // it is in a list of other matchers, this validation is disabled
            Assert.DoesNotThrow(() => matcher.Matches(null));
        }

        [Fact]
        public void Via_Assert_should_match() {
            Assert.IsInstanceOf(typeof(string), "tt");
            Assert.IsInstanceOf(typeof(int[]), new [] { 1, 2 });
        }

        [Fact]
        public void Via_Assert_should_match_negated() {
            Assert.IsNotInstanceOf(typeof(int), "tt");
            Assert.IsNotInstanceOf(typeof(int), new [] { 1, 2 });
        }

        [Fact]
        public void ExpectTo_should_obtain_matcher() {
            Expect("tt").To(Matchers.BeInstanceOf(typeof(string)));
            Assert.IsInstanceOf<InstanceOfMatcher>(Matchers.BeInstanceOf(typeof(object)));

            Expect(new [] { 1, 2 }).To(Matchers.BeInstanceOf(typeof(int[])));
            Assert.IsInstanceOf<InstanceOfMatcher>(Matchers.BeInstanceOf(typeof(object)));
        }

        [Fact]
        public void Expect_ToBe_should_obtain_matcher() {
            Expect("tt").ToBe.InstanceOf(typeof(string));
        }

        [Fact]
        public void Expect_Given_fluent_expression() {
            Given().Expect(() => "hello").To(Matchers.BeInstanceOf(typeof(string)));
        }
        [Fact]
        public void Expect_ToBe_InstanceOf_operand() {
            Expect("").ToBe.InstanceOf<string>();
        }
    }
}
#endif
