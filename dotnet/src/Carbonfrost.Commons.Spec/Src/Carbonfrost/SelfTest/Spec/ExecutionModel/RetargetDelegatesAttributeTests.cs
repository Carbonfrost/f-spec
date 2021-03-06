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
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.Commons.Spec {
    class __RetargetAttribute: Attribute {}
}

namespace Carbonfrost.SelfTest.Spec.ExecutionModel {

    public class RetargetAttributeTests {

        [__RetargetAttribute]
        class PHasTransparent {}

        [Fact]
        public void IsRetargeted_is_transparently_detected() {
            Assert.True(RetargetAttribute.IsRetargeted(typeof(PHasTransparent)));
        }

    }

}
#endif
