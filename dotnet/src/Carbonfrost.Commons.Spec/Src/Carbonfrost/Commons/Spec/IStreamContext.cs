//
// Copyright 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

    public interface IStreamContext {
        string Extension {
            get;
        }
        Uri Uri { get; }

        StreamWriter AppendText();
        StreamWriter AppendText(Encoding encoding);
        StreamWriter CreateText();
        StreamWriter CreateText(Encoding encoding);
        StreamReader OpenText();
        StreamReader OpenText(Encoding encoding);
        Stream Open();
        Stream OpenRead();
        Stream OpenWrite();
        byte[] ReadAllBytes();
        string[] ReadAllLines();
        IEnumerable<string> ReadLines();
        IEnumerable<string> ReadLines(Encoding encoding);
        string[] ReadAllLines(Encoding encoding);
        string ReadAllText(Encoding encoding);
        string ReadAllText();
        void WriteAllBytes(byte[] value);
        void WriteAllLines(IEnumerable<string> lines);
        void WriteAllLines(IEnumerable<string> lines, Encoding encoding);
        void WriteAllText(string text, Encoding encoding);
        void WriteAllText(string text);
    }
}
