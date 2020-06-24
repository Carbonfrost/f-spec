#if SELF_TEST
//
//
// Copyright 2016, 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Reflection;

using Carbonfrost.Commons.Spec;
using Carbonfrost.Commons.Spec.ExecutionModel;
using Carbonfrost.SelfTest.Spec.ExecutionModel;

namespace Carbonfrost.SelfTest.Spec {

    public class FixtureDataAttributeTests {

        [Theory]
        [InlineData("/var/tmp/Suite")]
        [InlineData(@"C:\\windowsy\Suite")]
        public void Constructor_should_treat_rooted_paths_as_files(string input) {
            var attr = new FixtureDataAttribute(input);
            Assert.True(attr.Url.IsFile);
        }

        [Fact]
        public void GetData_will_get_TestData_with_correct_tags() {
            var data = GetData(() => new FixtureDataAttribute("data:hello:world") {
                Tag = "platform:windows",
                Tags = new [] { "slow" }
            });
            Assert.Equal(new [] { TestTag.Parse("platform:windows"), TestTag.Slow }, data[0].Tags);
        }

        [Theory]
        [InlineData("", "Empty fixture <(data),>")]
        [InlineData("text", "Failed to load fixture <(data),text> (Expected `:', line 1)")]
        [InlineData("example:\n  \t tabs are illegal", "Failed to load fixture <(data),example:\\n  \\t tabs are illegal> (Illegal tabs, line 2)")]
        public void GetData_will_be_unable_to_load_fixture_with_errors(string text, string expectedMessage) {
            var data = GetData(() => new FixtureDataAttribute("data:," + text));

            Assert.Equal(expectedMessage, data[0].Reason);
            Assert.True(data[0].IsPending);
        }

        void FakeTheory(string text) {
        }

        private TestData[] GetData(Func<FixtureDataAttribute> func) {
            var fake = new ReflectedTheory(
                GetType().GetMethod("FakeTheory", BindingFlags.NonPublic | BindingFlags.Instance
            ));
            var context = TestContext.NewInitContext(fake, new FakeRunner());
            var fd = func();
            return ((ITestDataProvider) fd).GetData(context).ToArray();
        }
    }
}

#endif
