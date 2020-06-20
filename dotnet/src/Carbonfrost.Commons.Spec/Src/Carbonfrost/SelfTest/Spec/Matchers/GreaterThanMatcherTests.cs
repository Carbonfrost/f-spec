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
using System.Collections.Generic;
using System.Linq;
using Carbonfrost.Commons.Spec;
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.SelfTest.Spec.TestMatchers {

    public class GreaterThanMatcherTests : TestClass {

        [Fact]
        public void ExpectTo_should_obtain_matcher() {
            Expect("hello").To(Matchers.BeGreaterThan("eLL", StringComparer.OrdinalIgnoreCase));
            Assert.IsInstanceOf<GreaterThanMatcher<string>>(Matchers.BeGreaterThan("ell"));
        }

        [Fact]
        public void Expect_ToBe_should_have_fluent_expression() {
            Expect("hello").ToBe.GreaterThan("eLL", StringComparer.OrdinalIgnoreCase);
            Assert.IsInstanceOf<GreaterThanMatcher<string>>(Matchers.BeGreaterThan("ell"));
        }

        [Fact]
        public void Expect_ToBe_should_have_fluent_expression_negative() {
            Expect("Aura").Not.ToBe.GreaterThan("Bye", StringComparer.OrdinalIgnoreCase);
        }

        [Fact]
        public void Expect_ToBe_should_support_type_via_object() {
            Expect((object) "Aura").Not.ToBe.GreaterThan("Bye");
        }

        [Fact]
        [PassExplicitly]
        public void Expect_ToBe_should_throw_on_uncomparable_types() {
            try {
                Expect(new object()).ToBe.GreaterThan("Bye");

            } catch (AssertException e) {
                // e.TestFailure
                Assert.StartsWith("Unable to compare", e.Message);
                Assert.Pass();
            }
        }

        [Fact]
        [PassExplicitly]
        public void Expect_ToBe_should_have_test_failure_comparer() {
            try {
                Expect("S").ToBe.GreaterThan("Bye", new FComparerThatThrows());

            } catch (AssertException e) {
                Assert.Equal("Unable to compare values using the specified comparer", e.TestFailure.Message);
                Assert.Pass();
            }
        }

        [Fact]
        public void Expect_ToBe_Approximately_should_have_fluent_expression() {
            // 5.02 > 4.7 + 0.3
            Expect(5.02).ToBe.Approximately(0.3).GreaterThan(4.7);
        }

        private class FComparerThatThrows : IComparer<string> {
            public int Compare(string x, string y) {
                throw new InvalidCastException();
            }
        }
    }
}
#endif
