//
// Copyright 2016 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Text;

namespace Carbonfrost.Commons.Spec.ExecutionModel.Output {

    class BclConsole : ConsoleWrapperBase {

        private bool? _prettyFormat;
        private bool? _unicode;
        private bool _needIndent = true;

        public bool PrettyFormat {
            get {
                if (_prettyFormat.HasValue) {
                    return _prettyFormat.Value;
                }
                else {
                    bool result = false;
                    try {
                        result = Console.WindowWidth > 0;
                    }
                    catch (IOException) {
                    }
                    _prettyFormat = result;
                    return result;
                }
            }
        }

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

        private int Position {
            get {
                return Console.CursorLeft;
            }
        }

        public override void Write(string text) {
            if (!PrettyFormat) {
                Console.Write(text);
                return;
            }
            if (text == null) {
                return;
            }

            text = text.Replace("\r\n", "\n");
            int index = 0;
            while (index < text.Length) {
                WriteIndent();
                int availableWidth = Console.WindowWidth - Position;
                int cr = text.IndexOf('\n', index) - index;
                if (cr >= 0 && cr < availableWidth) {
                    availableWidth = cr + 1;
                }
                string sub;
                if (text.Length >= (index + availableWidth)) {
                    sub = text.Substring(index, availableWidth);
                    _needIndent = true;
                    Console.Write(sub);
                    // If we wrap after writing, then the calculation of WindowWidth worked
                    // otherwise, wrap to the next line ourselves
                    if (Position != 0) {
                        Console.WriteLine();
                    }

                } else {
                    sub = text.Substring(index);
                    Console.Write(sub);
                }
                index += availableWidth;
            }
        }

        public override void WriteLine() {
            Console.WriteLine();
            _needIndent = true;
        }

        private void WriteIndent() {
            if (_needIndent) {
                _needIndent = false;
                Console.Write(IndentString);
            }
        }

        public override void SetForeground(ConsoleColor color) {
            Console.ForegroundColor = color;
        }

        public override void ResetColor() {
            Console.ResetColor();
        }

    }
}
