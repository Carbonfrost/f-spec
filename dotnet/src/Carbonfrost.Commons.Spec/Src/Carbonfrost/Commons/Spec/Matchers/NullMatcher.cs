//
// Copyright 2017-2019 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

        public static NullMatcher BeNull() {
            return new NullMatcher();
        }

    }

    partial class Extensions {

        public static void Null<T>(this Expectation<T> e) where T : class {
            Null(e, null);
        }

        public static void Null<T>(this Expectation<T> e, string message, params object[] args) where T : class {
            e.As<object>().Should(Matchers.BeNull(), message, (object[]) args);
        }

    }

    partial class Asserter {

        public void Null<T>(T instance) where T : class {
            That(instance, Matchers.BeNull());
        }

        public void Null<T>(T instance, string message, params object[] args) where T:class {
            That(instance, Matchers.BeNull(), message, args);
        }

        public void NotNull<T>(T instance) where T : class {
            NotThat(instance, Matchers.BeNull());
        }

        public void NotNull<T>(T instance, string message, params object[] args) where T : class {
            NotThat(instance, Matchers.BeNull(), message, args);
        }

    }

    partial class Assert {

		public static void Null<T>(T instance) where T : class {
            Global.Null<T>(instance);
        }

        public static void Null<T>(T instance, string message, params object[] args) where T:class {
            Global.Null<T>(instance, message, (object[]) args);
        }

        public static void NotNull<T>(T instance) where T : class {
            Global.NotNull<T>(instance);
        }

        public static void NotNull<T>(T instance, string message, params object[] args) where T : class {
            Global.NotNull<T>(instance, message, (object[]) args);
        }

    }


    partial class Assume {

		public static void Null<T>(T instance) where T : class {
            Global.Null<T>(instance);
        }

        public static void Null<T>(T instance, string message, params object[] args) where T:class {
            Global.Null<T>(instance, message, (object[]) args);
        }

        public static void NotNull<T>(T instance) where T : class {
            Global.NotNull<T>(instance);
        }

        public static void NotNull<T>(T instance, string message, params object[] args) where T : class {
            Global.NotNull<T>(instance, message, (object[]) args);
        }

    }


    namespace TestMatchers {

        public class NullMatcher : TestMatcher<object> {

            public override bool Matches(object actual) {
                if (actual is ValueType) {
                    throw SpecFailure.CannotUseNullOnValueType(actual.GetType());
                }
                return actual == null;
            }
        }
    }

    [AttributeUsage(AttributeTargets.ReturnValue | AttributeTargets.Property)]
    public sealed class NullAttribute : Attribute, ITestMatcherFactory<object> {

        public string Message { get; set; }

        ITestMatcher<object> ITestMatcherFactory<object>.CreateMatcher(TestContext testContext) {
            return Matchers.BeNull();
        }

    }

    [AttributeUsage(AttributeTargets.ReturnValue | AttributeTargets.Property)]
    public sealed class NotNullAttribute : Attribute, ITestMatcherFactory<object> {

        public string Message { get; set; }

        ITestMatcher<object> ITestMatcherFactory<object>.CreateMatcher(TestContext testContext) {
            return Matchers.Not(Matchers.BeNull());
        }

    }

}
