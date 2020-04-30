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

using Carbonfrost.Commons.Spec;
using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.SelfTest.Spec.ExecutionModel {

    public class TestTheoryTests {

        // Has no data attributes
        public void PNoDataAttributesTheory() {}

        [Fact]
        public void Children_empty_implies_log_messages() {
            var tt = new ReflectedTheory(GetType().GetMethod("PNoDataAttributesTheory"));
            var runner = new FakeRunner();
            var context = new TestContext(tt, runner, null, null);
            tt.BeforeExecutingSafe(context);

            var evt = runner.Logger.Events;
            Assert.Equal("No test data for theory", ((TestMessageEventArgs) evt[0]).Message);
        }

        [Fact]
        public void Children_empty_implies_theory_is_failed() {
            Assert.UseStrictMode = true;
            try {
                var tt = new ReflectedTheory(GetType().GetMethod("PNoDataAttributesTheory"));
                var runner = new FakeRunner();
                var context = new TestContext(tt, runner, null, null);
                tt.BeforeExecutingSafe(context);

                Assert.True(tt.Failed);
                Assert.Equal("No test data for theory", tt.Reason);
            } finally {
                Assert.UseStrictMode = false;
            }
        }
    }
}
#endif
