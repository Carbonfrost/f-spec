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

    public class ReflectedTheoryCaseTests {

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
            var c = new ReflectedTheoryCase(method, 0, data);

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
    }

}
#endif
