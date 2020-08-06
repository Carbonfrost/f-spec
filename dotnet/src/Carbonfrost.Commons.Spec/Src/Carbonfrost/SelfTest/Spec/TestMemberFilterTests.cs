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

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.SelfTest.Spec.TestMatchers {

    public class TestMemberFilterTests {

        [Fact]
        public void TestMemberFilter_and_Matchers_have_same_API() {
            Assert.SetEqual(
                ApiNames(typeof(TestMemberFilter)),
                ApiNames(typeof(Matchers))
            );
        }

        [Fact]
        public void Properties_gets_all_properties() {
            Assert.SetEqual(
                new [] { "PrivateProperty", "PublicProperty" },
                Names(TestMemberFilter.Properties)
            );
        }

        [Fact]
        public void Fields_gets_all_fields() {
            Assert.SetEqual(
                new [] { "PrivateField", "PublicField", },
                Names(TestMemberFilter.Fields)
            );
        }

        [Fact]
        public void All_gets_all_members_except_compiler_generated() {
            Assert.SetEqual(
                new [] { "PrivateField", "PublicField", "PrivateProperty", "PublicProperty" },
                Names(TestMemberFilter.All)
            );
        }

        private static IEnumerable<string> Names(TestMemberFilter filter) {
            return filter.GetMembers(typeof(PObject).GetTypeInfo()).Select(p => p.Name);
        }

        private static IEnumerable<string> ApiNames(Type type) {
            return type.GetTypeInfo().GetProperties().Where(
                p => p.PropertyType == typeof(TestMemberFilter)
            ).Select(p => p.Name);
        }

#pragma warning disable 0169
#pragma warning disable 0649
        class PObject {
            public int PublicField;
            private int PrivateField;

            public int PublicProperty {
                get;
                set;
            }

            private int PrivateProperty {
                get;
                set;
            }
        }
    }
}
#endif
