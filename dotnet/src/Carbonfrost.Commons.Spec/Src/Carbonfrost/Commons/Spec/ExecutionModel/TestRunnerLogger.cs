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
using System.Threading;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    public abstract class TestRunnerLogger : IDisposable, ITestRunnerLogger, IServiceProvider {

        private IServiceProvider _serviceProvider;
        private ITestRunnerEvents _events;
        private int _isDisposed;

        public void Initialize(ITestRunnerEvents events, IServiceProvider serviceProvider) {
            if (events == null) {
                throw new ArgumentNullException("events");
            }
            _serviceProvider = serviceProvider;
            _events = events;
            events.TestClassStarting += events_TestClassStarting;
            events.TestClassStarted += events_TestClassStarted;
            events.TestClassFinished += events_TestClassFinished;
            events.TestAssemblyStarting += events_TestAssemblyStarting;
            events.TestAssemblyStarted += events_TestAssemblyStarted;
            events.TestAssemblyFinished += events_TestAssemblyFinished;
            events.TestNamespaceStarting += events_TestNamespaceStarting;
            events.TestNamespaceStarted += events_TestNamespaceStarted;
            events.TestNamespaceFinished += events_TestNamespaceFinished;
            events.TestCaseStarting += events_TestCaseStarting;
            events.TestCaseStarted += events_TestCaseStarted;
            events.TestCaseFinished += events_TestCaseFinished;
            events.TestRunnerFinished += events_TestRunnerFinished;
            events.TestRunnerStarted += events_TestRunnerStarted;
            events.TestRunnerStarting += events_TestRunnerStarting;
            events.TestUnitStarted += events_TestUnitStarted;
            events.TestUnitStarting += events_TestUnitStarting;
            events.TestUnitFinished += events_TestUnitFinished;
            events.TestTheoryStarted += events_TestTheoryStarted;
            events.TestTheoryStarting += events_TestTheoryStarting;
            events.TestTheoryFinished += events_TestTheoryFinished;
            events.Message += events_Message;
        }

        public virtual object GetService(Type serviceType) {
            if (serviceType == null) {
                throw new ArgumentNullException(nameof(serviceType));
            }
            return _serviceProvider.GetService(serviceType);
        }

        public void Dispose() {
            // Dispose and suppress finalization
            if (_isDisposed == 0) {
                // Only do client disposal the first time
                GC.SuppressFinalize(this);
                try {
                    Dispose(true);
                } finally {
                    Interlocked.Decrement(ref _isDisposed);
                }
            }
        }

        protected virtual void Dispose(bool disposing) {
            if (_events != null) {
                _events.TestClassStarting -= events_TestClassStarting;
                _events.TestClassStarted -= events_TestClassStarted;
                _events.TestClassFinished -= events_TestClassFinished;
                _events.TestAssemblyStarting -= events_TestAssemblyStarting;
                _events.TestAssemblyStarted -= events_TestAssemblyStarted;
                _events.TestAssemblyFinished -= events_TestAssemblyFinished;
                _events.TestNamespaceStarting -= events_TestNamespaceStarting;
                _events.TestNamespaceStarted -= events_TestNamespaceStarted;
                _events.TestNamespaceFinished -= events_TestNamespaceFinished;
                _events.TestCaseStarting -= events_TestCaseStarting;
                _events.TestCaseStarted -= events_TestCaseStarted;
                _events.TestCaseFinished -= events_TestCaseFinished;
                _events.TestRunnerFinished -= events_TestRunnerFinished;
                _events.TestRunnerStarted -= events_TestRunnerStarted;
                _events.TestRunnerStarting -= events_TestRunnerStarting;
                _events.Message -= events_Message;
                _events.TestUnitStarted -= events_TestUnitStarted;
                _events.TestUnitStarting -= events_TestUnitStarting;
                _events.TestUnitFinished -= events_TestUnitFinished;
                _events.TestTheoryStarted -= events_TestTheoryStarted;
                _events.TestTheoryStarting -= events_TestTheoryStarting;
                _events.TestTheoryFinished -= events_TestTheoryFinished;
                _events = null;
            }
            _serviceProvider = null;
        }

        ~TestRunnerLogger() {
            Dispose(false);
        }

        protected virtual void DefaultMessage(EventArgs e) {
            e.ToString();
        }

        protected virtual void OnTestClassStarting(TestClassStartingEventArgs e) {
            DefaultMessage(e);
        }

        protected virtual void OnTestClassStarted(TestClassStartedEventArgs e) {
            DefaultMessage(e);
        }

        protected virtual void OnTestClassFinished(TestClassFinishedEventArgs e) {
            DefaultMessage(e);
        }

        protected virtual void OnTestAssemblyStarting(TestAssemblyStartingEventArgs e) {
            DefaultMessage(e);
        }

        protected virtual void OnTestAssemblyStarted(TestAssemblyStartedEventArgs e) {
            DefaultMessage(e);
        }

        protected virtual void OnTestAssemblyFinished(TestAssemblyFinishedEventArgs e) {
            DefaultMessage(e);
        }

        protected virtual void OnTestNamespaceStarting(TestNamespaceStartingEventArgs e) {
            DefaultMessage(e);
        }

        protected virtual void OnTestNamespaceStarted(TestNamespaceStartedEventArgs e) {
            DefaultMessage(e);
        }

        protected virtual void OnTestNamespaceFinished(TestNamespaceFinishedEventArgs e) {
            DefaultMessage(e);
        }

        protected virtual void OnTestCaseStarting(TestCaseStartingEventArgs e) {
            DefaultMessage(e);
        }

        protected virtual void OnTestCaseStarted(TestCaseStartedEventArgs e) {
            DefaultMessage(e);
        }

        protected virtual void OnTestCaseFinished(TestCaseFinishedEventArgs e) {
            DefaultMessage(e);
        }

        protected virtual void OnTestUnitStarting(TestUnitStartingEventArgs e) {
            DefaultMessage(e);
        }

        protected virtual void OnTestUnitStarted(TestUnitStartedEventArgs e) {
            DefaultMessage(e);
        }

        protected virtual void OnTestUnitFinished(TestUnitFinishedEventArgs e) {
            DefaultMessage(e);
        }

        protected virtual void OnTestTheoryStarting(TestTheoryStartingEventArgs e) {
            DefaultMessage(e);
        }

        protected virtual void OnTestTheoryStarted(TestTheoryStartedEventArgs e) {
            DefaultMessage(e);
        }

        protected virtual void OnTestTheoryFinished(TestTheoryFinishedEventArgs e) {
            DefaultMessage(e);
        }

        protected virtual void OnTestRunnerFinished(TestRunnerFinishedEventArgs e) {
            DefaultMessage(e);
        }

        protected virtual void OnTestRunnerStarted(TestRunnerStartedEventArgs e) {
            DefaultMessage(e);
        }

        protected virtual void OnTestRunnerStarting(TestRunnerStartingEventArgs e) {
            DefaultMessage(e);
        }

        protected virtual void OnMessage(TestMessageEventArgs e) {
            DefaultMessage(e);
        }

        private void events_Message(object sender, TestMessageEventArgs e) {
            OnMessage(e);
        }

        private void events_TestRunnerFinished(object sender, TestRunnerFinishedEventArgs e) {
            OnTestRunnerFinished(e);
        }

        private void events_TestRunnerStarted(object sender, TestRunnerStartedEventArgs e) {
            OnTestRunnerStarted(e);
        }

        private void events_TestRunnerStarting(object sender, TestRunnerStartingEventArgs e) {
            OnTestRunnerStarting(e);
        }

        private void events_TestClassStarting(object sender, TestClassStartingEventArgs e) {
            OnTestClassStarting(e);
        }

        private void events_TestClassStarted(object sender, TestClassStartedEventArgs e) {
            OnTestClassStarted(e);
        }

        private void events_TestClassFinished(object sender, TestClassFinishedEventArgs e) {
            OnTestClassFinished(e);
        }

        private void events_TestAssemblyStarting(object sender, TestAssemblyStartingEventArgs e) {
            OnTestAssemblyStarting(e);
        }

        private void events_TestAssemblyStarted(object sender, TestAssemblyStartedEventArgs e) {
            OnTestAssemblyStarted(e);
        }

        private void events_TestAssemblyFinished(object sender, TestAssemblyFinishedEventArgs e) {
            OnTestAssemblyFinished(e);
        }

        private void events_TestNamespaceStarting(object sender, TestNamespaceStartingEventArgs e) {
            OnTestNamespaceStarting(e);
        }

        private void events_TestNamespaceStarted(object sender, TestNamespaceStartedEventArgs e) {
            OnTestNamespaceStarted(e);
        }

        private void events_TestNamespaceFinished(object sender, TestNamespaceFinishedEventArgs e) {
            OnTestNamespaceFinished(e);
        }

        private void events_TestCaseStarting(object sender, TestCaseStartingEventArgs e) {
            OnTestCaseStarting(e);
        }

        private void events_TestCaseStarted(object sender, TestCaseStartedEventArgs e) {
            OnTestCaseStarted(e);
        }

        private void events_TestCaseFinished(object sender, TestCaseFinishedEventArgs e) {
            OnTestCaseFinished(e);
        }

        private void events_TestUnitStarting(object sender, TestUnitStartingEventArgs e) {
            OnTestUnitStarting(e);
        }

        private void events_TestUnitStarted(object sender, TestUnitStartedEventArgs e) {
            OnTestUnitStarted(e);
        }

        private void events_TestUnitFinished(object sender, TestUnitFinishedEventArgs e) {
            OnTestUnitFinished(e);
        }

        private void events_TestTheoryStarting(object sender, TestTheoryStartingEventArgs e) {
            OnTestTheoryStarting(e);
        }

        private void events_TestTheoryStarted(object sender, TestTheoryStartedEventArgs e) {
            OnTestTheoryStarted(e);
        }

        private void events_TestTheoryFinished(object sender, TestTheoryFinishedEventArgs e) {
            OnTestTheoryFinished(e);
        }
    }
}
