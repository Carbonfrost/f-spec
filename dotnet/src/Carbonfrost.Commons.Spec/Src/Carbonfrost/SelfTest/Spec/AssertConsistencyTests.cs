#if SELF_TEST

//
// Copyright 2018 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

namespace Carbonfrost.SelfTest.Spec {

    public class AssertConsistencyTests : TestClass {

        public IEnumerable<MethodInfo> AllExpectMethods {
            get {
                return typeof(Assert).GetTypeInfo().GetMethods().Where(t => t.Name == "Expect")
                    .Concat(typeof(TestClass).GetTypeInfo().GetMethods().Where(t => t.Name == "Expect"));
            }
        }

        [Theory]
        [PropertyData("AllExpectMethods")]
        public void TestClass_should_have_some_overload(MethodInfo method) {
            var methods = typeof(TestClass).GetMethods();
            Assert.NotNull(FindOverload(methods, method), "TestClass should have a similar overload");
        }

        [Theory]
        [PropertyData("AllExpectMethods")]
        public void Assert_should_have_some_overload(MethodInfo method) {
            var methods = typeof(Assert).GetMethods();
            Assert.NotNull(FindOverload(methods, method), "Assert should have a similar overload");
        }

        static MethodInfo FindOverload(MethodInfo[] methods, MethodInfo method) {
            return methods.SingleOrDefault(m => m.ToString() == method.ToString());
        }
    }
}
#endif
