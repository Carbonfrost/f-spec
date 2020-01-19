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
using System.Text;

namespace Carbonfrost.Commons.Spec {

    interface ITestDirectory<T> : IReadOnlyTestDirectory<T>, ITestDirectory {
        T CreateFile(string fileName);
    }

    interface ITestDirectory : IReadOnlyTestDirectory {
        Stream OpenWrite(string fileName);
        StreamWriter AppendText(string fileName);
        StreamWriter AppendText(string fileName, Encoding encoding);
        void AppendAllText(string fileName, string contents, Encoding encoding);
        void AppendAllText(string fileName, string text);
        void WriteAllBytes(string fileName, byte[] value);
        void WriteAllLines(string fileName, IEnumerable<string> lines);
        void WriteAllLines(string fileName, IEnumerable<string> lines, Encoding encoding);
        void WriteAllText(string fileName, string contents);
        void WriteAllText(string fileName, string contents, Encoding encoding);
    }
}
