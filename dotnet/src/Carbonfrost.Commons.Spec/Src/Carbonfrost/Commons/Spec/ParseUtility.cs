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

namespace Carbonfrost.Commons.Spec {

    static class ParseUtility {

        internal static bool TryParseTimeSpan(string text, out TimeSpan result) {
            if (string.IsNullOrWhiteSpace(text)) {
                result = TimeSpan.Zero;
                return true;
            }

            text = text.Trim();

            if (text == "max" || text == "Infinite") {
                result = TimeSpan.MaxValue;
                return true;
            }
            if (text == "min") {
                result = TimeSpan.MinValue;
                return true;
            }

            if (ConvertUnits(text, TS_UNITS, TS_CONVERSIONS, out result)) {
                return true;
            }

            return TimeSpan.TryParse(text, out result);
        }

        static readonly string[] TS_UNITS = {
            "d",
            "h",
            "ms",
            "min",
            "s",
        };

        static readonly Func<Double, TimeSpan>[] TS_CONVERSIONS = {
            TimeSpan.FromDays,
            TimeSpan.FromHours,
            TimeSpan.FromMilliseconds,
            TimeSpan.FromMinutes,
            TimeSpan.FromSeconds,
        };

        static bool ConvertUnits<T>(string text,
                                    string[] units,
                                    Func<Double, T>[] conversions,
                                    out T value) {
            for (int i = 0; i < units.Length; i++) {
                string unit = units[i];
                if (text.EndsWith(unit, StringComparison.OrdinalIgnoreCase)) {
                    double am = double.Parse(text.Substring(0, text.Length - unit.Length));
                    value = conversions[i](am);
                    return true;
                }
            }
            value = default(T);
            return false;
        }
    }
}
