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
using System.Collections.Generic;
using System.Reflection;
using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.Commons.Spec {

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class FieldDataAttribute : Attribute, ITestDataApiAttributeConventions {

        private readonly string[] _fields;
        private readonly TestTagCache _tags = new TestTagCache();

        public string[] Tags {
            get {
                return _tags.Tags;
            }
            set {
                _tags.Tags = value;
            }
        }

        public string Tag {
            get {
                return _tags.Tag;
            }
            set {
                _tags.Tag = value;
            }
        }

        public IReadOnlyList<string> Fields {
            get {
                return _fields;
            }
        }

        public string Name {
            get;
            set;
        }

        public string Reason {
            get;
            set;
        }

        public bool Explicit {
            get;
            set;
        }

        public FieldDataAttribute(params string[] fields) {
            _fields = fields;
        }

        public FieldDataAttribute(string field) {
            _fields = new [] { field };
        }

        public FieldDataAttribute(string field1, string field2) {
            _fields = new [] { field1, field2 };
        }

        public FieldDataAttribute(string field1, string field2, string field3) {
            _fields = new [] { field1, field2, field3 };
        }

        public override string ToString() {
            return string.Format("FieldData({0})", string.Join(", ", _fields));
        }

        IEnumerable<TestData> ITestDataProvider.GetData(TestContext context) {
            TestUnit unit = context.CurrentTest;
            var declaringType = ((TestTheory) unit).TestMethod.DeclaringType;
            var all = new List<IMemberAccessor>();
            foreach (var f in _fields) {
                var fld = declaringType.GetField(f, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
                if (fld == null) {
                    throw SpecFailure.CannotFindDataField(f);
                }
                all.Add(MemberAccessors.Field(fld));
            }
            return this.WithNames(TestDataProvider.FromMemberAccessors(all).GetData(context), _tags.TestTags);
        }
    }
}
