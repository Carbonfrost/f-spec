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
using System.Diagnostics;
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
                "{ A = \"1\", B = \"2\", C = <InvalidCastException>, D = ..., F = \"Field has value\" }",
                DisplayActual.Create(new PHasNoToStringOverride()).Format(DisplayActualOptions.None)
            );
        }

        [Fact]
        public void Create_should_generate_type_information_if_no_properties() {
            Assert.Equal(
                "PHasNoToStringOverrideNoProperties {  }",
                DisplayActual.Create(new PHasNoToStringOverrideNoProperties()).Format(DisplayActualOptions.None)
            );
        }

        [Fact]
        public void Create_should_generate_default_no_string_override_value_type() {
            Assert.Equal(
                "{ Comparison = <null>, Operand = <null> }",
                DisplayActual.Create(new PHasNotStringOverrideStruct()).Format(DisplayActualOptions.None)
            );
        }

        class PDebuggerToString {
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public string Hidden { get; }

            [DebuggerBrowsable(DebuggerBrowsableState.Collapsed)]
            public NestedObject Collapsed { get; }

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public NestedObject RootHidden { get; }

            public PDebuggerToString() {
                Hidden = "nope";
                Collapsed = new NestedObject();
                RootHidden = new NestedObject();
            }

            public class NestedObject {
                public string A;

                public NestedObject() {
                    A = "T";
                }
            }
        }

        [Fact]
        public void Create_should_format_debugger_properties() {
            Assert.Equal(
                "{ A = \"T\", Collapsed = ... }",
                DisplayActual.Create(new PDebuggerToString()).Format(DisplayActualOptions.None)
            );
        }

#pragma warning disable 0649
        struct PHasNotStringOverrideStruct {
            public string Operand;
            public Comparison<string> Comparison;
        }

        class PHasToStringOverride {
            public override string ToString() {
                return "ToString method";
            }
        }

        class PHasNoToStringOverrideNoProperties {
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
