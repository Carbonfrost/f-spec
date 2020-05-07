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
using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.Commons.Spec {

    static class TextUtility {

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

        internal static string FormatLocation(object location) {
            if (location is Uri url) {
                if (url.IsAbsoluteUri) {
                    if (url.Scheme == "data") {
                        return string.Format(
                            "<(data){0}>",
                            TextUtility.Escape(TextUtility.Truncate(Uri.UnescapeDataString(url.PathAndQuery)))
                        );
                    }
                    if (url.Scheme == "file") {
                        return url.LocalPath;
                    }
                }
                return url.ToString();
            }

            return location.ToString();
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

        internal static string ConvertToSimpleTypeName(Type type, bool qualified = false) {
            string prefix = null;
            if (qualified) {
                prefix = type.DeclaringType != null
                    ? ConvertToSimpleTypeName(type.DeclaringType, true) + "+"
                    : type.Namespace + ".";
            }
            if (!type.GetTypeInfo().IsGenericType) {
                return prefix + type.Name;
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
            return string.Format("{0}{1}<{2}>", prefix, RemoveCompilerMangle(withoutTicks), String.Join(", ", simpleNames));
        }

        private static string RemoveCompilerMangle(string typeName) {
            return Regex.Replace(typeName, @"(<.+?>)(d__\d+)", "$1");
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
            if (value is Exception exceptionValue) {
                return GetExceptionFiltered(exceptionValue);
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

        internal static string GetExceptionFiltered(Exception exception) {
            return ExceptionStackTraceFilter.Apply(exception).ToString();
        }

        internal static string ShowWhitespace(string text) {
            return new WhitespaceVisibleString(text).ToString();
        }

        internal static string Escape(string text) {
            return text
                .Replace("\t", "\\t")
                .Replace("\r\n", "\\r\\n")
                .Replace("\r", "\\r")
                .Replace("\n", "\\n");
        }
    }
}
