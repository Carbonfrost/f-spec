//
// Copyright 2017, 2018-2019 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.Commons.Spec {

    partial class Matchers {

        public static TrueMatcher BeTrue() {
            return new TrueMatcher();
        }

    }

    partial class Asserter {

        public void True(bool condition) {
            That(condition, Matchers.BeTrue());
        }

        public void True(bool condition, string message, params object[] args) {
            That(condition, Matchers.BeTrue(), message, args);
        }

    }

    partial class Assert {

        public static void True(bool condition) {
            Global.True(condition);
        }

        public static void True(bool condition, string message, params object[] args) {
            Global.True(condition, message, (object[]) args);
        }

    }

    partial class Assume {

        public static void True(bool condition) {
            Global.True(condition);
        }

        public static void True(bool condition, string message, params object[] args) {
            Global.True(condition, message, (object[]) args);
        }

    }

    partial class Extensions {

        public static void True(this IExpectation<bool> e) {
            True(e, null);
        }

        public static void True(this IExpectation<bool> e, string message, params object[] args) {
            e.Like(Matchers.BeTrue(), message, (object[]) args);
        }

    }

    namespace TestMatchers {

        public class TrueMatcher : TestMatcher<bool> {

            public override bool Matches(bool actual) {
                return actual;
            }
        }
    }

    [AttributeUsage(AttributeTargets.ReturnValue | AttributeTargets.Property)]
    public sealed class TrueAttribute : Attribute, ITestMatcherFactory<bool> {

        public string Message { get; set; }

        ITestMatcher<bool> ITestMatcherFactory<bool>.CreateMatcher(TestContext testContext) {
            return Matchers.BeTrue();
        }

    }
}
