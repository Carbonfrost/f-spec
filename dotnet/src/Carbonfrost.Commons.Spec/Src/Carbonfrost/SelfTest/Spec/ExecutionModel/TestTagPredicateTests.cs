#if SELF_TEST

//
// Copyright 2017, 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Carbonfrost.Commons.Spec;
using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.SelfTest.Spec.ExecutionModel {

    public class TestTagPredicateTests : TestClass {

        public IEnumerable<MethodInfo> NameFactoryMethods {
            get {
                return typeof(TestTag).GetMethods().Where(IsNameFactoryMethod);
            }
        }

        [Fact]
        public void Parse_will_generate_negated_tag() {
            var pp = TestTagPredicate.Parse("~ego");
            Assert.IsInstanceOf<TestTagPredicate.NegatedImpl>(pp);
        }

        [Theory]
        [InlineData("hello")]
        [InlineData("~hello")]
        [InlineData("~hello:platform")]
        public void Parse_will_generate_roundtrip_string(string text) {
            var pp = TestTagPredicate.Parse(text);
            Assert.Equal(text, pp.ToString());
        }

        [Fact]
        public void Parse_will_generate_Anything() {
            Assert.Equal(TestTagPredicate.Anything, TestTagPredicate.Parse("*"));
        }

        [Fact]
        public void Parse_will_generate_Nothing() {
            Assert.Equal(TestTagPredicate.Nothing, TestTagPredicate.Parse("~"));
        }

        [Theory]
        [PropertyData(nameof(NameFactoryMethods))]
        public void Should_have_a_predicate_matching_every_TestTag_factory_method(MethodInfo mi) {
            var method = typeof(TestTagPredicate).GetTypeInfo().GetMethods().Where(m => m.Name == mi.Name);
            Expect(method).Not.ToBe.Empty();
        }

        [Fact]
        public void Invariant_Equals_should_match_by_value() {
            Assert.True(TestTagPredicate.Anything == TestTagPredicate.Parse("*"));
            Assert.True(TestTagPredicate.Nothing == TestTagPredicate.Parse("~"));
        }

        [Fact]
        public void Invariant_GetHashCode_should_match_by_value() {
            Given(TestTagPredicate.Anything).Expect(t => t.GetHashCode())
                .ToBe.EqualTo(TestTagPredicate.Invariant(true).GetHashCode());
            Given(TestTagPredicate.Nothing).Expect(t => t.GetHashCode())
                .ToBe.EqualTo(TestTagPredicate.Invariant(false).GetHashCode());
        }

        [Fact]
        public void And_Equals_should_match_by_value() {
            var and1 = TestTagPredicate.And(
                TestTagPredicate.Platform("a"), TestTagPredicate.Platform("b"), TestTagPredicate.Platform("c")
            );
            var and2 = TestTagPredicate.And(
                TestTagPredicate.Platform("a"), TestTagPredicate.Platform("b"), TestTagPredicate.Platform("c")
            );
            Assert.True(and1.Equals(and2));
        }

        [Fact]
        public void JsonUtility_LoadJson_should_parse_String() {
            Assert.Equal(
                TestTagPredicate.Platform("windows"),
                JsonUtility.LoadJson<TestTagPredicate>("\"platform:windows\"")
            );
        }

        [Fact]
        public void JsonUtility_ToJson_should_generate_String() {
            Assert.Equal(
                "\"platform:windows\"",
                JsonUtility.ToJson(TestTagPredicate.Platform("windows"))
            );
        }

        static bool IsNameFactoryMethod(MethodInfo m) {
            if (!m.IsStatic) {
                return false;
            }
            if (m.ReturnType != typeof(TestTag)) {
                return false;
            }
            if (m.Name == "Parse") {
                return false;
            }
            if (m.Name.StartsWith("op_")) {
                return false;
            }

            var pms = m.GetParameters();
            if (pms.Length != 1) {
                return false;
            }
            if (pms[0].ParameterType == typeof(string)) {
                return true;
            }
            return true;
        }
    }
}
#endif
