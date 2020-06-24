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
        public void Methods_should_have_message_overloads(IGrouping<string, MethodInfo> methods) {
            // There should be at least one overload with
            //      M(..., message, args)
            // For now, this test doesn't check that the overload applies to each variation

            DoesNotApplyToTestMatcherExtensions(methods);
            DoesNotApplyToExpectationBuilderAsserterExtensions(methods);
            DoesNotApplyToCardinals(methods);
            DoesNotApplyToExpectationBuilder(methods);
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

        private void DoesNotApplyToTestMatcherExtensions(IGrouping<string, MethodInfo> methods) {
            if (methods.First().GetParameters()[0].ParameterType == typeof(ITestMatcher)) {
                Assert.Pass("Doesn't apply to this method group: " + methods.Key);
            }
        }

        private void DoesNotApplyToExpectationBuilderAsserterExtensions(IGrouping<string, MethodInfo> methods) {
            if (methods.First().GetParameters()[0].ParameterType == typeof(IExpectationBuilderAsserter)) {
                Assert.Pass("Doesn't apply to this method group: " + methods.Key);
            }
        }

        private void DoesNotApplyToExpectationBuilder(IGrouping<string, MethodInfo> methods) {
            var pt = methods.First().GetParameters()[0].ParameterType;
            if (pt.IsGenericType && pt.GetGenericTypeDefinition() == typeof(GivenExpectationBuilder<>)) {
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
