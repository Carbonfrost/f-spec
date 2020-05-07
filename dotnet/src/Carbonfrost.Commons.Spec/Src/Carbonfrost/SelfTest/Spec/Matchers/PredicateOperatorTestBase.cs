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

    abstract class PredicateOperatorTestBase<T> : TestClass<T>
        where T : IPredicateOperator, new()
    {

        protected PredicateOperatorTestBase() {
            Subject = new T();
        }

        public abstract Predicate<int> ValidPredicate {
            get;
        }

        public abstract int[][] ValidData {
            get;
        }

        public abstract int[][] InvalidData {
            get;
        }

        public abstract int[][] PredicateValidData {
            get;
        }

        public abstract int[][] PredicateInvalidData {
            get;
        }

        public IEnumerable<ApplyWithMessageFunc> ApplicationsWithMessageData {
            get {
                yield return (data, message, args) => Subject.Apply(
                    CreateData(data), message, (object[]) args
                );
                yield return (data, message, args) => Subject.Apply(
                    CreateData1(data), message, (object[]) args
                );
            }
        }

        public IEnumerable<ApplyFunc> ApplicationsData {
            get {
                yield return (data) => Subject.Apply(
                    CreateData(data)
                );
                yield return (data) => Subject.Apply(
                    CreateData1(data)
                );
            }
        }

        public IEnumerable<ApplyWithMessageFunc> PredicateApplicationsWithMessageData {
            get {
                Predicate<object> objPredicate = o => ValidPredicate((int) o);
                yield return (data, message, args) => Subject.Apply(
                    CreateData(data), objPredicate, message, (object[]) args
                );
                yield return (data, message, args) => Subject.Apply(
                    CreateData1(data), ValidPredicate, message, (object[]) args
                );
            }
        }

        public IEnumerable<ApplyFunc> PredicateApplicationsData {
            get {
                Predicate<object> objPredicate = o => ValidPredicate((int) o);
                yield return (data) => Subject.Apply(
                    CreateData(data), objPredicate
                );
                yield return (data) => Subject.Apply(
                    CreateData1(data), ValidPredicate
                );
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
                return typeof(IPredicateOperator).GetMethods();
            }
        }

        public abstract string APIName {
            get;
        }

        static IEnumerableExpectation CreateData(IEnumerable data) {
            return new EnumerableExpectation(ExpectationCommand.Of(() => data));
        }

        static IEnumerableExpectation<int> CreateData1(IEnumerable<int> data) {
            return new EnumerableExpectation<int>(ExpectationCommand.Of(() => data));
        }

        [Theory]
        [PropertyData(nameof(PredicateValidData), nameof(PredicateApplicationsWithMessageData))]
        [PassExplicitly]
        public void Apply_should_take_predicate_with_message_and_pass(int[] data, ApplyWithMessageFunc apply) {
            apply(data, "message", "args");
            Assert.Pass();
        }

        [Theory]
        [PropertyData(nameof(PredicateValidData), nameof(PredicateApplicationsData))]
        [PassExplicitly]
        public void Apply_should_take_predicate_operand_and_pass(int[] data, ApplyFunc apply) {
            apply(data);
            Assert.Pass();
        }

        [Theory]
        [PropertyData(nameof(ValidData), nameof(ApplicationsWithMessageData))]
        [PassExplicitly]
        public void Apply_should_take_no_operand_with_message_and_pass(int[] data, ApplyWithMessageFunc apply) {
            apply(data, "message", "args");
            Assert.Pass();
        }

        [Theory]
        [PropertyData(nameof(ValidData), nameof(ApplicationsData))]
        [PassExplicitly]
        public void Apply_should_take_no_operand_operand_and_pass(int[] data, ApplyFunc apply) {
            apply(data);
            Assert.Pass();
        }

        [Theory]
        [PropertyData(nameof(PredicateInvalidData), nameof(PredicateApplicationsWithMessageData))]
        [PassExplicitly]
        public void Apply_should_take_operand_with_message_and_fail(int[] data, ApplyWithMessageFunc apply) {
            try {
                apply(data, "My message {0}", "arg1");

            } catch (AssertException aex) {
                Assert.Equal("My message arg1", aex.Message);
                Assert.Pass();
            }
        }

        [Theory]
        [PropertyData(nameof(PredicateInvalidData), nameof(PredicateApplicationsData))]
        [PassExplicitly]
        public void Apply_should_take_predicate_operand_and_fail(int[] data, ApplyFunc apply) {
            try {
                apply(data);

            } catch (AssertException) {
                Assert.Pass();
            }
        }

        [Theory]
        [PropertyData(nameof(InvalidData), nameof(ApplicationsWithMessageData))]
        [PassExplicitly]
        public void Apply_should_take_no_operand_with_message_and_fail(int[] data, ApplyWithMessageFunc apply) {
            try {
                apply(data, "My message {0}", "arg1");

            } catch (AssertException aex) {
                Assert.Equal("My message arg1", aex.Message);
                Assert.Pass();
            }
        }

        [Theory]
        [PropertyData(nameof(InvalidData), nameof(ApplicationsData))]
        [PassExplicitly]
        public void Apply_should_take_no_operand_and_fail(int[] data, ApplyFunc apply) {
            try {
                apply(data);

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

        public delegate void ApplyWithMessageFunc(int[] data, string message, params object[] args);
        public delegate void ApplyFunc(int[] data);

    }
}

#endif
