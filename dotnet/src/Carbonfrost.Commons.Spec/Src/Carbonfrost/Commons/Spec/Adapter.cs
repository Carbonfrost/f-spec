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

using System.Collections;
using System.Collections.Generic;

namespace Carbonfrost.Commons.Spec {

    static partial class Adapter {

        public static IEqualityComparer<object> Generic(IEqualityComparer inner) {
            if (inner == null) {
                return null;
            }
            return new GenericEqualityComparerAdapter(inner);
        }

        class GenericEqualityComparerAdapter : IEqualityComparer<object> {
            private readonly IEqualityComparer _inner;

            public GenericEqualityComparerAdapter(IEqualityComparer inner) {
                _inner = inner;
            }

            bool IEqualityComparer<object>.Equals(object x, object y) {
                return _inner.Equals(x, y);
            }

            public int GetHashCode(object obj) {
                return _inner.GetHashCode(obj);
            }
        }
    }
}
