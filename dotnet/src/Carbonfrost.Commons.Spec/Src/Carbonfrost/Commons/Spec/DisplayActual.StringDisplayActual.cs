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

using System.Linq;

namespace Carbonfrost.Commons.Spec {

    partial class DisplayActual {

        sealed class StringDisplayActual : IDisplayActual {

            private readonly string _text;
            private readonly bool _escape;

            public StringDisplayActual(string s, bool escape = false) {
                _text = s;
                _escape = escape;
            }

            public string Format(DisplayActualOptions options) {
                var w = _text;
                if (_escape) {
                    w = string.Format("\"{0}\"", TextUtility.Escape(w));
                }

                if (options.ShowWhitespace()) {
                    w = TextUtility.ShowWhitespace(w);
                }
                if (_text.Length == 0) {
                    w = "<empty>";
                }

                if (options.ShowType()) {
                    return w + " (string)";
                }
                return w;
            }
        }
    }

}
