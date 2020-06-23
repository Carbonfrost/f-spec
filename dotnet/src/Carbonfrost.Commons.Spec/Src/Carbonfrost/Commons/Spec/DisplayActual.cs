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

using System;
using System.Collections;
using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.Commons.Spec {

    static partial class DisplayActual {

        public static readonly IDisplayActual Null = new NullImpl();
        public static readonly IDisplayActual EmptyString = new StringDisplayActual("");

        private static string GetExceptionFiltered(Exception exception) {
            return ExceptionStackTraceFilter.Apply(exception).ToString();
        }

        public static IDisplayActual Create(object value, int depth = 0) {
            if (value is null) {
                return Null;
            }
            if (value is IDisplayActual da) {
                return da;
            }

            if (value is string stringValue) {
                if (stringValue.Length == 0) {
                    return EmptyString;
                }
                return new StringDisplayActual(stringValue, depth > 0);
            }
            if (value is Type typeValue) {
                return new BasicDisplayActual(TextUtility.ConvertToSimpleTypeName(typeValue), typeof(Type));
            }
            if (value is Exception exceptionValue) {
                return Exception(exceptionValue);
            }
            if (value is StringComparer) {
                return new BasicDisplayActual(GetStringComparerText(value), value.GetType());
            }

            if (depth < 3 && value is IEnumerable enumerableValue) {
                return new EnumerableDisplayActual(enumerableValue, depth + 1);
            }

            return new BasicDisplayActual(value.ToString(), value.GetType());
        }

        internal static IDisplayActual Exception(Exception exception) {
            if (exception == null) {
                return new BasicDisplayActual("<no exception>", null);
            }
            return new BasicDisplayActual(GetExceptionFiltered(exception), exception.GetType());
        }

        public static bool OnlyTypeDifferences(IDisplayActual a, IDisplayActual b) {
            if (a.GetType() == b.GetType()) {
                return false;
            }
            return a.Format(DisplayActualOptions.None)
                == b.Format(DisplayActualOptions.None);
        }

        static string GetStringComparerText(object comparison) {
            if (comparison == null) {
                return "<null>";
            }

            var str = comparison.ToString();

            if (comparison == StringComparer.OrdinalIgnoreCase) {
                str = "ordinal (ignore case)";

            } else if (comparison == StringComparer.Ordinal) {
                str = "ordinal";
            }

            else if (comparison == StringComparer.InvariantCulture) {
                str = "invariant culture";
            }
            else if (comparison == StringComparer.InvariantCultureIgnoreCase) {
                str = "invariant culture (ignore case)";
            }

            return str;
        }

        class NullImpl : IDisplayActual {

            public string Format(DisplayActualOptions options) {
                return "<null>";
            }
        }
    }
}
