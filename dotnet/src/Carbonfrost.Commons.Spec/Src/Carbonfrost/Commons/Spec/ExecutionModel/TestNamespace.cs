//
// Copyright 2016, 2017, 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

using System.Collections.Generic;
using System.Reflection;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    class TestNamespace : TestUnit {

        private readonly string _namespace;
        private readonly TestUnitCollection _children;

        public Assembly Assembly {
            get {
                return ((TestAssembly) Parent).Assembly;
            }
        }

        public string Namespace {
            get {
                return _namespace;
            }
        }

        public sealed override TestUnitCollection Children {
            get {
                return _children;
            }
        }

        internal override TestUnitMetadata Metadata {
            get {
                return TestUnitMetadata.Empty;
            }
        }

        public TestNamespace(string ns, IEnumerable<TestUnit> typeUnits) {
            _namespace = ns;
            _children = new TestUnitCollection(this);
            foreach (var unit in typeUnits) {
                if (unit == null) {
                    continue;
                }
                Children.Add(unit);
            }
        }

        public override string DisplayName {
            get {
                return Namespace;
            }
        }

        public override TestUnitType Type {
            get {
                return TestUnitType.Namespace;
            }
        }

    }
}
