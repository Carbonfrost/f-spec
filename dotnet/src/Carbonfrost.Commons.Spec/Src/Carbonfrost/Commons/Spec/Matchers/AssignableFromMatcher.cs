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
using System.Reflection;
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.Commons.Spec {

    partial class Matchers {

        public static AssignableFromMatcher BeAssignableFrom(Type expected) {
            return new AssignableFromMatcher(expected);
        }

    }

    static partial class Extensions {

        public static void AssignableFrom(this Expectation<Type> e, Type expected) {
            AssignableFrom(e, expected, null);
        }

        public static void AssignableFrom(this Expectation<TypeInfo> e, Type expected) {
            AssignableFrom(e, expected, null);
        }

        public static void AssignableFrom(this Expectation<Type> e, Type expected, string message, params object[] args) {
            e.Should(Matchers.BeAssignableFrom(expected), message, (object[]) args);
        }

        public static void AssignableFrom(this Expectation<TypeInfo> e, Type expected, string message, params object[] args) {
            e.Should(Matchers.BeAssignableFrom(expected), message, (object[]) args);
        }

    }

    partial class Asserter {

        public void AssignableFrom(Type expected, Type actual) {
            That(actual, Matchers.BeAssignableFrom(expected));
        }

        public void AssignableFrom(Type expected, Type actual, string message, params object[] args) {
            That(actual, Matchers.BeAssignableFrom(expected), message, args);
        }

        public void NotAssignableFrom(Type expected, Type actual) {
            NotThat(actual, Matchers.BeAssignableFrom(expected));
        }

        public void NotAssignableFrom(Type expected, Type actual, string message, params object[] args) {
            NotThat(actual, Matchers.BeAssignableFrom(expected), message, args);
        }

    }

    partial class Assert {

        public static void AssignableFrom(Type expected, Type actual) {
            Global.AssignableFrom(expected, actual);
        }

        public static void AssignableFrom(Type expected, Type actual, string message, params object[] args) {
            Global.AssignableFrom(expected, actual, message, (object[]) args);
        }

        public static void NotAssignableFrom(Type expected, Type actual) {
            Global.NotAssignableFrom(expected, actual);
        }

        public static void NotAssignableFrom(Type expected, Type actual, string message, params object[] args) {
            Global.NotAssignableFrom(expected, actual, message, (object[]) args);
        }

    }

    partial class Assume {

        public static void AssignableFrom(Type expected, Type actual) {
            Global.AssignableFrom(expected, actual);
        }

        public static void AssignableFrom(Type expected, Type actual, string message, params object[] args) {
            Global.AssignableFrom(expected, actual, message, (object[]) args);
        }

        public static void NotAssignableFrom(Type expected, Type actual) {
            Global.NotAssignableFrom(expected, actual);
        }

        public static void NotAssignableFrom(Type expected, Type actual, string message, params object[] args) {
            Global.NotAssignableFrom(expected, actual, message, (object[]) args);
        }

    }


    namespace TestMatchers {

        public class AssignableFromMatcher : TestMatcher<Type>, ITestMatcher<TypeInfo> {

            public Type Expected { get; private set; }

            public AssignableFromMatcher(Type expected) {
                Expected = expected;
            }

            public override bool Matches(Type actual) {
                return Expected.GetTypeInfo().IsAssignableFrom(actual);
            }

            bool ITestMatcher<TypeInfo>.Matches(ITestActualEvaluation<TypeInfo> actualFactory) {
                var actual = actualFactory.Value;
                var matches = Expected.GetTypeInfo().IsAssignableFrom(actual);
                return matches;
            }
        }
    }
}
