#if SELF_TEST

//
// Copyright 2018, 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
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
using System.Collections;
using System.Collections.Generic;

using Carbonfrost.Commons.Spec;
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.SelfTest.Spec.TestMatchers {

    public class ContainsMatcherTests : TestClass {

        [Fact]
        public void Matches_should_detect_item_nominal() {
            var subj = new ContainsMatcher<string>("C", StringComparer.OrdinalIgnoreCase);
            Assert.True(subj.Matches(new [] { "a", "b", "c" }));
        }

        [Fact]
        public void Matches_should_detect_item_failure() {
            var subj = new ContainsMatcher<string>("z");
            Assert.False(subj.Matches(new [] { "a", "b", "c" }));
        }

        [Fact]
        public void Matches_should_detect_item_via_collection_Contains() {
            var subj = new ContainsMatcher<string>("C");
            var collection = new PCollectionWithContains();

            Assert.True(subj.Matches(collection));
            Assert.Equal(1, collection.ContainsCallCount);
        }

        [Fact]
        public void Matches_should_detect_item_via_collection_Contains_unless_a_comparer_is_specified() {
            var subj = new ContainsMatcher<string>("c", StringComparer.OrdinalIgnoreCase);
            var collection = new PCollectionWithContains();

            Assert.True(subj.Matches(collection), "should ignore case");
            Assert.Equal(0, collection.ContainsCallCount, "should not be invoked");
        }

        [Fact]
        public void ExpectTo_Have_Element_fluent_expression() {
            Expect(new [] { "a", "b", "c" }).ToHave.Element("a");
            Expect(new [] { "a", "b", "c" }).Not.ToHave.Element("z");
        }

        [Fact]
        public void ExpectTo_Matchers_fluent_expression() {
            Expect(new [] { "a", "b", "c" }).To(Matchers.Contain("a"));
            Expect(new [] { "a", "b", "c" }).NotTo(Matchers.Contain("z"));
        }

        class PCollectionWithContains : ICollection<string> {

            public int ContainsCallCount;

            public int Count {
                get;
            }

            public bool IsReadOnly {
                get;
            }

            public void Add(string item) {
            }

            public void Clear() {
            }

            public bool Contains(string item) {
                ContainsCallCount += 1;
                return item == "C";
            }

            public void CopyTo(string[] array, int arrayIndex) {
            }

            public IEnumerator<string> GetEnumerator() {
                return ((ICollection<string>) new [] { "C" }).GetEnumerator();
            }

            public bool Remove(string item) {
                return false;
            }

            IEnumerator IEnumerable.GetEnumerator() {
                return GetEnumerator();
            }
        }
    }
}
#endif
