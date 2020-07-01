//
// Copyright 2018, 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Reflection;
using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.Commons.Spec {

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class TestFileDataAttribute : Attribute, ITestDataProvider {

        private readonly TestFileInput _input;

        public string PathPattern {
            get {
                return _input.PathPattern;
            }
        }

        public Uri Url {
            get {
                return _input.Url;
            }
        }

        public string Name {
            get;
            set;
        }

        public TestFileDataAttribute(string pathPattern) {
            _input = new TestFileInput(pathPattern);
        }

        IEnumerable<TestData> ITestDataProvider.GetData(TestContext context) {
            TestUnit unit = context.TestUnit;
            var rt = (TestTheory) unit;
            var pms = rt.TestMethod.GetParameters();
            if (pms.Length != 0) {
                throw SpecFailure.TestFileDataRequiresOneParameter();
            }
            var pt = pms[0].ParameterType.GetTypeInfo();
            return _input.ReadInputs(context,
                                     u => ToTestData(context.DownloadFile(u), pt),
                                     f => ToTestData(context.LoadFile(f.FileName), pt));
        }

        public override string ToString() {
            return string.Format("TestFileData({0})", PathPattern);
        }

        internal TestData ToTestData(TestFile fixture, TypeInfo parameterType) {
            string name = Name;
            if (parameterType.AsType() == typeof(TestFile)) {
                return new TestData(fixture).WithName(name);
            }

            if (parameterType.IsGenericType && parameterType.GetGenericTypeDefinition() == typeof(TestFile<>)) {
                var type = parameterType.GetGenericArguments()[0];
                return new TestData(fixture.Typed(type)).WithName(name);
            }

            //
            return new TestData(fixture.Typed(parameterType.AsType())).WithName(name);
        }
    }

}
