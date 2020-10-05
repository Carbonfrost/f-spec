//
// Copyright 2013 Outercurve Foundation
// Copyright 2016, 2017, 2019 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Carbonfrost.Commons.Spec {

    public static partial class Assert {

        static readonly IEqualityComparer DefaultInnerComparer
            = new AssertEqualityComparerAdapter<object>(new AssertEqualityComparer<object>());

        static Asserter Global {
            get {
                return Asserter.Default;
            }
        }

        public static bool UseStrictMode {
            get;
            set;
        }

        public static IDisposable Disabled() {
            return Global.Disabled();
        }

        public static IExpectationBuilder<IEnumerable> Expect() {
            return Global.Expect();
        }

        public static IExpectationBuilder<T> Expect<T>(T value) {
            return Global.Expect<T>(value);
        }

        public static IExpectationBuilder<TEnumerable, T> Expect<TEnumerable, T>(TEnumerable value) where TEnumerable : IEnumerable<T> {
            return Global.Expect<TEnumerable, T>(value);
        }

        public static IExpectationBuilder<TValue[], TValue> Expect<TValue>(params TValue[] values) {
            return Global.Expect<TValue>(values);
        }

        public static IExpectationBuilder Expect(Action value) {
            return Global.Expect(value);
        }

        public static IExpectationBuilder<T> Expect<T>(Func<T> func) {
            return Global.Expect<T>(func);
        }

        internal static IEqualityComparer<T> GetEqualityComparer<T>(bool skipTypeCheck = false, IEqualityComparer innerComparer = null) {
            return new AssertEqualityComparer<T>(skipTypeCheck, innerComparer);
        }

        internal static string MethodDisplay(Delegate d) {
            return Convert.ToString(d.GetMethodInfo());
        }

        internal class EqualityComparerAdapter<T> : IEqualityComparer<T>, IEqualityComparer {

            private readonly IComparer<T> _innerComparer;

            public EqualityComparerAdapter(IComparer<T> innerComparer) {
                _innerComparer = innerComparer;
            }

            public bool Equals(T x, T y) {
                return _innerComparer.Compare((T) x, (T) y) == 0;
            }

            public int GetHashCode(T obj) {
                return EqualityComparer<T>.Default.GetHashCode((T) obj);
            }

            int IEqualityComparer.GetHashCode(object obj) {
                return GetHashCode((T) obj);
            }

            bool IEqualityComparer.Equals(object x, object y) {
                return Equals((T) x, (T) y);
            }

            public override string ToString() {
                return _innerComparer.ToString();
            }
        }

        internal class EqualityComparisonAdapter<T> : IEqualityComparer<T>, IEqualityComparer {

            private readonly Comparison<T> _innerComparer;

            public EqualityComparisonAdapter(Comparison<T> innerComparer) {
                _innerComparer = innerComparer;
            }

            public bool Equals(T x, T y) {
                return _innerComparer((T) x, (T) y) == 0;
            }

            public int GetHashCode(T obj) {
                return EqualityComparer<T>.Default.GetHashCode((T) obj);
            }

            int IEqualityComparer.GetHashCode(object obj) {
                return GetHashCode((T) obj);
            }

            bool IEqualityComparer.Equals(object x, object y) {
                return Equals((T) x, (T) y);
            }

            public override string ToString() {
                return "EqualityComparer { " + MethodDisplay((Delegate) _innerComparer) + " }";
            }

        }

        internal class AssertEqualityComparerAdapter<T> : IEqualityComparer {

            private readonly IEqualityComparer<T> _innerComparer;

            public AssertEqualityComparerAdapter(IEqualityComparer<T> innerComparer) {
                _innerComparer = innerComparer;
            }

            public new bool Equals(object x, object y) {
                return _innerComparer.Equals((T) x, (T) y);
            }

            public int GetHashCode(object obj) {
                return _innerComparer.GetHashCode((T) obj);
            }

            public override string ToString() {
                return _innerComparer.ToString();
            }

        }

        internal class AssertEqualityComparer<T> : IEqualityComparer<T> {

            readonly Func<IEqualityComparer> _innerComparerFactory;
            readonly bool _skipTypeCheck;

            public AssertEqualityComparer(bool skipTypeCheck = false, IEqualityComparer innerComparer = null) {
                _skipTypeCheck = skipTypeCheck;

                // Use a thunk to delay evaluation of DefaultInnerComparer
                _innerComparerFactory = () => innerComparer ?? DefaultInnerComparer;
            }

            public override string ToString() {
                var c = _innerComparerFactory();
                if (c == DefaultInnerComparer) {
                    return "<default>";
                }

                return c.ToString();
            }


            public bool Equals(T x, T y) {
                var type = typeof(T).GetTypeInfo();

                // Null?
                if (!type.IsValueType || (type.IsGenericType && type.GetGenericTypeDefinition().IsAssignableFrom(typeof(Nullable<>)))) {
                    if (object.Equals(x, default(T))) {
                        return object.Equals(y, default(T));
                    }

                    if (object.Equals(y, default(T))) {
                        return false;
                    }
                }

                // Same type?
                if (!_skipTypeCheck && x.GetType() != y.GetType()) {
                    return false;
                }

                // Implements IEquatable<T>?
                var equatable = x as IEquatable<T>;
                if (equatable != null) {
                    return equatable.Equals(y);
                }

                // Implements IComparable<T>?
                var comparableGeneric = x as IComparable<T>;
                if (comparableGeneric != null) {
                    return comparableGeneric.CompareTo(y) == 0;
                }

                // Implements IComparable?
                var comparable = x as IComparable;
                if (comparable != null) {
                    return comparable.CompareTo(y) == 0;
                }

                // Enumerable?
                var enumerableX = x as IEnumerable;
                var enumerableY = y as IEnumerable;

                if (enumerableX != null && enumerableY != null) {
                    var enumeratorX = enumerableX.GetEnumerator();
                    var enumeratorY = enumerableY.GetEnumerator();
                    var equalityComparer = _innerComparerFactory();

                    while (true) {
                        bool hasNextX = enumeratorX.MoveNext();
                        bool hasNextY = enumeratorY.MoveNext();

                        if (!hasNextX || !hasNextY) {
                            return (hasNextX == hasNextY);
                        }

                        if (!equalityComparer.Equals(enumeratorX.Current, enumeratorY.Current)) {
                            return false;
                        }
                    }
                }

                // Last case, rely on Object.Equals
                return object.Equals(x, y);
            }

            public int GetHashCode(T obj) {
                if (ReferenceEquals(obj, null)) {
                    return -1;
                }
                return obj.GetHashCode();
            }
        }

        // Only used for Assert.InRange and Assert.NotInRange
        class AssertComparer<T> : IComparer<T> where T : IComparable {

            public override string ToString() {
                return "<default>";
            }

            public int Compare(T x, T y) {
                var type = typeof(T).GetTypeInfo();

                // Null?
                if (!type.IsValueType || (type.IsGenericType && type.GetGenericTypeDefinition().IsAssignableFrom(typeof(Nullable<>)))) {
                    if (Equals(x, default(T))) {
                        if (Equals(y, default(T))) {
                            return 0;
                        }
                        return -1;
                    }

                    if (Equals(y, default(T))) {
                        return -1;
                    }
                }

                // Same type?
                if (x.GetType() != y.GetType()) {
                    return -1;
                }

                // Implements IComparable<T>?
                IComparable<T> comparable1 = x as IComparable<T>;
                if (comparable1 != null) {
                    return comparable1.CompareTo(y);
                }

                // Implements IComparable
                return x.CompareTo(y);
            }
        }
    }
}
