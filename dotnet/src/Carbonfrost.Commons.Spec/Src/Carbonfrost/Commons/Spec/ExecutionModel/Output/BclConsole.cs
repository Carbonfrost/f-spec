//
// Copyright 2016, 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Security;
using System.Text;

namespace Carbonfrost.Commons.Spec.ExecutionModel.Output {

    class BclConsole : ConsoleWrapperBase {

        private bool? _unicode;
        private bool _needIndent = true;

        public override bool IsUnicodeEncoding {
            get {
                if (_unicode.HasValue) {
                    return _unicode.Value;
                }
                else {
                    try {
                        var enc = Console.Out.Encoding;

                        if (enc == null || enc.IsSingleByte) {
                            Console.OutputEncoding = enc = Encoding.UTF8;
                        }

                        return (_unicode = !enc.IsSingleByte).Value;
                    } catch (SecurityException) {
                    } catch (IOException) {
                    }

                    _unicode = false;
                    return false;
                }
            }
        }

        public override void Write(string text) {
            WriteIndent();
            Console.Write(text);
        }

        public override void WriteLine() {
            Console.WriteLine();
            _needIndent = true;
        }

        public override void SetForeground(ConsoleColor color) {
            Console.ForegroundColor = color;
        }

        public override void ResetColor() {
            Console.ResetColor();
        }

        private void WriteIndent() {
            if (_needIndent) {
                Console.Write(IndentString);
                _needIndent = false;
            }
        }

    }
}
