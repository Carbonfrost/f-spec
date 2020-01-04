#if SELF_TEST

//
// Copyright 2016, 2018 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

using System.Collections.Generic;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.SelfTest.Spec {

    public class TestDataBinderTests {

        public void FakeFixtureMethod(FixtureObject fob) {
        }

        public void FakeFixtureMethod2(string a, string b) {
        }

        public void FakeFixtureMethod3(string a) {
        }

        [Fact]
        public void Create_should_bind_singleton_parameter() {
            var dict = new Dictionary<string, string> {
                { "a", "" },
                { "b", "false" },
            };
            var method = typeof(TestDataBinderTests).GetMethod("FakeFixtureMethod");
            var binder = TestDataBinder.Create(method, dict.Keys);
            var fob = new FixtureObject { A = "", B = "false" };
            Assert.Equal(new object[] { fob }, binder.Bind(dict));
        }

        [Fact]
        public void Create_should_bind_parameters_by_name() {
            var dict = new Dictionary<string, string> {
                { "a", "" },
                { "b", "false" },
            };
            var method = typeof(TestDataBinderTests).GetMethod("FakeFixtureMethod2");
            var binder = TestDataBinder.Create(method, dict.Keys);
            Assert.Equal(new object[] { "", "false" }, binder.Bind(dict));
        }

        [Fact]
        public void Create_should_bind_parameters_by_name_singleton_key_set() {
            var dict = new Dictionary<string, string> {
                { "a", "" },
            };
            var method = typeof(TestDataBinderTests).GetMethod("FakeFixtureMethod3");
            var binder = TestDataBinder.Create(method, dict.Keys);
            Assert.Equal(new object[] { "" }, binder.Bind(dict));
        }

        public class FixtureObject {

            public string A { get; set; }
            public string B { get; set; }

            public override bool Equals(object obj) {
                FixtureObject other = obj as FixtureObject;
                if (other == null) {
                    return false;
                }
                return A == other.A && B == other.B;
            }

            public override int GetHashCode() {
                int hashCode = 0;
                unchecked {
                    if (A != null) {
                        hashCode += 1000000007 * A.GetHashCode();
                    }
                    if (B != null) {
                        hashCode += 1000000009 * B.GetHashCode();
                    }
                }
                return hashCode;
            }
        }
    }
}
#endif
