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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Carbonfrost.Commons.Spec {

    abstract class TestDataBinder {

        public abstract object[] Bind(IDictionary<string, string> items);

        public static TestDataBinder Create(MethodInfo method, ICollection<string> keys) {
            var parms = method.GetParameters();

            if (parms.Length == 0) {
                // Shouldn't happen for theories
                throw new NotImplementedException();

            }

            if (parms.Length > 1) {
                return new MapToParameterNamesBinder(parms);
            }

            // One parameter:
            // Assume that we need to bind exactly as the given parameter type

            if (keys.Count == 1 && string.Equals(keys.First(), parms[0].Name, StringComparison.OrdinalIgnoreCase)) {
                // Maybe the name matches -- hence use name binding
                return new MapToParameterNamesBinder(parms);
            }

            return new MapToObjectBinder(parms[0].ParameterType);
        }

        class MapToParameterNamesBinder : TestDataBinder {

            private readonly ParameterInfo[] _parameters;

            public MapToParameterNamesBinder(ParameterInfo[] names) {
                _parameters = names;
            }

            public override object[] Bind(IDictionary<string, string> items) {
                object[] result = new object[_parameters.Length];
                for (int i = 0; i < _parameters.Length; i++) {
                    var parmType = _parameters[i].ParameterType;
                    var parmName = _parameters[i].Name;
                    string value;

                    if (items.TryGetValue(parmName, out value)) {
                        result[i] = Activation.FromText(parmType, value);
                    }
                }
                return result;
            }
        }

        class MapToObjectBinder : TestDataBinder {

            private readonly Type _type;

            public MapToObjectBinder(Type type) {
                _type = type;
            }

            public override object[] Bind(IDictionary<string, string> items) {
                var instance = Activation.CreateInstance(_type, items);
                return new object[] { instance };
            }
        }
    }
}
