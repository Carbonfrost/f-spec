#if SELF_TEST

//
// Copyright 2017, 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
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

using Carbonfrost.Commons.Spec;
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.SelfTest.Spec.TestMatchers {

    public class EmptyMatcherTests : TestClass {

        [Fact]
        public void Matches_should_detect_empty_nominal() {
            var subj = new EmptyMatcher();
            Assert.True(subj.Matches(""));
        }

        [Fact]
        public void Matches_should_detect_non_empty_nominal() {
            var subj = new EmptyMatcher();
            Assert.False(subj.Matches("abc"));
        }

        [Fact]
        public void Matches_should_allow_null_and_detect_nominal() {
            var subj = new EmptyMatcher();
            Assert.False(subj.Matches((string) null));
        }

        [Fact]
        public void Matches_should_dispose_of_enumerator() {
            var subj = new EmptyMatcher();
            var item = new PHasDisposableEnumerator();

            subj.Matches(item);
            Assert.True(item.LastEnumerator.IsDisposed);
        }

        class PHasDisposableEnumerator : IEnumerable<char> {
            public Enumerator LastEnumerator;

            public IEnumerator<char> GetEnumerator() {
                return LastEnumerator = new Enumerator();
            }

            IEnumerator IEnumerable.GetEnumerator() {
                return GetEnumerator();
            }

            public class Enumerator : IEnumerator<char> {
                public bool IsDisposed {
                    get;
                    set;
                }

                public char Current {
                    get;
                }

                object IEnumerator.Current {
                    get;
                }

                public bool MoveNext() {
                    return false;
                }

                public void Reset() {
                }

                public void Dispose() {
                    IsDisposed = true;
                }
            }
        }

        [Fact]
        public void ExpectTo_should_obtain_matcher() {
            Expect("").To(Matchers.BeEmpty());
            Expect(new Int16[0]).To(Matchers.BeEmpty());
            Assert.IsInstanceOf<EmptyMatcher>(Matchers.BeEmpty());
        }

        [Fact]
        public void Expect_ToBe_fluent_expression() {
            Expect("").ToBe.Empty();
        }

        [Fact]
        public void Expect_via_enumerable_erasure_should_have_comprehensible_message() {
            // It is possible to write this at compile time because we treat T in Empty<T>()
            // as possibly erased.  (That is, T can be object, which is a common result
            // of the ToHave.Any/All operator.)  The error message that raises at runtime
            // should explain that an implicit conversion to IEnumerable that EmptyMatcher
            // needs has failed.
            try {
                Expect(new [] { 4 }).ToHave.All.Empty();

            } catch (AssertException e) {
                Assert.StartsWith("Invalid cast required by `spec.empty'.  This conversion may have been implicit", e.Message);
            }
        }

    }
}
#endif
