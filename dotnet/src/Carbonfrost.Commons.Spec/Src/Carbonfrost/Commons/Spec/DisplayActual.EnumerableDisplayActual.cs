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

namespace Carbonfrost.Commons.Spec {

    static partial class DisplayActual {

        class EnumerableDisplayActual : IDisplayActual {

            private readonly List<IDisplayActual> _values = new List<IDisplayActual>();
            private readonly Type _type;

            public Type Type {
                get {
                    return _type;
                }
            }

            public string Format(DisplayActualOptions options) {
                string formatString = options.ShowType() ? "{0} {{ {1} }}" : "{{ {1} }}";

                // On recursion, no need to display types
                var recursionOptions = options & ~DisplayActualOptions.ShowType;
                return string.Format(
                    formatString,
                    TextUtility.ConvertToSimpleTypeName(_type),
                    string.Join(", ", _values.Select(v => v.Format(recursionOptions)))
                );
            }

            public EnumerableDisplayActual(IEnumerable enumerableValue, int depth) {
                foreach (object valueObject in enumerableValue) {
                    _values.Add(DisplayActual.Create(valueObject, depth));
                }
                _type = enumerableValue.GetType();
            }
        }
    }
}
