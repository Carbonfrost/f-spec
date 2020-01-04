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
using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.Commons.Spec {

    static class Mixin {

        public static IEnumerable<TestUnit> Ancestors(this TestUnit self) {
            self = self.Parent;
            while (self != null) {
                yield return self;
                self = self.Parent;
            }
        }

        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> self, TKey k) {
            TValue result;
            if (!self.TryGetValue(k, out result)) {
                return default(TValue);
            }
            return result;
        }

        public static void AddAll<T>(this ICollection<T> self, IEnumerable<T> items) {
            if (items == null) {
                return;
            }
            foreach (var o in items) {
                self.Add(o);
            }
        }

        public static IEnumerable<T> NonNull<T>(this IEnumerable<T> self) {
            return self.Where(t => !Equals(t, null));
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> items, Random random) {
            var keys = items.Select(t => new KeyValuePair<double, T>(random.NextDouble(), t));
            return keys.OrderBy(m => m.Key).Select(m => m.Value);
        }
    }
}
