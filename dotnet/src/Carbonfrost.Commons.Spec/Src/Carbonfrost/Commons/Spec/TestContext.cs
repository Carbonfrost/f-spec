//
// Copyright 2016, 2017, 2018, 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.IO;
using System.Text;
using System.Threading;

using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.Commons.Spec {

    public partial class TestContext : DisposableObject, ITestContext, ITestUnitApiConventions {

        private readonly TestUnit _self;
        private readonly TestRunner _runner;
        private readonly TestLoader _loader;
        private readonly List<object> _disposables = new List<object>();
        private readonly TestLog _log;
        private TestTemporaryDirectory _defaultTemp;
        private readonly Random _random;
        private readonly TestEnvironment _environment = new TestEnvironment();

        private static ThreadLocal<TestContext> _current = new ThreadLocal<TestContext>();

        public static TestContext Current {
            get {
                return _current.Value;
            }
        }

        public TestEnvironment Environment {
            get {
                return _environment;
            }
        }

        internal IDisposable ApplyingContext() {
            _current.Value = this;
            return new Disposer(() => _current.Value = null);
        }

        public TestDataProviderCollection TestDataProviders {
            get {
                return CurrentTest.TestDataProviders;
            }
        }

        public TestTagCollection Tags {
            get {
                return CurrentTest.Tags;
            }
        }

        public TestLog Log {
            get {
                return _log;
            }
        }

        public ITestRunnerEvents TestRunnerEvents {
            get {
                return _runner;
            }
        }

        public TestRunnerOptions TestRunnerOptions {
            get {
                return _runner.Options;
            }
        }

        public Random Random {
            get {
                return _random;
            }
        }

        public TestUnit CurrentTest {
            get {
                return _self;
            }
        }

        // A dummy test object is required during initialization so we can
        // invoke public instance properties and fields for test data providers

        internal object DummyTestObject {
            get {
                return Activator.CreateInstance(CurrentTest.FindTestClass().TestClass);
            }
        }

        internal bool ShouldVerify {
            get {
                return Assert.UseStrictMode;
            }
        }

        private protected TestContext(TestUnit self, TestRunner runner, Random random) {
            _self = self;
            _runner = runner;
            _log = new TestLog(runner);
            _random = random;
            _loader = new TestLoader(_runner.Options, this);
        }

        internal static TestContext NewInitContext(TestUnit unit, TestRunner runner) {
            return new TestContext(unit, runner, runner.RandomCache);
        }

        internal static TestExecutionContext NewExecContext(TestUnit unit, TestRunner runner, object testObject) {
            return new TestExecutionContext(unit, runner, runner.RandomCache, testObject);
        }

        internal TestContext WithSelf(TestUnit newSelf) {
            return new TestContext(newSelf, _runner, Random);
        }

        public TestTemporaryDirectory CreateTempDirectory(string name) {
            return RegisterDisposable(new TestTemporaryDirectory(_runner.SessionId, name));
        }

        public TestTemporaryDirectory CreateTempDirectory() {
            return RegisterDisposable(new TestTemporaryDirectory(_runner.SessionId, null));
        }

        public TestTemporaryFile CreateTempFile(string name) {
            return DefaultTempDirectory().CreateFile(name);
        }

        public TestTemporaryFile CreateTempFile() {
            return DefaultTempDirectory().CreateFile();
        }

        public TestFixture LoadFixture(string fileName) {
            return _loader.LoadFixture(fileName);
        }

        public TestFixture DownloadFixture(Uri url) {
            return _loader.DownloadFixture(url);
        }

        public TestFixtureData LoadFixtureData(string fileName) {
            return _loader.LoadFixtureData(fileName);
        }

        public TestFixtureData DownloadFixtureData(Uri url) {
            return _loader.DownloadFixtureData(url);
        }

        public Stream OpenRead(string fileName) {
            return _loader.Open(fileName).OpenRead();
        }

        public Stream Download(Uri url) {
            return _loader.Download(url);
        }

        public StreamReader OpenText(string fileName) {
            return _loader.Open(fileName).OpenText();
        }

        public TextReader DownloadText(Uri url) {
            return _loader.DownloadText(url);
        }

        public TestFile DownloadFile(Uri url) {
            return _loader.DownloadFile(url);
        }

        public TestFile LoadFile(string fileName) {
            return _loader.LoadFile(fileName);
        }

        public TestFile OpenFile(string fileName) {
            return LoadFile(fileName);
        }

        public string GetFullPath(string fileName) {
            return _loader.GetFixtureFullPath(fileName);
        }

        public byte[] ReadAllBytes(string fileName) {
            return _loader.Open(fileName).ReadAllBytes();
        }

        public StreamReader OpenText(string fileName, Encoding encoding) {
            return _loader.Open(fileName).OpenText(encoding);
        }

        public IEnumerable<string> ReadLines(string fileName) {
            return _loader.Open(fileName).ReadLines();
        }

        public IEnumerable<string> ReadLines(string fileName, Encoding encoding) {
            return _loader.Open(fileName).ReadLines(encoding);
        }

        public string ReadAllText(string fileName) {
            return _loader.Open(fileName).ReadAllText();
        }

        public string ReadAllText(string fileName, Encoding encoding) {
            return _loader.Open(fileName).ReadAllText(encoding);
        }

        public string[] ReadAllLines(string fileName) {
            return _loader.Open(fileName).ReadAllLines();
        }

        public string[] ReadAllLines(string fileName, Encoding encoding) {
            return _loader.Open(fileName).ReadAllLines(encoding);
        }

        public TestProcess CreateProcess(string command, string arguments) {
            return RegisterDisposable(new TestProcess(command, arguments));
        }

        public TestProcess StartProcess(string command, string arguments) {
            var p = CreateProcess(command, arguments);
            p.Start();
            return p;
        }

        protected override void Dispose(bool manualDispose) {
            base.Dispose(manualDispose);
            if (manualDispose) {
                Safely.Dispose(_disposables);
            }
        }

        private TestTemporaryDirectory DefaultTempDirectory() {
            if (_defaultTemp == null) {
                _defaultTemp = CreateTempDirectory();
            }
            return _defaultTemp;
        }

        internal T RegisterDisposable<T>(T item) {
            _disposables.Add(item);
            return item;
        }
    }
}
