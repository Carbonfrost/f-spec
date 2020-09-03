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

namespace Carbonfrost.Commons.Spec {

    struct ProjectedReadOnlyList<TFrom, TTo> : IReadOnlyList<TTo> {
        private IReadOnlyList<TFrom> _items;
        private Func<TFrom, TTo> _p;

        public ProjectedReadOnlyList(IReadOnlyList<TFrom> items, Func<TFrom, TTo> conversion) {
            _items = items;
            _p = conversion;
        }

        public TTo this[int index] {
            get {
                return _p(_items[index]);
            }
        }

        public int Count {
            get {
                return _items.Count;
            }
        }

        public IEnumerator<TTo> GetEnumerator() {
            foreach (var p in _items) {
                yield return _p(p);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }

    static partial class Mixin {

        public static IReadOnlyList<TTo> ProjectedTo<TTo, TFrom>(this IReadOnlyList<TFrom> other, Func<TFrom, TTo> conversion) {
            return new ProjectedReadOnlyList<TFrom, TTo>(other, conversion);
        }
    }
}
