//
// Copyright 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     https://www.apache.org/licenses/LICENSE-2.0
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

    class TestUnitMetadata {
        private readonly IReadOnlyList<Attribute> _attrs;

        internal static readonly TestUnitMetadata Empty = new TestUnitMetadata(Array.Empty<Attribute>());

        public IEnumerable<ITestUnitDescendantMetadataProvider> DescendantProviders {
            get {
                return GetProvidersFromAttributes<ITestUnitDescendantMetadataProvider>(pp => pp.DescendantMetadataProviderType);
            }
        }

        public IEnumerable<ITestUnitMetadataProvider> Providers {
            get {
                return GetProvidersFromAttributes<ITestUnitMetadataProvider>(pp => pp.MetadataProviderType);
            }
        }

        public TestUnitMetadata(IEnumerable<Attribute> attrs) {
            _attrs = attrs.ToArray();
        }

        public void Apply(TestContext ctx) {
            foreach (var pro in Providers) {
                pro.Apply(ctx);
            }
        }

        public void ApplyDescendants(TestContext ctx, IEnumerable<TestUnit> descendants) {
            foreach (var pro in DescendantProviders) {
                foreach (var desc in descendants) {
                    pro.ApplyDescendant(ctx.WithSelf(desc));
                }
            }
        }

        private IEnumerable<T> GetProvidersFromAttributes<T>(Func<TestUnitMetadataProviderAttribute, Type> providerTypeFunc) {
            foreach (var attr in _attrs) {
                if (attr is T pro) {
                    yield return pro;

                } else {
                    foreach (var mp in CreateProviders<T>(attr, providerTypeFunc)) {
                        yield return mp;
                    }
                }
            }
        }

        private static IEnumerable<T> CreateProviders<T>(
            object adaptee,
            Func<TestUnitMetadataProviderAttribute, Type> providerTypeFunc
        ) {
            var type = adaptee.GetType();

            IEnumerable<Type> interfaces = type.GetTypeInfo().GetInterfaces();
            if (type.GetTypeInfo().IsInterface) {
                interfaces = interfaces.Concat(new [] { type });
            }

            foreach (var i in interfaces) {
                if (!i.IsDefined(typeof(TestUnitMetadataProviderAttribute))) {
                    continue;
                }
                var requiredType = providerTypeFunc(
                    ((TestUnitMetadataProviderAttribute) i.GetCustomAttribute(typeof(TestUnitMetadataProviderAttribute)))
                );
                if (requiredType == null) {
                    continue;
                }

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
                    yield return (T) activationCtor.Invoke(Array.Empty<object>());
                } else {
                    yield return (T) activationCtor.Invoke(new [] { adaptee });
                }
            }
        }
    }
}
