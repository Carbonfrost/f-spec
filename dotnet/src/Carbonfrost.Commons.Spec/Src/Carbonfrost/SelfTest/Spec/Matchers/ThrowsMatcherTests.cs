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
using System.Reflection;
using System.Threading.Tasks;
using Carbonfrost.Commons.Spec;
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.SelfTest.Spec.TestMatchers {

    public class ThrowsMatcherTests : TestClass {

        private void ThrowingMethod() {
            throw new InvalidOperationException("Error", new AccessViolationException());
        }

        private object ThrowingFunc() {
            throw new InvalidOperationException();
        }

        private void NonThrowingMethod() {
        }

        private object NonThrowingFunc() {
            return null;
        }

        [Fact]
        public void Matches_should_detect_empty_nominal() {
            var subj = new ThrowsMatcher();
            var val = subj.Matches(ThrowingMethod);
            Assert.True(val);
        }

        [Fact]
        public void Matches_should_detect_non_empty_nominal() {
            var subj = new ThrowsMatcher();
            Assert.False(subj.Matches(NonThrowingMethod));
        }

        [Fact]
        public void ExpectTo_should_obtain_matcher() {
            Expect(ThrowingMethod).To(Matchers.Throw<InvalidOperationException>());
            Expect(ThrowingMethod).To(Matchers.Throw(typeof(InvalidOperationException)));

            Assert.IsInstanceOf<ThrowsMatcher>(Matchers.Throw<Exception>());
            Assert.IsInstanceOf<ThrowsMatcher>(Matchers.Throw(typeof(Exception)));
            Assert.IsInstanceOf<ThrowsMatcher>(Matchers.Throw());
        }

        [Fact]
        public void Throws_fluent_Expression() {
            Expect(ThrowingMethod).Will.Throw<InvalidOperationException>();
            Expect(ThrowingMethod).ToThrow.Exception<InvalidOperationException>();
        }

        [Fact]
        public void Throws_Not_fluent_Expression() {
            Expect(NonThrowingMethod).Will.Not.Throw<InvalidOperationException>();
            Expect(ThrowingMethod).Not.ToThrow.Exception<InvalidCastException>();
        }

        [Fact]
        public void Throws_message_match() {
            Expect(ThrowingMethod).ToThrow.Message.EqualTo("Error");
            Expect(ThrowingMethod).ToThrow.InnerException.InstanceOf<AccessViolationException>();
        }

        [Fact]
        public void Assert_Throws_should_detect_error() {
            Assert.Throws(typeof(InvalidOperationException), (Action) ThrowingMethod);
        }

        [Fact]
        public void Assert_Throws_should_detect_target_error() {
            var method = ((Action) ThrowingMethod).Method;
            Expect(() => method.Invoke(this, null)).To(Matchers.Throw(
                typeof(InvalidOperationException)).UnwindingTargetExceptions
            );
        }

        [Fact]
        public void Expect_Given_fluent_expression() {
            Given().Expect<object>(() => { throw new InvalidOperationException(); })
                .To(Matchers.Throw<InvalidOperationException>());
            Given("").Expect<object>(_ => { throw new InvalidOperationException(); })
                .To(Matchers.Throw<InvalidOperationException>());
            Given("").Expect((Action<string>) (_ => { throw new InvalidOperationException(); })).Will.Throw<InvalidOperationException>();
        }

        [Fact]
        public void Expect_Given_fluent_expression_action() {
            Given().Expect(() => { throw new InvalidOperationException(); })
                .To(Matchers.Throw<InvalidOperationException>());
        }

        [Fact]
        [PassExplicitly]
        public void Assert_Throws_should_generate_correct_error_message() {
            try {
                Assert.Throws<ArgumentException>(() => {});
            } catch (AssertException ex) {
                Expect(ex.TestFailure.Message).ToBe.EqualTo("Expected to throw \"ArgumentException\"");
                Expect(ex.TestFailure.UserData).ToHave.KeyWithValue("Actual", "<no exception>");
                Assert.Pass();
            }
        }

        [Fact]
        public void Assert_Throws_should_not_unwrap_TargetInvocationException_by_default() {
            Assert.Throws<TargetInvocationException>(
                () => throw new TargetInvocationException(new Exception())
            );
        }

        [Explicit("This test is designed to fail.  Assertions happen on the output")]
        [Fact]
        [Tag("acceptance")]
        public void Assert_on_async_should_have_redirected_stack_trace() {
            Expect(AsyncMethod).Not.To(Matchers.Throw());
        }

        private static async void AsyncMethod() {
            await Task.Delay(20);
            throw new ArgumentException();
        }
    }
}
#endif
