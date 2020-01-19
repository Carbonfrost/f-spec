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

    static class TestUnitMetadata {

        public static void ApplyMetadata(this IEnumerable<Attribute> attributes, TestContext ctx) {
            foreach (var attr in attributes) {
                var pro = attr as ITestUnitMetadataProvider;

                if (pro != null) {
                    pro.Apply(ctx);

                } else {
                    foreach (var mp in CreateProviders(attr)) {
                        mp.Apply(ctx);
                    }
                }
            }
        }

        internal static IEnumerable<ITestUnitMetadataProvider> CreateProviders(object adaptee) {
            var type = adaptee.GetType();

            IEnumerable<Type> interfaces = type.GetTypeInfo().GetInterfaces();
            if (type.GetTypeInfo().IsInterface) {
                interfaces = interfaces.Concat(new [] { type });
            }

            foreach (var i in interfaces) {
                if (!i.IsDefined(typeof(TestUnitMetadataProviderAttribute))) {
                    continue;
                }
                var requiredType = ((TestUnitMetadataProviderAttribute) i.GetCustomAttribute(typeof(TestUnitMetadataProviderAttribute)))
                    .MetadataProviderType;

                if (requiredType.GetTypeInfo().IsGenericTypeDefinition) {
                    // Propagate generics from the adaptee to the adapter.
                    // This allows the adapter role to specify an open generic type that
                    // binds automatically to whatever generics are on the adaptee type.
                    // Where's this used?  ITestMatcherFactory`1 and FactoryMetadataProvider`1
                    requiredType = requiredType.MakeGenericType(i.GetTypeInfo().GetGenericArguments());
                }
                var activationCtor = requiredType.GetConstructors(BindingFlags.Public | BindingFlags.Instance).First();
                var parameters = activationCtor.GetParameters();

                if (parameters.Length == 0) {
                    yield return (ITestUnitMetadataProvider) activationCtor.Invoke(Array.Empty<object>());
                } else {
                    yield return (ITestUnitMetadataProvider) activationCtor.Invoke(new [] { adaptee });
                }
            }
        }
    }
}
