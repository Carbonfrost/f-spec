//
// Copyright 2016, 2017 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

namespace Carbonfrost.Commons.Spec {

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class FieldDataAttribute : Attribute, ITestDataProvider {

        private readonly string[] _fields;

        public IReadOnlyList<string> Fields {
            get {
                return _fields;
            }
        }

        public string Name { get; set; }

        public FieldDataAttribute(params string[] fields) {
            _fields = fields;
        }

        public override string ToString() {
            return string.Format("FieldData({0})", string.Join(", ", _fields));
        }

        private IEnumerable<TestData> WithNames(IEnumerable<TestData> data) {
            if (string.IsNullOrEmpty(Name)) {
                return data;
            }
            return data.Select(d => d.WithName(Name));
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
            return WithNames(TestDataProvider.FromMemberAccessors(all).GetData(context));
        }
    }
}
