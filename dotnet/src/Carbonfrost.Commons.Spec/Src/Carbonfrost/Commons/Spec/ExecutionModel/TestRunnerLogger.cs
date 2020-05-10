//
// Copyright 2016, 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
            events.ClassStarting += events_TestClassStarting;
            events.ClassStarted += events_TestClassStarted;
            events.ClassFinished += events_TestClassFinished;
            events.AssemblyStarting += events_TestAssemblyStarting;
            events.AssemblyStarted += events_TestAssemblyStarted;
            events.AssemblyFinished += events_TestAssemblyFinished;
            events.NamespaceStarting += events_TestNamespaceStarting;
            events.NamespaceStarted += events_TestNamespaceStarted;
            events.NamespaceFinished += events_TestNamespaceFinished;
            events.CaseStarting += events_TestCaseStarting;
            events.CaseStarted += events_TestCaseStarted;
            events.CaseFinished += events_TestCaseFinished;
            events.RunnerFinished += events_TestRunnerFinished;
            events.RunnerStarted += events_TestRunnerStarted;
            events.RunnerStarting += events_TestRunnerStarting;
            events.UnitStarted += events_TestUnitStarted;
            events.UnitStarting += events_TestUnitStarting;
            events.UnitFinished += events_TestUnitFinished;
            events.TheoryStarted += events_TestTheoryStarted;
            events.TheoryStarting += events_TestTheoryStarting;
            events.TheoryFinished += events_TestTheoryFinished;
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
                _events.ClassStarting -= events_TestClassStarting;
                _events.ClassStarted -= events_TestClassStarted;
                _events.ClassFinished -= events_TestClassFinished;
                _events.SubjectClassBindingStarting -= events_TestSubjectClassBindingStarting;
                _events.SubjectClassBindingStarted -= events_TestSubjectClassBindingStarted;
                _events.SubjectClassBindingFinished -= events_TestSubjectClassBindingFinished;
                _events.AssemblyStarting -= events_TestAssemblyStarting;
                _events.AssemblyStarted -= events_TestAssemblyStarted;
                _events.AssemblyFinished -= events_TestAssemblyFinished;
                _events.NamespaceStarting -= events_TestNamespaceStarting;
                _events.NamespaceStarted -= events_TestNamespaceStarted;
                _events.NamespaceFinished -= events_TestNamespaceFinished;
                _events.CaseStarting -= events_TestCaseStarting;
                _events.CaseStarted -= events_TestCaseStarted;
                _events.CaseFinished -= events_TestCaseFinished;
                _events.RunnerFinished -= events_TestRunnerFinished;
                _events.RunnerStarted -= events_TestRunnerStarted;
                _events.RunnerStarting -= events_TestRunnerStarting;
                _events.Message -= events_Message;
                _events.UnitStarted -= events_TestUnitStarted;
                _events.UnitStarting -= events_TestUnitStarting;
                _events.UnitFinished -= events_TestUnitFinished;
                _events.TheoryStarted -= events_TestTheoryStarted;
                _events.TheoryStarting -= events_TestTheoryStarting;
                _events.TheoryFinished -= events_TestTheoryFinished;
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

        protected virtual void OnClassStarting(TestClassStartingEventArgs e) {
            DefaultMessage(e);
        }

        protected virtual void OnClassStarted(TestClassStartedEventArgs e) {
            DefaultMessage(e);
        }

        protected virtual void OnClassFinished(TestClassFinishedEventArgs e) {
            DefaultMessage(e);
        }

        protected virtual void OnSubjectClassBindingStarting(TestSubjectClassBindingStartingEventArgs e) {
            DefaultMessage(e);
        }

        protected virtual void OnSubjectClassBindingStarted(TestSubjectClassBindingStartedEventArgs e) {
            DefaultMessage(e);
        }

        protected virtual void OnSubjectClassBindingFinished(TestSubjectClassBindingFinishedEventArgs e) {
            DefaultMessage(e);
        }

        protected virtual void OnAssemblyStarting(TestAssemblyStartingEventArgs e) {
            DefaultMessage(e);
        }

        protected virtual void OnAssemblyStarted(TestAssemblyStartedEventArgs e) {
            DefaultMessage(e);
        }

        protected virtual void OnAssemblyFinished(TestAssemblyFinishedEventArgs e) {
            DefaultMessage(e);
        }

        protected virtual void OnNamespaceStarting(TestNamespaceStartingEventArgs e) {
            DefaultMessage(e);
        }

        protected virtual void OnNamespaceStarted(TestNamespaceStartedEventArgs e) {
            DefaultMessage(e);
        }

        protected virtual void OnNamespaceFinished(TestNamespaceFinishedEventArgs e) {
            DefaultMessage(e);
        }

        protected virtual void OnCaseStarting(TestCaseStartingEventArgs e) {
            DefaultMessage(e);
        }

        protected virtual void OnCaseStarted(TestCaseStartedEventArgs e) {
            DefaultMessage(e);
        }

        protected virtual void OnCaseFinished(TestCaseFinishedEventArgs e) {
            DefaultMessage(e);
        }

        protected virtual void OnUnitStarting(TestUnitStartingEventArgs e) {
            DefaultMessage(e);
        }

        protected virtual void OnUnitStarted(TestUnitStartedEventArgs e) {
            DefaultMessage(e);
        }

        protected virtual void OnUnitFinished(TestUnitFinishedEventArgs e) {
            DefaultMessage(e);
        }

        protected virtual void OnTheoryStarting(TestTheoryStartingEventArgs e) {
            DefaultMessage(e);
        }

        protected virtual void OnTheoryStarted(TestTheoryStartedEventArgs e) {
            DefaultMessage(e);
        }

        protected virtual void OnTheoryFinished(TestTheoryFinishedEventArgs e) {
            DefaultMessage(e);
        }

        protected virtual void OnRunnerFinished(TestRunnerFinishedEventArgs e) {
            DefaultMessage(e);
        }

        protected virtual void OnRunnerStarted(TestRunnerStartedEventArgs e) {
            DefaultMessage(e);
        }

        protected virtual void OnRunnerStarting(TestRunnerStartingEventArgs e) {
            DefaultMessage(e);
        }

        protected virtual void OnMessage(TestMessageEventArgs e) {
            DefaultMessage(e);
        }

        private void events_Message(object sender, TestMessageEventArgs e) {
            OnMessage(e);
        }

        private void events_TestRunnerFinished(object sender, TestRunnerFinishedEventArgs e) {
            OnRunnerFinished(e);
        }

        private void events_TestRunnerStarted(object sender, TestRunnerStartedEventArgs e) {
            OnRunnerStarted(e);
        }

        private void events_TestRunnerStarting(object sender, TestRunnerStartingEventArgs e) {
            OnRunnerStarting(e);
        }

        private void events_TestClassStarting(object sender, TestClassStartingEventArgs e) {
            OnClassStarting(e);
        }

        private void events_TestClassStarted(object sender, TestClassStartedEventArgs e) {
            OnClassStarted(e);
        }

        private void events_TestClassFinished(object sender, TestClassFinishedEventArgs e) {
            OnClassFinished(e);
        }

        private void events_TestSubjectClassBindingStarting(object sender, TestSubjectClassBindingStartingEventArgs e) {
            OnSubjectClassBindingStarting(e);
        }

        private void events_TestSubjectClassBindingStarted(object sender, TestSubjectClassBindingStartedEventArgs e) {
            OnSubjectClassBindingStarted(e);
        }

        private void events_TestSubjectClassBindingFinished(object sender, TestSubjectClassBindingFinishedEventArgs e) {
            OnSubjectClassBindingFinished(e);
        }

        private void events_TestAssemblyStarting(object sender, TestAssemblyStartingEventArgs e) {
            OnAssemblyStarting(e);
        }

        private void events_TestAssemblyStarted(object sender, TestAssemblyStartedEventArgs e) {
            OnAssemblyStarted(e);
        }

        private void events_TestAssemblyFinished(object sender, TestAssemblyFinishedEventArgs e) {
            OnAssemblyFinished(e);
        }

        private void events_TestNamespaceStarting(object sender, TestNamespaceStartingEventArgs e) {
            OnNamespaceStarting(e);
        }

        private void events_TestNamespaceStarted(object sender, TestNamespaceStartedEventArgs e) {
            OnNamespaceStarted(e);
        }

        private void events_TestNamespaceFinished(object sender, TestNamespaceFinishedEventArgs e) {
            OnNamespaceFinished(e);
        }

        private void events_TestCaseStarting(object sender, TestCaseStartingEventArgs e) {
            OnCaseStarting(e);
        }

        private void events_TestCaseStarted(object sender, TestCaseStartedEventArgs e) {
            OnCaseStarted(e);
        }

        private void events_TestCaseFinished(object sender, TestCaseFinishedEventArgs e) {
            OnCaseFinished(e);
        }

        private void events_TestUnitStarting(object sender, TestUnitStartingEventArgs e) {
            OnUnitStarting(e);
        }

        private void events_TestUnitStarted(object sender, TestUnitStartedEventArgs e) {
            OnUnitStarted(e);
        }

        private void events_TestUnitFinished(object sender, TestUnitFinishedEventArgs e) {
            OnUnitFinished(e);
        }

        private void events_TestTheoryStarting(object sender, TestTheoryStartingEventArgs e) {
            OnTheoryStarting(e);
        }

        private void events_TestTheoryStarted(object sender, TestTheoryStartedEventArgs e) {
            OnTheoryStarted(e);
        }

        private void events_TestTheoryFinished(object sender, TestTheoryFinishedEventArgs e) {
            OnTheoryFinished(e);
        }
    }
}
