#if SELF_TEST

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
using System.Text;
using System.Linq;
using Carbonfrost.Commons.Spec.ExecutionModel.Output;

namespace Carbonfrost.SelfTest.Spec.ExecutionModel.Output {

    class TestConsole : ConsoleWrapperBase {

        private readonly StringBuilder _text = new StringBuilder();

        public IEnumerable<string> Lines {
            get {
                return ToString().Split('\n');
            }
        }

        public override bool IsUnicodeEncoding {
            get {
                return true;
            }
        }

        public IEnumerable<string> NonBlankLines {
            get {
                return Lines.Except(new [] { "" });
            }
        }

        public override void ResetColor() {
            Write("{ResetColor}");
        }

        public override void SetForeground(ConsoleColor color) {
            Write("{Foreground:" + color + "}");
        }

        public override void Write(string text) {
            _text.Append(text);
        }

        public override void WriteLine() {
            Write("\n");
        }

        public override string ToString() {
            return _text.ToString();
        }
    }
}

#endif
