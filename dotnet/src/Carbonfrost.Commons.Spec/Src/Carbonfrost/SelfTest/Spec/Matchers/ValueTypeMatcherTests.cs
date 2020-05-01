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

namespace Carbonfrost.SelfTest.Spec.TestMatchers {

    public class ValueTypeMatcherTests : TestClass {

        [Fact]
        public void Matches_should_detect_nominal() {
            var subj = new ValueTypeMatcher();
            Assert.True(subj.Matches(0xab));
        }

        [Fact]
        public void Matches_should_fail_on_null() {
            var subj = new ValueTypeMatcher();
            Assert.False(subj.Matches((string) null));
        }

        [Fact]
        public void Via_Assert_should_match() {
            Assert.IsValueType(230);
            Assert.IsValueType(ValueTuple.Create(1, 2, 3));
        }

        [Fact]
        public void Via_Assert_should_match_negated() {
            Assert.IsNotValueType("tt");
            Assert.IsNotValueType(new [] { 1, 2 });
        }

        [Fact]
        public void Expect_ToBe_should_obtain_matcher() {
            Expect(222).ToBe.ValueType();
        }

        [Fact]
        public void ExpectTo_should_obtain_matcher() {
            Expect(100).To(Matchers.BeValueType());
            Assert.IsInstanceOf<ValueTypeMatcher>(Matchers.BeValueType());
        }

        [Fact]
        public void Expect_Given_fluent_expression() {
            Given().Expect(() => 100).To(Matchers.BeValueType());
        }
        [Fact]
        public void Expect_ToBe_ValueType_operand() {
            Expect(222).ToBe.ValueType();
        }
    }
}
#endif
