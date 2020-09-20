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
using System.Reflection;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    interface IReflectionTestUnitFactory {
        string Description {
            get;
            set;
        }

        string Reason {
            get;
            set;
        }

        TestUnit CreateTestCase(MethodInfo method);
    }

    static partial class Extensions {

        internal static T CopyDescriptions<T>(
            this T unit,
            IReflectionTestUnitFactory from
        ) where T : TestUnit {
            if (!string.IsNullOrEmpty(from.Description)) {
                unit.Description = from.Description;
            }
            if (!string.IsNullOrEmpty(from.Reason)) {
                unit.Reason = from.Reason;
            }
            return unit;
        }
    }
}
