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
        [InlineData("2d 12h 15m 3.5s", 2 * 86400 + 12 * 3600 + 15 * 60 + 3.5, Name = "Composite")]
        public void TryParse_should_handle_unit_converions(string text, double seconds) {
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
    }
}
#endif
