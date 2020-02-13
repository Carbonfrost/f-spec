//
// Copyright 2016, 2017, 2018 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.Commons.Spec {

    public abstract partial class TestClass : ITestUnitAdapter, ITestContext {

        private TestContext _selfContext;
        private TestContext _descendantContext;

        internal static bool HasSelfTests {
            get {
                return Type.GetType("Carbonfrost.SelfTest.Spec.AssertTests") != null;
            }
        }

        public TestContext TestContext {
            get {
                return _descendantContext ?? _selfContext;
            }
        }

        public TestLog Log {
            get {
                return TestContext.Log;
            }
        }

        public ExpectationBuilder<IEnumerable> Expect() {
            return Assert.Expect();
        }

        public ExpectationBuilder<T> Expect<T>(T value) {
            return Assert.Expect(value);
        }

        public ExpectationBuilder<IEnumerable<TValue>> Expect<TValue>(params TValue[] value) {
            return Assert.Expect((TValue[]) value);
        }

        public ExpectationBuilder Expect(Action value) {
            return new ExpectationBuilder(value, false, null);
        }

        public ExpectationBuilder<T> Expect<T>(Func<T> func) {
            return Given().Expect(func);
        }

        protected virtual void Initialize() {
        }

        protected virtual void BeforeExecuting() {}
        protected virtual void AfterExecuting() {}

        protected virtual void BeforeTest() {
        }

        protected virtual void AfterTest() {
        }

        protected virtual void BeforeTest(TestUnit test) {
        }

        protected virtual void AfterTest(TestUnit test) {
        }

        public Stream OpenRead(string fileName) {
            return TestContext.OpenRead(fileName);
        }

        public TestFixture LoadFixture(string fileName) {
            return TestContext.LoadFixture(fileName);
        }

        public TestFixture DownloadFixture(Uri url) {
            return TestContext.DownloadFixture(url);
        }

        public TestFixtureData LoadFixtureData(string fileName) {
            return TestContext.LoadFixtureData(fileName);
        }

        public TestFixtureData DownloadFixtureData(Uri url) {
            return TestContext.DownloadFixtureData(url);
        }

        public TestFile DownloadFile(Uri url) {
            return TestContext.DownloadFile(url);
        }

        public TextReader DownloadText(Uri url) {
            return TestContext.DownloadText(url);
        }

        public Stream Download(Uri url) {
            return TestContext.Download(url);
        }

        public TestProcess CreateProcess(string command, string arguments) {
            return TestContext.CreateProcess(command, arguments);
        }

        public TestProcess StartProcess(string command, string arguments) {
            return TestContext.StartProcess(command, arguments);
        }

        public TestTemporaryDirectory CreateTempDirectory() {
            return TestContext.CreateTempDirectory();
        }

        public TestTemporaryDirectory CreateTempDirectory(string name) {
            return TestContext.CreateTempDirectory(name);
        }

        public TestTemporaryFile CreateTempFile() {
            return TestContext.CreateTempFile();
        }

        public TestTemporaryFile CreateTempFile(string name) {
            return TestContext.CreateTempFile(name);
        }

        public StreamReader OpenText(string fileName) {
            return TestContext.OpenText(fileName);
        }

        public TestFile LoadFile(string fileName) {
            return TestContext.LoadFile(fileName);
        }

        public TestFile OpenFile(string fileName) {
            return TestContext.OpenFile(fileName);
        }

        public string GetFullPath(string fileName) {
            return TestContext.GetFullPath(fileName);
        }

        public byte[] ReadAllBytes(string fileName) {
            return TestContext.ReadAllBytes(fileName);
        }

        public StreamReader OpenText(string fileName, Encoding encoding) {
            return TestContext.OpenText(fileName, encoding);
        }

        public IEnumerable<string> ReadLines(string fileName) {
            return TestContext.ReadLines(fileName);
        }

        public IEnumerable<string> ReadLines(string fileName, Encoding encoding) {
            return TestContext.ReadLines(fileName, encoding);
        }

        public string ReadAllText(string fileName) {
            return TestContext.ReadAllText(fileName);
        }

        public string ReadAllText(string fileName, Encoding encoding) {
            return TestContext.ReadAllText(fileName, encoding);
        }

        public string[] ReadAllLines(string fileName) {
            return TestContext.ReadAllLines(fileName);
        }

        public string[] ReadAllLines(string fileName, Encoding encoding) {
            return TestContext.ReadAllLines(fileName, encoding);
        }

        public TestCaseResult RunTest(Action<TestContext> testFunc) {
            return TestContext.RunTest(testFunc);
        }

        public TestCaseResult RunTest(Action<TestContext> testFunc, TestOptions options) {
            return TestContext.RunTest(testFunc, options);
        }

        public TestCaseResult RunTest(Func<TestContext, object> testFunc) {
            return TestContext.RunTest(testFunc);
        }

        public TestCaseResult RunTest(Func<TestContext, object> testFunc, TestOptions options) {
            return TestContext.RunTest(testFunc, options);
        }

        void ITestUnitAdapter.Initialize(TestContext testContext) {
            try {
                _selfContext = testContext;
                Initialize();
            } finally {
                _selfContext = null;
            }
        }

        void ITestExecutionFilter.BeforeExecuting(TestContext testContext) {
            _selfContext = testContext;
            BeforeExecuting();
        }

        void ITestExecutionFilter.AfterExecuting(TestContext testContext) {
            try {
                AfterExecuting();
            } finally {
                _selfContext = null;
            }
        }

        void ITestUnitAdapter.BeforeExecutingDescendant(TestContext descendantContext) {
            _descendantContext = descendantContext;
            BeforeTest(descendantContext.CurrentTest);
            BeforeTest();
        }

        void ITestUnitAdapter.AfterExecutingDescendant(TestContext descendantContext) {
            try {
                AfterTest(descendantContext.CurrentTest);
                AfterTest();
            } finally {
                _descendantContext = null;
            }
        }
    }
}
