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
using System.Collections.Generic;

namespace Carbonfrost.Commons.Spec.MyersDiff {

    class StringLinesSequence : ISequence {

        private readonly string[] _lines;

        public string[] Lines {
            get {
                return _lines;
            }
        }

        public StringLinesSequence(string content) {
            _lines = content.Split(new[] {
                                       "\r\n",
                                       "\n"
                                   }, StringSplitOptions.None);
        }

        public IEnumerable<string> EnumerateLines(int begin, int end) {
            for (int i = begin; i < end && i < Lines.Length; i++) {
                if (i >= 0) {
                    yield return Lines[i];
                }
            }
        }

        public int Size() {
            return _lines.Length;
        }

        public bool Equals(int i, ISequence other, int j) {
            var seq = other as StringLinesSequence;
            if (seq == null) {
                return false;
            }
            return _lines[i] == seq._lines[j];
        }
    }
}
