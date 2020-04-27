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
using System.Collections.Generic;
using System.Linq;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    class BufferMessageEventCache : TestRunnerLogger {

        private readonly IList<TestMessageEventArgs> _bufferLog = new List<TestMessageEventArgs>();
        private readonly Dictionary<int, IList<TestMessageEventArgs>> _items = new Dictionary<int, IList<TestMessageEventArgs>>();

        protected override void OnClassFinished(TestClassFinishedEventArgs e) {
            Store(e.Results);
        }

        protected override void OnCaseFinished(TestCaseFinishedEventArgs e) {
            Store(e.Result);
        }

        protected override void OnTheoryFinished(TestTheoryFinishedEventArgs e) {
            Store(e.Results);
        }

        protected override void OnMessage(TestMessageEventArgs e) {
            _bufferLog.Add(e);
        }

        private void Store(TestUnitResult result) {
            result.Messages.AddRange(_bufferLog);
            _bufferLog.Clear();
        }
    }
}
