#if SELF_TEST

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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.SelfTest.Spec {

    static class ApiSupport {

        public static string GetSignatureString(MethodInfo method) {
            string result = "";
            if (method.IsGenericMethod) {
                result += "<" + new string(',', method.GetGenericArguments().Length - 1) + ">";
            }
            result += GetSignatureString(method.GetParameters().Select(t => t.ParameterType));
            return result;
        }

        public static string GetSignatureString(IEnumerable<Type> parameterTypes) {
            return "(" + string.Join(",", parameterTypes.Select(t => t.Name)) + ")";
        }

        public static string[] SignatureSet(IEnumerable<MethodInfo> methods) {
            var signatures = methods.Select(GetSignatureString).ToArray();
            Array.Sort(signatures);
            return signatures;
        }

        internal static int SignatureOrder(ParameterInfo pi) {
            // Ensure message and args are last and that the thing to match precedes them
            if (pi.Name == "message") {
                return 99;
            }
            if (pi.Name == "args") {
                return 100;
            }
            if (pi.Position == 0) {
                return 98;
            }
            return pi.Position;
        }

        internal static Type ReplaceSignatureType(Type type) {
            if (type == typeof(EnumerableExpectation)) {
                return typeof(IEnumerable);
            }
            if (type == typeof(Expectation)) {
                return typeof(object);
            }
            if (type.Name == "Expectation`1") {
                return type.GetGenericArguments()[0];
            }
            if (type.Name == "EnumerableExpectation`1") {
                return typeof(IEnumerable<>).MakeGenericType(
                    type.GetGenericArguments()[0]
                );
            }
            return type;
        }
    }
}
#endif
