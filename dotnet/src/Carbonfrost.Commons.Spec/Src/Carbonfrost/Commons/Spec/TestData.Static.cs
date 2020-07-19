//
// Copyright 2016-2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Linq;
using System.Reflection;

using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.Commons.Spec {

    partial struct TestData {

        static readonly TryConvertTestDataDelegate[] CONVERSIONS = {
            TryNominalConversions,

            // If the property returns TestData<T> (or less likely), FTestData<T>/XTestData<T>
            TryTestDataOfT,

            // The property returns IEnumerable<TestData<?>>
            TryTestDataOfTList,

            // Tuple<...>
            TryTestDataTupleOfT,

            // IEnumerable<Tuple<...>>
            TryTestDataTupleOfTList,

            // ValueTuple<...>
            TryTestDataValueTupleOfT,

            // IEnumerable<ValueTuple<...>>
            TryTestDataValueTupleOfTList,
        };

        static readonly Type[] VALUE_TUPLE_TYPES = {
            typeof(ValueTuple<>),
            typeof(ValueTuple<,>),
            typeof(ValueTuple<,,>),
            typeof(ValueTuple<,,,>),
            typeof(ValueTuple<,,,,>),
            typeof(ValueTuple<,,,,,>),
            typeof(ValueTuple<,,,,,,>),
            typeof(ValueTuple<,,,,,,,>),
        };

        static readonly Type[] TUPLE_TYPES = {
            typeof(Tuple<>),
            typeof(Tuple<,>),
            typeof(Tuple<,,>),
            typeof(Tuple<,,,>),
            typeof(Tuple<,,,,>),
            typeof(Tuple<,,,,,>),
            typeof(Tuple<,,,,,,>),
            typeof(Tuple<,,,,,,,>),
        };

        delegate bool TryConvertTestDataDelegate(Type returnType, object myValue, out IEnumerable<TestData> results);

        public static TestDataCollection<T> Table<T>(params TestData<T>[] items) {
            return new TestDataCollection<T>(items);
        }

        public static TestDataCollection<T> Table<T>(IEnumerable<TestData<T>> items) {
            return new TestDataCollection<T>(items);
        }

        public static TestDataCollection Table(params TestData[] items) {
            return new TestDataCollection(items);
        }

        public static TestDataCollection Table(IEnumerable<TestData> items) {
            return new TestDataCollection(items);
        }

        internal static IEnumerable<TestData> Create(TestContext ctxt, IMemberAccessor accessor) {
            var rt = (TestTheory) ctxt.TestUnit;
            var testObject = ctxt.DummyTestObject;
            return Create(rt.TestMethod, testObject, accessor);
        }

        internal static IEnumerable<TestData> Create(TestContext ctxt, IMemberAccessor[] accessors) {
            var rt = (TestTheory) ctxt.TestUnit;
            var testObject = ctxt.DummyTestObject;
            return Create(rt.TestMethod, testObject, accessors);
        }

        private static IEnumerable<TestData> Create(MethodInfo testMethod, object testObject, IMemberAccessor accessor) {
            Type returnType = accessor.ReturnType;
            object myValue = accessor.GetValue(testObject);
            IEnumerable<TestData> results;

            foreach (var c in CONVERSIONS) {
                if (c(returnType, myValue, out results)) {
                    return results;
                }
            }

            // Otherwise, we can detect the test method signature to determine which
            // interface type is required
            var types = testMethod.GetParameters().Select(p => p.ParameterType).ToArray();
            if (types.Length == 0) {
                throw new NotImplementedException();
            }

            if (types.Length == 1) {
                // Could be IEnumerable<T> or just T
                if (typeof(IEnumerable<>).MakeGenericType(types[0]).IsAssignableFrom(returnType)) {
                    return ((IEnumerable) myValue).Cast<object>().Select(t => new TestData(t));
                }

                if (types[0] == returnType) {
                    return new [] { new TestData(myValue) };
                }
            }
            throw new NotImplementedException();
        }

        private static bool TryNominalConversions(Type returnType, object myValue, out IEnumerable<TestData> results) {
            // If the property returns IEnumerable<TestData>, then return it as is.
            if (typeof(IEnumerable<TestData>).IsAssignableFrom(returnType)) {
                results = (IEnumerable<TestData>) myValue;
                return true;
            }

            // If the property returns TestData, then return it as is.
            if (returnType == typeof(TestData)) {
                results = new [] { (TestData) myValue };
                return true;
            }

            // If the property returns IEnumerable<object[]>
            if (typeof(IEnumerable<object[]>).IsAssignableFrom(returnType)) {
                results = ((IEnumerable<object[]>) myValue).Select(t => new TestData(t));
                return true;
            }

            results = null;
            return false;
        }

        // Given IEnumerable<T> ==> extract T
        private static bool TryExtractEnumerableItemType(Type returnType, out Type itemType) {
            itemType = null;

            var iFace = returnType.GetInterface("IEnumerable`1")
                ?? (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(IEnumerable<>) ? returnType : null);
            if (iFace == null) {
                return false;
            }

            itemType = iFace.GetGenericArguments()[0];
            return true;
        }

        private static bool TryTestDataOfTList(Type returnType, object myValue, out IEnumerable<TestData> results) {
            Type targetType = null;
            if (TryExtractEnumerableItemType(returnType, out targetType) && targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(TestData<>)) {
                var list = new List<TestData>();
                foreach (ITestDataUntyped data in (IEnumerable) myValue) {
                    list.Add(data.Untyped());
                }
                results = list;
                return true;
            }

            results = null;
            return false;
        }

        private static bool TryTestDataOfT(Type returnType, object myValue, out IEnumerable<TestData> results) {
            if (typeof(ITestDataUntyped).IsAssignableFrom(returnType)) {
                results = new [] { ((ITestDataUntyped) myValue).Untyped() };
                return true;
            }
            results = null;
            return false;
        }

        private static bool TryTestDataTupleOfT(Type returnType, object myValue, out IEnumerable<TestData> results) {
            if (IsTupleType(returnType)) {
                results = new [] { TupleToValues(myValue) };
                return true;
            }
            results = null;
            return false;
        }

        private static bool TryTestDataValueTupleOfT(Type returnType, object myValue, out IEnumerable<TestData> results) {
            if (IsValueTupleType(returnType)) {
                results = new [] { ValueTupleToValues(myValue) };
                return true;
            }
            results = null;
            return false;
        }

        private static bool TryTestDataTupleOfTList(Type returnType, object myValue, out IEnumerable<TestData> results) {
            Type targetType = null;
            if (TryExtractEnumerableItemType(returnType, out targetType) && IsTupleType(targetType)) {
                results = ((IEnumerable) myValue).Cast<object>().Select(s => TupleToValues(s));
                return true;
            }

            results = null;
            return false;
        }

        private static bool TryTestDataValueTupleOfTList(Type returnType, object myValue, out IEnumerable<TestData> results) {
            Type targetType = null;
            if (TryExtractEnumerableItemType(returnType, out targetType) && IsValueTupleType(targetType)) {
                results = ((IEnumerable) myValue).Cast<object>().Select(s => ValueTupleToValues(s));
                return true;
            }

            results = null;
            return false;
        }

        internal static IEnumerable<TestData> Create(MethodInfo testMethod, object testObject, IMemberAccessor[] accessors) {
            if (accessors.Length == 1) {
                return Create(testMethod, testObject, accessors[0]);
            }

            if (testMethod.GetParameters().Length != accessors.Length) {
                throw SpecFailure.MultiAccessorsTheoryParameterMismatch();
            }

            // TODO It would be better to enforce IEnumerable instead of failing with cast

            var elements = accessors
                .Select(t => ((IEnumerable) t.GetValue(testObject)).Cast<object>().ToArray())
                .ToList();

            var actualData = Combinatorial(elements)
                .Select(t => new TestData(t));
            return actualData;
        }

        internal static IEnumerable<object[]> Combinatorial(IList<object[]> vars) {
            int totalCombinations = vars.Aggregate(1, (a, v) => a * v.Length);
            int paramCount = vars.Count;

            var result = new List<object[]>(totalCombinations);
            for (int i = 0; i < totalCombinations; i++) {
                result.Add(new object[paramCount]);
            }

            // A tessellation generator
            int tes = totalCombinations;
            for (int key = 0; key < paramCount; key++) {
                var values = vars[key];
                tes = tes / values.Length; // Always divides evenly (due to totalCombinations)

                for (int combo = 0; combo < totalCombinations; combo++) {
                    result[combo][key] = values[(combo / tes) % values.Length];
                }
            }
            return result;
        }

        internal TestData VerifiableProblem(bool shouldVerify, string reason) {
            if (shouldVerify) {
                return Fail(reason);
            }
            return Pending(reason);
        }

        static bool IsTupleType(Type type) {
            if (type.IsGenericType) {
                return Array.IndexOf(TUPLE_TYPES, type.GetGenericTypeDefinition()) >= 0;
            }
            return false;
        }

        static bool IsValueTupleType(Type type) {
            if (type.IsGenericType) {
                return Array.IndexOf(VALUE_TUPLE_TYPES, type.GetGenericTypeDefinition()) >= 0;
            }
            return false;
        }

        static TestData TupleToValues(object tuple) {
            var props = tuple.GetType().GetProperties();
            int sz = props.Length;
            var result = new object[sz];
            for (int i = 0; i < sz; i++) {
                result[i] = props[i].GetValue(tuple, null);
            }
            return TestData.Create(result);
        }

        static TestData ValueTupleToValues(object tuple) {
            var fields = tuple.GetType().GetFields();
            int sz = fields.Length;
            var result = new object[sz];
            for (int i = 0; i < sz; i++) {
                result[i] = fields[i].GetValue(tuple);
            }
            return TestData.Create(result);
        }
    }
}
