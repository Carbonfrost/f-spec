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

    public class LessThanMatcherTests : TestClass {

        [Fact]
        public void ExpectTo_should_obtain_matcher() {
            Expect("hello").To(Matchers.BeLessThan("Zoo", StringComparer.OrdinalIgnoreCase));
            Assert.IsInstanceOf<LessThanMatcher<string>>(Matchers.BeLessThan("ell"));
        }

        [Fact]
        public void Expect_ToHave_should_have_fluent_expression() {
            Expect("hello").ToBe.LessThan("zoo", StringComparer.OrdinalIgnoreCase);
            Assert.IsInstanceOf<LessThanMatcher<string>>(Matchers.BeLessThan("ell"));
        }

        [Fact]
        public void Expect_ToHave_should_have_fluent_expression_negative() {
            Expect("Planet").Not.ToBe.LessThan("Bye", StringComparer.OrdinalIgnoreCase);
        }
    }
}
#endif
