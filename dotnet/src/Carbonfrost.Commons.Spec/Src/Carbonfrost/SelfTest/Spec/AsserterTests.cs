#if SELF_TEST

//
// Copyright 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

    public class AsserterTests : TestClass {

        [Fact]
        public void Disabled_makes_calling_Assert_log_warnings() {
            var events = Log.CaptureEvents(() => {
                using (Assert.Disabled()) {
                    Assert.Fail();
                };
            });

            Assert.Equal("Can't assert in this context, and assertion was not evaluated: Explicitly failed" + Environment.NewLine, events.Last().Message);
        }

        [Fact]
        public void Disabled_makes_asserter_not_throw_exceptions() {
            Exception actualException = null;
            var events = Log.CaptureEvents(() => {
                using (Assert.Disabled()) {
                    actualException = Record.Exception(() => Assert.Fail());
                }
            });

            Assert.Null(actualException);
        }
    }
}
#endif
