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
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    public abstract class TestAssembly : TestUnit {

        private readonly Assembly _assembly;
        private readonly TestUnitCollection _children;
        private readonly TestAssemblyOptions _options;

        public Assembly Assembly {
            get {
                return _assembly;
            }
        }

        public TestAssemblyOptions Options {
            get {
                return _options;
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

        private protected TestAssembly(Assembly assembly) {
            _assembly = assembly;
            _children = new TestUnitCollection(this);
            _options = TestAssemblyOptions.ForAssembly(assembly);
        }

        private class DefaultTestAssembly : TestAssembly {
            public DefaultTestAssembly(Assembly assembly) : base(assembly) {
            }
        }

        public static TestAssembly Create(Assembly assembly) {
            if (assembly == null) {
                throw new ArgumentNullException(nameof(assembly));
            }
            return new DefaultTestAssembly(assembly);
        }

        protected override void Initialize(TestContext testContext) {
            Metadata.Apply(testContext);

            foreach (var nsGroup in GetTestTypes().GroupBy(t => t.Namespace)) {
                var tests = nsGroup.Select(t => TestUnitFromType(t));
                var unit = TestNamespace.Create(nsGroup.Key, tests);
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
                throw new ArgumentNullException(nameof(type));
            }
            if (!IsTestClassByAccess(type)) {
                return null;
            }
            if (typeof(ITestUnitAdapter).IsAssignableFrom(type)) {
                return new UserTestClassAdapter(type);
            }
            if (IsTestClassByConvention(type)) {
                return new ReflectedTestClass(type);
            }

            return null;
        }

        private IEnumerable<Type> GetTestTypes() {
            if (Options.IncludeNonPublicTests) {
                return _assembly.GetTypes();
            }
            return _assembly.ExportedTypes;
        }

        internal bool IsTestClassByAccess(Type type) {
            var tt = type.GetTypeInfo();
            if (!tt.IsClass || tt.IsAbstract || tt.IsValueType || tt.IsNested) {
                return false;
            }

            if (!Options.IncludeNonPublicTests && !tt.IsVisible) {
                return false;
            }

            return true;
        }

        private bool IsTestClassByConvention(Type type) {
            return type.GetRuntimeMethods().SelectMany(m => m.CustomAttributes).Any(
                a => typeof(IReflectionTestUnitFactory).IsAssignableFrom(a.AttributeType)
            );
        }
    }
}
