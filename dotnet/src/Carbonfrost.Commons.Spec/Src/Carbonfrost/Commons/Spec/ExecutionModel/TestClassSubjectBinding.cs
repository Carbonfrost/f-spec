//
// Copyright 2017, 2018 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

    class TestClassSubjectBinding : ReflectedTestClass {

        private readonly object _testSubject;
        private readonly object _testObject;

        public object TestSubject {
            get {
                return _testSubject;
            }
        }

        public override TestUnitType Type {
            get {
                return TestUnitType.SubjectClassBinding;
            }
        }

        internal TestClassSubjectBinding(Type testClassType, object testSubject) : base(testClassType) {
            if (testSubject == null) {
                throw new ArgumentNullException("testSubject");
            }

            _testSubject = testSubject;
            _testObject = Activator.CreateInstance(testClassType);
            _testObject.SetProperty("Subject", testSubject);
        }

        public override string DisplayName {
            get {
                return _testSubject.GetType().FullName;
            }
        }

        public override object FindTestObject() {
            return _testObject;
        }

        public override object FindTestSubject() {
            return _testSubject;
        }

        protected override void Initialize(TestContext testContext) {
            IEnumerable<Attribute> attrs = TestType.GetTypeInfo().GetCustomAttributes(false).Cast<Attribute>();
            attrs.ApplyMetadata(testContext);
            AddTestMethods();
        }
    }
}
