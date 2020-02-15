//
// Copyright 2016-2018 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

    public struct TestData : IReadOnlyList<object> {

        private readonly string _name;
        private readonly object[] _data;

        public string Name {
            get {
                return _name;
            }
        }

        private TestData(string name, object[] data) {
            _name = name;
            _data = data ?? Array.Empty<object>();
        }

        public TestData(params object[] data)
            : this(null, data) {
        }

        public TestData WithName(string name) {
            return new TestData(name, _data);
        }

        public object this[int index] {
            get {
                return _data[index];
            }
        }

        public int Count {
            get {
                return _data.Length;
            }
        }

        public IEnumerator<object> GetEnumerator() {
            return ((IEnumerable<object>) _data).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        internal IEnumerable<object> Evaluate(TestContext testContext) {
            var method = ((TestCase) testContext.CurrentTest).TestMethod;
            var pms = method.GetParameters();
            int index = 0;

            foreach (var o in _data) {
                var convert = o as ITestDataConversion;

                if (convert != null) {
                    yield return convert.Convert(testContext, pms[index]);
                } else {
                    yield return o;
                }
                index++;
            }
        }

        internal static IEnumerable<TestData> Create(TestUnit unit, IMemberAccessor accessor) {
            var rt = (TestTheory) unit;
            return Create(rt.TestMethod, rt.TestObject, accessor);
        }

        internal static IEnumerable<TestData> Create(TestUnit unit, IMemberAccessor[] accessors) {
            var rt = (TestTheory) unit;
            return Create(rt.TestMethod, rt.TestObject, accessors);
        }

        private static IEnumerable<TestData> Create(MethodInfo testMethod, object testObject, IMemberAccessor accessor) {
            Type returnType = accessor.ReturnType;
            object myValue = accessor.GetValue(testObject);

            // If the property returns IEnumerable<TestData>, then return it as is.
            if (typeof(IEnumerable<TestData>).IsAssignableFrom(returnType)) {
                return (IEnumerable<TestData>) myValue;
            }

            // If the property returns TestData, then return it as is.
            if (returnType == typeof(TestData)) {
                return new [] { (TestData) myValue };
            }

            // If the property returns IEnumerable<object[]>
            if (typeof(IEnumerable<object[]>).IsAssignableFrom(returnType)) {
                return ((IEnumerable<object[]>) myValue).Select(t => new TestData(t));
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
    }
}
