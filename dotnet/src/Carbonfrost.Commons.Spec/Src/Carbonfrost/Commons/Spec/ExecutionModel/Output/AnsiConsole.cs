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

namespace Carbonfrost.Commons.Spec.ExecutionModel.Output {

    class AnsiConsole : BclConsole {

        protected override string IndentString {
            get {
                // Move forward columns (this prevents the indent from being written with underlined spaces)
                return string.Concat("\x1B[", 2 * Indent, "C");
            }
        }

        public override void Bold() {
            WriteCode("1");
        }

        public override void Underline() {
            WriteCode("4");
        }

        public override void ResetStyles() {
            WriteCode("0");
        }

        static void WriteCode(string cs) {
            Console.Out.Write((char) 0x1B);
            Console.Out.Write((char) '[');
            foreach (var c in cs) {
                Console.Out.Write(c);
            }
            Console.Out.Write((char) 'm');
        }
    }
}
