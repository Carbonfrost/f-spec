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
using System.Linq;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.SelfTest.Spec.ExecutionModel {

    public class TestFuncTests {

        public static IEnumerable<Type> AttributeTypes {
            get {
                return typeof(TestFuncTests).Assembly.GetTypes().Where(t => t.Namespace == "Carbonfrost.Commons.Spec" && t.Name.StartsWith("TestFunc"));
            }
        }

        [Theory]
        [PropertyData(nameof(AttributeTypes))]
        public void TestFunc_type_should_define_RetargetAttribute(Type type) {
            Assert.True(type.IsDefined(typeof(RetargetAttribute), false));
        }

        [Fact]
        public void TestFunc_and_TestAction_have_same_count() {
            Assert.Equal(
                TestActionTests.AttributeTypes.Count(),
                TestFuncTests.AttributeTypes.Count()
            );
        }
    }

}
#endif
