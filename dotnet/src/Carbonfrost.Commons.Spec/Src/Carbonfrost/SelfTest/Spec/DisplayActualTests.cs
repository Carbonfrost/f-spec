#if SELF_TEST

//
// Copyright 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     https://www.apache.org/licenses/LICENSE-2.0
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

namespace Carbonfrost.SelfTest.Spec {

    public class DisplayActualTests {

        [Theory]
        [InlineData("", "<empty>")]
        [InlineData((string) null, "<null>")]
        public void Create_should_generate_string(string a, string expected) {
            var display = DisplayActual.Create(a);
            Assert.Equal(expected, display.Format(DisplayActualOptions.None));
        }

        [Fact]
        public void Create_ToString_linq_operator_should_reduce_noise() {
            var cast = new object[] { 3, "string" }.OfType<string>();
            Assert.Equal(
                "<OfTypeIterator><String> { \"string\" }",
                DisplayActual.Create(cast).Format(DisplayActualOptions.ShowType)
            );
        }

        [Fact]
        public void Create_should_generate_string_with_type_and_whitespce() {
            var display = DisplayActual.Create("text with spaces");
            Assert.Equal(
                "text⋅with⋅spaces (string)",
                display.Format(DisplayActualOptions.ShowWhitespace | DisplayActualOptions.ShowType)
            );
        }

        [Fact]
        public void OnlyTypeDifferences_should_detect_this_case() {
            var a = DisplayActual.Create(123);
            var b = DisplayActual.Create("123");
            Assert.True(DisplayActual.OnlyTypeDifferences(a, b));
        }

        [Theory]
        [InlineData(typeof(PHasNoToStringOverride), false)]
        [InlineData(typeof(PHasToStringOverride), true)]
        public void HasToStringOverride_is_expected_value(Type type, bool expected) {
            Assert.Equal(expected, DisplayActual.HasToStringOverride(type));
        }

        [Fact]
        public void Create_should_generate_default_no_string_override() {
            Assert.Equal(
                "{ F = \"Field has value\", A = \"1\", B = \"2\", C = <InvalidCastException>, D = ... }",
                DisplayActual.Create(new PHasNoToStringOverride()).Format(DisplayActualOptions.None)
            );
        }

        class PHasToStringOverride {
            public override string ToString() {
                return "ToString method";
            }
        }

        class PHasNoToStringOverride {
            public string A {
                get;
                set;
            }

            public string B {
                get;
                set;
            }

            public string C {
                get {
                    throw new InvalidCastException();
                }
            }

            public object D { // Self-reference which should be elided
                get {
                    return this;
                }
            }

            public string F = "Field has value";

            public PHasNoToStringOverride() {
                A = "1";
                B = "2";
            }
        }
    }
}
#endif
