//
// Copyright 2018 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Reflection;
using System.Text;

namespace Carbonfrost.Commons.Spec {

    public abstract class TestFileBase : DisposableObject, ITestFile {

        private readonly string _fileName;
        private static readonly MethodInfo OfMethod =
            typeof(TestFile).GetTypeInfo().GetMethod("Of");
        private string _textContents;

        public string FileName {
            get {
                return _fileName;
            }
        }

        public string TextContents {
            get {
                ThrowIfDisposed();

                if (_textContents == null) {
                    _textContents = ReadAllText();
                }
                return _textContents;
            }
        }

        internal TestFileBase(string fileName) {
            _fileName = fileName;
        }

        public void AppendAllText(string text) {
            ThrowIfDisposed();
            File.AppendAllText(_fileName, text);
        }

        public void AppendAllText(string contents, Encoding encoding) {
            ThrowIfDisposed();
            File.AppendAllText(_fileName, contents, encoding);
        }

        public Stream OpenRead() {
            ThrowIfDisposed();
            return File.OpenRead(_fileName);
        }

        public void WriteAllText(string contents) {
            ThrowIfDisposed();
            File.WriteAllText(_fileName, contents);
        }

        public void WriteAllText(string contents, Encoding encoding) {
            ThrowIfDisposed();
            File.WriteAllText(_fileName, contents, encoding);
        }

        public byte[] ReadAllBytes() {
            ThrowIfDisposed();
            return File.ReadAllBytes(_fileName);
        }

        public IEnumerable<string> ReadLines() {
            ThrowIfDisposed();
            return File.ReadLines(_fileName);
        }

        public IEnumerable<string> ReadLines(Encoding encoding) {
            ThrowIfDisposed();
            return File.ReadLines(_fileName, encoding);
        }

        public Stream OpenWrite() {
            ThrowIfDisposed();
            return File.OpenWrite(_fileName);
        }

        public StreamReader OpenText() {
            ThrowIfDisposed();
            return File.OpenText(_fileName);
        }

        public StreamReader OpenText(Encoding encoding) {
            ThrowIfDisposed();
            return StreamContext.FromFile(_fileName).OpenText(encoding);
        }

        public StreamWriter AppendText() {
            ThrowIfDisposed();
            return File.AppendText(_fileName);
        }

        public StreamWriter AppendText(Encoding encoding) {
            ThrowIfDisposed();
            return StreamContext.FromFile(_fileName).AppendText(encoding);
        }

        public string ReadAllText() {
            ThrowIfDisposed();
            return File.ReadAllText(_fileName);
        }

        public string ReadAllText(Encoding encoding) {
            ThrowIfDisposed();
            return File.ReadAllText(_fileName, encoding);
        }

        public string[] ReadAllLines() {
            ThrowIfDisposed();
            return File.ReadAllLines(_fileName);
        }

        public string[] ReadAllLines(Encoding encoding) {
            ThrowIfDisposed();
            return File.ReadAllLines(_fileName, encoding);
        }

        public void WriteAllBytes(byte[] value) {
            ThrowIfDisposed();
            File.WriteAllBytes(_fileName, value);
        }

        public void WriteAllLines(IEnumerable<string> lines) {
            ThrowIfDisposed();
            File.WriteAllLines(_fileName, lines);
        }

        public void WriteAllLines(IEnumerable<string> lines, Encoding encoding) {
            ThrowIfDisposed();
            File.WriteAllLines(_fileName, lines, encoding);
        }
    }
}
