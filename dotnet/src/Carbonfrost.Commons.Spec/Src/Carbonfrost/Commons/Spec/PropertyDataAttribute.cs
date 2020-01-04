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
    public sealed class PropertyDataAttribute : Attribute, ITestDataProvider {

        private readonly string[] _properties;

        public IReadOnlyList<string> Properties {
            get {
                return _properties;
            }
        }

        public string Name { get; set; }

        public PropertyDataAttribute(params string[] properties) {
            _properties = properties;
        }

        public override string ToString() {
            return string.Format("PropertyData({0})", string.Join(", ", _properties));
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
            foreach (var p in _properties) {
                var prop = declaringType.GetProperty(p, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
                if (prop == null) {
                    throw SpecFailure.CannotFindDataProperty(p);
                }

                if (prop.GetMethod == null) {
                    throw SpecFailure.DataPropertyIncorrectGetter();
                }
                all.Add(MemberAccessors.Property(prop));
            }
            return WithNames(TestDataProvider.FromMemberAccessors(all).GetData(context));
        }
    }
}
