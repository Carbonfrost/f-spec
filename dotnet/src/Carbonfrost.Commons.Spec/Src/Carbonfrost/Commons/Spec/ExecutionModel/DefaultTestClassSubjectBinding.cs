//
// Copyright 2017, 2018, 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    class DefaultTestClassSubjectBinding : TestSubjectClassBinding {

        private readonly object _testSubject;

        public override object TestSubject {
            get {
                return _testSubject;
            }
        }

        internal override TestUnitMetadata Metadata {
            get {
                return new TestUnitMetadata(
                    TestClass.GetTypeInfo().GetCustomAttributes(false).Cast<Attribute>()
                );
            }
        }

        internal DefaultTestClassSubjectBinding(Type testClassType, object testSubject) : base(testClassType) {
            if (testSubject == null) {
                throw new ArgumentNullException(nameof(testSubject));
            }

            _testSubject = testSubject;
        }

        public override string DisplayName {
            get {
                return _testSubject.GetType().FullName;
            }
        }

        internal override object FindTestSubject() {
            return _testSubject;
        }

        internal override object CreateTestObject() {
            var result = Activator.CreateInstance(TestClass);
            result.SetProperty("Subject", _testSubject);
            return result;
        }

        protected override void Initialize(TestContext testContext) {
            Metadata.Apply(testContext);
            TestClassInfo.AddTestMethods(TestClass, Children);
            Metadata.ApplyDescendants(testContext, Descendants);
        }
    }
}
