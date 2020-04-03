#if SELF_TEST

//
// Copyright 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

namespace Carbonfrost.SelfTest.Spec {

    public class DataAttributeConsistencyTests : TestClass {

        public Type[] BaseDataAttributeTypes {
            get {
                return new [] {
                    typeof(InlineDataAttribute),
                    typeof(PropertyDataAttribute),
                    typeof(FieldDataAttribute),
                    typeof(FixtureDataAttribute),
                };
            }
        }

        public string[] Prefixes {
            get {
                return new [] { "X", "F" };
            }
        }

        [Theory]
        [PropertyData(nameof(BaseDataAttributeTypes), nameof(Prefixes))]
        public void Helpers_should_have_same_attribute_usage_as_base(Type parentType, string prefix) {
            // Given (say) InlineDataAttribute, XInlineDataAttribute should have the same
            // attribute usage
            var parentAttr = parentType.GetCustomAttribute<AttributeUsageAttribute>();

            var item = GetType().Assembly.GetType($"Carbonfrost.Commons.Spec.{prefix}{parentType.Name}");
            var attr = item.GetCustomAttribute<AttributeUsageAttribute>();
            Assert.Equal(parentAttr.ValidOn, attr.ValidOn);
        }

    }
}
#endif
