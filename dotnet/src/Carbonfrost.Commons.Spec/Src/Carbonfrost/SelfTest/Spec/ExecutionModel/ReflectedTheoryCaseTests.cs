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
using System;
using System.Collections.Generic;
using Carbonfrost.Commons.Spec;
using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.SelfTest.Spec.ExecutionModel {

    public class ReflectedTheoryCaseTests : TestClass {

        public IEnumerable<Func<ReflectedTheoryCaseTests>> DelegateTestCaseData {
            get {
                return new Func<ReflectedTheoryCaseTests> [] {
                    () => this,
                };
            }
        }

        [Fact]
        public void Constructor_should_copy_test_data_flags() {
            var method = GetType().GetMethod("Constructor_should_copy_test_data_flags");
            var data = new FTestData().WithReason("Reasons");
            var c = new ReflectedTheoryCase(method, new TestDataInfo(data, null, 0));

            Assert.Equal("Reasons", c.Reason);
            Assert.True(c.IsFocused);
        }

        [Theory]
        [PropertyData(nameof(DelegateTestCaseData))]
        public void CoreRunTest_with_action_will_rebind_delegate_target(Func<ReflectedTheoryCaseTests> func) {
            Assert.Same(this, func());
        }

        [Theory(RetargetDelegates = RetargetDelegates.Enabled)]
        [PropertyData(nameof(DelegateTestCaseData))]
        public void CoreRunTest_with_action_will_rebind_delegate_target_via_theory(Func<ReflectedTheoryCaseTests> func) {
            Assert.Same(this, func());
        }

        [Theory]
        [PropertyData(nameof(DelegateTestCaseData), RetargetDelegates = RetargetDelegates.Disabled)]
        public void CoreRunTest_can_opt_out_of_rebind_delegate_target(Func<ReflectedTheoryCaseTests> func) {
            Assert.NotSame(this, func());
        }

        [Theory]
        [PropertyData(nameof(PropertyWithNames))]
        public void Initialize_should_bind_Name_property_automatically_in_test_case(PHasNameProperty s) {
            var method = nameof(Initialize_should_bind_Name_property_automatically_in_test_case);
            var type = GetType();

            var index = TestContext.CurrentTest.Position;
            Assert.Equal($"{type}.{method} PropertyWithNames[{index}] #{index} ({{ Name = \"expected\" }})", TestContext.CurrentTest.DisplayName);
        }

        [Theory]
        [FixtureData("data:,name:expected")]
        public void Initialize_should_bind_Name_property_automatically_in_test_case_fixtures(PHasNameProperty s) {
            var method = nameof(Initialize_should_bind_Name_property_automatically_in_test_case_fixtures);
            var type = GetType();
            Assert.Equal($"{type}.{method} #0 ({{ Name = \"expected\" }})", TestContext.CurrentTest.DisplayName);
        }

        [Theory]
        [InlineData("OK", Name = "expected")]
        public void Initialize_should_set_Name_specified_in_inline_data(string s) {
            var method = nameof(Initialize_should_set_Name_specified_in_inline_data);
            var type = GetType();
            Assert.Equal($"{type}.{method} expected #0 (OK)", TestContext.CurrentTest.DisplayName);
        }

        public IEnumerable<PHasNameProperty> PropertyWithNames {
            get {
                return new [] {
                    new PHasNameProperty {
                        Name = "expected"
                    }
                };
            }
        }

        public class PHasNameProperty {
            public string Name {
                get;
                set;
            }
        }
    }

}
#endif
