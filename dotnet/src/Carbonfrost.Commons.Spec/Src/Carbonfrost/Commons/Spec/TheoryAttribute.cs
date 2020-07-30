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
using System.Reflection;
using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.Commons.Spec {

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class TheoryAttribute : Attribute, ITestTheoryAttributeApiConventions {

        public RetargetDelegates RetargetDelegates {
            get;
            set;
        }

        public string Reason {
            get;
            set;
        }

        void ITestUnitMetadataProvider.Apply(TestContext testContext) {
            this.ApplyRetargetDelegates(testContext);
        }

        TestUnit IReflectionTestUnitFactory.CreateTestCase(MethodInfo method) {
            return new ReflectedTheory(method);
        }
    }
}
