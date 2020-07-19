#if SELF_TEST

//
// Copyright 2016, 2017 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Linq;
using System.Reflection;
using Carbonfrost.Commons.Spec;
using Carbonfrost.Commons.Spec.ExecutionModel;
using Carbonfrost.SelfTest.Spec.ExecutionModel;

namespace Carbonfrost.SelfTest.Spec {

    public class TestDataTests {

        private static readonly MethodInfo fakeTestMethod = typeof(TestDataTests).GetMethod("FakeTestMethod");
        private static readonly MethodInfo fakeTestMethod2 = typeof(TestDataTests).GetMethod("FakeTestMethod2");

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

        public TestData P7 {
            get {
                return new TestData("420");
            }
        }

        public IEnumerable<TestData> P8 {
            get {
                yield return new TestData("420");
                yield return new TestData("421");
            }
        }

        public TestData<string> PTypedTestData {
            get {
                return TestData<string>.Create("420");
            }
        }

        public IEnumerable<TestData<string>> PTypedTestDataEnumerable {
            get {
                yield return TestData<string>.Create("420");
                yield return TestData.Create<string>("421");
            }
        }

        public ICollection<TestData<string>> PTypedTestDataCollection {
            get {
                return new [] {
                    TestData<string>.Create("420"),
                    TestData.Create<string>("421"),
                };
            }
        }

        public TestData<string>[] PTypedTestDataArray {
            get {
                return new [] {
                    TestData<string>.Create("420"),
                    TestData.Create<string>("421"),
                };
            }
        }

        public IEnumerable<string> A {
            get {
                return new [] { "a", "b", };
            }
        }

        public IEnumerable<string> B {
            get {
                return new [] { "c", "d", };
            }
        }

        public void FakeTestMethod(string arg) {}
        public void FakeTestMethod2(string arg1, string arg2) {}

        [Theory]
        [InlineData("P6")]
        [InlineData("P7")]
        [InlineData(nameof(PTypedTestData))]
        public void Create_from_property_should_yield_singleton_return_type(string property) {
            var prop = Property(property);
            var ctxt = TestContext.NewExecContext(new FakeTheory(fakeTestMethod), new FakeRunner(), null);
            var actual = TestData.Create(ctxt, MemberAccessors.Property(prop));
            Assert.Equal(1, actual.Count());
            Assert.Equal("420", actual.First()[0]);
        }

        [Theory]
        [InlineData("P1")]
        [InlineData("P2")]
        [InlineData("P3")]
        [InlineData("P4")]
        [InlineData("P5")]
        [InlineData("P8")]
        [InlineData(nameof(PTypedTestDataEnumerable))]
        [InlineData(nameof(PTypedTestDataCollection))]
        [InlineData(nameof(PTypedTestDataArray))]
        public void Create_from_property_should_yield_items_from_lists(string property) {
            var prop = Property(property);
            var ctxt = TestContext.NewExecContext(new FakeTheory(fakeTestMethod), new FakeRunner(), null);
            Assert.Equal(2, TestData.Create(ctxt, MemberAccessors.Property(prop)).Count());
        }

        public IEnumerable<(string, int)> PValueTuples {
            get {
                return new [] {
                    ("a", 0),
                    ("b", 1),
                };
            }
        }

        public IEnumerable<Tuple<string, int>> PTuples {
            get {
                return new [] {
                    Tuple.Create("a", 0),
                    Tuple.Create("b", 1),
                };
            }
        }

        public void PTupleMethodTheory(string arg1, int arg2) {}

        [Theory]
        [InlineData(nameof(PValueTuples))]
        [InlineData(nameof(PTuples))]
        public void Create_from_Tuple_type(string property) {
            var prop = Property(property);

            var theory = new FakeTheory(GetType().GetMethod("PTupleMethodTheory"));

            var ctxt = TestContext.NewExecContext(theory, new FakeRunner(), null);
            var data = TestData.Create(ctxt, MemberAccessors.Property(prop));
            Assert.Equal(2, data.Count());
            Assert.Equal(new object[] { "a", 0 }, TestData.Create(ctxt, MemberAccessors.Property(prop)).First());
        }

