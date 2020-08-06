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
using System.Diagnostics;
using System.Linq;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.SelfTest.Spec {

    public class DisplayActualSchemaTests {

        class PCirclularDep {

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public PCirclularDep A {
                get;
                set;
            }

            public int B {
                get;
                set;
            }
        }

        [Fact]
        public void Create_should_handle_circular_dependencies_on_hidden_root() {
            var schema = DisplayActualSchema.Create(typeof(PCirclularDep));
            Assert.HasCount(2, schema.Accessors);
            Assert.SetEqual(new [] { "A", "B" }, schema.Accessors.Select(a => a.Name));

            // The hidden root gets collapsed because there is no way to represent this clearly
            var display = DisplayActual.Create(new PCirclularDep());
            Assert.Equal("{ A = ..., B = 0 }", display.Format(DisplayActualOptions.None));
        }
    }
}
#endif
