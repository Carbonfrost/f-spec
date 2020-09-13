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
using Carbonfrost.Commons.Spec;
using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.SelfTest.Spec.ExecutionModel {

    class PClosedGeneric<T> {}

    class FTestClassInfo : TestClassInfo {
        public FTestClassInfo(Type type): base(type) {
        }
    }

    public class TestClassInfoTests {

        class PClosedGenericNested<T> {}

        [Theory]
        [InlineData(typeof(string), "System.String")]
        [InlineData(typeof(PClosedGeneric<string>), "Carbonfrost.SelfTest.Spec.ExecutionModel.PClosedGeneric<String>")]
        [InlineData(typeof(PClosedGenericNested<string>), "Carbonfrost.SelfTest.Spec.ExecutionModel.TestClassInfoTests+PClosedGenericNested<String>")]
        public void DisplayName_has_simplified_naming(Type type, string expected) {
            var tc = new FTestClassInfo(type);
            Assert.Equal(expected, tc.DisplayName);
        }

        class PHasMethodWithMultipleAttributes {
            [Fact, Theory]
            public void F() {}
        }

        [Fact]
        public void CreateTest_will_be_skipped_with_error_when_multiple_test_attributes_specified() {
            var tc = new ReflectedTestClass(typeof(PHasMethodWithMultipleAttributes));
            var testContext = SelfTestUtility.NewTestContext(tc, new FakeRunner());
            tc.InitializeSafe(testContext);

            var exec = new FakeTestExecutionContext(testContext, (TestCaseInfo) tc.Children[0], new PHasMethodWithMultipleAttributes());
            var result = exec.RunCurrentTest();

            Assert.Equal("Problem creating test (SpecException: Method has more than one fact or theory attribute)", result.Reason);
            Assert.True(result.Failed);
        }
    }
}
#endif