        [Fact]
        public void Create_from_property_cross_join_should_yield_items_from_lists() {
            var a = Property("A");
            var b = Property("B");
            var data = new [] {
                new [] { "a", "b" },
                new [] { "c", "d" },
            };
            var theory = new FakeTheory(fakeTestMethod2);
            var ctxt = TestContext.NewExecContext(theory, new FakeRunner(), null);
            var guineaPig = TestData.Create(ctxt, new[] {
                                                MemberAccessors.Property(a), MemberAccessors.Property(b)
                                            });

            var expected = ConvertToStrings(TestData.Combinatorial(data));
            var actual = ConvertToStrings(guineaPig);
            Assert.SetEqual(actual, expected);
        }

        static string[] ConvertToStrings(IEnumerable<object[]> items) {
            return items.Select(t => string.Join(", ", (object[]) t)).ToArray();
        }

        static string[] ConvertToStrings(IEnumerable<TestData> items) {
            return items.Select(t => string.Join(", ", (IEnumerable<object>) t)).ToArray();
        }

        [Fact]
        public void Combinatorial_should_generate_trivial() {
            var nvc = new [] {
                new [] { "a", "b" },
            };
            var combo = TestData.Combinatorial(nvc).ToList();

            var all = ConvertToStrings(combo);
            Assert.HasCount(2, all);
            Assert.Contains("a", all);
            Assert.Contains("b", all);
        }

        [Fact]
        public void Combinatorial_should_generate_combinations_two_operands() {
            var nvc = new [] {
                new [] { "a", "b" },
                new [] { "a", "b" },
            };
            var combo = TestData.Combinatorial(nvc).ToList();
            Assert.HasCount(4, combo);
            var all = ConvertToStrings(combo);
            Assert.Contains("a, a", all);
            Assert.Contains("a, b", all);
            Assert.Contains("b, a", all);
            Assert.Contains("b, b", all);
        }

        [Fact]
        public void Combinatorial_should_generate_combinations_three_operands() {
            var nvc = new [] {
                new [] { "a", "b" },
                new [] { "c", "d" },
                new [] { "e", "f", "g" },
            };

            var combo = TestData.Combinatorial(nvc).ToList();
            string[] all = ConvertToStrings(combo);
            Assert.Contains("a, c, e", all);
            Assert.Contains("a, c, f", all);
            Assert.Contains("a, c, g", all);
            Assert.Contains("a, d, e", all);
            Assert.Contains("a, d, f", all);
            Assert.Contains("a, d, g", all);
            Assert.Contains("b, c, e", all);
            Assert.Contains("b, c, f", all);
            Assert.Contains("b, c, g", all);
            Assert.Contains("b, d, e", all);
            Assert.Contains("b, d, f", all);
            Assert.Contains("b, d, g", all);
        }

        [Fact]
        public void FCreate_should_create_focused_test_data() {
            Assert.True(TestData.FCreate("arg").Focus().IsFocused);
        }

        [Fact]
        public void XCreate_should_create_pending_test_data() {
            Assert.True(TestData.XCreate("arg").Pending().IsPending);
        }

        [Fact]
        public void FTestData_conversion_should_create_focused_test_data() {
            Assert.True(((TestData) new FTestData("arg")).IsFocused);
        }

        [Fact]
        public void FTestData_conversion_empty_should_create_focused_test_data() {
            Assert.True(((TestData) new FTestData()).IsFocused);
        }

        [Fact]
        public void XTestData_conversion_should_create_focused_test_data() {
            Assert.True(((TestData) new XTestData("arg")).IsPending);
        }

        [Fact]
        public void XTestData_conversion_empty_should_create_focused_test_data() {
            Assert.True(((TestData) new XTestData()).IsPending);
        }

        [Fact]
        public void TestData_use_cases() {
            // Simply set up a value
            var simple = new object[] {
                new TestData("hello"),
                TestData.Create("hello")
            };

            // Prepend X or F to change pending/focus
            var pending = new object[] {
                new XTestData("hello"),
                XTestData.Create("hello"),
                TestData.XCreate("hello")
            };

            var focus = new object[] {
                new FTestData("hello"),
                FTestData.Create("hello"),
                TestData.FCreate("hello")
            };
        }

        static PropertyInfo Property(string property) {
            return typeof(TestDataTests).GetProperty(property, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
        }

        class FakeTheory : TestTheory {

            public override TestUnitCollection Children {
                get {
                    return TestUnitCollection.Empty;
                }
            }

            public FakeTheory(MethodInfo method) : base(method) {
                SetParent(new FakeTestClassInfo());
            }
        }

        class FakeTestClassInfo : TestClassInfo {

            public FakeTestClassInfo() : base(typeof(TestDataTests)) {}
        }
    }
}
#endif
