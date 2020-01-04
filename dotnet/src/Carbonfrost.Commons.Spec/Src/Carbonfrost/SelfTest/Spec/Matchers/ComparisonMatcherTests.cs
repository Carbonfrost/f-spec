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

    public class ComparisonMatcherTests : TestClass {
        [Theory]
        [InlineData(typeof(GreaterThanMatcher<string>), "a")]
        [InlineData(typeof(LessThanMatcher<string>), "z")]
        [InlineData(typeof(GreaterThanOrEqualToMatcher<string>), "a")]
        [InlineData(typeof(LessThanOrEqualToMatcher<string>), "z")]
        [InlineData(typeof(GreaterThanOrEqualToMatcher<string>), "g")]
        [InlineData(typeof(LessThanOrEqualToMatcher<string>), "g")]
        public void Matches_should_apply_nominal(Type matcherType, string arg) {
            var subj = (TestMatcher<string>)Activator.CreateInstance(matcherType, new object[] {
                                                                         arg,
                                                                         StringComparer.Ordinal
                                                                     });
            Assert.True(subj.Matches("g"));
        }

        [Theory]
        [InlineData(typeof(GreaterThanMatcher<string>), "A")]
        [InlineData(typeof(LessThanMatcher<string>), "Z")]
        [InlineData(typeof(GreaterThanOrEqualToMatcher<string>), "A")]
        [InlineData(typeof(LessThanOrEqualToMatcher<string>), "Z")]
        [InlineData(typeof(GreaterThanOrEqualToMatcher<string>), "G")]
        [InlineData(typeof(LessThanOrEqualToMatcher<string>), "G")]
        public void Matches_should_apply_with_comparer(Type matcherType, string arg) {
            var subj = (TestMatcher<string>)Activator.CreateInstance(matcherType, new object[] {
                                                                         arg,
                                                                         StringComparer.OrdinalIgnoreCase
                                                                     });
            Assert.True(subj.Matches("g"));
        }
    }
}
#endif
