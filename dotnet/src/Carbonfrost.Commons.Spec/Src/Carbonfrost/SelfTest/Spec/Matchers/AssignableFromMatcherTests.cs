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

    public class AssignableFromMatcherTests : TestClass {

        class C {}
        class C1 : C {}

        [Fact]
        public void Matches_should_detect_subtypes_nominal() {
            var subj = new AssignableFromMatcher(typeof(C));
            Assert.True(subj.Matches(typeof(C1)));
        }

        [Fact]
        public void Via_Assert_should_match() {
            Assert.AssignableFrom(typeof(C), typeof(C1));
        }

        [Fact]
        public void ExpectTo_should_obtain_matcher() {
            Expect(typeof(C1)).To(Matchers.BeAssignableFrom(typeof(C)));
            Assert.IsInstanceOf<AssignableFromMatcher>(Matchers.BeAssignableFrom(typeof(object)));
        }

        [Fact]
        public void Expect_ToBe_should_have_fluent_expression() {
            Expect(typeof(C1)).ToBe.AssignableFrom(typeof(C));
        }

        [Fact]
        public void Expect_Given_fluent_expression() {
            Given().Expect(() => typeof(C1)).To(Matchers.BeAssignableFrom(typeof(C)));
        }
    }
}
#endif
