#if SELF_TEST

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
using System.Reflection;
using Carbonfrost.Commons.Spec;
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.SelfTest.Spec {

    public class TestMatcherFactoryConsistencyTests : TestClass {

        public IEnumerable<TypeInfo> AttributeTypes {
            get {
                return typeof(Extensions).GetTypeInfo().Assembly.ExportedTypes
                    .Where(t => typeof(Attribute).IsAssignableFrom(t))
                    .Select(t => t.GetTypeInfo());
            }
        }

        public IEnumerable<TypeInfo> TestMatcherFactoryAttributeTypes {
            get {
                return AttributeTypes.Where(t => typeof(ITestMatcherFactory).GetTypeInfo().IsAssignableFrom(t));
            }
        }

        public IEnumerable<TypeInfo> TestMatcherFactoryOfTAttributeTypes {
            get {
                foreach (var attr in AttributeTypes) {
                    if (attr.GetInterfaces().Any(i => i.GetTypeInfo().IsGenericType && i.GetTypeInfo().GetGenericTypeDefinition() == typeof(ITestMatcherFactory<>))) {
                        yield return attr;
                    }
                }
            }
        }

        [Theory]
        [PropertyData("TestMatcherFactoryAttributeTypes")]
        public void AttributeUsage_should_have_correct_types(TypeInfo attrType) {
            var usage = (AttributeUsageAttribute) attrType.GetCustomAttribute(typeof(AttributeUsageAttribute));
            Assert.Equal(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property, usage.ValidOn);
        }

        [Theory]
        [PropertyData("TestMatcherFactoryOfTAttributeTypes")]
        public void AttributeUsage_should_have_correct_types_of_T(TypeInfo attrType) {
            var usage = (AttributeUsageAttribute) attrType.GetCustomAttribute(typeof(AttributeUsageAttribute));
            Assert.Equal(AttributeTargets.ReturnValue | AttributeTargets.Property, usage.ValidOn);
        }

        [Theory]
        [PropertyData("TestMatcherFactoryAttributeTypes")]
        [PropertyData("TestMatcherFactoryOfTAttributeTypes")]
        public void TestMatcherFactory_should_have_factory_methods(TypeInfo attrType) {
            string name = attrType.Name;
            string expected = name.Substring(0, name.Length - "Attribute".Length);

            var methods = typeof(TestMatcherFactory).GetMethods().Where(m => m.Name == expected).ToArray();
            Assert.HasCount(2, methods, "Should have {0}() and {0}(string,object[])", expected);

            // Look for the message and args method
            var parameters = methods.Select(t => t.GetParameters().Select(p => p.ParameterType).ToArray());
            Expect(parameters).ToHave.Single<IEnumerable<Type>>().EndsWith(
                new[] { typeof(string), typeof(object[]) }
            );
        }

        [Theory]
        [PropertyData("TestMatcherFactoryAttributeTypes")]
        [PropertyData("TestMatcherFactoryOfTAttributeTypes")]
        public void TestMatcherFactory_should_have_Message_property(TypeInfo attrType) {
            Assert.NotNull(attrType.GetDeclaredProperty("Message"), "Should have Message property");
        }
    }
}
#endif
