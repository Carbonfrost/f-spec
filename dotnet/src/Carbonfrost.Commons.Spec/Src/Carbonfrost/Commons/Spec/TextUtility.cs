//
// Copyright 2013 Outercurve Foundation
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Carbonfrost.Commons.Spec {

    static class TextUtility {

        private static readonly Regex PLACEHOLDERS = new Regex(@"\{(?<placeholder>[^}]+)\}");

        public static string Truncate(string str) {
            if (str == null) {
                return null;
            }
            const int LEN = 400;
            const int A = (int) (LEN * 0.20);
            if (str.Length > LEN) {
                return str.Substring(0, LEN - A) + "..." + str.Substring(str.Length - A);
            }
            return str;
        }

        public static string Truncate(IList<int> list) {
            string rest = null;
            IEnumerable<int> myList = list;
            if (list.Count > 4) {
                rest = ", ...";
                myList = list.Take(4);
            }
            return string.Join(", ", myList) + rest;
        }

        internal static string FormatDuration(TimeSpan duration) {
            if (duration.TotalMilliseconds < 600) {
                return string.Format("{0:0.#} ms", duration.TotalMilliseconds);
            }
            if (duration.TotalSeconds < 120) {
                return string.Format("{0:0.000} seconds", duration.TotalSeconds);
            }
            return duration.ToString();
        }

        public static string FormatArgs(params object[] data) {
            return FormatArgs((IEnumerable<object>) data);
        }

        public static string FormatArgs(IEnumerable<object> data) {
            var sb = new StringBuilder();
            bool needComma = false;
            foreach (var s in data) {
                if (needComma) {
                    sb.Append(", ");
                }
                sb.Append(TextUtility.Truncate(TextUtility.ConvertToString(s)));

                needComma = true;
            }
            return sb.ToString();
        }

        public static string Fill(string msg, IDictionary<string, string> data) {
            // Fill any placeholders in the message
            MatchEvaluator replThunk = m => {
                string[] nameAndFormat = m.Groups["placeholder"].Value.Split(':');
                string name = nameAndFormat[0];
                string format = nameAndFormat.ElementAtOrDefault(1);
                string value;

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

            return PLACEHOLDERS.Replace(msg, replThunk);
        }

        public static StringComparison ToComparison(StringComparer comparer) {
            if (comparer == StringComparer.OrdinalIgnoreCase) {
                return StringComparison.OrdinalIgnoreCase;
            }
            if (comparer == StringComparer.Ordinal) {
                return StringComparison.Ordinal;
            }

            if (comparer == StringComparer.InvariantCulture) {
                return StringComparison.InvariantCulture;
            }
            if (comparer == StringComparer.InvariantCultureIgnoreCase) {
                return StringComparison.InvariantCultureIgnoreCase;
            }
            if (comparer == StringComparer.CurrentCulture) {
                return StringComparison.CurrentCulture;
            }
            if (comparer == StringComparer.CurrentCultureIgnoreCase) {
                return StringComparison.CurrentCultureIgnoreCase;
            }
            return StringComparison.Ordinal;
        }

        private static string ConvertToSimpleTypeName(Type type) {
            if (!type.GetTypeInfo().IsGenericType) {
                return type.Name;
            }

            Type[] genericTypes = type.GetTypeInfo().GetGenericArguments();
            string[] simpleNames = new string[genericTypes.Length];
            for (int idx = 0; idx < genericTypes.Length; idx++) {
                simpleNames[idx] = ConvertToSimpleTypeName(genericTypes[idx]);
            }

            string baseTypeName = type.Name;
            int backTickIdx = type.Name.IndexOf('`');
            if (backTickIdx < 0) {
                backTickIdx = type.Name.Length;
            }

            // F# doesn't use backticks for generic type names
            var withoutTicks = baseTypeName.Substring(0, backTickIdx);
            return string.Format("{0}<{1}>", withoutTicks, String.Join(", ", simpleNames));
        }

        internal static string ConvertToString(object value, int depth = 0) {
            if (value == null) {
                return "<null>";
            }
            string stringValue = value as string;
            if (stringValue != null) {
                if (stringValue.Length == 0) {
                    return "<empty>";
                }
                return stringValue;
            }

            IEnumerable enumerableValue = value as IEnumerable;

            if (enumerableValue == null) {
                return value.ToString();
            }

            List<string> valueStrings = new List<string>();
            foreach (object valueObject in enumerableValue) {
                string displayName;
                if (valueObject == null) {
                    displayName = "<null>";
                } else {
                    string stringValueObject = valueObject as string;
                    if (stringValueObject != null) {
                        displayName = string.Format("\"{0}\"", stringValueObject);
                    } else if (depth == 3) {
                        displayName = valueObject.ToString();
                    } else {
                        displayName = ConvertToString(valueObject, depth + 1);
                    }
                }
                valueStrings.Add(displayName);
            }

            return string.Format("{0} {{ {1} }}",
                                 ConvertToSimpleTypeName(value.GetType()),
                                 string.Join(", ", valueStrings.ToArray()));
        }

        internal static string ShowWhitespace(string text) {
            // TODO These aren't universal -- won't work with some Windows fonts
            // bullet might work better in some fonts • U+2022
            if (text == null) {
                return null;
            }
            return text.Replace(" ", "⋅") //  U+22C5
                .Replace("\t", "→"); // U+2192
        }
    }
}
