#if SELF_TEST

//
// Copyright 2016 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.SelfTest.Spec {

    public class TestProcessTests : TestClass {

        [XFact]
        public void Output_should_acquire_output_of_process() {
            // TODO This test is not universal
            using (var pro = TestContext.StartProcess(@"\apps\lib\cfbuild\CFBuild.exe", "--help")) {
                pro.WaitForExit();

                Assert.NotNull(pro.Output);
            }
        }
    }
}
#endif
