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
using System.ComponentModel;
using static Carbonfrost.Commons.Spec.Matchers;

namespace Carbonfrost.Commons.Spec {

    public interface IExpectationBuilderAsserter {
        void To(ITestMatcher matcher, string message = null, params object[] args);

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This is an override of Object.Equals(). Call Assert.Equal() instead.", true)]
        bool Equals(object obj);
    }

    public interface IExpectationBuilderAsserter<out T> : IExpectationBuilderAsserter {
        void To(ITestMatcher<T> matcher, string message = null, params object[] args);
        void To(ITestMatcher<object> matcher, string message = null, params object[] args);
    }

    partial class Extensions {

        public static void NotTo(this IExpectationBuilderAsserter self, ITestMatcher matcher, string message = null, params object[] args) {
            self.To(Not(matcher), message, (object[]) args);
        }

        public static void ToNot(this IExpectationBuilderAsserter self, ITestMatcher matcher, string message = null, params object[] args) {
            self.To(Not(matcher), message, (object[]) args);
        }

        public static void NotTo<T>(this IExpectationBuilderAsserter<T> self, ITestMatcher<T> matcher, string message = null, params object[] args) {
            self.To(Not(matcher), message, (object[]) args);
        }

        public static void ToNot<T>(this IExpectationBuilderAsserter<T> self, ITestMatcher<T> matcher, string message = null, params object[] args) {
            self.To(Not(matcher), message, (object[]) args);
        }

        public static void NotTo<T>(this IExpectationBuilderAsserter<T> self, ITestMatcher<object> matcher, string message = null, params object[] args) {
            self.To(Not(matcher), message, (object[]) args);
        }

        public static void ToNot<T>(this IExpectationBuilderAsserter<T> self, ITestMatcher<object> matcher, string message = null, params object[] args) {
            self.To(Not(matcher), message, (object[]) args);
        }
    }
}
