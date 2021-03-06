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
using System.Linq;
using Carbonfrost.Commons.Spec.ExecutionModel;
using Carbonfrost.Commons.Spec;
using System.Reflection;

namespace Carbonfrost.Commons.Spec {

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class XTestFileDataAttribute : Attribute, ITestDataProvider, IReflectionTestCaseFactory {

        private readonly TestFileDataAttribute _inner;

        public string PathPattern {
            get {
                return _inner.PathPattern;
            }
        }

        public Uri Url {
            get {
                return _inner.Url;
            }
        }

        public string Name {
            get {
                return _inner.Name;
            }
            set {
                _inner.Name = value;
            }
        }

        public string Reason {
            get;
            set;
        }

        public XTestFileDataAttribute(string pathPattern) {
            _inner = new TestFileDataAttribute(pathPattern);
        }

        public override string ToString() {
            return string.Format("XTestFileData({0})", PathPattern);
        }

        IEnumerable<TestData> ITestDataProvider.GetData(TestContext context) {
            return ((ITestDataProvider)_inner).GetData(context);
        }

        TestCaseInfo IReflectionTestCaseFactory.CreateTestCase(MethodInfo method, TestDataInfo row) {
            return new ReflectedTheoryCase(method, row) {
                IsPending = true,
                Reason = Reason,
            };
        }
    }
}
