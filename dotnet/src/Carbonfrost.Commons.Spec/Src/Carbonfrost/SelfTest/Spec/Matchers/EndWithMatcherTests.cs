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

using Carbonfrost.Commons.Spec;
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.SelfTest.Spec.TestMatchers {

    public class EndWithMatcherTests : TestClass {

        [Fact]
        public void Matches_should_detect_suffix_nominal() {
            var subj = new EndWithMatcher<string>(new [] { "c" });
            Assert.True(subj.Matches(new [] { "a", "b", "c" }));
        }

        [Fact]
        public void Matches_should_detect_suffix_failure() {
            var subj = new EndWithMatcher<string>(new [] { "z" });
            Assert.False(subj.Matches(new [] { "a", "b", "c" }));
        }
    }
}
#endif
