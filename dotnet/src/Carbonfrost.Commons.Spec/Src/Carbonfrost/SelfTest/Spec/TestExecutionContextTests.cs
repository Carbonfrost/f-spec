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
using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.SelfTest.Spec {

    public class TestExecutionContextTests : TestClass {

        [Fact]
        public void TestName_is_expected_value() {
            var type = GetType().FullName;
            var method = nameof(TestName_is_expected_value);
            Assert.Equal(
                $"{type}.{method}", TestContext.CurrentTest.DisplayName
            );
        }

        [Fact]
        public void RunTest_Action_should_add_child_context() {
            string testUnit = null;

            RunTest(tc => {
                testUnit = tc.TestUnit.DisplayName;
            });

            string method = nameof(RunTest_Action_should_add_child_context);

            Assert.NotNull(testUnit);
            Assert.Equal($"{GetType()}.{method} dynamic A", testUnit);
        }

        [Fact]
        public void RunTest_Action_should_allow_pass_from_within() {
            var results = RunTest(tc => {
                Assert.Pass("passed within");
            }, new TestOptions {
                PassExplicitly = true
            });

            Assert.True(results.Passed);
            Assert.True(results.IsStatusExplicit);
        }

        [Fact]
        public void RunTests_should_process_test_data() {
            var handler = TestActionDispatcher.Create((TestExecutionContext c) => { });
            var results = RunTests(
                TestData.Table(
                    TestData.Create("a", "b"),
                    TestData.Create("x", "y")
                ), handler.Invoke
            );

            Assert.Equal(2, handler.CallCount);
            Assert.Equal(new [] { "a", "b" }, handler.ArgsForCall(0).TestData);
            Assert.Equal(new [] { "x", "y" }, handler.ArgsForCall(1).TestData);
        }

        [Fact]
        public void RunTests_should_generate_correct_test_results() {
            var handler = TestActionDispatcher.Create((TestExecutionContext c) => { });
            var results = RunTests(
                TestData.Table(
                    TestData.Create("a", "b"),
                    TestData.Create("x", "y")
                ), handler.Invoke
            );

            var baseName = GetType() + "." + nameof(RunTests_should_generate_correct_test_results);
            Assert.HasCount(2, results.Children);
            Assert.Equal(TestStatus.Passed, results.Children[0].Status);
            Assert.Equal(TestStatus.Passed, results.Children[1].Status);
            Assert.Equal($"{baseName} dynamic A #0 (a,b)", results.Children[0].DisplayName);
            Assert.Equal($"{baseName} dynamic A #1 (x,y)", results.Children[1].DisplayName);
        }
    }
}
#endif
