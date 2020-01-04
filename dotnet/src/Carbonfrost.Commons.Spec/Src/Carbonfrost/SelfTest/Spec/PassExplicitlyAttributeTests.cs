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
using System.Linq;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.SelfTest.Spec {

    public class PassExplicitlyAttributeTests {

        [Explicit("This test is designed to fail.  Assertions happen on the output")]
        [Fact]
        [PassExplicitly]
        public void Acceptance_pass_explicitly_required() {
        }

        [Explicit("This test is designed to fail.  Assertions happen on the output")]
        [Fact]
        [PassExplicitly]
        public void Acceptance_pass_explicitly_pending() {
            Assert.Pending();
        }

        [Explicit("This test is designed to fail.  Assertions happen on the output")]
        [Fact]
        [PassExplicitly]
        public void Acceptance_pass_explicitly_fail() {
            Assert.Fail();
        }

        [Fact]
        [PassExplicitly]
        public void Acceptance_pass_explicitly_should_work() {
            Assert.Pass();
        }
    }
}
#endif
