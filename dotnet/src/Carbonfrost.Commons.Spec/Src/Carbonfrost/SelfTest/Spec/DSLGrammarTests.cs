#if SELF_TEST

//
// Copyright 2018 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

namespace Carbonfrost.SelfTest.Spec {

    public class DSLGrammarTests : VerificationTestBase {

        public IEnumerable<Example> BadGrammar {
            get {
                return new [] {
                    E(Bad_No_Item, "Don't use Exactly(0).Item() or No.Item() -- use Items"),
                    E(Bad_Exactly0_Item, "Don't use Exactly(0).Item() or No.Item() -- use Items"),
                    E(Bad_Single_Items, "Don't use Exactly(1).Items() or Single.Items() -- use Item"),
                    E(Bad_Between_1_and_1_Items, "Don't use Exactly(1).Items() or Single.Items() -- use Item"),
                };
            }
        }

        public IEnumerable<Action> OKGrammar {
            get {
                return new Action[] {
                    OK_AtLeast1_Item,
                    OK_Between15_Items,
                };
            }
        }

        [Theory]
        [PassExplicitly]
        [PropertyData("BadGrammar")]
        public void BadGrammar_should_raise_warnings(Example e) {
            try {
                e.Action();
            } catch (AssertVerificationException ex) {
                Expect(ex.Message).ToBe.EqualTo(e.MessagePattern, "Log message should match " + e.MessagePattern);
                Assert.Pass();
            }
        }

        [Theory]
        [PropertyData("OKGrammar")]
        public void OKGrammar_should_be_OK(Action a) {
            a();
        }

        void Bad_No_Item() {
            Expect().ToHave.No.Item();
        }

        void Bad_Exactly0_Item() {
            Expect().ToHave.Exactly(0).Item();
        }

        void Bad_Single_Items() {
            Expect(new [] { 1 }).ToHave.Single.Items();
        }

        void Bad_Between_1_and_1_Items() {
            Expect().ToHave.Between(1, 1).Items();
        }

        void OK_AtLeast1_Item() {
            Expect(1, 2, 3).ToHave.AtLeast(1).Item();
        }

        void OK_Between15_Items() {
            Expect(1, 2, 3).ToHave.Between(1, 5).Items();
        }

        public struct Example {
            public readonly Action Action;
            public readonly string MessagePattern;

            public Example(Action action, string messagePattern) {
                Action = action;
                MessagePattern = messagePattern;
            }

            public override string ToString() {
                return Action.GetMethodInfo().Name;
            }
        }

        public static Example E(Action a, string s) {
            return new Example(a, s);
        }
    }
}
#endif
