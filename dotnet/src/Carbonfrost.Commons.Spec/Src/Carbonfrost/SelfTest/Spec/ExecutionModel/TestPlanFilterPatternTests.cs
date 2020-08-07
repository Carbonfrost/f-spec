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
using static Carbonfrost.Commons.Spec.Matchers;
using Carbonfrost.Commons.Spec.ExecutionModel;
using System;

namespace Carbonfrost.SelfTest.Spec.ExecutionModel {

    public class TestPlanFilterPatternTests : TestClass {

        [InlineData("n", "n", "z")]
        [InlineData("n", "contains an n", "where is it?")]
        [InlineData("w*", "wildcard", "contains a w but doesn't start with one")]
        [InlineData(@"regex:\d+", "123", "letters")]
        [InlineData("regex:(?i)abc", "ABC", "xyz")]
        [Theory]
        public void Parse_should_generate_pattern_that_can_match(string text, string example, string counterexample) {
            var pat = TestPlanFilterPattern.Parse(text);
            Expect(pat.IsMatch(new FakeTestUnit(example))).ToBe.True();
            Expect(pat.IsMatch(new FakeTestUnit(counterexample))).ToBe.False();
        }

        [InlineData((string) null)]
        [InlineData("")]
        [InlineData("   \t", Name = "all whitespace")]
        [Theory]
        public void Parse_should_throw_on_invalid_string(string text) {
            var exception = Record.Exception(
                () => TestPlanFilterPattern.Parse(text)
            );
            Expect(exception).To(
                BeInstanceOf(typeof(ArgumentException))
            );
        }
    }
}
#endif
