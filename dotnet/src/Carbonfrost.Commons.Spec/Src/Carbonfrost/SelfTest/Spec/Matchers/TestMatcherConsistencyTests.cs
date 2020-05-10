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
#endif

using System;
using System.Collections.Generic;
using System.Linq;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.SelfTest.Spec.TestMatchers {

    public class TestMatcherConsistencyTests {

        public IEnumerable<Type> TestMatcherTypes {
            get {
                return GetType().Assembly.GetTypes().Where(
                    e => !e.IsNested && e.GetInterfaces().Any(IsMatcherInterface)
                );
            }
        }

        [Theory]
        [PropertyData(nameof(TestMatcherTypes))]
        public void TestMatcher_is_in_correct_namespace(Type type) {
            Assert.Equal("Carbonfrost.Commons.Spec.TestMatchers", type.Namespace);
        }

        static bool IsMatcherInterface(Type type) {
            return type == typeof(ITestMatcher) || type == typeof(ITestMatcher<>);
        }
    }

}
