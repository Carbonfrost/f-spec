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
using System.Collections.Generic;
using System.Linq;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.SelfTest.Spec {

    public class FixtureParserTests {

        public IEnumerable<TestData> Examples {
            get {
                yield return new TestData(
                    @"example: >\n
                    ··Several lines of text,\n
                    ··with some ""quotes"" of various 'types',\n
                    ··and also a blank line:\n
                    ··\n
                    ··plus another line at the end.\n
                    ··\n
                    ··\n",
                    "Several lines of text, with some \"quotes\" of various 'types', and also a blank line:\nplus another line at the end.\n"
                ).WithName("Folded and clipped");

                yield return new TestData(
                    @"example: >-\n
                    ··Several lines of text,\n
                    ··with some ""quotes"" of various 'types',\n
                    ··and also a blank line:\n
                    ··\n
                    ··plus another line at the end.\n
                    ··\n
                    ··\n",
                    "Several lines of text, with some \"quotes\" of various 'types', and also a blank line:\nplus another line at the end."
                ).WithName("Folded and stripped"); // no new line at end

                yield return new TestData(
                    @"example: >+\n
                    ··Several lines of text,\n
                    ··with some ""quotes"" of various 'types',\n
                    ··and also a blank line:\n
                    ··\n
                    ··plus another line at the end.\n
                    ··\n
                    ··\n",
                    "Several lines of text, with some \"quotes\" of various 'types', and also a blank line:\nplus another line at the end.\n\n\n"
                ).WithName("Folded and keep"); // all new lines at end

                yield return new TestData(
                    @"example: |\n
                    ··Several lines of text,\n
                    ··with some ""quotes"" of various 'types',\n
                    ··and also a blank line:\n
                    ··\n
                    ··plus another line at the end.\n
                    ··\n
                    ··\n",
                    "Several lines of text,\nwith some \"quotes\" of various 'types',\nand also a blank line:\n\nplus another line at the end.\n"
                ).WithName("Literal and clipped");

                yield return new TestData(
                    @"example: _\n
                    ··Several lines of text,\n
                    ··with some ""quotes"" of various 'types',\n
                    ··and also a blank line:\n
                    ··\n
                    ··plus another line at the end.\n
                    ··\n
                    ··\n",
                    "Severallinesoftext,withsome\"quotes\"ofvarious'types',andalsoablankline:plusanotherlineattheend."
                ).WithName("No whitespace");
            }
        }

        [Theory]
        [PropertyData(nameof(Examples))]
        public void Parse_should_create_correct_heredoc(string fixture, string expected) {
            var parser = new FixtureParser(null);
            var result = parser.Parse(
                ConvertVisibleWhitespace(fixture)
            ).ToList().Single();

            Assert.Equal(expected, result["example"]);
        }

        [Fact]
        public void Parse_should_throw_if_leading_tabs() {
            var parser = new FixtureParser(null);
            Assert.Throws<FormatException>(
                () => parser.Parse("example:\n  \t tabs are illegal")
            );

            var message = Record.Exception(
                () => parser.Parse("example:\n  \t tabs are illegal")
            ).Message;
            Assert.Equal("Illegal tabs, line 2", message);
        }

        static string ConvertVisibleWhitespace(string text) {
            return string.Concat(
                text.Split('\n').Select(l => l.Trim().Replace("\\n", "\n").Replace("·", " "))
            );
        }
    }
}

#endif
