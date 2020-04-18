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

namespace Carbonfrost.Commons.Spec {

    struct Time {

        private static readonly Time Zero = new Time(TimeSpan.Zero);
        private static readonly Time MinValue = new Time(TimeSpan.MinValue);
        private static readonly Time MaxValue = new Time(TimeSpan.MaxValue);

        public readonly TimeSpan Value;

        public Time(TimeSpan value) {
            Value = value;
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

            return ParseExprs(text.Split(new [] { ' ' }, StringSplitOptions.RemoveEmptyEntries), out result);
        }

        private static bool ParseExprs(string[] exprs, out Time result) {
            TimeSpan total;
            foreach (var term in exprs) {
                if (ParseUtility.ConvertUnits(term, TS_UNITS, TS_CONVERSIONS, out TimeSpan timeSpan)) {
                    total += timeSpan;
                } else {
                    return false;
                }
            }

            result = new Time(total);
            return true;
        }

        static readonly string[] TS_UNITS = {
            "d",
            "h",
            "ms",
            "min",
            "m",
            "s",
        };

        static readonly Func<Double, TimeSpan>[] TS_CONVERSIONS = {
            TimeSpan.FromDays,
            TimeSpan.FromHours,
            TimeSpan.FromMilliseconds,
            TimeSpan.FromMinutes,
            TimeSpan.FromMinutes,
            TimeSpan.FromSeconds,
        };
    }
}
