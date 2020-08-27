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
using Carbonfrost.Commons.Spec;
using Carbonfrost.Commons.Spec.ExecutionModel;
using System;

namespace Carbonfrost.SelfTest.Spec.ExecutionModel {

    public class TestAssemblyTests {

        [Fact]
        public void Description_comes_from_assembly_attribute() {
            var asm = TestAssembly.Create(GetType().Assembly);
            Assert.Equal("A unit testing framework", asm.Description);
        }

        [Theory]
        [InlineData(typeof(PInternalTestClass))]
        [InlineData(typeof(PDerivedTestClass))]
        [InlineData(typeof(PInternalTestClassInternalMethod))]
        public void IsTestClassByAccess_should_find_private_classes(Type type) {
            var asm = TestAssembly.Create(GetType().Assembly);
            Assert.True(asm.IsTestClassByAccess(type));
        }

        [Theory]
        [InlineData(typeof(PInternalTestClass))]
        [InlineData(typeof(PDerivedTestClass))]
        [InlineData(typeof(PInternalTestClassInternalMethod))]
        public void TestUnitFromType_should_find_private_methods(Type type) {
            var asm = TestAssembly.Create(GetType().Assembly);
            var test = asm.TestUnitFromType(type);
            var testContext = SelfTestUtility.NewTestContext(null, new FakeRunner());
            test.InitializeSafe(testContext);
            Assert.HasCount(
                1,
                test.Children
            );
            Assert.EndsWith(
                "PFact",
                test.Children[0].DisplayName
            );
        }
    }

    class PInternalTestClass {

        [Fact] // fixture
        public void PFact() {
            // Causes this to be a test class
        }
    }

    class PInternalTestClassInternalMethod {

        [Fact] // fixture
        private void PFact() {
            // Causes this to be a test class
        }
    }

    class PDerivedTestClass : PInternalTestClass {
    }
}

#endif
