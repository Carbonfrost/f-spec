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

    public class TestTagCollectionTests {

        [Fact]
        public void Add_returns_false_on_duplicate_added() {
            var tt = new TestTagCollection();
            Assert.True(tt.Add("hello"));
            Assert.False(tt.Add("hello"));
        }

        [Fact]
        public void Add_returns_false_on_duplicate_kvp_added() {
            var tt = new TestTagCollection();
            Assert.True(tt.Add("hello:world"));
            Assert.False(tt.Add("hello:WORLD"));
        }

        [Fact]
        public void Contains_test_tag_match() {
            Assert.Contains(TestTag.Parse("hello:world"), new TestTagCollection {
                { "hello:world" }
            });
        }

        [Fact]
        public void Contains_parses_test_tag_match() {
            Assert.Contains("hello:world", new TestTagCollection {
                { "hello:world" }
            });
        }

        [Theory]
        [InlineData("hello:WORLD")]
        [InlineData("HELLO:world")]
        public void Contains_parses_test_tag_match_case_insensitive(string expected) {
            Assert.Contains(expected, new TestTagCollection {
                { "hello:world" }
            });
        }

        [Fact]
        public void Contains_treats_blank_value_as_wildcard() {
            // If any "hello:*" tag is present, this should match
            Assert.Contains("hello", new TestTagCollection {
                { "hello:world" }
            });
        }
    }
}
#endif
