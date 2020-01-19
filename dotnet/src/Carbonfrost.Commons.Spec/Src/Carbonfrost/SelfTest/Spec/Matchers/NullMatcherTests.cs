#if SELF_TEST

//
// Copyright 2017, 2018 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Threading.Tasks;
using Carbonfrost.Commons.Spec;
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.SelfTest.Spec.TestMatchers {

    public class NullMatcherTests : TestClass {

        [Fact]
        public void Matches_should_detect_nonnull_nominal() {
            var subj = new NullMatcher();
            Assert.True(subj.Matches((object) null));
        }

        [Fact]
        public void Matches_should_detect_null_nominal() {
            var subj = new NullMatcher();
            Assert.False(subj.Matches("abc"));
        }

        [Fact]
        public void ExpectTo_should_obtain_matcher() {
            Expect((string) null).To(Matchers.BeNull());
            Assert.IsInstanceOf<NullMatcher>(Matchers.BeNull());
        }

        [Fact]
        public void Expect_ToBe_fluent_expression() {
            Expect((string) null).ToBe.Null();
        }

        [Fact]
        public void Expect_To_should_throw_if_used_on_value_type() {
            try {
                Expect((object) 420).To(Matchers.BeNull());
                Assert.Fail("Expected a validation exception to be thrown.");

            } catch (InvalidOperationException) {
                Assert.Pass();
            }
        }

        [Fact]
        public void NullAttribute_should_create_matcher() {
            ITestMatcherFactory<object> at = new NullAttribute();
            Assert.IsInstanceOf(typeof(NullMatcher), at.CreateMatcher(null));
        }

        [Fact]
        [return: Null]
        public string NullAttribute_should_apply_null() {
            return null;
        }

        [Fact]
        [return: Null]
        public async Task<string> NullAttribute_should_apply_null_async() {
            // We implicitly unwind the result of a task
            return await Task.Run(() => (string) null);
        }
    }
}
#endif
