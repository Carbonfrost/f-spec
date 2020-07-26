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
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace Carbonfrost.Commons.Spec {

    struct Time : IFormattable {

        private static readonly Regex UNIT_PATTERN = new Regex(@"((?:\d*\.)?\d+)\s*(\p{L}+)");

        static readonly IDictionary<string, string> TS_ALIASES = new Dictionary<string, string> {
            ["day"] = "d",
            ["days"] = "d",
            ["hour"] = "h",
            ["hours"] = "h",
            ["hr"] = "h",
            ["sec"] = "s",
            ["seconds"] = "s",
            ["second"] = "s",
            ["us"] = "μs",
        };

        static readonly string[] TS_UNITS = {
            "d",
            "h",
            "ms",
            "min",
            "m",
            "s",
            "μs",
        };

        static readonly Func<Double, TimeSpan>[] TS_CONVERSIONS = {
            TimeSpan.FromDays,
            TimeSpan.FromHours,
            TimeSpan.FromMilliseconds,
            TimeSpan.FromMinutes,
            TimeSpan.FromMinutes,
            TimeSpan.FromSeconds,
            FromMicroseconds,
        };

        private static readonly TimeSpan TS_30_SEC = TimeSpan.FromSeconds(30);
        private static readonly TimeSpan TS_30_MIN = TimeSpan.FromMinutes(30);
        private static readonly TimeSpan TS_12_HR = TimeSpan.FromHours(12);

        private static readonly Time Zero = new Time(TimeSpan.Zero);
        private static readonly Time MinValue = new Time(TimeSpan.MinValue);
        private static readonly Time MaxValue = new Time(TimeSpan.MaxValue);

        public readonly TimeSpan Value;

        private string GeneralString {
            get {
                string result = "";
                if (Value.Days > 0) {
                    result = $"{Value.Days} d ";
                }
                double sec = Value.Seconds + 0.001 * Value.Milliseconds;

                return $"{result}{Value.Hours} hr {Value.Minutes} min {sec:0.###} sec";
            }
        }

        private TimeSpan TruncatedByOrder {
            get {
                if (Value.Days == 0 && Value.Hours == 0) {
                        return Value;
                }
                if (Value.Days == 0) {
                    return Value + TS_30_SEC;
                }
                return Value + TS_30_MIN;
            }
        }

        private string NaturalString {
            get {
                var duration = Value;
                if (duration.TotalMilliseconds < 0.6) {
                    return string.Format("{0:0.#} μs", duration.TotalMilliseconds * 1000);
                }
                if (duration.TotalMilliseconds < 600) {
                    return string.Format("{0:0.##} ms", duration.TotalMilliseconds);
                }
                if (duration.TotalSeconds < 10) {
                    return string.Format("{0:0.00} sec", duration.TotalSeconds);
                }
                if (duration.TotalSeconds < 120) {
                    return string.Format("{0:0.#} sec", duration.TotalSeconds);
                }

                var t = TruncatedByOrder;
                return string.Join(
                    " ",
                    new [] {
                        Term(t.Days, "day", "days"),
                        Term(t.Hours, "hour", "hours"),
                        Term(t.Minutes, "min", "min"),
                        t.Seconds == 0 ? null : $"{t.Seconds:0.##} sec",
                    }.SkipWhile(s => s is null).TakeWhile(s => s != null).Take(2)
                );
            }
        }

        public Time(TimeSpan value) {
            Value = value;
        }

        public static Time Parse(string text) {
            if (TryParse(text, out var result)) {
                return result;
            }
            throw new FormatException();
        }

        public static bool TryParse(string text, out Time result) {
            if (string.IsNullOrWhiteSpace(text)) {
                result = Time.Zero;
                return true;
            }

            // Unlike TimeSpan.Parse, which returns in days, we return in seconds
            if (double.TryParse(text, out double seconds)) {
                result = new Time(TimeSpan.FromSeconds(seconds));
                return true;
            }

            text = text.Trim();

            if (text == "max" || text == "Infinite") {
                result = Time.MaxValue;
                return true;
            }
            if (text == "min") {
                result = Time.MinValue;
                return true;
            }

            if (TimeSpan.TryParse(text, out TimeSpan timeSpan2)) {
                result = new Time(timeSpan2);
                return true;
            }

            return ParseExprs(UNIT_PATTERN.Matches(text), out result);
        }

        public static implicit operator Time(TimeSpan value) {
            return new Time(value);
        }

        public override string ToString() {
            return ToString(null);
        }

        public string ToString(string format) {
            return ToString(format, null);
        }

        public string ToString(string format, IFormatProvider formatProvider) {
            switch (format) {
                case null:
                case "":
                case "s":
                case "S":
                    return Value.ToString(null, formatProvider);
                case "g":
                case "G":
                    return GeneralString;
                case "n":
                case "N":
                    return NaturalString;
                default:
                    throw new FormatException();
            }
        }

        public override bool Equals(object obj) {
            return obj is Time time && Value.Equals(time.Value);
        }

        public override int GetHashCode() {
            return -1937169414 + Value.GetHashCode();
        }

        private static bool ParseExprs(MatchCollection exprs, out Time result) {
            TimeSpan total;
            foreach (Match term in exprs) {
                if (ConvertUnits(term.Groups[1].Value, term.Groups[2].Value, out TimeSpan timeSpan)) {
                    total += timeSpan;
                } else {
                    return false;
                }
            }

            result = new Time(total);
            return true;
        }

        private static TimeSpan FromMicroseconds(double d) {
            return TimeSpan.FromMilliseconds(d / 1000.0);
        }

        internal static bool ConvertUnits(string valueMatch, string unitMatch,
            out TimeSpan value
        ) {
            var units = TS_UNITS;
            var conversions = TS_CONVERSIONS;

            if (TS_ALIASES.TryGetValue(unitMatch, out var actual)) {
                unitMatch = actual;
            }

            for (int i = 0; i < units.Length; i++) {
                string unit = units[i];

                if (unitMatch.Equals(unit, StringComparison.OrdinalIgnoreCase)) {
                    double am = double.Parse(valueMatch);
                    value = conversions[i](am);
                    return true;
                }
            }
            value = default(TimeSpan);
            return false;
        }

        private static string Term(int value, string sing, string plural) {
            if (value == 0) {
                return null;
            }
            return value + " " + (value == 1 ? sing : plural);
        }
    }
}
