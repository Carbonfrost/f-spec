//
// Copyright 2016 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Linq;
using System.Reflection;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    static class MemberAccessors {

        public static IMemberAccessor Property(PropertyInfo prop) {
            return new PropertyAccessor(prop);
        }

        public static IMemberAccessor Field(FieldInfo field) {
            return new FieldAccessor(field);
        }

        struct PropertyAccessor : IMemberAccessor {
            private readonly PropertyInfo _property;

            public PropertyAccessor(PropertyInfo property) {
                _property = property;
            }

            public object GetValue(object instance) {
                return _property.GetValue(instance);
            }

            public Type ReturnType {
                get {
                    return _property.PropertyType;
                }
            }
        }

        struct FieldAccessor : IMemberAccessor {
            private readonly FieldInfo _field;

            public FieldAccessor(FieldInfo field) {
                _field = field;
            }

            public object GetValue(object instance) {
                return _field.GetValue(instance);
            }

            public Type ReturnType {
                get {
                    return _field.FieldType;
                }
            }

        }
    }
}
