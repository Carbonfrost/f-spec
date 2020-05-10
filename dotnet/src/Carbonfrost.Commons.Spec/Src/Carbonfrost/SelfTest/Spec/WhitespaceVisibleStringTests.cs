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

using Carbonfrost.Commons.Spec;

namespace Carbonfrost.SelfTest.Spec {

    public class WhitespaceVisibleStringTests {

        [Theory]
        [InlineData("space as dot", "space⋅as⋅dot")]
        [InlineData("newline\nchar", "newline↓\nchar")]
        [InlineData("\t\ttab", "   →   →tab")]
        [InlineData("newlines\r\r\n\n", "newlines←\n↵\n↓\n")]
        public void ToString_should_convert_whitespace_nominal(string text, string expected) {
            var ws = new WhitespaceVisibleString(text);
            Assert.Equal(expected, ws.ToString());
        }
    }
}

#endif
