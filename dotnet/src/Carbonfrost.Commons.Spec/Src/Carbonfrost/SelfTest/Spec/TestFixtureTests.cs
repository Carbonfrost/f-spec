#if SELF_TEST

//
// Copyright 2016, 2018 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

namespace Carbonfrost.SelfTest.Spec {

    public class TestFixtureTests {

        static readonly string SingleRecord = string.Format(@"hello: world
goodbye:earth
file: >
  Hello, world
  Goodbye, earth
file2: >
  Hello, Jupiter
  from Juno

{0} comment
            ", "#");
        // HACK -- '#' can be detected as a preprocessor directive in some compilers --
        // so we do this format string

        const string FoldingRecord = @"simple: >
    abcd efgh abcd efgh
total: _
    abcd efgh abcd efgh
";



        const string MultipleRecords = @"hello: world
---
hello: terra
---
hello: monde";

        const string MultipleRecordsSomeEmpty = @"hello: world
---
---
hello: terra
---
hello: monde
---
---";

        const string TreatUnfoldedHeredoc = @"hello:
  a heredoc since it is indented
  and is folded
goodbye:
note: goodbye was treated as an empty value";

        [Fact]
        public void Parse_should_gather_correct_keys() {
            var fixture = TestFixture.Parse(SingleRecord);
            Assert.Equal(1, fixture.Items.Count);
            Assert.Contains("goodbye", fixture.Items[0].Values.Keys);
            Assert.Contains("hello", fixture.Items[0].Values.Keys);
            Assert.Contains("file", fixture.Items[0].Values.Keys);
            Assert.Contains("file2", fixture.Items[0].Values.Keys);
        }

        [Fact]
        public void Parse_should_gather_simple_values_trim_text() {
            var fixture = TestFixture.Parse(SingleRecord);
            Assert.Equal("earth", fixture.Items[0]["goodbye"]);
            Assert.Equal("world", fixture.Items[0]["hello"]);
        }

        [Fact]
        public void Parse_shoud_allow_heredocs_without_folds() {
            var fixture = TestFixture.Parse(TreatUnfoldedHeredoc);
            Assert.ContainsKey("hello", fixture.Items[0].Values);
            Assert.Equal("a heredoc since it is indented and is folded", fixture.Items[0].Values["hello"]);

            Assert.ContainsKey("goodbye", fixture.Items[0].Values);
            Assert.Empty(fixture.Items[0].Values["goodbye"]);
            Assert.ContainsKey("note", fixture.Items[0].Values);
        }

        [Fact]
        public void Parse_should_fold_heredocs() {
            var fixture = TestFixture.Parse(SingleRecord);
            Assert.Equal("Hello, world Goodbye, earth\n", fixture.Items[0].Values["file"]);
            Assert.Equal("Hello, Jupiter from Juno\n", fixture.Items[0].Values["file2"]);
        }

        [Fact]
        public void Parse_should_fold_heredocs_according_to_specifier() {
            var fixture = TestFixture.Parse(FoldingRecord);
            Assert.Equal("abcd efgh abcd efgh\n", fixture.Items[0].Values["simple"]);
            Assert.Equal("abcdefghabcdefgh", fixture.Items[0].Values["total"]);
        }

        static string EOL(string s) {
            return s.Replace(Environment.NewLine, "\n");
        }

        [Fact]
        public void Parse_multiple_records() {
            var fixture = TestFixture.Parse(MultipleRecords);
            foreach (var m in fixture.Items) {
                Assert.Equal(1, m.Values.Count);
            }

            Assert.Equal(3, fixture.Items.Count);
        }

        [Fact]
        public void Parse_multiple_records_should_ignore_empty_records() {
            var fixture = TestFixture.Parse(MultipleRecordsSomeEmpty);
            Assert.Equal(3, fixture.Items.Count);
        }
    }
}
#endif
