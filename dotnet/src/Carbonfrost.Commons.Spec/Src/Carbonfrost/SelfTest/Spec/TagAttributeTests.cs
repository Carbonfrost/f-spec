#if SELF_TEST

//
// Copyright 2017, 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

namespace Carbonfrost.SelfTest.Spec {

    public class TagAttributeTests : TestClass {

        [Fact]
        [Tag("d")]
        public void CurrentTest_should_have_tags() {
            Assert.Contains("d", TestContext.CurrentTest.Tags);
        }

        [Fact]
        [Tag("name:value")]
        public void CurrentTest_should_have_qualified_tags() {
            Assert.Contains("name:value", TestContext.CurrentTest.Tags);
        }

        [Fact]
        [Tag("name:value")]
        public void CurrentTest_should_have_qualified_tags_by_name() {
            Assert.Contains("name", TestContext.CurrentTest.Tags);
        }
    }
}
#endif
