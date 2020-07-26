#if SELF_TEST

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
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.SelfTest.Spec {

    public class TimeTests {

        [Fact]
        public void TryParse_should_handle_seconds_by_default() {
            Assert.True(Time.TryParse("4", out Time actual));
            Assert.Equal(TimeSpan.FromSeconds(4), actual.Value);
        }

        [Fact]
        public void TryParse_should_handle_empty_string_as_zero() {
            Assert.True(Time.TryParse("", out Time actual));
            Assert.Equal(TimeSpan.Zero, actual.Value);
        }

        [Theory]
        [InlineData("2s", 2)]
        [InlineData("2min", 2 * 60)]
        [InlineData("2m", 2 * 60)]
        [InlineData("2h", 2 * 60 * 60)]
        [InlineData("2d", 2 * 86400)]
        [InlineData("2ms", 2.0 / 1000)]
        [InlineData("2us", 2.0 / 1000 / 1000)]
        [InlineData("2μs", 2.0 / 1000 / 1000, Name = "unicode")]
        [InlineData("2d 12h 15m 3.5s", 2 * 86400 + 12 * 3600 + 15 * 60 + 3.5, Name = "Composite")]
        public void TryParse_should_handle_unit_conversions(string text, double seconds) {
            Assert.True(Time.TryParse(text, out Time actual));
            Assert.Equal(TimeSpan.FromSeconds(seconds), actual.Value);
        }

        [Fact]
        public void TryParse_should_parse_like_TimeSpan() {
            string text = "4.19:59:59.000004";
            Assert.True(Time.TryParse(text, out Time actual));
            Assert.Equal(TimeSpan.Parse(text), actual.Value);
        }

        [Fact]
        public void TryParse_should_parse_min_and_max_value() {
            Assert.True(Time.TryParse("min", out Time actual));
            Assert.Equal(TimeSpan.MinValue, actual.Value);

            Assert.True(Time.TryParse("max", out actual));
            Assert.Equal(TimeSpan.MaxValue, actual.Value);

            Assert.True(Time.TryParse("Infinite", out actual));
            Assert.Equal(TimeSpan.MaxValue, actual.Value);
        }

        [Theory]
        [InlineData("0 hr 0 min 2 sec", 2)]
        [InlineData("0 hr 2 min 0 sec", 2 * 60)]
        [InlineData("1 hr 0 min 0 sec", 1 * 60 * 60)]
        [InlineData("2 hr 0 min 0 sec", 2 * 60 * 60)]
        [InlineData("2 d 0 hr 0 min 0 sec", 2 * 86400)]
        [InlineData("0 hr 0 min 0.002 sec", 2.0 / 1000)]
        [InlineData("2 d 12 hr 15 min 3.5 sec", 2 * 86400 + 12 * 3600 + 15 * 60 + 3.5, Name = "Composite")]
        public void ToString_format_general_should_use_unit_syntax(string expected, double seconds) {
            var time = (Time) TimeSpan.FromSeconds(seconds);
            Assert.Equal(expected, time.ToString("G"));
        }

        [Theory]
        [InlineData("2 hr 0 min 0 sec", "g")]
        [InlineData("2 hours", "n")]
        [InlineData("02:00:00", "s")]
        public void ToString_format_should_use_correct_syntax(string expected, string format) {
            var time = (Time) TimeSpan.FromHours(2);
            Assert.Equal(expected, time.ToString(format));
        }

        [Theory]
        [InlineData("2 hours 1 min", "2h 1min 25s 30ms")]
        [InlineData("520 μs", "520μs")]
        [InlineData("0.6 ms", "0.6ms", Name = "threshold for ms")]
        [InlineData("500 ms", "500ms")]
        [InlineData("0.60 sec", "600ms", Name = "threshold for seconds")]
        [InlineData("20.3 sec", "20.25sec")]
        [InlineData("2 min", "2min", Name = "threshold for minutes")]
        [InlineData("2 hours 2 min", "2hr 2min")]
        [InlineData("2 days 2 hours", "2day 2hr 2min")]
        [InlineData("2 days", "2day 2min", Name = "truncate minutes")]
        [InlineData("3 days", "2day 23hr 35min", Name = "rounded days")]
        [InlineData("1 day", "0 days 23 hr 59 min 30 sec", Name = "rounded days 2")]
        [InlineData("2 days 3 hours", "2day 2hr 35min", Name = "rounded hours")]
        [InlineData("3 hours", "2hr 59min 35sec", Name = "rounded hours 2")]
        [InlineData("2 days", "2day 2min", Name = "rounded minutes")]
        public void ToString_format_natural_should_truncate_to_readable(string expected, string exact) {
            var time = Time.Parse(exact);
            Assert.Equal(expected, time.ToString("N"));
        }
    }
}
#endif
