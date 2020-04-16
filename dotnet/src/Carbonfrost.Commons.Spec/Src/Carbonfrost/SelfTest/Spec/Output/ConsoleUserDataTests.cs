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
using System.Linq;

using Carbonfrost.Commons.Spec;
using Carbonfrost.Commons.Spec.ExecutionModel;
using Carbonfrost.Commons.Spec.ExecutionModel.Output;

namespace Carbonfrost.SelfTest.Spec.ExecutionModel.Output {

    public class ConsoleUserDataTests {

        [Fact]
        public void Render_should_ignore_Expected_when_it_is_consumed() {
            var ud = new UserDataCollection {
                ExpectedConsumedInMessage = true,
                ["Expected"] = "not here",
                ["Actual"] = "Text of A",
            };
            Assert.Equal(
                new [] {
                    "        Actual: Text of A"
                },
                Render(ud)
            );
        }

        private string[] Render(UserDataCollection ud) {
            var console = new TestConsole();
            var context = new RenderContext {
                Parts = new ConsoleOutputParts(new TestRunnerOptions()),
                Console = console
            };
            new ConsoleUserData().Render(context, ud);
            return console.NonBlankLines.ToArray();
        }
    }
}

#endif