#if SELF_TEST

//
// Copyright 2018 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Linq;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.SelfTest.Spec.TestMatchers {

    public class TemporalExpectationBuilderTests : TestClass {

        [Fact]
        public void Eventually_Should_should_pass_nominal() {
            Expect(Generator(0, 0, 2)).Eventually.To(Matchers.Equal(2));
        }

        [Fact]
        public void Eventually_ShouldNot_should_pass_nominal() {
            Expect(Generator(2, 2, 2, 0)).Eventually.ToNot(Matchers.Equal(2));
        }

        [Fact]
        public void Eventually_ShouldSatisfy_all() {
            Expect(Generator(2, 2, 2, 0)).Eventually.ToBe.EqualTo(0);
            Expect(Generator(2, 2, 2, 0)).Eventually.ToSatisfy.All(Matchers.Equal(0));
        }

        [Fact]
        public void Eventually_WillNot_Throw() {
            Expect(ThrowGenerator(true)).Eventually.Will.Not.Throw();
        }

        [Fact]
        public void Eventually_Will_Throw() {
            Expect(ThrowGenerator(false)).Eventually.Will.Throw();
        }

        [Fact]
        [PassExplicitly]
        public void Eventually_Should_should_fail() {
            try {
                Expect(() => 2).Eventually.To(Matchers.Equal(3));

            } catch (AssertException ex) {
                var lines = ex.Message.Split('\n').Select(t => t.Trim()).Take(2);
                var expected = new [] {
                    "Timed out after 500 ms.  Eventually expected to:",
                    "- be equal to 3",
                };
                Assert.Equal(expected, lines);
                Assert.ContainsKeyWithValue("Actual", "2", ex.TestFailure.UserData);
                Assert.Pass();
            }
        }

        [Fact]
        public void Consistently_Should_should_pass_nominal() {
            Expect(() => 2).Consistently.To(Matchers.Equal(2));
        }

        [Fact]
        [PassExplicitly]
        public void Consistently_Should_should_fail() {
            try {
                Expect(Generator(2, 2, 2, 1)).Consistently.To(Matchers.Equal(2));

            } catch (AssertException ex) {
                var lines = ex.Message.Split('\n').Select(t => t.Trim()).Take(2);
                var expected = new [] {
                    "Elapsed before 500 ms.  Consistently expected to:",
                    "- be equal to 2",
                };
                Assert.Equal(expected, lines);
                Assert.ContainsKeyWithValue("Actual", "1", ex.TestFailure.UserData);
                Assert.Pass();
            }
        }

        private Func<int> Generator(params int[] values) {
            int counter = -1;
            return () => {
                counter++;
                if (counter < values.Length) {
                    return values[counter];
                }
                return values[values.Length - 1];
            };
        }

        private Action ThrowGenerator(bool startsByThrowing) {
            int counter = -1;
            if (startsByThrowing) {
                return () => {
                    counter++;
                    if (counter < 5) {
                        throw new Exception();
                    }
                };
            }

            return () => {
                counter++;
                if (counter < 5) {
                    return;
                }
                throw new Exception();
            };
        }

    }
}
#endif
