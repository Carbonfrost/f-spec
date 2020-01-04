#if SELF_TEST

//
// Copyright 2017 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.SelfTest.Spec.TestMatchers {

    public class SameMatcherTests : TestClass {

        [Fact]
        public void Matches_should_detect_object_equivalence_nominal() {
            var item = new object();
            var subj = new SameMatcher(item);
            Assert.True(subj.Matches(item));
            Assert.Same(item, item);
        }

        [Fact]
        public void Matches_should_detect_object_nonequivalence_nominal() {
            var item = new object();
            var other = new object();
            var subj = new SameMatcher(item);
            Assert.False(subj.Matches(other));
            Assert.NotSame(item, subj);
        }

        [Fact]
        public void Matches_should_not_detect_value_types() {
            const int item = 10;
            const int other = 10;
            var subj = new SameMatcher(item);
            Assert.False(subj.Matches(other));
        }

        [Fact]
        public void ExpectTo_should_obtain_matcher() {
            Expect("hello").To(Matchers.BeSameAs("hello"));
            Assert.IsInstanceOf<SameMatcher>(Matchers.BeSameAs("hello"));
        }

        [Fact]
        public void Expect_ToBe_fluent_expression() {
            Expect("hello").ToBe.SameAs("hello");
        }
    }
}
#endif
