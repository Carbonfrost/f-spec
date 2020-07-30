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
using System.Reflection;
using Carbonfrost.Commons.Spec;
using Carbonfrost.Commons.Spec.ExecutionModel;
using Carbonfrost.SelfTest.Spec.ExecutionModel;

namespace Carbonfrost.SelfTest.Spec.ExecutionModel {

    public class TestNameTests {

        class FTestCaseInfo : TestCaseInfo {

            static readonly MethodInfo TEST_METHOD = typeof(FTestCaseInfo).GetMethod(
                nameof(MyTestMethod)
            );

            public override int Position {
                get {
                    return 3;
                }
            }

            public override string Name {
                get {
                    return "RunTest";
                }
            }

            public override TestUnitType Type {
                get {
                    return TestUnitType.Theory;
                }
            }

            protected override TestCaseResult RunTestCore(TestExecutionContext testContext) {
                throw new NotImplementedException();
            }

            public FTestCaseInfo() : base(TEST_METHOD) {

            }

            public void MyTestMethod() {
            }
        }

        public IEnumerable<TestUnitDisplayNameData> TestUnitDisplayNames {
            get {
                return new TestUnitDisplayNameData[] {
                    Data(new FTestCaseInfo(), "MyTestMethod #3"),
                };
            }
        }

        [Theory]
        [PropertyData(nameof(TestUnitDisplayNames))]
        public void ToString_get_expected_test_name(TestUnitDisplayNameData data) {
            Assert.Equal(data.Name, data.Unit.TestName.ToString());
        }

        public TestUnitDisplayNameData Data(TestCaseInfo unit, string name) {
            return new TestUnitDisplayNameData {
                Unit = unit,
                Name = name,
            };
        }

        public class TestUnitDisplayNameData {
            public TestCaseInfo Unit;
            public string Name;
        }
    }
}
