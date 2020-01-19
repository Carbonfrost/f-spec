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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Carbonfrost.Commons.Spec;
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.SelfTest.Spec.TestMatchers {

    public class HaveKeyMatcherTests : TestClass {

        [Fact]
        public void Matches_should_detect_key_nominal() {
            var subj = new HaveKeyMatcher<string>("hello");
            Assert.True(subj.Matches(new Dictionary<string, int> {
                                         { "hello", 420 }
                                     }));
        }

        [Fact]
        public void Matches_should_detect_non_matching_key_and_value_nominal() {
            var subj = new HaveKeyMatcher<string>("hello");
            Assert.False(subj.Matches(new Dictionary<string, int> {
                                          { "nope", 420 }
                                      }));
        }

        [Fact]
        public void Matches_should_detect_non_matching_key_and_value_list() {
            var subj = new HaveKeyMatcher<string>("hello");
            Assert.True(subj.Matches(new List<KeyValuePair<string, int>> {
                                         { new KeyValuePair<string, int>("hello", 420) }
                                     }));
        }

        [Fact]
        public void Matches_should_detect_matching_groupings() {
            var subj = new HaveKeyMatcher<string>("hello");
            var groupings = new List<KeyValuePair<string, int>> {
                { new KeyValuePair<string, int>("hello", 420) }
            }.GroupBy(t => t.Key);
            Assert.True(subj.Matches(groupings));
        }

        [Fact]
        public void Matches_should_detect_multiple_interface_enumerators() {
            var subj = new HaveKeyMatcher<string>("hello");

            var items = new MCollection();
            Assert.True(subj.Matches(items));
        }

        class MCollection : IEnumerable, IEnumerable<object>, IEnumerable<KeyValuePair<string, string>> {

            // We must invoke this enumerator -- it is the only one that supports keys
            public IEnumerator<KeyValuePair<string, string>> GetEnumerator() {
                yield return new KeyValuePair<string, string>("hello", "world");
            }

            IEnumerator<object> IEnumerable<object>.GetEnumerator() {
                Assert.Fail("Shouldn't invoke this enumerator");
                return null;
            }

            IEnumerator IEnumerable.GetEnumerator() {
                Assert.Fail("Shouldn't invoke this enumerator");
                return null;
            }
        }

        [Fact]
        [PassExplicitly]
        public void Matches_should_throw_on_type_without_keys() {
            var subj = new HaveKeyMatcher<string>("hello");
            try {
                subj.Matches(new ArrayList());

            } catch (AssertException e) {
                Assert.Equal("Can't assert that instance of `System.Collections.ArrayList' has keys because it doesn't support it.",
                             e.Message);
                Assert.Pass();
            }
        }

        [Fact]
        public void ExpectTo_should_obtain_matcher() {
            Assert.IsInstanceOf<HaveKeyMatcher<int>>(Matchers.HaveKey(1));
        }

        [Fact]
        public void Expect_ToHave_fluent_expression() {
            var dict = new Dictionary<string, int> {
                { "zzz", 420 }
            };

            Expect(dict).ToHave.Key("zzz");
        }

        [Fact]
        public void Expect_NotToHave_fluent_expression() {
            var dict = new Dictionary<string, int> {
                { "zzz", 420 }
            };

            Expect(dict).Not.ToHave.Key("aaa");
        }

        [Fact]
        public void Assert_ContainsKey() {
            var dict = new Dictionary<string, int> {
                { "zzz", 420 }
            };

            Assert.ContainsKey("zzz", dict);
        }

        [Fact]
        public void Assert_DoesNotContainKey() {
            var dict = new Dictionary<string, int> {
                { "zzz", 420 }
            };

            Assert.DoesNotContainKey("aa", dict);
        }

        [Fact]
        public void Expect_ToHave_matcher_expression() {
            var dict = new Dictionary<string, int> {
                { "zzz", 420 }
            };
            Expect(dict).To(Matchers.HaveKey("zzz"));
        }
    }
}
#endif
