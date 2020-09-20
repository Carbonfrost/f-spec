//
// Copyright 2016, 2017, 2018, 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     https://www.apache.org/licenses/LICENSE-2.0
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
using System.Text;
using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.Commons.Spec {

    public abstract partial class TestClass : ITestExecutionFilter, ITestExecutionContext {

        private readonly Stack<TestExecutionContext> _context = new Stack<TestExecutionContext>();
        private char _runTestIndex = 'A';

        internal static bool HasSelfTests {
            get {
                return Type.GetType("Carbonfrost.SelfTest.Spec.AssertTests") != null;
            }
        }

        public TestExecutionContext TestContext {
            get {
                return _context.Peek();
            }
        }

        public TestLog Log {
            get {
                return TestContext.Log;
            }
        }

        private string DefaultTestName {
            get {
                var c = _runTestIndex++;
                return $"dynamic {c}";
            }
        }

        public IExpectationBuilder<IEnumerable> Expect() {
            return Assert.Expect();
        }

        public IExpectationBuilder<T> Expect<T>(T value) {
            return Assert.Expect(value);
        }

        public IExpectationBuilder<TEnumerable, T> Expect<TEnumerable, T>(TEnumerable value) where TEnumerable : IEnumerable<T> {
            return Assert.Expect<TEnumerable, T>(value);
        }

        public IExpectationBuilder<TValue[], TValue> Expect<TValue>(params TValue[] value) {
            return Assert.Expect((TValue[]) value);
        }

        public IExpectationBuilder Expect(Action value) {
            return Assert.Expect(value);
        }

        public IExpectationBuilder<T> Expect<T>(Func<T> func) {
            return Assert.Expect<T>(func);
        }

        public TestFixture Fixture(string path) {
            return TestFixture.FromFile(path);
        }

        public TestFixtureData FixtureData(string path) {
            return TestFixtureData.FromFile(path);
        }

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

        public TestCaseResult RunTest(Action<TestExecutionContext> testFunc) {
            return TestContext.RunTest(DefaultTestName, testFunc);
        }

        public TestCaseResult RunTest(Action<TestExecutionContext> testFunc, TestOptions options) {
            return TestContext.RunTest(testFunc, options);
        }

        public TestCaseResult RunTest(Func<TestExecutionContext, object> testFunc) {
            return TestContext.RunTest(DefaultTestName, testFunc);
        }

        public TestCaseResult RunTest(Func<TestExecutionContext, object> testFunc, TestOptions options) {
            return TestContext.RunTest(testFunc, options);
        }

        public TestCaseResult RunTest(string name, Action<TestExecutionContext> testFunc) {
            return TestContext.RunTest(name, testFunc);
        }

        public TestCaseResult RunTest(string name, Func<TestExecutionContext, object> testFunc) {
            return TestContext.RunTest(name, testFunc);
        }

        public TestUnitResults RunTests(string name, ITestDataProvider testDataProvider, Action<TestExecutionContext> testFunc) {
            return TestContext.RunTests(name, testDataProvider, testFunc);
        }

        public TestUnitResults RunTests(string name, ITestDataProvider testDataProvider, Func<TestExecutionContext, object> testFunc) {
            return TestContext.RunTests(name, testDataProvider, testFunc);
        }

        public TestUnitResults RunTests(ITestDataProvider testDataProvider, Action<TestExecutionContext> testFunc) {
            return TestContext.RunTests(DefaultTestName, testDataProvider, testFunc);
        }

        public TestUnitResults RunTests(ITestDataProvider testDataProvider, Action<TestExecutionContext> testFunc, TestOptions options) {
            return TestContext.RunTests(testDataProvider, testFunc, options);
        }

        public TestUnitResults RunTests(ITestDataProvider testDataProvider, Func<TestExecutionContext, object> testFunc) {
            return TestContext.RunTests(DefaultTestName, testDataProvider, testFunc);
        }

        public TestUnitResults RunTests(ITestDataProvider testDataProvider, Func<TestExecutionContext, object> testFunc, TestOptions options) {
            return TestContext.RunTests(testDataProvider, testFunc, options);
        }

        public TestCaseResult XRunTest(Action<TestExecutionContext> testFunc) {
            return TestContext.XRunTest(DefaultTestName, testFunc);
        }

        public TestCaseResult XRunTest(Action<TestExecutionContext> testFunc, TestOptions options) {
            return TestContext.XRunTest(testFunc, options);
        }

        public TestCaseResult XRunTest(Func<TestExecutionContext, object> testFunc) {
            return TestContext.XRunTest(DefaultTestName, testFunc);
        }

        public TestCaseResult XRunTest(Func<TestExecutionContext, object> testFunc, TestOptions options) {
            return TestContext.XRunTest(testFunc, options);
        }

        public TestCaseResult XRunTest(string name, Action<TestExecutionContext> testFunc) {
            return TestContext.XRunTest(name, testFunc);
        }

        public TestCaseResult XRunTest(string name, Func<TestExecutionContext, object> testFunc) {
            return TestContext.XRunTest(name, testFunc);
        }

        public TestUnitResults XRunTests(string name, ITestDataProvider testDataProvider, Action<TestExecutionContext> testFunc) {
            return TestContext.XRunTests(name, testDataProvider, testFunc);
        }

        public TestUnitResults XRunTests(string name, ITestDataProvider testDataProvider, Func<TestExecutionContext, object> testFunc) {
            return TestContext.XRunTests(name, testDataProvider, testFunc);
        }

        public TestUnitResults XRunTests(ITestDataProvider testDataProvider, Action<TestExecutionContext> testFunc) {
            return TestContext.XRunTests(DefaultTestName, testDataProvider, testFunc);
        }

        public TestUnitResults XRunTests(ITestDataProvider testDataProvider, Action<TestExecutionContext> testFunc, TestOptions options) {
            return TestContext.XRunTests(testDataProvider, testFunc, options);
        }

        public TestUnitResults XRunTests(ITestDataProvider testDataProvider, Func<TestExecutionContext, object> testFunc) {
            return TestContext.XRunTests(DefaultTestName, testDataProvider, testFunc);
        }

        public TestUnitResults XRunTests(ITestDataProvider testDataProvider, Func<TestExecutionContext, object> testFunc, TestOptions options) {
            return TestContext.XRunTests(testDataProvider, testFunc, options);
        }

        public TestCaseResult FRunTest(Action<TestExecutionContext> testFunc) {
            return TestContext.FRunTest(DefaultTestName, testFunc);
        }

        public TestCaseResult FRunTest(Action<TestExecutionContext> testFunc, TestOptions options) {
            return TestContext.FRunTest(testFunc, options);
        }

        public TestCaseResult FRunTest(Func<TestExecutionContext, object> testFunc) {
            return TestContext.FRunTest(DefaultTestName, testFunc);
        }

        public TestCaseResult FRunTest(Func<TestExecutionContext, object> testFunc, TestOptions options) {
            return TestContext.FRunTest(testFunc, options);
        }

        public TestCaseResult FRunTest(string name, Action<TestExecutionContext> testFunc) {
            return TestContext.FRunTest(name, testFunc);
        }

        public TestCaseResult FRunTest(string name, Func<TestExecutionContext, object> testFunc) {
            return TestContext.FRunTest(name, testFunc);
        }

        public TestUnitResults FRunTests(string name, ITestDataProvider testDataProvider, Action<TestExecutionContext> testFunc) {
            return TestContext.FRunTests(name, testDataProvider, testFunc);
        }

        public TestUnitResults FRunTests(string name, ITestDataProvider testDataProvider, Func<TestExecutionContext, object> testFunc) {
            return TestContext.FRunTests(name, testDataProvider, testFunc);
        }

        public TestUnitResults FRunTests(ITestDataProvider testDataProvider, Action<TestExecutionContext> testFunc) {
            return TestContext.FRunTests(DefaultTestName, testDataProvider, testFunc);
        }

        public TestUnitResults FRunTests(ITestDataProvider testDataProvider, Action<TestExecutionContext> testFunc, TestOptions options) {
            return TestContext.FRunTests(testDataProvider, testFunc, options);
        }

        public TestUnitResults FRunTests(ITestDataProvider testDataProvider, Func<TestExecutionContext, object> testFunc) {
            return TestContext.FRunTests(DefaultTestName, testDataProvider, testFunc);
        }

        public TestUnitResults FRunTests(ITestDataProvider testDataProvider, Func<TestExecutionContext, object> testFunc, TestOptions options) {
            return TestContext.FRunTests(testDataProvider, testFunc, options);
        }

        void ITestExecutionFilter.BeforeExecuting(TestContext testContext) {
            _context.Push((TestExecutionContext) testContext);
            BeforeTest(testContext.TestUnit);
            BeforeTest();
        }

        void ITestExecutionFilter.AfterExecuting(TestExecutionContext testContext) {
            try {
                AfterTest(testContext.TestUnit);
                AfterTest();
            } finally {
                _context.Pop();
            }
        }
    }
}
