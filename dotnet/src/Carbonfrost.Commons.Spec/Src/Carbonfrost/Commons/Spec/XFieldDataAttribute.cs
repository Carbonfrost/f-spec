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
using System.Collections.Generic;
using System.Reflection;

using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.Commons.Spec {

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class XFieldDataAttribute : Attribute, ITestDataApiAttributeConventions, IReflectionTestCaseFactory {

        private readonly FieldDataAttribute _inner;

        public string[] Tags {
            get {
                return _inner.Tags;
            }
            set {
                _inner.Tags = value;
            }
        }

        public string Tag {
            get {
                return _inner.Tag;
            }
            set {
                _inner.Tag = value;
            }
        }

        public IReadOnlyList<string> Fields {
            get {
                return _inner.Fields;
            }
        }

        public RetargetDelegates RetargetDelegates {
            get {
                return _inner.RetargetDelegates;
            }
            set {
                _inner.RetargetDelegates = value;
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
            get {
                return _inner.Reason;
            }
            set {
                _inner.Reason = value;
            }
        }

        public bool Explicit {
            get {
                return _inner.Explicit;
            }
            set {
                _inner.Explicit = value;
            }
        }

        public XFieldDataAttribute(params string[] fields) {
            _inner = new FieldDataAttribute(fields);
        }

        public XFieldDataAttribute(string field) {
            _inner = new FieldDataAttribute(field);
        }

        public XFieldDataAttribute(string field1, string field2) {
            _inner = new FieldDataAttribute(field1, field2);
        }

        public XFieldDataAttribute(string field1, string field2, string field3) {
            _inner = new FieldDataAttribute(field1, field2, field3);
        }

        public override string ToString() {
            return string.Format("XFieldData({0})", string.Join(", ", Fields));
        }

        IEnumerable<TestData> ITestDataProvider.GetData(TestContext context) {
            return ((ITestDataProvider) _inner).GetData(context);
        }

        TestCaseInfo IReflectionTestCaseFactory.CreateTestCase(MethodInfo method, TestDataInfo row) {
            return new ReflectedTheoryCase(method, row) {
                IsPending = true,
                Reason = Reason,
                RetargetDelegates = RetargetDelegates,
            };
        }
    }
}
