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

    public abstract partial class TestContext : DisposableObject, ITestContext, ITestUnitApiConventions {

        private readonly List<object> _disposables = new List<object>();

        private static ThreadLocal<TestContext> _current = new ThreadLocal<TestContext>();

        public static TestContext Current {
            get {
                return _current.Value;
            }
        }

        public abstract TestEnvironment Environment {
            get;
        }

        internal abstract TestLoader Loader {
            get;
        }

        internal IDisposable ApplyingContext() {
            _current.Value = this;
            return new Disposer(() => _current.Value = null);
        }

        public TestDataProviderCollection TestDataProviders {
            get {
                if (TestUnit == null) {
                    return TestDataProviderCollection.Empty;
                }
                return TestUnit.TestDataProviders;
            }
        }

        public TestTagCollection Tags {
            get {
                if (TestUnit == null) {
                    return TestTagCollection.Empty;
                }
                return TestUnit.Tags;
            }
        }

        public abstract TestLog Log {
            get;
        }

        public abstract ITestRunnerEvents TestRunnerEvents {
            get;
        }

        public abstract TestRunnerOptions TestRunnerOptions {
            get;
        }

        public abstract Random Random {
            get;
        }

        public abstract TestUnit TestUnit {
            get;
        }

        // A dummy test object is required during initialization so we can
        // invoke public instance properties and fields for test data providers

        internal object DummyTestObject {
            get {
                return Activator.CreateInstance(TestUnit.FindTestClass().TestClass);
            }
        }

        internal bool ShouldVerify {
            get {
                return Assert.UseStrictMode;
            }
        }

        protected TestContext() {
        }

        internal TestContext WithSelf(TestUnit newSelf) {
            return new DefaultTestContext(this, newSelf);
        }

        public abstract TestTemporaryDirectory CreateTempDirectory(string name);

        public TestTemporaryDirectory CreateTempDirectory() {
            return CreateTempDirectory(null);
        }

        public abstract TestTemporaryFile CreateTempFile(string name);

        public TestTemporaryFile CreateTempFile() {
            return CreateTempFile(null);
        }

        public TestFixture LoadFixture(string fileName) {
            return Loader.LoadFixture(fileName);
        }

        public TestFixture DownloadFixture(Uri url) {
            return Loader.DownloadFixture(url);
        }

        public TestFixtureData LoadFixtureData(string fileName) {
            return Loader.LoadFixtureData(fileName);
        }

        public TestFixtureData DownloadFixtureData(Uri url) {
            return Loader.DownloadFixtureData(url);
        }

        public Stream OpenRead(string fileName) {
            return Loader.Open(fileName).OpenRead();
        }

        public Stream Download(Uri url) {
            return Loader.Download(url);
        }

        public StreamReader OpenText(string fileName) {
            return Loader.Open(fileName).OpenText();
        }

        public TextReader DownloadText(Uri url) {
            return Loader.DownloadText(url);
        }

        public TestFile DownloadFile(Uri url) {
            return Loader.DownloadFile(url);
        }

        public TestFile LoadFile(string fileName) {
            return Loader.LoadFile(fileName);
        }

        public TestFile OpenFile(string fileName) {
            return LoadFile(fileName);
        }

        public string GetFullPath(string fileName) {
            return Loader.GetFixtureFullPath(fileName);
        }

        public byte[] ReadAllBytes(string fileName) {
            return Loader.Open(fileName).ReadAllBytes();
        }

        public StreamReader OpenText(string fileName, Encoding encoding) {
            return Loader.Open(fileName).OpenText(encoding);
        }

        public IEnumerable<string> ReadLines(string fileName) {
            return Loader.Open(fileName).ReadLines();
        }

        public IEnumerable<string> ReadLines(string fileName, Encoding encoding) {
            return Loader.Open(fileName).ReadLines(encoding);
        }

        public string ReadAllText(string fileName) {
            return Loader.Open(fileName).ReadAllText();
        }

        public string ReadAllText(string fileName, Encoding encoding) {
            return Loader.Open(fileName).ReadAllText(encoding);
        }

        public string[] ReadAllLines(string fileName) {
            return Loader.Open(fileName).ReadAllLines();
        }

        public string[] ReadAllLines(string fileName, Encoding encoding) {
            return Loader.Open(fileName).ReadAllLines(encoding);
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

        internal T RegisterDisposable<T>(T item) {
            _disposables.Add(item);
            return item;
        }
    }
}
