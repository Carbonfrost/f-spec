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
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.SelfTest.Spec {

    public interface ISubject {
    }

    public class ASubject : ISubject {}
    public class BSubject : ISubject {}

    public class MySubjectTestClass : TestClass<ISubject> {

        [Fact]
        public void Subject_must_be_either_A_or_B() {
            Assert.NotNull(Subject);
            Expect(Subject.GetType()).ToSatisfy.Any(
                Matchers.Equal(typeof(ASubject)),
                Matchers.Equal(typeof(BSubject))
            );
        }

        [Fact]
        [PassExplicitly]
        public void Failure_should_contain_subject() {
            try {
                Assert.Null("");

            } catch (AssertException ex) {
                Assert.ContainsKey("Subject", ex.TestFailure.UserData);
                Assert.Pass();
            }
        }
    }
}
#endif
