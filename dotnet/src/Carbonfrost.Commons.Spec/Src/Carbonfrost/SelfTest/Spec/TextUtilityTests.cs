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
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.SelfTest.Spec {

    public class TextUtilityTests {

        [Fact]
        public void Fill_should_apply_format_strings() {
            var dict = new Dictionary<string, string> {
                { "BoundsExclusive", "true" },
            };
            Assert.Equal("hello (exclusive)", TextUtility.Fill("hello {BoundsExclusive:B:(exclusive)}", dict));
        }
    }
}
#endif
