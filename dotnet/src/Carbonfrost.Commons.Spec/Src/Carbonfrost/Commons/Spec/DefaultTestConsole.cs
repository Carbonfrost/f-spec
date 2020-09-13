//
// Copyright 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
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
using System.IO;
using System.Text;

namespace Carbonfrost.Commons.Spec {

    class DefaultTestConsole : TestConsole {

        public override TextWriter Out {
            get;
        }

        public override Encoding OutputEncoding {
            get;
            set;
        }

        public override TextWriter Error {
            get {
                throw new NotImplementedException();
            }
        }

        public override TextReader In {
            get {
                throw new NotImplementedException();
            }
        }

        public override Encoding InputEncoding {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }

        public override bool IsErrorRedirected {
            get {
                throw new NotImplementedException();
            }
        }

        public override bool IsInputRedirected {
            get {
                throw new NotImplementedException();
            }
        }

        public override bool IsOutputRedirected {
            get {
                throw new NotImplementedException();
            }
        }

        public override Stream OpenStandardError(int bufferSize) {
            throw new InvalidOperationException();
        }

        public override Stream OpenStandardError() {
            throw new InvalidOperationException();
        }

        public override Stream OpenStandardInput(int bufferSize) {
            throw new InvalidOperationException();
        }

        public override Stream OpenStandardInput() {
            throw new InvalidOperationException();
        }

        public override Stream OpenStandardOutput(int bufferSize) {
            throw new InvalidOperationException();
        }

        public override Stream OpenStandardOutput() {
            throw new InvalidOperationException();
        }

        public override int Read() {
            throw new InvalidOperationException();
        }

        public override string ReadLine() {
            throw new InvalidOperationException();
        }

        public override void SetError(TextWriter newError) {
            throw new InvalidOperationException();
        }

        public override void SetIn(TextReader newIn) {
            throw new InvalidOperationException();
        }

        public override void SetOut(TextWriter newOut) {
            throw new InvalidOperationException();
        }
    }
}
