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

    public class TestFuncDispatcherTests {

        [Fact]
        public void Before_will_be_called_first() {
            bool beforeCalled = false;
            var dispatcher = new TestFuncDispatcher<int>(() => 420);
            dispatcher.Before(() => { beforeCalled = true; });

            dispatcher.Invoke();
            Assert.True(beforeCalled);
        }

        [Fact]
        public void After_will_be_called_after() {
            bool afterCalled = false;
            var dispatcher = new TestFuncDispatcher<int>(() => 420);
            dispatcher.After(() => { afterCalled = true; });

            dispatcher.Invoke();
            Assert.True(afterCalled);
        }

        [Fact]
        public void Invoke_will_return_value_from_thunk() {
            var dispatcher = new TestFuncDispatcher<int, int, int>((s, t) => 420);
            Assert.Equal(420, dispatcher.Invoke(0, 0));
        }

        [Fact]
        public void Invoke_will_track_call_arguments_and_return_value() {
            var dispatcher = new TestFuncDispatcher<int, int, int>((s, t) => 420);
            dispatcher.Invoke(50, 54);

            Assert.Equal(1, dispatcher.CallCount);
            Assert.True(dispatcher.Called);
            Assert.Equal((50, 54), dispatcher.LastArgs);
            Assert.Equal((50, 54), dispatcher.ArgsForCall(0));
            Assert.Equal(420, dispatcher.ResultForCall(0));
        }
    }
}
