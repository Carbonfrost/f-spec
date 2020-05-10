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

using System.Linq;

using Carbonfrost.Commons.Spec;

namespace Carbonfrost.SelfTest.Spec {

    public class TextUtilityTests : TestClass {

        [Fact]
        public void ConvertToString_linq_operator_should_reduce_noise() {
            var cast = new object[] { 3, "string" }.OfType<string>();
            Assert.Equal(
                "<OfTypeIterator><String> { \"string\" }",
                TextUtility.ConvertToString(cast)
            );
        }
    }
}
#endif
