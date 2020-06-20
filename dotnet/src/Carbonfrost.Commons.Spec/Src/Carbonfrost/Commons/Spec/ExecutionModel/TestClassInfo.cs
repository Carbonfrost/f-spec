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
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    public abstract class TestClassInfo : TestUnit {

        private readonly Type _type;
        private readonly TestUnitCollection _children;

        public override TestUnitType Type {
            get {
                return TestUnitType.Class;
            }
        }

        internal override TestUnitMetadata Metadata {
            get {
                return new TestUnitMetadata(
                    TestClass.GetTypeInfo().GetCustomAttributes(false).Cast<Attribute>()
                );
            }
        }

        public Type TestClass {
            get {
                return _type;
            }
        }

        public sealed override TestUnitCollection Children {
            get {
                return _children;
            }
        }

        public virtual ITestSubjectProvider TestSubjectProvider {
            get {
                return null;
            }
        }

        public override string DisplayName {
            get {
                return TextUtility.ConvertToSimpleTypeName(TestClass, qualified: true);
            }
        }

        private protected TestClassInfo(Type type) {
            if (type == null) {
                throw new ArgumentNullException(nameof(type));
            }
            if (!type.IsClass) {
                throw new ArgumentException();
            }
            _type = type;
            _children = new TestUnitCollection(this);
        }

        internal override TestClassInfo FindTestClass() {
            return this;
        }

        internal static void AddTestMethods(Type testType, TestUnitCollection addTo) {
            foreach (var m in testType.GetRuntimeMethods()) {
                try {
                    var myCase = CreateTest(m);
                    if (myCase != null) {
                        addTo.Add(myCase);
                    }
                } catch (Exception ex) {
                    addTo.Add(SkippedInitFailure.CreateTestUnitFactoryProblem(m, ex));
                }
            }
            foreach (var p in testType.GetRuntimeProperties()) {
                try {
                    var myCase = CreateTest(p);
                    if (myCase != null) {
                        addTo.Add(myCase);
                    }
                } catch (Exception ex) {
                    addTo.Add(SkippedInitFailure.CreateTestUnitFactoryProblem(p.GetMethod, ex));
                }
            }
        }

        internal static TestUnit CreateTest(PropertyInfo property) {
            if (property == null) {
                throw new ArgumentNullException(nameof(property));
            }
            var attrs = (property.GetCustomAttributes() ?? Empty<Attribute>.Array)
                .Concat(property.GetMethod.GetCustomAttributes() ?? Empty<Attribute>.Array);
            return CreateCore(property.GetMethod, attrs);
        }

        internal static TestUnit CreateTest(MethodInfo method) {
            if (method == null) {
                throw new ArgumentNullException(nameof(method));
            }
            var attrs = method.GetCustomAttributes();
            return CreateCore(method, attrs);
        }

        static TestUnit CreateCore(MethodInfo method, IEnumerable<Attribute> attrs) {
            TestUnit testCase = null;
            foreach (var attr in attrs.OfType<IReflectionTestUnitFactory>()) {
                if (testCase == null) {
                    testCase = attr.CreateTestCase(method);
                } else {
                    throw SpecFailure.MultipleTestUnitFactories();
                }
            }
            return testCase;
        }

    }
}
