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
using System;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.SelfTest.Spec {

    public class TestTagTests : TestClass {

        [Theory]
        [InlineData("name")]
        [InlineData("name:value")]
        public void Parse_should_extract_Name_from_string(string text) {
            Assert.Equal("name", TestTag.Parse(text).Name);
        }

        [Theory]
        [InlineData("name:value")]
        public void Parse_should_extract_Value_from_string(string text) {
            Assert.Equal("value", TestTag.Parse(text).Value);
        }

        [InlineData("n", "n", null)]
        [InlineData("n:v", "n", "v")]
        [InlineData(" a : b", "a", "b")]
        [InlineData("anything", "anything", null)]
        [InlineData("letters-and-hyphens", "letters-and-hyphens", null)]
        [InlineData("letters_underscore", "letters_underscore", null)]
        [InlineData("numbers9:allowed", "numbers9", "allowed")]
        [Theory]
        public void Parse_should_handle_test_tags(string text, string name, string value) {
            Given(text).Expect(TestTag.Parse).ToBe.EqualTo(new TestTag(name, value));
        }

        [InlineData("")]
        [InlineData((string) null)]
        [InlineData("  ")]
        [InlineData("no internal whitespace allowed")]
        [InlineData("no ws in a name:allowed")]
        [InlineData("name:", Name = "missing value")]
        [Theory]
        public void Parse_should_throw_on_invalid_text(string text) {
            var ex = Record.Exception(() => TestTag.Parse(text));

            Expect(ex).Not.ToBe.Null();
            Expect(ex).ToSatisfy.Any(
                Matchers.BeInstanceOf(typeof(FormatException)),
                Matchers.BeInstanceOf(typeof(ArgumentException))
            );
        }

        [Fact]
        public void AliasTo_should_obtain_alias_of_macOS() {
            Assert.Equal(
                TestTag.macOSPlatform,
                TestTag.Platform("osx").AliasTo
            );
        }

        [Fact]
        public void IsAlias_should_apply_to_osx_platform() {
            Assert.True(
                TestTag.Platform("osx").IsAlias
            );
        }

        [Fact]
        public void AreEquivalent_is_symmetric_and_applies_to_macos() {
            Assert.True(
                TestTag.AreEquivalent(TestTag.Platform("osx"), TestTag.macOSPlatform)
            );

            Assert.True(
                TestTag.AreEquivalent(TestTag.macOSPlatform, TestTag.Platform("osx"))
            );
        }

    }
}

#endif
