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

using Carbonfrost.Commons.Spec;
using Carbonfrost.Commons.Spec.ExecutionModel;
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.SelfTest.Spec {

    public class ExpectationCommandTests {

        [Theory]
        [PropertyData(nameof(MatcherHandlingOfNullExamples))]
        public void Should_will_handle_null_correctly(NullExampleData data) {
            switch (data.Mode) {
                case NullMode.FailsMatch:
                    var failure = data.CreateInvoker().Invoke();
                    Assert.NotNull(failure);
                    Assert.StartsWith("Expected", failure.Message);
                    break;

                case NullMode.RequiresValidation:
                    Assert.Throws<AssertVerificationException>(
                        () => data.CreateInvoker().Invoke()
                    );

                    var failure2 = data.CreateInvoker().InvokeWithValidation();
                    Assert.NotNull(failure2);
                    Assert.StartsWith("Expected", failure2.Message);
                    break;

                case NullMode.RequiresValidationButPasses:
                    Assert.Throws<AssertVerificationException>(
                        () => data.CreateInvoker().Invoke()
                    );

                    var failure3 = data.CreateInvoker().InvokeWithValidation();
                    Assert.Null(failure3);
                    break;
            }

        }

        public IEnumerable<NullExampleData> MatcherHandlingOfNullExamples {
            get {
                return new [] {
                    // The driving rationale for these as failures is whehter
                    // the error message is scrutable.
                    // e.g. "expected empty but was <null>" BUT
                    // "expected type of String but was <null>" (because null is
                    // untyped, this message is less useful)
                    FailsMatch(new EmptyMatcher()),
                    FailsMatch(new StartWithSubstringMatcher("t")),
                    FailsMatch(new StartWithSubstringMatcher("t")),
                    FailsMatch(new ContainsSubstringMatcher("t")),
                    FailsMatch(new EndWithSubstringMatcher("t")),
                    FailsMatch(new HaveCountMatcher(2)),
                    FailsMatch(new HaveKeyMatcher<string>("t")),
                    FailsMatch(new ContainsMatcher<string>("t")),
                    FailsMatch<IEnumerable<KeyValuePair<string, string>>>(new HaveKeyWithValueMatcher<string, string>("k", "v")),
                    FailsMatch(new HaveLengthMatcher(2)),
                    FailsMatch(new HaveSingleMatcher()),
                    FailsMatch(new SequenceEqualMatcher<string>(new [] { "a" })),
                    FailsMatch(new SetEqualMatcher<string>(new [] { "t" })),

                    RequiresValidationButPasses(new ReferenceTypeMatcher()),
                    RequiresValidation(new ValueTypeMatcher()),
                    RequiresValidation(new InstanceOfMatcher(typeof(string))),
                };
            }
        }

        private static NullExampleData FailsMatch<T>(ITestMatcher<T> matcher) {
            return new NullExampleData(typeof(T), matcher, NullMode.FailsMatch);
        }

        private static NullExampleData RequiresValidation<T>(ITestMatcher<T> matcher) {
            return new NullExampleData(typeof(T), matcher, NullMode.RequiresValidation);
        }

        private static NullExampleData RequiresValidationButPasses<T>(ITestMatcher<T> matcher) {
            return new NullExampleData(typeof(T), matcher, NullMode.RequiresValidationButPasses);
        }

        public struct NullExampleData {
            private readonly Type _argType;

            public object Matcher {
                get;
            }

            public NullMode Mode {
                get;
            }

            public NullExampleData(Type argType, object matcher, NullMode mode) {
                _argType = argType;
                Matcher = matcher;
                Mode = mode;
            }

            internal IInvoker CreateInvoker() {
                return (IInvoker) Activator.CreateInstance(
                    typeof(Invoker<>).MakeGenericType(_argType),
                    this
                );
            }
        }

        private class Invoker<T> : IInvoker where T: class {
            private readonly NullExampleData _data;

            public Invoker(NullExampleData data) {
                _data = data;
            }

            public TestFailure Invoke() {
                var ec = ExpectationCommand.Of(() => (T) null);
                return ec.Should((ITestMatcher<T>) _data.Matcher);
            }

            public TestFailure InvokeWithValidation() {
                var ec = ExpectationCommand.Of(() => (T) null);
                var m = ((ITestMatcherValidations) _data.Matcher).AllowingNullActualValue();
                return ec.Should((ITestMatcher<T>) m);
            }
        }

        internal interface IInvoker {
            TestFailure Invoke();
            TestFailure InvokeWithValidation();
        }

        public enum NullMode {
            FailsMatch,
            RequiresValidation,
            RequiresValidationButPasses,
        }
    }
}
#endif
