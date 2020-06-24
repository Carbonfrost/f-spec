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
using System.Collections.Generic;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.SelfTest.Spec {

    public class FieldDataAttributeTests {
        public string State;

        public readonly IEnumerable<string> FieldDataExample = new [] { "A", "B" };

        [FieldData(nameof(FieldDataExample))]
        public void Create_new_instance_on_FieldData_test_cases(string name) {
            Assert.Empty(State);

            // Detect shared state by setting something
            State = name;
        }
    }
}
#endif
