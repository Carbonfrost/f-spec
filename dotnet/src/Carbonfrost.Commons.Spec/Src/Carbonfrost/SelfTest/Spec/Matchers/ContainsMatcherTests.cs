#if SELF_TEST

//
// Copyright 2018 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

    public class ContainsMatcherTests : TestClass {

        [Fact]
        public void Matches_should_detect_suffix_nominal() {
            var subj = new ContainsMatcher<string>("C", StringComparer.OrdinalIgnoreCase);
            Assert.True(subj.Matches(new [] { "a", "b", "c" }));
        }

        [Fact]
        public void Matches_should_detect_suffix_failure() {
            var subj = new ContainsMatcher<string>("z");
            Assert.False(subj.Matches(new [] { "a", "b", "c" }));
        }

        [Fact]
        public void ExpectTo_Have_Element_fluent_expression() {
            Expect(new [] { "a", "b", "c" }).ToHave.Element("a");
            Expect(new [] { "a", "b", "c" }).Not.ToHave.Element("z");
        }

        [Fact]
        public void ExpectTo_Matchers_fluent_expression() {
            Expect(new [] { "a", "b", "c" }).To(Matchers.Contain("a"));
            Expect(new [] { "a", "b", "c" }).NotTo(Matchers.Contain("z"));
        }
    }
}
#endif
