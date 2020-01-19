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
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Carbonfrost.Commons.Spec {

    public class TestTemporaryDirectory : DisposableObject, ITestDirectory<TestTemporaryFile> {

        private readonly string _basePath;
        private static readonly Regex replaceChars = new Regex(@"[^a-z0-9_]+", RegexOptions.IgnoreCase);
        private readonly IList<TestTemporaryFile> _files = new List<TestTemporaryFile>();

        internal TestTemporaryDirectory(string parentSession, string nameHint) {
            nameHint = Utility.PathSafe(nameHint);
            _basePath = Path.Combine(Path.GetTempPath(), "spec", string.Concat(parentSession, "-", nameHint, RandomName()));
            Directory.CreateDirectory(_basePath);
        }

        public void AppendAllText(string fileName, string text) {
            CreateFile(fileName).AppendAllText(text);
        }

        public void AppendAllText(string fileName, string contents, Encoding encoding) {
            CreateFile(fileName).AppendAllText(contents, encoding);
        }

        public TestTemporaryFile OpenFile(string fileName) {
            return AddFile(new TestTemporaryFile(CheckFileName(fileName)));
        }

        public TestTemporaryFile CreateFile() {
            return CreateFile(RandomName());
        }

        public TestTemporaryFile CreateFile(string fileName) {
            return AddFile(new TestTemporaryFile(CheckFileName(fileName)));
        }

        public Stream OpenRead(string fileName) {
            return OpenFile(fileName).OpenRead();
        }

        public void WriteAllText(string fileName, string contents) {
            CreateFile(fileName).WriteAllText(contents);
        }

        public void WriteAllText(string fileName, string contents, Encoding encoding) {
            CreateFile(fileName).WriteAllText(contents, encoding);
        }

        public byte[] ReadAllBytes(string fileName) {
            return OpenFile(fileName).ReadAllBytes();
        }

        public IEnumerable<string> ReadLines(string fileName) {
            return OpenFile(fileName).ReadLines();
        }

        public IEnumerable<string> ReadLines(string fileName, Encoding encoding) {
            return OpenFile(fileName).ReadLines(encoding);
        }

        public Stream OpenWrite(string fileName) {
            return CreateFile(fileName).OpenWrite();
        }

        public StreamReader OpenText(string fileName) {
            return OpenFile(fileName).OpenText();
        }

        public StreamReader OpenText(string fileName, Encoding encoding) {
            return OpenFile(fileName).OpenText(encoding);
        }

        public StreamWriter AppendText(string fileName) {
            return CreateFile(fileName).AppendText();
        }

        public StreamWriter AppendText(string fileName, Encoding encoding) {
            return StreamContext.FromFile(CheckFileName(fileName)).AppendText(encoding);
        }

        public string ReadAllText(string fileName) {
            return OpenFile(fileName).ReadAllText();
        }

        public string ReadAllText(string fileName, Encoding encoding) {
            return OpenFile(fileName).ReadAllText(encoding);
        }

        public string[] ReadAllLines(string fileName) {
            return CreateFile(fileName).ReadAllLines();
        }

        public string[] ReadAllLines(string fileName, Encoding encoding) {
            return CreateFile(fileName).ReadAllLines(encoding);
        }

        public void WriteAllBytes(string fileName, byte[] value) {
            CreateFile(fileName).WriteAllBytes(value);
        }

        public void WriteAllLines(string fileName, IEnumerable<string> lines) {
            CreateFile(fileName).WriteAllLines(lines);
        }

        public void WriteAllLines(string fileName, IEnumerable<string> lines, Encoding encoding) {
            CreateFile(fileName).WriteAllLines(lines, encoding);
        }

        public string GetFullPath(string fileName) {
            if (fileName == null) {
                throw new ArgumentNullException("fileName");
            }
            if (string.IsNullOrEmpty(fileName)) {
                throw SpecFailure.EmptyString("fileName");
            }
            return CheckFileName(fileName);
        }

        protected override void Dispose(bool manualDispose) {
            if (manualDispose) {
                foreach (var f in _files) {
                    Safely.Dispose(f);
                }
            }
            base.Dispose(manualDispose);
        }

        private string CheckFileName(string fileName) {
            if (string.IsNullOrEmpty(fileName)) {
                fileName = RandomName();
            }

            if (Path.IsPathRooted(fileName) || fileName.StartsWith("../", StringComparison.Ordinal)) {
                throw SpecFailure.TemporaryDirectoryFileNameRooted("fileName");
            }
            return Path.Combine(_basePath, fileName);
        }

        private static string RandomName() {
            byte[] array = new byte[8];
            RandomNumberGenerator.Create().GetBytes(array);
            return replaceChars.Replace(Convert.ToBase64String(array), "_").Trim('_');
        }

        private TestTemporaryFile AddFile(TestTemporaryFile testTemporaryFile) {
            _files.Add(testTemporaryFile);
            return testTemporaryFile;
        }
    }
}
