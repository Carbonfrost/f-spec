//
// Copyright 2016, 2017, 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Collections.Generic;
using System.Linq;
using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    public partial class TestLog {

        private readonly ITestRunnerEventSink _evt;
        private IList<TestMessageEventArgs> _buffer;

        internal TestLog(ITestRunnerEventSink evt) {
            _evt = evt;
            _buffer = new List<TestMessageEventArgs>();
        }

        public void Error(string message) {
            CoreLog(message, TestMessageSeverity.Error);
        }

        public void Warn(string message) {
            CoreLog(message, TestMessageSeverity.Warning);
        }

        public void Fatal(string message) {
            CoreLog(message, TestMessageSeverity.Fatal);
        }

        public void Info(string message) {
            CoreLog(message, TestMessageSeverity.Information);
        }

        public void Trace(string message) {
            CoreLog(message, TestMessageSeverity.Trace);
        }

        public void Debug(string message) {
            CoreLog(message, TestMessageSeverity.Debug);
        }

        internal void Flush() {
            if (_buffer == null) {
                return;
            }
            foreach (var e in _buffer) {
                _evt.NotifyMessage(e);
            }
            // Removing buffering of messages from now on
            _buffer = null;
        }

        void CoreLog(string message, TestMessageSeverity sev) {
            var msg = new TestMessageEventArgs {
                Message = message,
                Severity = sev
            };

            // We're either buffering messages or writing them out
            // immediately
            if (_buffer == null) {
                _evt.NotifyMessage(msg);
            } else {
                _buffer.Add(msg);
            }
        }
    }
}
