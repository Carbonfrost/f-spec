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
using System.Collections.Generic;
using System.Linq;
using Carbonfrost.Commons.Spec;
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.SelfTest.Spec.TestMatchers {

    public class HaveKeyWithValueMatcherTests : TestClass {

        [Fact]
        public void Matches_should_detect_key_and_value_nominal() {
            var subj = new HaveKeyWithValueMatcher<string, int>("hello", 420);
            Assert.True(subj.Matches(new Dictionary<string, int> {
                                         { "hello", 420 }
                                     }));
        }

        [Fact]
        public void Matches_should_detect_key_and_value_list() {
            var subj = new HaveKeyWithValueMatcher<string, int>("hello", 420);
            Assert.True(subj.Matches(new List<KeyValuePair<string, int>> {
                                         { new KeyValuePair<string, int>("hello", 420) }
                                     }));
        }

        [Fact]
        public void Matches_should_detect_key_and_value_groupings() {
            var subj = new HaveKeyWithValueMatcher<char, string>('h', "hello");
            var groupings = new [] { "hello", "hi", "goodbye" }.GroupBy(t => t[0]);

            Assert.True(subj.Matches(groupings));
        }


        [Fact]
        public void Matches_should_detect_non_matching_key_and_value_nominal() {
            var subj = new HaveKeyWithValueMatcher<string, int>("hello", 420);
            Assert.False(subj.Matches(new Dictionary<string, int> {
                                          { "nope", 420 }
                                      }));
        }

        [Fact]
        public void ExpectTo_should_obtain_matcher() {
            Assert.IsInstanceOf<HaveKeyWithValueMatcher<int, int>>(Matchers.HaveKeyWithValue(1, 2));
        }

        [Fact]
        public void Expect_ToHave_fluent_expression() {
            var dict = new Dictionary<string, int> {
                { "zzz", 420 }
            };
            Expect(dict).ToHave.KeyWithValue("zzz", 420);
        }

        [Fact]
        public void Expect_NotToHave_fluent_expression() {
            var dict = new Dictionary<string, int> {
                { "zzz", 420 }
            };
            Expect(dict).Not.ToHave.KeyWithValue("aaa", 420);
        }

        [Fact]
        public void Assert_key_with_value() {
            var dict = new Dictionary<string, int> {
                { "zzz", 420 }
            };
            Assert.ContainsKeyWithValue("zzz", 420, dict);
        }

        [Fact]
        public void Expect_ToHave_matcher_expression() {
            var dict = new Dictionary<string, int> {
                { "zzz", 420 }
            };
            Expect(dict).To(Matchers.HaveKeyWithValue("zzz", 420));
        }

    }
}
#endif
