#if SELF_TEST

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
using System.Reflection;
using Carbonfrost.Commons.Spec;
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.SelfTest.Spec.TestMatchers {

    abstract class EnumerableComparisonOperatorTestBase<T> : TestClass<T>
        where T : IEnumerableComparisonOperator, new()
    {

        protected EnumerableComparisonOperatorTestBase() {
            Subject = new T();
        }

        public abstract string[] Input {
            get;
        }

        public abstract ExampleData[] Examples {
            get;
        }

        public abstract ExampleData[] CounterExamples {
            get;
        }

        public IEnumerable<Type> CompatibleTypes {
            get {
                return new [] {
                    typeof(Asserter),
                    typeof(Assume),
                    typeof(Assert),
                };
            }
        }

        public IEnumerable<MethodInfo> CompatibleMethods {
            get {
                return typeof(IEnumerableComparisonOperator).GetMethods();
            }
        }

        public abstract string APIName {
            get;
        }

        static IEnumerableExpectation CreateData(IEnumerable data) {
            return new EnumerableExpectation(ExpectationCommand.Of(() => data));
        }

        static IEnumerableExpectation<string> CreateData1(IEnumerable<string> data) {
            return new EnumerableExpectation<string>(ExpectationCommand.Of(() => data));
        }

        [Theory]
        [PropertyData(nameof(Examples))]
        [PassExplicitly]
        public void Apply_should_take_EnumerableExpectation(ExampleData example) {
            example.Apply(Subject, Input);
            Assert.Pass();
        }

        [Theory]
        [PropertyData(nameof(Examples))]
        [PassExplicitly]
        public void Apply_should_take_EnumerableExpectationOfT(ExampleData example) {
            example.Apply1(Subject, Input);
            Assert.Pass();
        }

        [Theory]
        [PropertyData(nameof(CounterExamples))]
        [PassExplicitly]
        public void Apply_should_fail_with_EnumerableExpectation(ExampleData example) {
            try {
                example.Apply(Subject, Input);
            } catch (AssertException) {
                Assert.Pass();
            }
        }

        [Theory]
        [PropertyData(nameof(CounterExamples))]
        [PassExplicitly]
        public void Apply_should_fail_with_EnumerableExpectationOfT(ExampleData example) {
            try {
                example.Apply1(Subject, Input);
            } catch (AssertException) {
                Assert.Pass();
            }
        }

        [Tag("api")]
        [Fact]
        public void Extensions_has_public_extension_methods() {
            // We expect that all methods on the interface are also present on Extensions.
            var signatures = ApiSupport.SignatureSet(CompatibleMethods);
            var actual = ApiSupport.SignatureSet(
                typeof(Extensions).GetMethods().Where(
                    m => m.Name == APIName && m.ReturnType == typeof(void)
                )
            );

            Assert.SetEqual(
                signatures,
                actual,
                $"Expected to have signature Extensions.{APIName}"
            );
        }

        public enum ExampleType {
            Nominal,
            Comparer,
            Comparison,
        }

        internal static ExampleData Example(string[] operand) {
            return new ExampleData {
                Operand = operand,
                Type = ExampleType.Nominal,
            };
        }

        internal static ExampleData Example(string[] operand, IEqualityComparer<string> comparer) {
            return new ExampleData {
                Operand = operand,
                Type = ExampleType.Comparer,
                Comparer = comparer,
            };
        }

        internal static ExampleData Example(string[] operand, Comparison<string> comparison) {
            return new ExampleData {
                Operand = operand,
                Type = ExampleType.Comparison,
                Comparison = comparison,
            };
        }

        public class ExampleData {
            public string[] Operand;
            public IEqualityComparer<string> Comparer;
            public Comparison<string> Comparison;
            public ExampleType Type;

            public override string ToString() {
                var ops = string.Join(",", Operand);
                return $"{Type}: {ops}";
            }

            public void Apply(T instance, string[] input) {
                switch (Type) {
                    case ExampleType.Nominal:
                        instance.Apply(CreateData(input), Operand);
                        break;
                    case ExampleType.Comparer:
                        IEqualityComparer<object> c = new ObjectAdapter(Comparer);
                        instance.Apply(CreateData(input), Operand, c);
                        break;
                    case ExampleType.Comparison:
                        Comparison<object> c1 = (x, y) => Comparison((string) x, (string) y);
                        instance.Apply(CreateData(input), Operand, c1);
                        break;
                }
            }

            public void Apply1(T instance, string[] input) {
                switch (Type) {
                    case ExampleType.Nominal:
                        instance.Apply(CreateData1(input), Operand);
                        break;
                    case ExampleType.Comparer:
                        instance.Apply(CreateData1(input), Operand, Comparer);
                        break;
                    case ExampleType.Comparison:
                        instance.Apply(CreateData1(input), Operand, Comparison);
                        break;
                }
            }
        }

        internal class ObjectAdapter : IEqualityComparer<object> {
            private IEqualityComparer<string> _comparer;

            public ObjectAdapter(IEqualityComparer<string> comparer) {
                _comparer = comparer;
            }

            public new bool Equals(object x, object y) {
                return _comparer.Equals((string) x, (string) y);
            }

            public int GetHashCode(object obj) {
                return _comparer.GetHashCode((string) obj);
            }
        }
    }
}

#endif
