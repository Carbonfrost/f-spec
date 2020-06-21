//
// Copyright 2016, 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.Commons.Spec {

    public static partial class Extensions {

        internal static StringComparer MapComparer(this StringComparison comparison) {
            switch (comparison) {
                case StringComparison.CurrentCulture:
                    return StringComparer.CurrentCulture;
                case StringComparison.CurrentCultureIgnoreCase:
                    return StringComparer.CurrentCultureIgnoreCase;
                case StringComparison.InvariantCulture:
                    return StringComparer.InvariantCulture;
                case StringComparison.InvariantCultureIgnoreCase:
                    return StringComparer.InvariantCultureIgnoreCase;
                case StringComparison.Ordinal:
                    return StringComparer.Ordinal;
                case StringComparison.OrdinalIgnoreCase:
                    return StringComparer.OrdinalIgnoreCase;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        internal static IEnumerable<TestData> WithNames(this ITestDataApiAttributeConventions self, IEnumerable<TestData> data, TestTag[] tags) {
            return data.Select(d => d.Update(
                new TestDataState(self.Name, self.Reason, d.Flags | (self.Explicit ? TestUnitFlags.Explicit : TestUnitFlags.None), tags)
            ));
        }

        // IgnoreCase versions are odd values

        internal static StringComparison MakeIgnoreCase(this StringComparison c) {
            var comp = (int) c;
            return (StringComparison) (2 * (comp / 2) + 1);
        }

        internal static StringComparison MakeCurrentCulture(this StringComparison c) {
            var comp = (int) c;
            return StringComparison.CurrentCulture + (comp % 2);
        }
    }
}
