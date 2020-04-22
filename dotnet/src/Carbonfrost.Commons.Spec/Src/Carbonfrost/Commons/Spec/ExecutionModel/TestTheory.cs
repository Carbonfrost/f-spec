//
// Copyright 2017, 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using Carbonfrost.Commons.Spec.Resources;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    public abstract class TestTheory : TestUnit {

        private readonly MethodInfo _method;

        public MethodInfo TestMethod {
            get {
                return _method;
            }
        }

        public virtual object TestObject {
            get {
                if (Parent == null) {
                    return null;
                }
                return Parent.FindTestObject();
            }
        }

        protected TestTheory(MethodInfo testMethod) {
            if (testMethod == null) {
                throw new ArgumentNullException(nameof(testMethod));
            }
            _method = testMethod;
        }

        public override TestUnitType Type {
            get {
                return TestUnitType.Theory;
            }
        }

        public override string DisplayName {
            get {
                return Parent.DisplayName + "." + TestMethod.Name;
            }
        }

        internal override TestUnitMetadata Metadata {
            get {
                return new TestUnitMetadata(
                    TestMethod.GetCustomAttributes(false).Cast<Attribute>()
                );
            }
        }

        protected override void BeforeExecuting(TestContext testContext) {
            if (Children.Count == 0) {
                testContext.Log.TheoryHasNoDataProviders();
                testContext.VerifiableProblem(SR.TheoryHasNoDataProviders());
            }
            base.BeforeExecuting(testContext);
        }
    }
}
