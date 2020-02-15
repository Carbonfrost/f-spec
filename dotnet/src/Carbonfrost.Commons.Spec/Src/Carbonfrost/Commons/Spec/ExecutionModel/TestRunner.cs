//
// Copyright 2016, 2018 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
        public event EventHandler<TestClassStartingEventArgs> TestClassStarting;
        public event EventHandler<TestClassStartedEventArgs> TestClassStarted;
        public event EventHandler<TestClassFinishedEventArgs> TestClassFinished;
        public event EventHandler<TestAssemblyStartingEventArgs> TestAssemblyStarting;
        public event EventHandler<TestAssemblyStartedEventArgs> TestAssemblyStarted;
        public event EventHandler<TestAssemblyFinishedEventArgs> TestAssemblyFinished;
        public event EventHandler<TestNamespaceStartingEventArgs> TestNamespaceStarting;
        public event EventHandler<TestNamespaceStartedEventArgs> TestNamespaceStarted;
        public event EventHandler<TestNamespaceFinishedEventArgs> TestNamespaceFinished;
        public event EventHandler<TestCaseStartingEventArgs> TestCaseStarting;
        public event EventHandler<TestCaseStartedEventArgs> TestCaseStarted;
        public event EventHandler<TestCaseFinishedEventArgs> TestCaseFinished;
        public event EventHandler<TestRunnerStartingEventArgs> TestRunnerStarting;
        public event EventHandler<TestRunnerStartedEventArgs> TestRunnerStarted;
        public event EventHandler<TestRunnerFinishedEventArgs> TestRunnerFinished;
        public event EventHandler<TestUnitStartingEventArgs> TestUnitStarting;
        public event EventHandler<TestUnitStartedEventArgs> TestUnitStarted;
        public event EventHandler<TestUnitFinishedEventArgs> TestUnitFinished;

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

            var lp = (LoaderPathCollection) Options.LoaderPaths;
            var list = lp.LoadAssemblies();
            var assemblyPath = list.Select(t => Path.GetDirectoryName(new Uri(t.CodeBase).LocalPath)).Distinct();

            lp.RegisterAssemblyResolve(assemblyPath);
            foreach (var asm in list) {
                run.AddAssembly(asm);
            }
            SpecLog.DidCreateTestRun();
            return run;
        }

        protected abstract TestRunResults RunTestsCore(TestRun run);

        protected virtual void OnTestRunnerStarted(TestRunnerStartedEventArgs e) {
            if (TestRunnerStarted != null) {
                TestRunnerStarted(this, e);
            }
        }

        protected virtual void OnTestRunnerFinished(TestRunnerFinishedEventArgs e) {
            if (TestRunnerFinished != null) {
                TestRunnerFinished(this, e);
            }
        }

        protected virtual void OnTestRunnerStarting(TestRunnerStartingEventArgs e) {
            if (TestRunnerStarting != null) {
                TestRunnerStarting(this, e);
            }
        }

        protected virtual void OnTestClassStarting(TestClassStartingEventArgs e) {
            if (TestClassStarting != null) {
                TestClassStarting(this, e);
            }
        }

        protected virtual void OnTestClassStarted(TestClassStartedEventArgs e) {
            if (TestClassStarted != null) {
                TestClassStarted(this, e);
            }
        }

        protected virtual void OnTestClassFinished(TestClassFinishedEventArgs e) {
            if (TestClassFinished != null) {
                TestClassFinished(this, e);
            }
        }

        protected virtual void OnTestAssemblyStarting(TestAssemblyStartingEventArgs e) {
            if (TestAssemblyStarting != null) {
                TestAssemblyStarting(this, e);
            }
        }

        protected virtual void OnTestAssemblyStarted(TestAssemblyStartedEventArgs e) {
            if (TestAssemblyStarted != null) {
                TestAssemblyStarted(this, e);
            }
        }

        protected virtual void OnTestAssemblyFinished(TestAssemblyFinishedEventArgs e) {
            if (TestAssemblyFinished != null) {
                TestAssemblyFinished(this, e);
            }
        }

        protected virtual void OnTestNamespaceStarting(TestNamespaceStartingEventArgs e) {
            if (TestNamespaceStarting != null) {
                TestNamespaceStarting(this, e);
            }
        }

        protected virtual void OnTestNamespaceStarted(TestNamespaceStartedEventArgs e) {
            if (TestNamespaceStarted != null) {
                TestNamespaceStarted(this, e);
            }
        }

        protected virtual void OnTestNamespaceFinished(TestNamespaceFinishedEventArgs e) {
            if (TestNamespaceFinished != null) {
                TestNamespaceFinished(this, e);
            }
        }

        protected virtual void OnTestCaseStarting(TestCaseStartingEventArgs e) {
            if (TestCaseStarting != null) {
                TestCaseStarting(this, e);
            }
        }

        protected virtual void OnTestCaseStarted(TestCaseStartedEventArgs e) {
            if (TestCaseStarted != null) {
                TestCaseStarted(this, e);
            }
        }

        protected virtual void OnTestCaseFinished(TestCaseFinishedEventArgs e) {
            if (TestCaseFinished != null) {
                TestCaseFinished(this, e);
            }
        }

        protected virtual void OnTestUnitStarting(TestUnitStartingEventArgs e) {
            if (TestUnitStarting != null) {
                TestUnitStarting(this, e);
            }
        }

        protected virtual void OnTestUnitStarted(TestUnitStartedEventArgs e) {
            if (TestUnitStarted != null) {
                TestUnitStarted(this, e);
            }
        }

        protected virtual void OnTestUnitFinished(TestUnitFinishedEventArgs e) {
            if (TestUnitFinished != null) {
                TestUnitFinished(this, e);
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
            OnTestRunnerStarting(e);
        }

        void ITestRunnerEventSink.NotifyTestRunnerStarted(TestRunnerStartedEventArgs e) {
            OnTestRunnerStarted(e);
        }

        void ITestRunnerEventSink.NotifyTestRunnerFinished(TestRunnerFinishedEventArgs e) {
            OnTestRunnerFinished(e);
        }

        void ITestRunnerEventSink.NotifyUnitStarting(TestUnitStartingEventArgs e) {
            switch (e.TestUnit.Type) {
                case TestUnitType.Assembly:
                    OnTestAssemblyStarting(new TestAssemblyStartingEventArgs(e));
                    break;
                case TestUnitType.Namespace:
                    OnTestNamespaceStarting(new TestNamespaceStartingEventArgs(e));
                    break;
                case TestUnitType.Class:
                case TestUnitType.SubjectClassBinding:
                    OnTestClassStarting(new TestClassStartingEventArgs(e));
                    break;
                case TestUnitType.Theory:
                    break;
                case TestUnitType.Fact:
                case TestUnitType.Case:
                    OnTestCaseStarting(new TestCaseStartingEventArgs(e));
                    break;
            }
            OnTestUnitStarting(e);
        }

        void ITestRunnerEventSink.NotifyUnitStarted(TestUnitStartedEventArgs e) {
            switch (e.TestUnit.Type) {
                case TestUnitType.Assembly:
                    OnTestAssemblyStarted(new TestAssemblyStartedEventArgs(e));
                    break;
                case TestUnitType.Namespace:
                    OnTestNamespaceStarted(new TestNamespaceStartedEventArgs(e));
                    break;
                case TestUnitType.Class:
                case TestUnitType.SubjectClassBinding:
                    OnTestClassStarted(new TestClassStartedEventArgs(e));
                    break;
                case TestUnitType.Theory:
                    break;
                case TestUnitType.Fact:
                case TestUnitType.Case:
                    OnTestCaseStarted(new TestCaseStartedEventArgs(e));
                    break;
            }
            OnTestUnitStarted(e);
        }

        void ITestRunnerEventSink.NotifyUnitFinished(TestUnitFinishedEventArgs e) {
            switch (e.TestUnit.Type) {
                case TestUnitType.Assembly:
                    OnTestAssemblyFinished(new TestAssemblyFinishedEventArgs(e));
                    break;
                case TestUnitType.Namespace:
                    OnTestNamespaceFinished(new TestNamespaceFinishedEventArgs(e));
                    break;
                case TestUnitType.Class:
                case TestUnitType.SubjectClassBinding:
                    OnTestClassFinished(new TestClassFinishedEventArgs(e));
                    break;
                case TestUnitType.Theory:
                    break;
                case TestUnitType.Fact:
                case TestUnitType.Case:
                    OnTestCaseFinished(new TestCaseFinishedEventArgs(e));
                    break;
            }
            OnTestUnitFinished(e);
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
