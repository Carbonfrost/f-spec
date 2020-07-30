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

namespace Carbonfrost.Commons.Spec {

    static class Activation {

        internal static IEnumerable<Type> AllTypes() {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(
                a => GetTypesHelper(a)
            );
        }

        internal static object CreateInstance(Type type, IDictionary<string, string> args) {
            var ctor = type.GetConstructors().Single();
            var parameters = ctor.GetParameters();
            var items = new object[parameters.Length];
            int index = 0;

            foreach (var p in parameters) {
                if (args.TryGetValue(p.Name, out string argValue)) {
                    items[index] = FromText(p.ParameterType, argValue);
                    args.Remove(p.Name);
                }
                else if (args.TryGetValue(index.ToString(), out string argFromIndex)) {
                    items[index] = FromText(p.ParameterType, argFromIndex);
                    args.Remove(p.Name);
                }

                index++;
            }
            var result = ctor.Invoke(items);
            foreach (var kvp in args) {
                SetPropertyAllowTextConversion(result, kvp.Key, kvp.Value);
            }
            return result;
        }

        internal static void SetProperty(this object o, string name, object value) {
            if (o == null) {
                return;
            }

            var pi = o.GetType().GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
            if (pi == null) {
                return;
            }
            pi.SetValue(o, value);
        }

        private static void SetPropertyAllowTextConversion(object o, string name, string value) {
            var pi = o.GetType().GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
            if (pi == null) {
                return;
            }
            pi.SetValue(o, FromText(pi.PropertyType, value));
        }

        internal static object FromText(Type type, string value) {
            if (type == typeof(string)) {
                return value;
            }
            if (type.IsEnum) {
                return Enum.Parse(type, value, true);
            }
            var parseMethod = type.GetMethod("Parse", BindingFlags.Public | BindingFlags.Static);
            if (parseMethod == null) {
                throw new NotImplementedException(type.ToString());
            }

            return parseMethod.Invoke(null, new[] { value });
        }

        static IEnumerable<TypeInfo> GetTypesHelper(Assembly a) {
            // Sometimes we don't care about type load exceptions
            // because we can't recover anyway:
            try {
                return a.DefinedTypes;

            } catch (ReflectionTypeLoadException e) {
                return e.Types.Where(t => t != null).Select(t => t.GetTypeInfo());
            }
        }
    }

}
