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
using System.Diagnostics;
using System.Reflection;
using System.Linq;
using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.Commons.Spec {

    class DisplayActualSchema {
        private readonly Lazy<List<IMemberAccessor>> _accessors;
        private readonly Type _type;
        private static readonly Dictionary<Type, DisplayActualSchema> _cache = new Dictionary<Type, DisplayActualSchema>();

        public Type Type {
            get {
                return _type;
            }
        }

        public string SimpleTypeName {
            get {
                return TextUtility.ConvertToSimpleTypeName(_type);
            }
        }

        public IList<IMemberAccessor> Accessors {
            get {
                return _accessors.Value;
            }
        }

        public DisplayActualSchema(Type type) {
            _type = type;

            // Processing is lazy so that reentrancy on circular type dependencies
            // don't cause problems
            _accessors = new Lazy<List<IMemberAccessor>>(FindAccessors);
        }

        private List<IMemberAccessor> FindAccessors() {
            var result = new List<IMemberAccessor>();
            var fields = _type.GetFields(BindingFlags.Public | BindingFlags.Instance);
            var props = _type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var all = fields.Concat<MemberInfo>(props);

            foreach (var mem in all) {
                var attr = mem.GetCustomAttribute<DebuggerBrowsableAttribute>();
                var access = MemberAccessors.PropertyOrField(mem);

                if (attr == null) {
                    result.Add(access);
                    continue;
                }

                switch (attr.State) {
                    case DebuggerBrowsableState.Never:
                        break;

                    case DebuggerBrowsableState.Collapsed:
                        result.Add(new CollapsedImpl(mem.Name));
                        break;

                    case DebuggerBrowsableState.RootHidden:
                        IList<IMemberAccessor> accessors;
                        try {
                            accessors = DisplayActualSchema.Create(access.ReturnType).Accessors;

                        } catch (InvalidOperationException) {
                            // There was a circular dependency in the type dependency graph because
                            // we tried to resolve the factory value of a schema that is already in
                            // progress of being written.  Handle this by collapsing
                            result.Add(new CollapsedImpl(mem.Name));
                            break;
                        }
                        result.AddRange(
                            accessors.Select(a => HideRoot(access, a))
                        );
                        break;
                }
            }
            result.Sort((x, y) => x.Name.CompareTo(y.Name));
            return result;
        }

        public static DisplayActualSchema Create(Type type) {
            return _cache.GetValueOrCache(type, t => new DisplayActualSchema(t));
        }

        private static IMemberAccessor HideRoot(IMemberAccessor initialAccess, IMemberAccessor derivedAccess) {
            return new HiddenRootImpl(initialAccess, derivedAccess);
        }

        sealed class HiddenRootImpl : IMemberAccessor {

            private readonly IMemberAccessor _initial;
            private readonly IMemberAccessor _derived;

            public string Name {
                get {
                    return _derived.Name;
                }
            }

            public HiddenRootImpl(IMemberAccessor initialAccess, IMemberAccessor derivedAccess) {
                _initial = initialAccess;
                _derived = derivedAccess;
            }

            public Type ReturnType {
                get {
                    return _derived.ReturnType;
                }
            }

            public object GetValue(object instance) {
                return _derived.GetValue(
                    _initial.GetValue(instance)
                );
            }
        }

        sealed class CollapsedImpl : IMemberAccessor {

            public string Name {
                get;
            }

            public CollapsedImpl(string name) {
                Name = name;
            }

            public Type ReturnType {
                get {
                    return typeof(object);
                }
            }

            public object GetValue(object instance) {
                return DisplayActual.Ellipsis;
            }
        }
    }
}
