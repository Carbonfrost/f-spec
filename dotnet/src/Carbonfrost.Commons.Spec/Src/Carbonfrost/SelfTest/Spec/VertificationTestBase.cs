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
using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.SelfTest.Spec {

    public abstract class VerificationTestBase : TestClass {

        private TestVerificationMode _previousMode;

        protected override void BeforeTest(TestUnit test) {
            _previousMode = TestRunner.Current.Options.Verification;
            TestRunner.Current.Options.Verification = TestVerificationMode.Strict;
        }

        protected override void AfterTest(TestUnit test) {
            TestRunner.Current.Options.Verification = _previousMode;
        }
    }
}
#endif
