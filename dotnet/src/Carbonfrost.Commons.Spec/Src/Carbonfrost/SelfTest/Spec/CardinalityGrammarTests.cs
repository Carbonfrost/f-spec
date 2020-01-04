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
using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.SelfTest.Spec {

    public class CardinalityGrammarTests : VerificationTestBase {

        [Fact]
        [PassExplicitly]
        public void Cardinality_should_throw_if_min_and_max() {
            try {
                Expect("").ToHave.Between(5, 2).GreaterThan("a");

            } catch (AssertVerificationException ex) {
                Expect(ex.Message).ToBe.EqualTo("Make sure that Between() specifies min < max");
                Assert.Pass();
            }
        }

        [Fact]
        [PassExplicitly]
        public void Cardinality_should_throw_if_min_is_negative() {
            try {
                Expect("").ToHave.Between(-1, 2).GreaterThan("a");

            } catch (AssertVerificationException ex) {
                Expect(ex.Message).ToBe.EqualTo("Make sure that AtLeast or min value is non-negative");
                Assert.Pass();
            }
        }
    }
}
#endif
