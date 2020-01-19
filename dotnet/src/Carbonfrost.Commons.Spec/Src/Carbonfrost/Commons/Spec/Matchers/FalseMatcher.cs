//
// Copyright 2017, 2019 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using Carbonfrost.Commons.Spec.Resources;
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.Commons.Spec {

    partial class Matchers {

        public static FalseMatcher BeFalse() {
            return new FalseMatcher();
        }

    }

    partial class Asserter {

        public void False(bool condition) {
            That(condition, Matchers.BeFalse());
        }

        public void False(bool condition, string message, params object[] args) {
            That(condition, Matchers.BeFalse(), message, args);
        }
    }

    partial class Assert {

        public static void False(bool condition) {
            Global.False(condition);
        }

        public static void False(bool condition, string message, params object[] args) {
            Global.False(condition, message, (object[]) args);
        }
    }

    partial class Assume {

        public static void False(bool condition) {
            Global.False(condition);
        }

        public static void False(bool condition, string message, params object[] args) {
            Global.False(condition, message, (object[]) args);
        }
    }

    partial class Extensions {

        public static void False(this Expectation<bool> e) {
            False(e, null);
        }

        public static void False(this Expectation<bool> e, string message, params object[] args) {
            e.Should(Matchers.BeFalse(), message, (object[]) args);
        }

    }

    namespace TestMatchers {

        public class FalseMatcher : TestMatcher<bool> {

            public override bool Matches(bool actual) {
                return !actual;
            }

        }
    }

    [AttributeUsage(AttributeTargets.ReturnValue | AttributeTargets.Property)]
    public sealed class FalseAttribute : Attribute, ITestMatcherFactory<bool> {

        public string Message { get; set; }

        ITestMatcher<bool> ITestMatcherFactory<bool>.CreateMatcher(TestContext testContext) {
            return Matchers.BeFalse();
        }

    }

}
