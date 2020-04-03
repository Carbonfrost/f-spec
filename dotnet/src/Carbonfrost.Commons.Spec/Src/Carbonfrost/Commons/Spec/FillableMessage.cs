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
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Carbonfrost.Commons.Spec {

    class FillableMessage {

        private static readonly Regex PLACEHOLDERS = new Regex(@"\{(?<placeholder>[^}]+)\}");

        public IReadOnlyCollection<string> Keys {
            get;
            private set;
        }

        public string Result {
            get;
            private set;
        }

        public override string ToString() {
            return Result;
        }

        private FillableMessage(string result, IReadOnlyCollection<string> keys) {
            Result = result;
            Keys = keys;
        }

        public static FillableMessage Fill(string msg, IDictionary<string, string> data) {
            // Fill any placeholders in the message
            List<string> keys = new List<string>();
            MatchEvaluator replThunk = m => {
                string[] nameAndFormat = m.Groups["placeholder"].Value.Split(':');
                string name = nameAndFormat[0];
                string format = nameAndFormat.ElementAtOrDefault(1);
                string value;
                keys.Add(name);

                if (data.TryGetValue(name, out value)) {
                    data.Remove(name);

                    switch (format) {
                        case "B": // For BoundsExclusive
                            return bool.Parse(value) ? nameAndFormat[2] : "";
                    }
                    return value;
                }
                return "{" + name + "}";
            };

            return new FillableMessage(
                PLACEHOLDERS.Replace(msg, replThunk),
                keys
            );
        }

    }
}
