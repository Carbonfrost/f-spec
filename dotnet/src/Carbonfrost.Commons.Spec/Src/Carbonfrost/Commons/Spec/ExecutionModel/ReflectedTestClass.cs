//
// Copyright 2016-2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Reflection;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    class ReflectedTestClass : TestClassInfo {

        private object _instanceCache;
        private ITestSubjectProvider _subjectProviderCache;

        public override ITestSubjectProvider TestSubjectProvider {
            get {
                return _subjectProviderCache ??
                    (_subjectProviderCache = Spec.TestSubjectProvider.ForType(TestClass));
            }
        }

        internal ReflectedTestClass(Type type) : base(type) {
        }

        protected override void Initialize(TestContext testContext) {
            Metadata.Apply(testContext);

            // Either treat as a subject test class or a default one
            if (TestSubjectProvider != null) {
                var subjects = TestSubjectProvider.GetTestSubjects(testContext);
                foreach (var s in subjects) {
                    Type testClassType = ClosedTestClassType(TestClass, s);
                    var binding = new DefaultTestClassSubjectBinding(testClassType, s);
                    Children.Add(binding);
                }
                _instanceCache = TestUnitAdapter.Empty;

            } else {
                TestClassInfo.AddTestMethods(TestClass, Children);
            }

            Metadata.ApplyDescendants(testContext, Descendants);
        }

        internal override object FindTestObject() {
            if (_instanceCache == null) {
                try {
                    _instanceCache = Activator.CreateInstance(TestClass);
                } catch (TargetInvocationException ex) {
                    _instanceCache = new TypeLoadFailure(TestClass, ex.InnerException);
                } catch (Exception ex) {
                    _instanceCache = new TypeLoadFailure(TestClass, ex);
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
