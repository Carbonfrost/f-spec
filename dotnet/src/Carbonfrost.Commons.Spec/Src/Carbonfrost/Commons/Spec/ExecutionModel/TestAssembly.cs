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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    class TestAssembly : TestUnit {

        private readonly Assembly _assembly;
        private readonly TestUnitCollection _children;

        public Assembly Assembly {
            get {
                return _assembly;
            }
        }

        public override string DisplayName {
            get {
                return Utility.PrettyCodeBase(_assembly);
            }
        }

        public override TestUnitType Type {
            get {
                return TestUnitType.Assembly;
            }
        }

        public sealed override TestUnitCollection Children {
            get {
                return _children;
            }
        }

        internal override TestUnitMetadata Metadata {
            get {
                return new TestUnitMetadata(
                    _assembly.GetCustomAttributes()
                );
            }
        }

        private TestAssembly(Assembly assembly) {
            _assembly = assembly;
            _children = new TestUnitCollection(this);
        }

        public static TestUnit Create(Assembly assembly) {
            if (assembly == null) {
                throw new ArgumentNullException(nameof(assembly));
            }
            return new TestAssembly(assembly);
        }

        protected override void Initialize(TestContext testContext) {
            Metadata.Apply(testContext);

            foreach (var nsGroup in _assembly.ExportedTypes.GroupBy(t => t.Namespace)) {
                var tests = nsGroup.Select(t => TestUnitFromType(t));
                var unit = new TestNamespace(nsGroup.Key, tests);
                SpecLog.DiscoveredTests(unit.Children);
                if (unit.Children.Count == 0) {
                    // if the ns has no tests, we don't even report it exists
                    continue;
                }

                Children.Add(unit);
            }

            Metadata.ApplyDescendants(testContext, Descendants);
        }

        internal ReflectedTestClass TestUnitFromType(Type type) {
            if (type == null) {
                throw new ArgumentNullException("type");
            }
            var tt = type.GetTypeInfo();
            if (tt.IsAbstract || tt.IsValueType || tt.IsNested || !tt.IsVisible) {
                return null;
            }

            if (typeof(ITestUnitAdapter).IsAssignableFrom(type)) {
                return new UserTestClassAdapter(type);
            }

            if (type.GetRuntimeMethods().SelectMany(m => m.CustomAttributes).Any(
                a => typeof(IReflectionTestUnitFactory).IsAssignableFrom(a.AttributeType))) {
                return new ReflectedTestClass(type);
            }

            return null;
        }
    }
}
