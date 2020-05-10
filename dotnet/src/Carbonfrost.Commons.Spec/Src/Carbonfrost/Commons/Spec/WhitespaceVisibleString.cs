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

namespace Carbonfrost.Commons.Spec {

    struct WhitespaceVisibleString {

        internal const string LF = "↓";

        private readonly string _text;

        public WhitespaceVisibleString(string text) {
            _text = text;
        }

        public override string ToString() {
            // TODO These aren't universal -- won't work with some Windows fonts
            // bullet might work better in some fonts • U+2022
            if (_text == null) {
                return null;
            }
            return _text.Replace(" ", "⋅") //  U+22C5
                .Replace("\t", "   →") // U+2192
                .Replace("\r\n", "↵")
                .Replace("\r", "←")
                .Replace("\n", LF)
                .Replace("↵", "↵\n")
                .Replace(LF, LF + "\n")
                .Replace("←", "←\n");
        }
    }
}
