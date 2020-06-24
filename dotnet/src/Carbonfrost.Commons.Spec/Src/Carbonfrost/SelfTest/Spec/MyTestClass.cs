#if SELF_TEST

//
// Copyright 2016, 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Collections.Generic;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.SelfTest.Spec {

    public class MyTestClass : TestClass {

        public int BeforeTestCallCount;

        public IEnumerable<object[]> P1 {
            get {
                return P2;
            }
        }

        public static IEnumerable<object[]> P2 {
            get {
                return P5;
            }
        }

        public IEnumerable<string> P3 {
            get {
                return P4;
            }
        }

        public static IEnumerable<string> P4 {
            get {
                return new [] { "420", "420" };
            }
        }

        public static IEnumerable<string[]> P5 {
            get {
                yield return new [] { "420" };
                yield return new [] { "420" };
            }
        }

        public static string P6 {
            get {
                return "420";
            }
        }

        [Fact]
        public void Parse_should_work() {
            int result;
            Assert.True(Int32.TryParse("420", out result));
            Assert.Equal(420, result);
        }

        [Fact]
        [ExpectedException(typeof(FormatException))]
        public void Parse_should_throw() {
            Int32.Parse("s");
        }

        [Theory]
        [InlineData("420")]
        [InlineData("420")]
        public void Parse_with_data(string text) {
            int result;
            Assert.True(Int32.TryParse(text, out result));
            Assert.Equal(420, result);
        }

        [Theory]
        [PropertyData("P1")]
        [PropertyData("P2")]
        [PropertyData("P3")]
        [PropertyData("P4")]
        [PropertyData("P5")]
        public void Parse_with_propertydata(string text) {
            int result;
            Assert.True(Int32.TryParse(text, out result));
            Assert.Equal(420, result);
        }

        [Fact]
        public void TestContext_should_have_this_method_name() {
            var current = "TestContext_should_have_this_method_name";
            string expectedName = GetType().FullName + "." + current;
            Assert.Equal(expectedName, TestContext.CurrentTest.DisplayName);
        }

        protected override void BeforeTest() {
            BeforeTestCallCount++;
        }

        [Fact]
        public void BeforeTest_should_have_been_called() {
            Assert.Equal(1, BeforeTestCallCount);
        }
    }
}
#endif
