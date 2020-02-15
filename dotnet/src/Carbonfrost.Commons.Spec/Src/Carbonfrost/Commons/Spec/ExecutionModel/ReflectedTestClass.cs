//
// Copyright 2016-2018 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    class ReflectedTestClass : TestUnit {

        private readonly Type _type;
        private object _instanceCache;
        private ITestSubjectProvider _subjectProviderCache;
        private readonly TestUnitCollection _children;

        public Type TestType {
            get {
                return _type;
            }
        }

        public override TestUnitType Type {
            get {
                return TestUnitType.Class;
            }
        }

        public override ITestSubjectProvider TestSubjectProvider {
            get {
                return _subjectProviderCache ??
                    (_subjectProviderCache = Spec.TestSubjectProvider.ForType(TestType));
            }
        }

        public sealed override TestUnitCollection Children {
            get {
                return _children;
            }
        }

        internal ReflectedTestClass(Type type) {
            _type = type;
            _children = new TestUnitCollection(this);
        }

        public override string DisplayName {
            get {
                return _type.FullName;
            }
        }

        protected override void Initialize(TestContext testContext) {
            IEnumerable<Attribute> attrs = TestType.GetTypeInfo().GetCustomAttributes(false).Cast<Attribute>();
            attrs.ApplyMetadata(testContext);

            // Either treat as a subject test class or a default one
            if (TestSubjectProvider != null) {
                var subjects = TestSubjectProvider.GetTestSubjects(testContext);
                foreach (var s in subjects) {
                    Type testClassType = ClosedTestClassType(TestType, s);
                    var binding = new TestClassSubjectBinding(testClassType, s);
                    Children.Add(binding);
                }
                _instanceCache = TestUnitAdapter.Empty;

            } else {
                AddTestMethods();
            }
        }

        public override object FindTestObject() {
            if (_instanceCache == null) {
                try {
                    _instanceCache = Activator.CreateInstance(_type);
                } catch (TargetInvocationException ex) {
                    _instanceCache = new TypeLoadFailure(_type, ex.InnerException);
                } catch (Exception ex) {
                    _instanceCache = new TypeLoadFailure(_type, ex);
                }
            }
            return _instanceCache;
        }

        protected override void BeforeExecuting(TestContext testContext) {
            if (Children.Count > 0) {
                return;
            }

            if (TestSubjectProvider != null) {
                testContext.Log.NoTestSubjects();
            } else {
                testContext.Log.NoTestMethods();
            }

            base.BeforeExecuting(testContext);
        }

        internal void AddTestMethods() {
            Type testType = TestType;
            foreach (var m in testType.GetRuntimeMethods()) {
                var attrs = m.GetCustomAttributes();
                var myCase = CreateCase(m, attrs);
                if (myCase != null) {
                    Children.Add(myCase);
                }
            }
            foreach (var p in testType.GetRuntimeProperties()) {
                var attrs = (p.GetCustomAttributes() ?? Empty<Attribute>.Array)
                    .Concat(p.GetMethod.GetCustomAttributes() ?? Empty<Attribute>.Array);
                var myCase = CreateCase(p.GetMethod, attrs);
                if (myCase != null) {
                    Children.Add(myCase);
                }
            }
        }

        private TestUnit CreateCase(MethodInfo method, IEnumerable<Attribute> attrs) {
            if (attrs == null) {
                return null;
            }
            TestUnit testCase = null;
            foreach (var attr in attrs.OfType<IReflectionTestUnitFactory>()) {
                if (testCase == null) {
                    testCase = attr.CreateTestCase(method);
                } else {
                    throw new NotImplementedException();
                }
            }

            return testCase;
        }

        private static Type ClosedTestClassType(Type testClassType, object testSubject) {
            // If the test class is generic, then bind it given the subject
            if (testClassType.GetTypeInfo().IsGenericType) {
                testClassType = testClassType.MakeGenericType(testSubject.GetType());
            }
            return testClassType;
        }

        class TypeLoadFailure : ITestCaseFilter, ITestUnitAdapter {
            private readonly Type _type;
            private readonly Exception _ex;

            public TypeLoadFailure(Type type, Exception ex) {
                _type = type;
                _ex = ex;
            }

            public void RunTest(TestContext context, Action<TestContext> next) {
                throw SpecFailure.CouldNotLoadType(_type, _ex);
            }

            public void Initialize(TestContext testContext) {
            }

            public void BeforeExecuting(TestContext testContext) {
            }

            public void AfterExecuting(TestContext testContext) {
            }

            public void BeforeExecutingDescendant(TestContext descendantTestContext) {
            }

            public void AfterExecutingDescendant(TestContext descendantTestContext) {
            }

        }

    }
}
