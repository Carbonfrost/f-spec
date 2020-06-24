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
using System.Threading;
using System.Threading.Tasks;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.SelfTest.Spec {

    public class AsyncTestClassTests : TestClass {

        [Fact]
        public async void Async_fact_method_should_run_and_complete_Pass() {
            await Task.Run(() => {
                               Assert.Pass("Pass the test");
                           });
        }

        [Theory]
        [InlineData(30)]
        public async void Async_theory_method_should_run_and_complete_Pass(int a) {
            await Task.Run(() => {
                               Assert.Pass("Pass the test");
                           });
        }

        [Fact]
        [ExpectedException(typeof(InvalidOperationException))]
        public async void Async_throw_on_ExpectedException() {
            await Task.Run(() => {
                               throw new InvalidOperationException();
                           });
        }

        [Fact]
        [Explicit("This test is designed to fail.  Assertions happen on the output")]
        [Timeout(10)]
        [Tag("integration")]
        public async void Async_should_handle_timeouts() {
            CancellationToken token = TestContext.CancellationToken;
            await Task.Delay(1000, token)
                .ContinueWith(t => {
                                  Console.WriteLine("Shouldn't be printed!  Tasks should have been canceled.");
                                  Assert.Fail();
                              }, token);
        }
    }
}
#endif
