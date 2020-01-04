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

    public class AndMatcherTests : TestClass {

        [Fact]
        public void Matches_should_detect_nominal() {
            var subj = new AndMatcher<string>(Matchers.BeEmpty(), Matchers.HaveCount(0));
            Assert.True(subj.Matches(""));
        }

        [Fact]
        public void Matches_should_detect_contra_nominal() {
            var subj = Matchers.And(Matchers.BeEmpty(), Matchers.HaveCount(0));
            Assert.False(subj.Matches(new List<string> { "A", "B" }));
        }

        [Fact]
        public void LocalizerFailure_should_generate_children() {
            var subj = new AndMatcher<string>(Matchers.BeEmpty(), Matchers.HaveCount(0));
            var failure = TestMatcherLocalizer.Failure(subj, "");
            var exception = failure.ToException();

            var lines = exception.Message.Split('\n').Select(t => t.Trim()).Take(3);
            var expected = new [] {
                "Expected to:",
                "- be empty",
                "- and have count 0",
            };
            Assert.Equal(expected, lines);
        }

        [Fact]
        public void LocalizerFailure_should_generate_negated_children() {
            var subj = new AndMatcher<string>(Matchers.BeEmpty(), Matchers.HaveCount(0));
            var failure = TestMatcherLocalizer.Failure(Matchers.Not(subj), "");
            var exception = failure.ToException();

            var label = exception.Message.Split('\n').Select(t => t.Trim()).First();
            Assert.Equal("Not expected to:", label);
        }

        [Fact]
        public void LocalizerCode_should_be_expected_value() {
            var subj = new AndMatcher<string>(Matchers.BeEmpty(), Matchers.HaveCount(0));
            var failure = TestMatcherLocalizer.Failure(subj, "");
            Assert.Equal("spec.and", failure.Name);
        }
    }
}
#endif
