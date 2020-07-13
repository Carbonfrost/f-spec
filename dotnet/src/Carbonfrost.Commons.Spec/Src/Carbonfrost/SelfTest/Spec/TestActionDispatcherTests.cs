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

namespace Carbonfrost.SelfTest.Spec {

    public class TestActionDispatcherTests {

        [Fact]
        public void Before_will_be_called_first() {
            bool beforeCalled = false;
            var dispatcher = new TestActionDispatcher(() => {});
            dispatcher.Before(() => { beforeCalled = true; });

            dispatcher.Invoke();
            Assert.True(beforeCalled);
        }

        [Fact]
        public void After_will_be_called_after() {
            bool afterCalled = false;
            var dispatcher = new TestActionDispatcher(() => {});
            dispatcher.After(() => { afterCalled = true; });

            dispatcher.Invoke();
            Assert.True(afterCalled);
        }
    }
}
