#if SELF_TEST

//
// Copyright 2016, 2019 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Reflection;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.SelfTest.Spec {

    public class AssertTests {

        static readonly HashSet<string> IgnoredMethodNamesCompat = new HashSet<string> {
            "Expect",
            "Fail",
            "Given",
            "Pass",
            "Pending",
            "get_UseStrictMode",
            "set_UseStrictMode",

            "Equals", // from object.Equals
        };

        [Theory]
        [InlineData(typeof(Asserter))]
        [InlineData(typeof(Assume))]
        public void Assert_public_api_matches_compatible_classes(Type where) {
            // We expect that all methods on Assert are also present on Assume
            // (e.g. Assert.True() means we should have Assume.True())
            var actual = MethodNames(where);
            var expected = MethodNames(typeof(Assert));

            var diff = expected.Except(actual);
            Assert.HasCount(0, diff);
        }

        static IEnumerable<string> MethodNames(Type type) {
            return type.GetTypeInfo().GetRuntimeMethods()
                .Where(mi => mi.IsPublic && !IgnoredMethodNamesCompat.Contains(mi.Name))
                .Select(m => m.ToString());
        }
    }
}
#endif
