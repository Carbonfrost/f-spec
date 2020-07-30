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
using System.Linq;

namespace Carbonfrost.Commons.Spec {

    [AttributeUsage(AttributeTargets.Delegate, Inherited = false, AllowMultiple = true)]
    public sealed class RetargetAttribute : Attribute {

        internal static bool IsRetargeted(Type type) {
            return type.IsDefined(typeof(RetargetAttribute), false)
                || HasRetargetAttributeSlow(type);
        }

        private static bool HasRetargetAttributeSlow(Type type) {
            return type.CustomAttributes.Any(
                t => t.AttributeType.Namespace == "Carbonfrost.Commons.Spec"
                    && t.AttributeType.Name.Replace("_", "") == nameof(RetargetAttribute));
        }
    }
}
