#if SELF_TEST

//
// Copyright 2018, 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.SelfTest.Spec {

    public class ExtensionsConsistencyTests : TestClass {

        public IEnumerable<MethodInfo> AllExtensionMethods {
            get {
                return typeof(Extensions).GetTypeInfo().GetMethods().Where(
                    t => t.IsStatic
                );
            }
        }

        public IEnumerable<IGrouping<string, MethodInfo>> ExtensionMethods {
            get {
                return AllExtensionMethods.GroupBy(t => t.Name);
            }
        }

        [Theory]
        [PropertyData("ExtensionMethods")]
        public void EnumerableExpectation_should_also_have_untyped_overload(IGrouping<string, MethodInfo> methods) {
            // If the method M<TValue>(EnumerableExpectation<TValue>, ...) exists then we also
            // want          M(EnumerableExpectation) to exist.

            // This helps with the ToHave expressions
            // Expect(new [] { "a" }).ToHave.M("a");

            // "Substring" is a special case and should be marked with the attribute to opt
            // out of this

            var applies = methods.Any(IsEnumerableOfTMethod) || methods.Any(IsEnumerableUntypedMethod);
            bool optOut = methods.Any(m => m.IsDefined(typeof(IgnoreEnumerableExpectationAttribute)));
            if (optOut || !applies) {
                Assert.Pass("Doesn't apply to this method group: " + methods.Key);
            }
            DoesNotApplyToCardinals(methods);

            var result = methods.Any(IsEnumerableOfTMethod) && methods.Any(IsEnumerableUntypedMethod);
            if (!result) {
                Assert.Fail($"{methods.Key}: Method group should have both M(EnumerableExpectation, ...) and M(EnumerableExpectation<T>, ...)");
            }
        }

        [Theory]
        [PropertyData("ExtensionMethods")]
        public void Methods_should_have_message_overloads(IGrouping<string, MethodInfo> methods) {
            // There should be at least one overload with
            //      M(..., message, args)
            // For now, this test doesn't check that the overload applies to each variation

            DoesNotApplyToTestMatcherExtensions(methods);
            DoesNotApplyToCardinals(methods);
            var result = methods.Any(IsMessageFormatMethod);
            if (!result) {
                Assert.Fail("Method group should have at least one method that can format a message M(..., message:, args:)");
            }
        }

        [Theory]
        [PropertyData("AllExtensionMethods")]
        public void Methods_should_not_extend_builder(MethodInfo method) {
            var pm = method.GetParameters().FirstOrDefault();
            if (pm == null) {
                return;
            }

            Assert.NotEqual(typeof(ExpectationBuilder), pm.ParameterType);
            Assert.NotEqual(typeof(ExpectationBuilder<>), pm.ParameterType);
        }

        static bool IsMessageFormatMethod(MethodInfo info) {
            var pms = info.GetParameters();
            if (pms.Length < 2) {
                return false;
            }

            var msg = pms[pms.Length - 2];
            var args = pms[pms.Length - 1];
            return msg.Name == "message"
                && args.Name == "args"
                && !msg.IsOptional
                && msg.ParameterType == typeof(string)
                && args.ParameterType == typeof(object[]);
        }

        // M(EnumerableExpectation<T>, ...)
        static bool IsEnumerableOfTMethod(MethodInfo info) {
            if (!info.IsGenericMethod || info.GetParameters().Length < 1) {
                return false;
            }

            var fp = info.GetParameters()[0].ParameterType.GetTypeInfo();
            return fp.IsGenericType
                && fp.GetGenericTypeDefinition() == typeof(EnumerableExpectation<>);
        }

        // M(EnumerableExpectation, ...)
        static bool IsEnumerableUntypedMethod(MethodInfo info) {
            if (info.GetParameters().Length < 1) {
                return false;
            }

            var fp = info.GetParameters()[0].ParameterType;
            return fp == typeof(EnumerableExpectation);
        }

        private void DoesNotApplyToTestMatcherExtensions(IGrouping<string, MethodInfo> methods) {
            if (methods.First().GetParameters()[0].ParameterType == typeof(ITestMatcher)) {
                Assert.Pass("Doesn't apply to this method group: " + methods.Key);
            }
        }

        private void DoesNotApplyToCardinals(IGrouping<string, MethodInfo> methods) {
            if (methods.Key == "Any"
                || methods.Key == "All"
                || methods.Key == "Exactly"
                || methods.Key == "AtMost"
                || methods.Key == "AtLeast"
                || methods.Key == "Between"
                || methods.Key == "No"
                || methods.Key == "None"
                || methods.Key == "Single") {
                Assert.Pass("Doesn't apply to this method group: " + methods.Key);
            }
        }
    }
}
#endif
