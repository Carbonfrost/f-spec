//
// Copyright 2017 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Reflection;

namespace Carbonfrost.Commons.Spec {

    static class EpsilonComparer {

        internal static IComparer<T> Create<T>(T epsilon) {
            return Create<T, T>(epsilon);
        }

        public static IComparer<TSelf> Create<TSelf, TEpsilon>(TEpsilon epsilon) {
            if (typeof(TSelf) == typeof(double) && typeof(TEpsilon) == typeof(double)) {
                return (IComparer<TSelf>) new DoubleComparer((double) (object) epsilon);
            }
            var epsilonSubMethod = typeof(TSelf).GetTypeInfo()
                .GetMethod("op_Subtraction", new [] { typeof(TSelf), typeof(TSelf) });

            if (epsilonSubMethod == null) {
                throw SpecFailure.BadEpsilonComparerTypes(typeof(TSelf));
            }
            var epsilonType = epsilonSubMethod.ReturnType;

            if (epsilonType == typeof(TSelf)) {
                return new ReflectedEpsilonComparer<TSelf>((TSelf) (object) epsilon);
            }

            var type = typeof(ReflectedEpsilonComparer<,>).MakeGenericType(typeof(TSelf), epsilonType);
            return (IComparer<TSelf>) Activator.CreateInstance(type, epsilon);
        }

        class DoubleComparer : IComparer<double> {

            private readonly double _epsilon;

            public DoubleComparer(double epsilon) {
                _epsilon = epsilon;
            }

            public int Compare(double x, double y) {
                // abs(x - y) <= epsilon implies 0
                // abs(x - y) >  epsilon implies Compare(x, y)
                if (Math.Abs(x - y) <= _epsilon) {
                    return 0;
                }
                return x.CompareTo(y);
            }

            public override string ToString() {
                return string.Format("close by {0}", _epsilon);
            }
        }

        class ReflectedEpsilonComparer<T> : IComparer<T> {

            private readonly T _epsilon;
            private readonly MethodInfo _subtract;

            public ReflectedEpsilonComparer(T epsilon) {
                _epsilon = epsilon;
                _subtract = typeof(T).GetTypeInfo().GetMethod("op_Subtraction", new [] { typeof(T), typeof(T) });
            }

            private bool LessThan(object x, object y) {
                return Comparer<object>.Default.Compare(x, y) < 0;
            }

            public int Compare(T x, T y) {
                // abs(x - y) <= epsilon implies 0
                // abs(x - y) >  epsilon implies Compare(x, y)

                // Since no abs, compare y and x and exchange
                bool exchange = LessThan(x, y);
                if (exchange) {
                    var temp = y;
                    y = x;
                    x = temp;
                }
                var result = _subtract.Invoke(null, new object[] { x, y });
                if (Equals(result, _epsilon) || LessThan(result, _epsilon)) {
                    return 0;
                }
                return exchange ? -1 : 1;
            }

            public override string ToString() {
                return string.Format("close by {0}", _epsilon);
            }
        }

        class ReflectedEpsilonComparer<TSelf, TEpsilon> : IComparer<TSelf> {

            private readonly TEpsilon _epsilon;
            private readonly MethodInfo _subtract;

            public ReflectedEpsilonComparer(TEpsilon epsilon) {
                _epsilon = epsilon;
                _subtract = typeof(TSelf).GetTypeInfo().GetMethod("op_Subtraction", new [] { typeof(TSelf), typeof(TSelf) });
            }

            private bool LessThan(object x, object y) {
                return Comparer<object>.Default.Compare(x, y) < 0;
            }

            public int Compare(TSelf x, TSelf y) {
                bool exchange = LessThan(x, y);
                if (exchange) {
                    var temp = y;
                    y = x;
                    x = temp;
                }
                var result = _subtract.Invoke(null, new object[] { x, y });
                if (Equals(result, _epsilon) || LessThan(result, _epsilon)) {
                    return 0;
                }
                return exchange ? -1 : 1;
            }

            public override string ToString() {
                return string.Format("close by {0}", _epsilon);
            }
        }
    }

}
