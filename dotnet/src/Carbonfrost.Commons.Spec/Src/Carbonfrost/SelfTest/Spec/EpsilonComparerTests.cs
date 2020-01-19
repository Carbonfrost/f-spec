#if SELF_TEST

//
// Copyright 2017 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Linq;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.SelfTest.Spec {

    public class EpsilonComparerTests {

        [Theory]
        [InlineData(0, 5.0, 5.4)]
        [InlineData(1, 5.0, 4.0)]
        [InlineData(-1, 4.0, 4.6)]
        public void Compare_should_apply_to_Timespan_using_reflection(int expected, double x, double y) {
            var ep = EpsilonComparer.Create(TimeSpan.FromMilliseconds(500));
            Assert.Equal(expected, ep.Compare(
                TimeSpan.FromSeconds(x), TimeSpan.FromSeconds(y)));
        }

        [Theory]
        [InlineData(0, "5.0", "5.4")]
        [InlineData(1, "5.0", "4.0")]
        [InlineData(-1, "4.0", "4.6")]
        public void Compare_should_apply_to_Decimal_using_reflection(int expected, string x, string y) {
            var ep = EpsilonComparer.Create(0.5m);
            Assert.Equal(expected, ep.Compare(decimal.Parse(x), decimal.Parse(y)));
        }

        [Theory]
        [InlineData(0, 50, 54)]
        [InlineData(1, 50, 40)]
        [InlineData(-1, 40, 46)]
        public void Compare_should_apply_to_DateTime_using_reflection(int expected, double x, double y) {
            var ep = EpsilonComparer.Create<DateTime, TimeSpan>(TimeSpan.FromSeconds(5));
            Assert.Equal(expected, ep.Compare(DateTime.Now.AddSeconds(x), DateTime.Now.AddSeconds(y)));
        }

        [Fact]
        public void Compare_should_apply_to_Double_natively() {
            var ep = EpsilonComparer.Create<double>(0.05);
            Assert.Equal(0, ep.Compare(0.05, 0));
        }

        [Fact]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_should_throw_for_Int32() {
            var ep = EpsilonComparer.Create<int>(2);
        }
    }
}
#endif
