#if SELF_TEST

//
// Copyright 2016 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Threading.Tasks;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.SelfTest.Spec {

    public class RecordTests {

        [Fact]
        public void Exception_can_record_nominal() {
            Assert.IsInstanceOf<InvalidOperationException>(Record.Exception(() => { throw new InvalidOperationException(); }));
        }

        [Fact]
        public void Exception_can_record_null_when_no_error() {
            Assert.Null(Record.Exception(() => { }));
        }

        [Fact]
        public void Exception_can_record_thread_pool_exceptions() {
            Assert.IsInstanceOf<InvalidOperationException>(Record.Exception(async () => await ThrowsAnErrorAsync()));
        }

        [Fact]
        public void Exception_can_record_null_when_no_thread_pool_exceptions() {
            Assert.Null(Record.Exception(async () => await ThrowsNothing()));
        }

#pragma warning disable 1998
        private async Task ThrowsAnErrorAsync() {
            throw new InvalidOperationException();
        }

        private async Task ThrowsNothing() {
        }
#pragma warning restore 1998

    }
}
#endif
