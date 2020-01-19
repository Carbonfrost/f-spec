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
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.SelfTest.Spec {

    public class ExpectedExceptionAttributesTests : VerificationTestBase {

        [Fact]
        [PassExplicitly]
        public void ExpectedExceptionAttribute_should_raise_verification_error() {
            ITestMatcherFactory attr = new ExpectedExceptionAttribute(typeof(Exception));
            var matcher = attr.CreateMatcher(TestContext);
            try {
                matcher.Matches(() => { throw new AssertException(); });

            } catch (AssertVerificationException ex) {
                const string actualMessage = "Can't use `ExpectedExceptionAttribute` or `ThrowsAttribute` on a test that throws assertions exceptions if those exceptions might be caught by the attribute.  Replace with `Assert.Throws`.";
                Expect(ex.Message).ToBe.EqualTo(actualMessage);
                Assert.Pass();
            }
        }

    }
}
#endif
