#if SELF_TEST

//
// Copyright 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using Carbonfrost.Commons.Spec.TestMatchers;
using static Carbonfrost.Commons.Spec.Matchers;

namespace Carbonfrost.SelfTest.Spec.TestMatchers {

    public class MemberwiseEqualMatcherTests : TestClass {

        [Fact]
        public void Matches_should_detect_same_reference() {
            var sameReference = new PObject();
            var subj = new MemberwiseEqualMatcher<PObject>(
                sameReference
            );
            Assert.True(subj.Matches(sameReference));
        }

        [Fact]
        public void Matches_should_detect_nominal() {
            var obj1 = new PObject {
                Field = "Item",
                Property = 420,
                Value = new PValueObject(),
            };
            var obj2 = new PObject {
                Field = "Item",
                Property = 420,
                Value = new PValueObject(),
            };
            var subj = new MemberwiseEqualMatcher<PObject>(
                obj1
            );
            Assert.True(subj.Matches(obj2));
        }

        [Fact]
        public void ExpectTo_should_obtain_matcher() {
            var obj1 = new PObject();
            var obj2 = new PObject();
            Expect(obj1).To(Matchers.BeMemberwiseEqualTo(obj2));
            Assert.IsInstanceOf<MemberwiseEqualMatcher<PObject>>(Matchers.BeMemberwiseEqualTo(obj2));
        }

        [Fact]
        public void Assert_MemberwiseEqual_should_apply_to_values_nominal() {
            var obj1 = new PObject {
                Field = "Item",
                Property = 420,
                Value = new PValueObject(),
            };
            var obj2 = new PObject {
                Field = "Item",
                Property = 420,
                Value = new PValueObject(),
            };
            Assert.MemberwiseEqual(obj1, obj2);
        }

        [Fact]
        public void Expect_ToBe_should_have_fluent_expression_anonymous_object() {
            var obj = new PObject {
                Field = "Item",
                Property = 420,
            };
            Expect(obj).ToBe.MemberwiseEqualTo(new {
                Field = "Item",
                Property = 420,
            });
        }

        [Fact]
        public void Expect_ToBe_should_have_fluent_expression_dictionary() {
            var obj = new PObject {
                Field = "Item",
                Property = 420,
            };
            Expect(obj).ToBe.MemberwiseEqualTo(new Dictionary<string, object> {
                ["Field"] = "Item",
                ["Property"] = 420,
            });
        }

        [Fact]
        public void Expect_ToBe_should_have_fluent_expression_dictionary_works_with_matchers() {
            var obj = new PObject {
                Field = "Item",
                Property = 420,
            };
            Expect(obj).ToBe.MemberwiseEqualTo(new Dictionary<string, object> {
                ["Field"] = Not(BeEmpty()),
                ["Property"] = BeGreaterThan(400),
            });
        }

        [Fact]
        public void Expect_Given_fluent_expression() {
            Given().Expect(() => new PObject()).To(BeMemberwiseEqualTo(new PObject()));
        }

        [Fact]
        public void Expect_Given_fluent_expression_filter_expressions() {
            Given().Expect(() => new PObjectPrivateProperties("cool") { PublicProperty = "Different" }).To(
                BeMemberwiseEqualTo(
                    new PObjectPrivateProperties("cool"),
                    PrivateProperties
                ));
        }

        [Fact]
        public void TestFailure_from_long_string_should_produce_diff() {
            var obj1 = new PObject {
                Field = "Item",
                Property = 420,
                Value = new PValueObject(),
            };
            var obj2 = new PObject {
                Field = "Different Item",
                Property = 999,
            };
            var subj = new MemberwiseEqualMatcher<PObject>(obj1);
            var failure = TestMatcherLocalizer.Failure(subj, obj2);
            Assert.NotNull(failure.UserData.Diff);

            var diff = failure.UserData.Diff.ToString().Split(new [] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            Assert.Equal(new [] { "@@@ -1,5 +1,5",
                             " {",
"-  Property = 420",
"+  Property = 999",
"@@@ -2,6 +2,6",
"   Property = 420",
"   Value = PValueObject {  }",
"-  Field = Item",
"+  Field = Different Item",
}, diff); // last line contains } which is removed
        }

#pragma warning disable 0169
#pragma warning disable 0649
        class PObject {
            public string Field;
            public int Property {
                get;
                set;
            }
            public PValueObject Value {
                get;
                set;
            }
        }

        class PObjectPrivateProperties {
            private string A {
                get;
                set;
            }

            public string PublicProperty {
                get;
                set;
            }

            public PObjectPrivateProperties(string a) {
                A = a;
            }
        }

        class PObjectSameShape {
            public string Field;
            public long Property {
                get;
                set;
            }
            public PValueObject Value {
                get;
                set;
            }
        }

        struct PValueObject {
        }
    }
}
#endif
