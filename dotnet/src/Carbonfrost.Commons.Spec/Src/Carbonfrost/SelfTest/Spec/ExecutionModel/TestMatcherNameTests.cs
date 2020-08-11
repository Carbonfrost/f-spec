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

using Carbonfrost.Commons.Spec;
using Carbonfrost.Commons.Spec.TestMatchers;
using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.SelfTest.Spec.ExecutionModel {

    public class TestMatcherNameTests {

        [Theory]
        [InlineData(typeof(EmptyMatcher), "spec.empty")]
        [InlineData(typeof(InstanceOfMatcher), "spec.instanceOf")]
        [InlineData(typeof(BetweenMatcher<int>), "spec.between")]
        public void FromType_gets_type_names(Type type, string expected) {
            Assert.Equal(
                expected, TestMatcherName.FromType(type).Name
            );
        }

        [Fact]
        public void For_unwinds_support_test_matcher() {
            var wrapped = TestMatcher.UnitWrapper(new ThrowsMatcher());
            Assert.Equal(
                "spec.throws", TestMatcherName.For(wrapped).Name
            );
        }

        [Fact]
        public void For_unwinds_negation_support_test_matcher() {
            var wrapped = Matchers.Not(Matchers.BeEmpty());
            Assert.Equal(
                "spec.empty.not", TestMatcherName.For(wrapped).Name
            );
        }

        [Fact]
        public void For_supports_the_And_and_Or_matchers() {
            Assert.Equal(
                "spec.and",
                TestMatcherName.For(Matchers.And(Matchers.BeEmpty(), Matchers.BeEmpty())).Name
            );

            Assert.Equal(
                "spec.or",
                TestMatcherName.For(Matchers.Or(Matchers.BeEmpty(), Matchers.BeEmpty())).Name
            );
        }

        [Fact]
        public void For_supports_the_invariant_matchers() {
            Assert.Equal(
                "spec.nothing",
                TestMatcherName.For(TestMatcher.Nothing).Name
            );

            Assert.Equal(
                "spec.anything",
                TestMatcherName.For(TestMatcher.Anything).Name
            );
        }
    }
}
#endif
