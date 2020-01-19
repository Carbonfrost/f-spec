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
using System.IO;
using System.Linq;

using Carbonfrost.Commons.Spec.ExecutionModel;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.Commons.Spec {

    interface ITestLoader {
        TestFile LoadFile(string fileName);
        TestFixture LoadFixture(string fileName);
        TestFixtureData LoadFixtureData(string fileName);

        TestFile DownloadFile(Uri url);
        TestFixture DownloadFixture(Uri url);
        TestFixtureData DownloadFixtureData(Uri url);
        TextReader DownloadText(Uri url);
        Stream Download(Uri url);
    }

    class TestLoader : ITestLoader {

        private readonly TestRunnerOptions _opts;
        private readonly TestContext _context;

        public TestRunnerOptions TestRunnerOptions {
            get {
                return _opts;
            }
        }

        internal TestLoader(TestRunnerOptions opts,
                            TestContext parent) {
            _opts = opts;
            _context = parent;
        }

        public StreamContext Open(string fileName) {
            string actualPath = FindActualPath(fileName);
            return StreamContext.FromFile(actualPath);
        }

        public string GetFixtureFullPath(string fileName) {
            return FindActualPath(fileName);
        }

        public TestFixture LoadFixture(string fileName) {
            string actualPath = FindActualPath(fileName);
            return TestFixture.FromFile(actualPath);
        }

        public TestFixture DownloadFixture(Uri url) {
            return ReadUrlOrLocalFile(url,
                                      u => TestFixture.FromSource(u),
                                      u => LoadFixture(u));
        }

        public TestFixtureData LoadFixtureData(string fileName) {
            string actualPath = FindActualPath(fileName);
            return TestFixtureData.FromFile(actualPath);
        }

        public TestFixtureData DownloadFixtureData(Uri url) {
            return ReadUrlOrLocalFile(url,
                                      u => TestFixtureData.FromSource(u),
                                      u => LoadFixtureData(u));
        }

        public StreamReader OpenText(string fileName) {
            string actualPath = FindActualPath(fileName);
            return StreamContext.FromFile(actualPath).OpenText();
        }

        public TestFile LoadFile(string fileName) {
            return Add(new TestFile(FindActualPath(fileName)));
        }

        public TextReader DownloadText(Uri url) {
            return ReadUrlOrLocalFile(url,
                                      u => StreamContext.FromSource(u).OpenText(),
                                      u => OpenText(u));
        }

        public TestFile DownloadFile(Uri url) {
            return Add(ReadUrlOrLocalFile(url,
                                      u => DownloadCache(url),
                                      u => LoadFile(u)));
        }

        public Stream Download(Uri url) {
            return ReadUrlOrLocalFile(url,
                                      u => StreamContext.FromSource(u).OpenRead(),
                                      u => OpenRead(u));
        }

        private Stream OpenRead(string u) {
            return File.OpenRead(FindActualPath(u));
        }

        private TestFile DownloadCache(Uri url) {
            if (url.IsAbsoluteUri && url.IsFile) {
                return new TestFile(url.LocalPath);
            }

            var tempFile = _createTemp(url.ToString());
            tempFile.WriteAllBytes(StreamContext.FromSource(url).ReadAllBytes());
            return new TestFile(tempFile.FileName);
        }

        static T ReadUrlOrLocalFile<T>(Uri url, Func<Uri, T> urlReader, Func<string, T> fileReader) {
            if (url == null) {
                throw new ArgumentNullException("url");
            }
            if (url.IsAbsoluteUri) {
                return urlReader(url);
            }
            return fileReader(url.ToString());
        }

        private TestFile Add(TestFile f) {
            return _context.RegisterDisposable(f);
        }

        private TestTemporaryFile _createTemp(string n) {
            return _context.CreateTempFile(n);
        }

        string FindActualPath(string fileName) {
            if (Path.IsPathRooted(fileName)) {
                return fileName;
            }
            if (File.Exists(fileName)) {
                return Path.GetFullPath(fileName);
            }
            foreach (var fd in TestRunnerOptions.FixtureDirectories) {
                string actualPath = Path.Combine(fd, fileName);
                if (File.Exists(actualPath)) {
                    return Path.GetFullPath(actualPath);
                }
            }

            throw SpecFailure.CannotFindFixture(fileName, TestRunnerOptions.FixtureDirectories);
        }
    }
}
