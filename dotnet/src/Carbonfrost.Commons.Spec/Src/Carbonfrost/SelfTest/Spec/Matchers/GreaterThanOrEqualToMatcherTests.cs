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

    public class GreaterThanOrEqualToMatcherTests : TestClass {

        [Fact]
        public void ExpectTo_should_obtain_matcher() {
            Expect("hello").To(Matchers.BeGreaterThanOrEqualTo("eLL", StringComparer.OrdinalIgnoreCase));
            Assert.IsInstanceOf<GreaterThanOrEqualToMatcher<string>>(Matchers.BeGreaterThanOrEqualTo("ell"));
        }

        [Fact]
        public void Expect_ToHave_should_have_fluent_expression() {
            Expect("hello").ToBe.GreaterThanOrEqualTo("eLL", StringComparer.OrdinalIgnoreCase);
            Assert.IsInstanceOf<GreaterThanOrEqualToMatcher<string>>(Matchers.BeGreaterThanOrEqualTo("ell"));
        }

        [Fact]
        public void Expect_ToHave_should_have_fluent_expression_negative() {
            Expect("Aura").Not.ToBe.GreaterThanOrEqualTo("Bye", StringComparer.OrdinalIgnoreCase);
        }

        [Fact]
        public void Expect_ToBe_Approximately_should_have_fluent_expression() {
            Expect(5.02).ToBe.Approximately(0.3).GreaterThanOrEqualTo(5);
        }
    }
}
#endif
