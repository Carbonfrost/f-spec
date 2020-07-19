#if SELF_TEST

//
// Copyright 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Reflection;
using Carbonfrost.Commons.Spec;
using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.SelfTest.Spec.ExecutionModel {

    public class TestPlanFilterTests {

        class A {
            [Fact] public void TestA1() {}
        }

        class B {
            [Fact] public void TestB1() {}
            [Fact] public void TestB2() {}
            [Fact] public void TestB3() {}
        }

        class C {
            [Fact, Tag("acc")] public void TestC1() {}
            [Fact, Tag("os:acc")] public void TestC2() {}
            [Fact] public void TestC3() {}
        }

        class D {
            [Fact]
            public void TestD1() {}
            [Fact]
            public void TestD2() {}
        }

        private static IEnumerable<TestUnit> TestUnits(params string[] names) {
            var testContext = TestContext.NewInitContext(null, new FakeRunner());
            foreach (var name in names) {
                var type = typeof(TestPlanFilterTests).GetNestedType(name, BindingFlags.NonPublic);
                yield return new ReflectedTestClass(type);
            }
        }

        [Fact]
        public void Tags_will_selected_matching_tests_only() {
            var names = WillRun(pf => pf.Tags.AddNew("acc"));
            Assert.Equal(new [] { "TestC1" }, names);
        }

        [Theory]
        [InlineData("os")]
        [InlineData("os:acc")]
        public void Tags_will_selected_matching_tests_tag_with_value(string tag) {
            var names = WillRun(pf => pf.Tags.AddNew(tag));
            Assert.Equal(new [] { "TestC2" }, names);
        }

        [Fact]
        public void Tags_will_exclude_selected_matching_tests() {
            var names = WillRun(pf => pf.Tags.AddNew("~acc"));
            Assert.DoesNotContain("TestC1", names);
        }

        [Fact]
        public void Includes_will_include_matching_tests() {
            var names = WillRun(pf => pf.Includes.AddNew("TestD"));
            Assert.Equal(new [] { "TestD1", "TestD2" }, names);
        }

        [Fact]
        public void Excludes_will_excludes_matching_tests() {
            var names = WillRun(pf => pf.Excludes.AddNew("TestB"));
            Assert.DoesNotContain("TestB1", names);
            Assert.DoesNotContain("TestB2", names);
        }

        static string[] WillRun(Action<TestPlanFilter> planner) {
            var testRun = new TestRun();
            var opts = new TestRunnerOptions();
            planner(opts.PlanFilter);

            testRun.Children.AddAll(TestUnits("A", "B", "C", "D"));
            var runner = new DefaultTestRunner(opts);
            var plan = (DefaultTestRunner.TestPlan) runner.CreatePlan(testRun);

            Func<string, string> removePrefix = s => {
                const string prefix = "Carbonfrost.SelfTest.Spec.ExecutionModel.TestPlanFilterTests+";
                if (s.StartsWith(prefix, StringComparison.Ordinal)) {
                    return s.Substring(prefix.Length + 2);
                }
                return s;
            };

            return plan.WillRunTestCases.Select(t => removePrefix(t.DisplayName)).ToArray();
        }
    }
}
#endif
