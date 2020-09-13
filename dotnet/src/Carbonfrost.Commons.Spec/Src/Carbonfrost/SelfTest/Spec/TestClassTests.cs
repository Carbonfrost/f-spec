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

using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Carbonfrost.SelfTest.Spec {

    public class TestClassTests {

        public IEnumerable<MethodInfo> RunTestMethods {
            get {
                return typeof(TestClass).GetMethods().Where(m => m.Name.StartsWith("RunTest"));
            }
        }

        [Theory]
        [PropertyData(nameof(RunTestMethods))]
        public void RunTest_should_have_congruent_API_for_pending(MethodInfo mi) {
            var expected = mi.ToString().Replace("RunTest", "XRunTest");
            Assert.Single(typeof(TestClass).GetMethods().Where(m => expected == m.ToString()));
        }

        [Theory]
        [PropertyData(nameof(RunTestMethods))]
        public void RunTest_should_have_congruent_API_for_focus(MethodInfo mi) {
            var expected = mi.ToString().Replace("RunTest", "FRunTest");
            Assert.Single(typeof(TestClass).GetMethods().Where(m => expected == m.ToString()));
        }
    }
}
#endif
