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
using System.Threading;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.SelfTest.Spec {

    public class TestPropertyTests : TestClass {

        [Fact]
        public string Property {
            [return: Null]
            get {
                return null;
            }
        }

        [Fact]
        [PassExplicitly]
        [Tag("acceptance")]
        public string Acceptance_Property {
            get {
                Assert.Pass("Expected property getter executed");
                return null;
            }
        }

        [Fact]
        [ExpectedException(typeof(InvalidOperationException))]
        [Tag("acceptance")]
        public string Acceptance_InvalidOperationException {
            get {
                throw new InvalidOperationException();
            }
        }

        [Explicit("This test is designed to fail.  Assertions happen on the output")]
        [Fact]
        [Tag("acceptance")]
        [Timeout(20)]
        public string Acceptance_Timeout {
            get {
                Thread.Sleep(500);
                return null;
            }
        }
    }
}
#endif
