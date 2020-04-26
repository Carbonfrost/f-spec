//
// Copyright 2016, 2018, 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Reflection;
using System.Linq;
using System.IO;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    public abstract class TestRunner : ITestRunnerEvents, ITestRunnerEventSink, IServiceProvider {

        public event EventHandler<TestMessageEventArgs> Message;
        public event EventHandler<TestClassStartingEventArgs> ClassStarting;
        public event EventHandler<TestClassStartedEventArgs> ClassStarted;
        public event EventHandler<TestClassFinishedEventArgs> ClassFinished;
        public event EventHandler<TestSubjectClassBindingStartingEventArgs> SubjectClassBindingStarting;
        public event EventHandler<TestSubjectClassBindingStartedEventArgs> SubjectClassBindingStarted;
        public event EventHandler<TestSubjectClassBindingFinishedEventArgs> SubjectClassBindingFinished;
        public event EventHandler<TestAssemblyStartingEventArgs> AssemblyStarting;
        public event EventHandler<TestAssemblyStartedEventArgs> AssemblyStarted;
        public event EventHandler<TestAssemblyFinishedEventArgs> AssemblyFinished;
        public event EventHandler<TestNamespaceStartingEventArgs> NamespaceStarting;
        public event EventHandler<TestNamespaceStartedEventArgs> NamespaceStarted;
        public event EventHandler<TestNamespaceFinishedEventArgs> NamespaceFinished;
        public event EventHandler<TestCaseStartingEventArgs> CaseStarting;
        public event EventHandler<TestCaseStartedEventArgs> CaseStarted;
        public event EventHandler<TestCaseFinishedEventArgs> CaseFinished;
        public event EventHandler<TestRunnerStartingEventArgs> RunnerStarting;
        public event EventHandler<TestRunnerStartedEventArgs> RunnerStarted;
        public event EventHandler<TestRunnerFinishedEventArgs> RunnerFinished;
        public event EventHandler<TestUnitStartingEventArgs> UnitStarting;
        public event EventHandler<TestUnitStartedEventArgs> UnitStarted;
        public event EventHandler<TestUnitFinishedEventArgs> UnitFinished;
        public event EventHandler<TestTheoryStartingEventArgs> TheoryStarting;
        public event EventHandler<TestTheoryStartedEventArgs> TheoryStarted;
        public event EventHandler<TestTheoryFinishedEventArgs> TheoryFinished;

        internal static string Version {
            get {
                var asm = typeof(TestRunner).GetTypeInfo().Assembly;
                string version = asm.GetName().Version.ToString();

                var fva = (AssemblyFileVersionAttribute) asm.GetCustomAttribute(typeof(AssemblyFileVersionAttribute));
                if (fva != null) {
                    version = fva.Version;
                }

                var attr = (AssemblyInformationalVersionAttribute) asm.GetCustomAttribute(typeof(AssemblyInformationalVersionAttribute));
                if (attr != null) {
                    string infoVersion = attr.InformationalVersion;
                    version += " - " + infoVersion;
                }
                return version;
            }
        }

        public TestRunnerOptions Options {
            get;
            private set;
        }

        internal string SessionId {
            get; private set;
        }

        protected TestRunner(TestRunnerOptions options) {
            Options = options.Clone();

            // HACK -- This is unsealed when the self-tests run because it could be modified
            // to facilitate runner-specific test setup (e.g. DSLGrammarTests)
            if (!Options.IsSelfTest) {
                Options.MakeReadOnly();
            }
            SessionId = Utility.RandomName();
        }

        public static TestRunner Create(TestRunnerOptions testRunnerOptions) {
            return new DefaultTestRunner(testRunnerOptions);
        }

        public TestRunResults RunTests() {
            var run  = CreateTestRun();
            return RunTestsCore(run);
        }

        protected virtual TestRun CreateTestRun() {
            var run = new TestRun();
            if (Options.IsSelfTest) {
                SpecLog.ActivatedSelfTestMode();
                run.AddAssembly(typeof(TestMatcher).GetTypeInfo().Assembly);
            }

            foreach (var asm in Options.LoadAssembliesAndBindLoader()) {
                run.AddAssembly(asm);
            }

            SpecLog.DidCreateTestRun();
            return run;
        }

        protected abstract TestRunResults RunTestsCore(TestRun run);

        protected virtual void OnRunnerStarted(TestRunnerStartedEventArgs e) {
            if (RunnerStarted != null) {
                RunnerStarted(this, e);
            }
        }

        protected virtual void OnRunnerFinished(TestRunnerFinishedEventArgs e) {
            if (RunnerFinished != null) {
                RunnerFinished(this, e);
            }
        }

        protected virtual void OnRunnerStarting(TestRunnerStartingEventArgs e) {
            if (RunnerStarting != null) {
                RunnerStarting(this, e);
            }
        }

        protected virtual void OnClassStarting(TestClassStartingEventArgs e) {
            if (ClassStarting != null) {
                ClassStarting(this, e);
            }
        }

        protected virtual void OnClassStarted(TestClassStartedEventArgs e) {
            if (ClassStarted != null) {
                ClassStarted(this, e);
            }
        }

        protected virtual void OnClassFinished(TestClassFinishedEventArgs e) {
            if (ClassFinished != null) {
                ClassFinished(this, e);
            }
        }

        protected virtual void OnSubjectClassBindingStarting(TestSubjectClassBindingStartingEventArgs e) {
            if (SubjectClassBindingStarting != null) {
                SubjectClassBindingStarting(this, e);
            }
        }

        protected virtual void OnSubjectClassBindingStarted(TestSubjectClassBindingStartedEventArgs e) {
            if (SubjectClassBindingStarted != null) {
                SubjectClassBindingStarted(this, e);
            }
        }

        protected virtual void OnSubjectClassBindingFinished(TestSubjectClassBindingFinishedEventArgs e) {
            if (SubjectClassBindingFinished != null) {
                SubjectClassBindingFinished(this, e);
            }
        }

        protected virtual void OnAssemblyStarting(TestAssemblyStartingEventArgs e) {
            if (AssemblyStarting != null) {
                AssemblyStarting(this, e);
            }
        }

        protected virtual void OnAssemblyStarted(TestAssemblyStartedEventArgs e) {
            if (AssemblyStarted != null) {
                AssemblyStarted(this, e);
            }
        }

        protected virtual void OnAssemblyFinished(TestAssemblyFinishedEventArgs e) {
            if (AssemblyFinished != null) {
                AssemblyFinished(this, e);
            }
        }

        protected virtual void OnNamespaceStarting(TestNamespaceStartingEventArgs e) {
            if (NamespaceStarting != null) {
                NamespaceStarting(this, e);
            }
        }

        protected virtual void OnNamespaceStarted(TestNamespaceStartedEventArgs e) {
            if (NamespaceStarted != null) {
                NamespaceStarted(this, e);
            }
        }

        protected virtual void OnNamespaceFinished(TestNamespaceFinishedEventArgs e) {
            if (NamespaceFinished != null) {
                NamespaceFinished(this, e);
            }
        }

        protected virtual void OnCaseStarting(TestCaseStartingEventArgs e) {
            if (CaseStarting != null) {
                CaseStarting(this, e);
            }
        }

        protected virtual void OnCaseStarted(TestCaseStartedEventArgs e) {
            if (CaseStarted != null) {
                CaseStarted(this, e);
            }
        }

        protected virtual void OnCaseFinished(TestCaseFinishedEventArgs e) {
            if (CaseFinished != null) {
                CaseFinished(this, e);
            }
        }

        protected virtual void OnUnitStarting(TestUnitStartingEventArgs e) {
            if (UnitStarting != null) {
                UnitStarting(this, e);
            }
        }

        protected virtual void OnUnitStarted(TestUnitStartedEventArgs e) {
            if (UnitStarted != null) {
                UnitStarted(this, e);
            }
        }

        protected virtual void OnUnitFinished(TestUnitFinishedEventArgs e) {
            if (UnitFinished != null) {
                UnitFinished(this, e);
            }
        }

        protected virtual void OnTheoryStarting(TestTheoryStartingEventArgs e) {
            if (TheoryStarting != null) {
                TheoryStarting(this, e);
            }
        }

        protected virtual void OnTheoryStarted(TestTheoryStartedEventArgs e) {
            if (TheoryStarted != null) {
                TheoryStarted(this, e);
            }
        }

        protected virtual void OnTheoryFinished(TestTheoryFinishedEventArgs e) {
            if (TheoryFinished != null) {
                TheoryFinished(this, e);
            }
        }

        protected virtual void OnMessage(TestMessageEventArgs e) {
            if (Message != null) {
                Message(this, e);
            }
        }

        void ITestRunnerEventSink.NotifyMessage(TestMessageEventArgs e) {
            OnMessage(e);
        }

        void ITestRunnerEventSink.NotifyTestRunnerStarting(TestRunnerStartingEventArgs e) {
            OnRunnerStarting(e);
        }

        void ITestRunnerEventSink.NotifyTestRunnerStarted(TestRunnerStartedEventArgs e) {
            OnRunnerStarted(e);
        }

        void ITestRunnerEventSink.NotifyTestRunnerFinished(TestRunnerFinishedEventArgs e) {
            OnRunnerFinished(e);
        }

        void ITestRunnerEventSink.NotifyUnitStarting(TestUnitStartingEventArgs e) {
            switch (e.TestUnit.Type) {
                case TestUnitType.Assembly:
                    OnAssemblyStarting(new TestAssemblyStartingEventArgs(e));
                    break;
                case TestUnitType.Namespace:
                    OnNamespaceStarting(new TestNamespaceStartingEventArgs(e));
                    break;
                case TestUnitType.Class:
                    OnClassStarting(new TestClassStartingEventArgs(e));
                    break;
                case TestUnitType.SubjectClassBinding:
                    OnSubjectClassBindingStarting(new TestSubjectClassBindingStartingEventArgs(e));
                    break;
                case TestUnitType.Theory:
                    OnTheoryStarting(new TestTheoryStartingEventArgs(e));
                    break;
                case TestUnitType.Fact:
                case TestUnitType.Case:
                    OnCaseStarting(new TestCaseStartingEventArgs(e));
                    break;
            }
            OnUnitStarting(e);
        }

        void ITestRunnerEventSink.NotifyUnitStarted(TestUnitStartedEventArgs e) {
            switch (e.TestUnit.Type) {
                case TestUnitType.Assembly:
                    OnAssemblyStarted(new TestAssemblyStartedEventArgs(e));
                    break;
                case TestUnitType.Namespace:
                    OnNamespaceStarted(new TestNamespaceStartedEventArgs(e));
                    break;
                case TestUnitType.Class:
                    OnClassStarted(new TestClassStartedEventArgs(e));
                    break;
                case TestUnitType.SubjectClassBinding:
                    OnSubjectClassBindingStarted(new TestSubjectClassBindingStartedEventArgs(e));
                    break;
                case TestUnitType.Theory:
                    OnTheoryStarted(new TestTheoryStartedEventArgs(e));
                    break;
                case TestUnitType.Fact:
                case TestUnitType.Case:
                    OnCaseStarted(new TestCaseStartedEventArgs(e));
                    break;
            }
            OnUnitStarted(e);
        }

        void ITestRunnerEventSink.NotifyUnitFinished(TestUnitFinishedEventArgs e) {
            switch (e.TestUnit.Type) {
                case TestUnitType.Assembly:
                    OnAssemblyFinished(new TestAssemblyFinishedEventArgs(e));
                    break;
                case TestUnitType.Namespace:
                    OnNamespaceFinished(new TestNamespaceFinishedEventArgs(e));
                    break;
                case TestUnitType.Class:
                    OnClassFinished(new TestClassFinishedEventArgs(e));
                    break;
                case TestUnitType.SubjectClassBinding:
                    OnSubjectClassBindingFinished(new TestSubjectClassBindingFinishedEventArgs(e));
                    break;
                case TestUnitType.Theory:
                    OnTheoryFinished(new TestTheoryFinishedEventArgs(e));
                    break;
                case TestUnitType.Fact:
                case TestUnitType.Case:
                    OnCaseFinished(new TestCaseFinishedEventArgs(e));
                    break;
            }
            OnUnitFinished(e);
        }

        public virtual object GetService(Type serviceType) {
            if (serviceType == null) {
                throw new ArgumentNullException("serviceType");
            }
            if (serviceType.GetTypeInfo().IsInstanceOfType(this)) {
                return this;
            }
            return null;
        }
    }
}
