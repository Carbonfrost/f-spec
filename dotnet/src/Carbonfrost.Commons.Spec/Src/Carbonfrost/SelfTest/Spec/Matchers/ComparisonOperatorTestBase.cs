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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Carbonfrost.Commons.Spec;
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.SelfTest.Spec.TestMatchers {

    abstract class ComparisonOperatorTestBase<T> : TestClass<T>
        where T : IComparisonOperator, new()
    {

        protected ComparisonOperatorTestBase() {
            Subject = new T();
        }

        public abstract string Input {
            get;
        }

        public abstract ExampleData[] Examples {
            get;
        }

        public abstract ExampleData[] CounterExamples {
            get;
        }

        public virtual bool HasStringExpectations {
            get {
                return false;
            }
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
                return typeof(IComparisonOperator).GetMethods();
            }
        }

        public abstract string APIName {
            get;
        }

        static Expectation<string> CreateData1(string data) {
            return new Expectation<string>(ExpectationCommand.Of(() => data));
        }

        [Theory]
        [PropertyData(nameof(Examples))]
        [PassExplicitly]
        public void Apply_should_take_EnumerableExpectationOfT(ExampleData example) {
            example.Apply1(Subject, Input);
            example.Apply1(Subject, Input, "message {0}", "arg"); // primarily for coverage
            Assert.Pass();
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

        [Theory]
        [PropertyData(nameof(CounterExamples))]
        [PassExplicitly]
        public void Apply_should_fail_with_EnumerableExpectationOfT_with_message(ExampleData example) {
            try {
                example.Apply1(Subject, Input, "message {0}", "arg"); // primarily for coverage
            } catch (AssertException aex) {
                Assert.Equal("message arg", aex.Message);
                Assert.Pass();
            }
        }

        [Fact]
        [Tag("api")]
        public void Extensions_has_public_extension_methods() {
            // We expect that all methods on the interface are also present on Extensions.
            var signatures = ApiSupport.SignatureSet(CompatibleMethods);
            var methods =
                typeof(Extensions).GetMethods().Where(
                    m => m.Name == APIName && m.ReturnType == typeof(void)
                );
            if (HasStringExpectations) {
                // We could have explicit string handling so that the user can benefit from
                // string-specific handling, but we don't care about these API differences
                methods = methods.Where(
                    m => m.GetParameters()[0].ParameterType != typeof(Expectation<string>)
                        && m.GetParameters()[0].ParameterType != typeof(Expectation<string>)
                );
            }
            var actual = ApiSupport.SignatureSet(methods);

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

        internal static ExampleData Example(string operand) {
            return new ExampleData {
                Operand = operand,
                Type = ExampleType.Nominal,
            };
        }

        internal static ExampleData Example(string operand, IComparer<string> comparer) {
            return new ExampleData {
                Operand = operand,
                Type = ExampleType.Comparer,
                Comparer = comparer,
            };
        }

        internal static ExampleData Example(string operand, Comparison<string> comparison) {
            return new ExampleData {
                Operand = operand,
                Type = ExampleType.Comparison,
                Comparison = (x, y) => comparison((string) x, (string) y),
            };
        }

        public struct ExampleData {
            public string Operand;
            public IComparer<string> Comparer;
            public Comparison<string> Comparison;
            public ExampleType Type;

            public void Apply1(T instance, string input) {
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

            public void Apply1(T instance, string input, string message, params object[] args) {
                switch (Type) {
                    case ExampleType.Nominal:
                        instance.Apply(CreateData1(input), Operand, message, (object[]) args);
                        break;
                    case ExampleType.Comparer:
                        instance.Apply(CreateData1(input), Operand, Comparer, message, (object[]) args);
                        break;
                    case ExampleType.Comparison:
                        instance.Apply(CreateData1(input), Operand, Comparison, message, (object[]) args);
                        break;
                }
            }
        }
    }
}

#endif
