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
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Carbonfrost.Commons.Spec {

    static partial class DisplayActual {

        class DefaultDisplayActual : IDisplayActual {

            private readonly DisplayActualSchema _schema;
            private readonly List<(string property, IDisplayActual actual)> _entries = new List<(string, IDisplayActual)>();

            public Type Type {
                get {
                    return _schema.Type;
                }
            }

            public string Format(DisplayActualOptions options) {
                // On recursion, no need to display types
                var recursionOptions = options & ~DisplayActualOptions.ShowType;
                var entries = new StringBuilder();
                bool comma = false;
                foreach (var e in _entries) {
                    if (comma) {
                        entries.Append(", ");
                    }
                    entries.Append(e.property);
                    entries.Append(" = ");
                    entries.Append(e.actual.Format(recursionOptions));
                    comma = true;
                }

                // Show the type if it was requested or if there are no fields within
                string formatString = (entries.Length == 0 || options.ShowType())
                    ? "{0} {{ {1} }}"
                    : "{{ {1} }}";

                return string.Format(
                    formatString, _schema.SimpleTypeName, entries
                );
            }

            public DefaultDisplayActual(object value, int depth, ObjectIDGenerator graph) {
                _schema = DisplayActualSchema.Create(value.GetType());
                depth += 1;

                foreach (var accessor in _schema.Accessors) {
                    IDisplayActual pv;
                    try {
                        pv = DisplayActual.Create(DisplayActual.Create(accessor.GetValue(value), depth, graph));
                    } catch (Exception ex) {
                        ex = Record.UnwindTargetException(ex);
                        pv = DisplayActual.Create($"<{ex.GetType().Name}>");
                    }

                    _entries.Add(
                        (accessor.Name, pv)
                    );
                }
            }
        }
    }
}
