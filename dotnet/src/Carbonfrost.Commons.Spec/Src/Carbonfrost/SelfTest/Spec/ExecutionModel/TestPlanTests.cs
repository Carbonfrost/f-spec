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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Carbonfrost.Commons.Spec;
using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.SelfTest.Spec.ExecutionModel {

    public class TestPlanTests {

        class A {
            [Fact] public void TestA1() {}
        }

        [Focus]
        class B {
            [Fact] public void TestB1() {}
            [Focus, Fact] public void TestB2() {}
            [Fact] public void TestB3() {}
        }

        class C {
            [Fact] public void TestC1() {}
            [Fact] public void TestC2() {}
            [Fact] public void TestC3() {}
        }

        [Focus]
        class D {
            [Fact]
            public void TestD1() {}
            [Fact]
            public void TestD2() {}
        }

        [Fact]
        public void WillRunTestCases_should_contain_all_names() {
            var plan = CreatePlan("A", "C");
            var names = WillRun(plan);
            Assert.Equal(new [] { "TestA1", "TestC1", "TestC2", "TestC3" }, names);
        }

        [Fact]
        public void WillRunTestCases_should_contain_all_when_ignoring_focus() {
            var opts = new TestRunnerOptions {
                IgnoreFocus = true,
            };
            var testRun = new TestRun();
            testRun.Children.AddAll(TestUnits("A", "B"));

            var runner = new DefaultTestRunner(opts);
            var plan = (DefaultTestRunner.TestPlan) runner.CreatePlan(testRun);
            var names = WillRun(plan);
            Assert.Equal(new [] { "TestA1", "TestB1", "TestB2", "TestB3"  }, names);
        }

        [Fact]
        public void WillRunTestCases_should_contain_focussed_units_recursive() {
            var plan = CreatePlan("D");
            var names = WillRun(plan);
            Assert.Equal(new [] { "TestD1", "TestD2" }, names);
        }

        [Fact]
        public void WillRunTestCases_should_contain_only_most_focussed_units() {
            // Though B is focussed, TestB2 is narrower scope
            var plan = CreatePlan("A", "B");
            var names = WillRun(plan);
            Assert.Equal(new [] { "TestB2" }, names);
        }

        [Fact]
        public void WillRunTestCases_should_contain_focus_match_names() {
            var opts = new TestRunnerOptions {
                FocusPatterns = {
                    "(2|3)$",
                },
            };
            var testRun = new TestRun();
            testRun.Children.AddAll(TestUnits("C"));
            var runner = new DefaultTestRunner(opts);
            var plan = (DefaultTestRunner.TestPlan) runner.CreatePlan(testRun);

            var names = WillRun(plan);
            Assert.Equal(new [] { "TestC2", "TestC3" }, names);
        }

        [Fact]
        public void WillRunTestCases_should_prefer_focus_specified_by_options() {
            var opts = new TestRunnerOptions {
                FocusPatterns = {
                    "2$",
                },
            };
            var testRun = new TestRun();
            testRun.Children.AddAll(TestUnits("A", "B", "C", "D"));
            var runner = new DefaultTestRunner(opts);
            var plan = (DefaultTestRunner.TestPlan) runner.CreatePlan(testRun);

            var names = WillRun(plan);
            Assert.Equal(new [] { "TestB2", "TestC2", "TestD2" }, names);
        }

        private static DefaultTestRunner.TestPlan CreatePlan(params string[] names) {
            var opts = new TestRunnerOptions();
            var testRun = new TestRun();
            testRun.Children.AddAll(TestUnits(names));
            var runner = new DefaultTestRunner(opts);
            return (DefaultTestRunner.TestPlan) runner.CreatePlan(testRun);
        }

        private static IEnumerable<TestUnit> TestUnits(params string[] names) {
            foreach (var name in names) {
                var type = typeof(TestPlanTests).GetNestedType(name, BindingFlags.NonPublic);
                yield return new ReflectedTestClass(type);
            }
        }

        static string[] WillRun(DefaultTestRunner.TestPlan plan) {
            Func<string, string> removePrefix = s => {
                const string prefix = "Carbonfrost.SelfTest.Spec.ExecutionModel.TestPlanTests+";
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
