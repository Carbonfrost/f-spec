#if SELF_TEST

//
// Copyright 2016 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

    public class UtilityTests {

        [Fact]
        public void PathSafe_should_remove_slashes() {
            Assert.Equal("a-b-c", Utility.PathSafe("a/b/c"));
        }

        [Fact]
        public void PathSafe_should_remove_colons() {
            Assert.Equal("a-b", Utility.PathSafe("a::b"));
        }
    }
}
#endif
