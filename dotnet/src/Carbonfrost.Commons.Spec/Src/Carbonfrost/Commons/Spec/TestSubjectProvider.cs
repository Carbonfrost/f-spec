//
// Copyright 2017, 2018 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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


namespace Carbonfrost.Commons.Spec {

    public abstract class TestSubjectProvider : ITestSubjectProvider {

        static Func<Type, bool> Never = _ => false;

        public static readonly ITestSubjectProvider Default = new DefaultImpl();

        public virtual IEnumerable<object> GetTestSubjects(TestContext context) {
            return GetTestSubjectTypes(context).Select(t => Activator.CreateInstance(t));
        }

        protected abstract IEnumerable<Type> GetTestSubjectTypes(TestContext context);

        internal static ITestSubjectProvider ForType(Type testClassType) {
            var attr = (TestSubjectProviderAttribute) testClassType.GetTypeInfo().GetCustomAttribute(typeof(TestSubjectProviderAttribute));
            if (attr != null) {
                return (ITestSubjectProvider) Activator.CreateInstance(attr.SubjectProviderType);
            }

            Type _;
            var t = TestSubjectProvider.FindTestClassOfT(testClassType, out _);
            if (t != null) {
                return Default;
            }
            return null;
        }

        internal static Func<Type, bool> GetTestSubjectTypePredicate(Type type) {
            Type subjectType;
            var testClassType = FindTestClassOfT(type, out subjectType);
            if (testClassType == null) {
                return Never;
            }

            var gps = type.GetGenericArguments();
            if (gps.Length == 0) {
                // A : TestClass<SubjectType>
                return subjectType.IsAssignableFrom;
            }
            else if (gps.Length == 1 && testClassType.GetGenericArguments()[0] == gps[0]) {
                // A<T> : TestClass<A>
                return t => gps[0].GetTypeInfo().GetGenericParameterConstraints().All(u => u.GetTypeInfo().IsAssignableFrom(t));
            }
            else {
                throw new NotImplementedException();
            }
        }

        internal static TypeInfo FindTestClassOfT(Type t, out Type subjectType) {
            var ti = t.GetTypeInfo();
            subjectType = null;
            var baseType = ti.BaseType == null ? null : ti.BaseType.GetTypeInfo();
            if (baseType != null && baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(TestClass<>)) {
                subjectType = baseType.GetGenericArguments()[0];
                return baseType;
            }
            return null;
        }

        class DefaultImpl : TestSubjectProvider {

            protected override IEnumerable<Type> GetTestSubjectTypes(TestContext context) {
                var pred = GetTestSubjectTypePredicate(context.CurrentTest.FindTestClass().TestClass);
                Func<Type, bool> canActivateType = t => !(t.GetTypeInfo().IsInterface || t.GetTypeInfo().IsAbstract);
                return Activation.AllTypes().Where(pred).Where(canActivateType);
            }

        }
    }
}
