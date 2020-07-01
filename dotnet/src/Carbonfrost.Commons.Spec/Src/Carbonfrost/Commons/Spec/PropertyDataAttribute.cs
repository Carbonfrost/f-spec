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
    public sealed class PropertyDataAttribute : Attribute, ITestDataApiAttributeConventions, IReflectionTestCaseFactory {

        private readonly string[] _properties;
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

        public IReadOnlyList<string> Properties {
            get {
                return _properties;
            }
        }

        public RetargetDelegates RetargetDelegates {
            get;
            set;
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

        public PropertyDataAttribute(params string[] properties) {
            _properties = properties;
        }

        public PropertyDataAttribute(string property) {
            _properties = new [] { property };
        }

        public PropertyDataAttribute(string property1, string property2) {
            _properties = new [] { property1, property2 };
        }

        public PropertyDataAttribute(string property1, string property2, string property3) {
            _properties = new [] { property1, property2, property3 };
        }

        public override string ToString() {
            return string.Format("PropertyData({0})", string.Join(", ", _properties));
        }

        IEnumerable<TestData> ITestDataProvider.GetData(TestContext context) {
            TestUnit unit = context.TestUnit;
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
            return this.WithNames(TestDataProvider.FromMemberAccessors(all).GetData(context), _tags.TestTags);
        }

        TestCaseInfo IReflectionTestCaseFactory.CreateTestCase(MethodInfo method, int index, TestData row) {
            return new ReflectedTheoryCase(method, index, row) {
                RetargetDelegates = RetargetDelegates,
            };
        }
    }
}
