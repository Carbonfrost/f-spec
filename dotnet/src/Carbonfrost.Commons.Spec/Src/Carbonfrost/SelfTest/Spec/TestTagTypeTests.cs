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
using Carbonfrost.Commons.Spec;
using System.Collections.Generic;
using System.Linq;

namespace Carbonfrost.SelfTest.Spec {

    public class TestTagTypeTests : TestClass {

        [Fact]
        public void IsAutomatic_applies_to_previously() {
            Assert.True(TestTagType.Parse("previously").Automatic);
        }

        public IEnumerable<string> ExpectedOperators {
            get {
                return new [] {
                    "op_Equality",
                    "op_Inequality",
                };
            }
        }

        public IEnumerable<string> Names {
            get {
                return new [] {
                    "Previously",
                    "Platform"
                };
            }
        }

        public TestTagType[] Values {
            get {
                return new [] {
                    TestTagType.Previously,
                    TestTagType.Platform,
                };
            }
        }

        [Theory]
        [PropertyData(nameof(ExpectedOperators))]
        public void Equals_operators_should_be_present(string name) {
            Assert.NotNull(
                typeof(TestTagType).GetMethod(name)
            );
        }

        [Theory]
        [PropertyData(nameof(Names))]
        public void Parse_should_roundtrip_on_values(string name) {
            var expected = typeof(TestTagType).GetField(name).GetValue(null);
            Assert.Equal(
                expected,
                TestTagType.Parse(name)
            );
        }

        [Theory]
        [InlineData("Previously")]
        [InlineData("previously")]
        public void Constructor_should_get_canonical_name(string name) {
            Assert.Equal(
                "previously",
                new TestTagType(name).ToString()
            );
        }


        [Fact]
        public void Unspecified_is_the_default_value() {
            Assert.Equal(
                TestTagType.Unspecified,
                default(TestTagType)
            );
        }
    }
}

#endif
