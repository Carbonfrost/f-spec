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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

using Carbonfrost.Commons.Spec;
using Carbonfrost.Commons.Spec.ExecutionModel;
using Carbonfrost.Commons.Spec.ExecutionModel.Output;

namespace Carbonfrost.SelfTest.Spec {

    [TestSubjectProvider(typeof(AllTestMatchersProvider))]
    public class TestMatcherTests<T> : TestClass<T> {

        public object SubjectNegated {
            get {
                return new NotWrapper(Subject);
            }
        }

        public UserDataCollection ExtractedUserData {
            get {
                return new UserDataCollection(Subject);
            }
        }

        [Fact]
        public void Localizer_should_generate_failure() {
            var failure = TestMatcherLocalizer.Failure(Subject, new object());
            Assert.DoesNotContain("FAILED TO LOCALIZE", failure.Message);
        }

        [Fact]
        public void Localizer_should_generate_failure_with_predicate() {
            var failure = TestMatcherLocalizer.FailurePredicate(Subject);
            Assert.DoesNotContain("FAILED TO LOCALIZE", failure.Message);
        }

        [Fact]
        public void Localizer_should_generate_not_failure() {
            var failure = TestMatcherLocalizer.Failure(SubjectNegated, new object());
            Assert.DoesNotContain("FAILED TO LOCALIZE", failure.Message);
        }

        [Fact]
        public void Localizer_should_generate_not_failure_with_predicate() {
            var failure = TestMatcherLocalizer.FailurePredicate(SubjectNegated);
            Assert.DoesNotContain("FAILED TO LOCALIZE", failure.Message);
        }

        [Fact]
        public void Localizer_should_generate_property_localization() {
            var data = ExtractedUserData;
            foreach (var kvp in data) {
                var caption = ConsoleExceptionInfo.Caption(kvp.Key);
                Assert.DoesNotContain("FAILED TO LOCALIZE", caption);
            }
        }

        [Fact]
        public void WithComparison_should_implement_methods() {
            var methods = typeof(T).GetTypeInfo().GetMethods();
            var ifaces = typeof(T).GetTypeInfo().GetInterfaces();

            var comparer = methods.FirstOrDefault(t => t.Name == "WithComparer");
            var comparison = methods.FirstOrDefault(t => t.Name == "WithComparison");
            var orClose = methods.FirstOrDefault(t => t.Name == "OrClose");
            var iface = ifaces.FirstOrDefault(t => t.GetTypeInfo().IsGenericType && t.GetTypeInfo().GetGenericTypeDefinition() == typeof(ITestMatcherWithComparer<>));

            if (comparer == null && comparison == null && iface == null) {
                Assert.Pass("Doesn't apply to {0}", typeof(T));
            }

            Assert.NotNull(comparer, "If matcher implements comparers, it should have WithComparer(IComparer)");
            Assert.NotNull(comparison, "If matcher implements comparers, it should have WithComparison(Comparison)");

            // Skip in the special case of ITestMatcher<IEnumerable>
            var elementIface = ifaces.Any(
                t => t.GetTypeInfo().IsGenericType
                && t.GetTypeInfo().GetGenericTypeDefinition() == typeof(ITestMatcher<>)
                && typeof(IEnumerable).IsAssignableFrom(t.GetGenericArguments()[0]));
            if (elementIface ) {
                Assert.Pass("Doesn't apply to {0} (ITestMatcher<IEnumerable>)", typeof(T));
            }

            Assert.NotNull(orClose, "If matcher implements comparers, it should have OrClose(TEpsilon)");

            Assert.NotNull(iface, "If matcher appears to implement comparers, it should implement ITestMatcherWithComparer");
        }

        private class NotWrapper : INotMatcher {

            public NotWrapper(object o) {
                InnerMatcher = o;
            }

            public object InnerMatcher {
                get; set;
            }
        }

    }

    class AllTestMatchersProvider : ITestSubjectProvider {

        public IEnumerable<object> GetTestSubjects(TestContext context) {
            var result = new List<object>();

            foreach (var t in typeof(TestContext).GetTypeInfo().Assembly.ExportedTypes) {
                if (t.Namespace != "Carbonfrost.Commons.Spec.TestMatchers") {
                    continue;
                }
                if (t.GetTypeInfo().IsInterface || t.GetTypeInfo().IsAbstract) {
                    continue;
                }
                if (t.Name.Contains("Attribute")) {
                    continue;
                }

                var type = t;
                if (t.GetTypeInfo().IsGenericType) {
                    var def = t.GetTypeInfo().GetGenericTypeDefinition().GetGenericArguments().Length;
                    var typeArgs = Enumerable.Repeat(typeof(object), def);
                    type = t.MakeGenericType(typeArgs.ToArray());
                }

                // Generate some useful arguments when activating the
                object[] args = type.GetConstructors()[0].GetParameters().Select(
                    p => {
                        if (p.ParameterType == typeof(string)) {
                            return (object) "";
                        }
                        if (p.ParameterType == typeof(Type)) {
                            return typeof(object);
                        }
                        if (p.ParameterType == typeof(Regex)) {
                            return new Regex(".*");
                        }
                        if (p.ParameterType == typeof(ITestMatcher)) {
                            return TestMatcher.Anything;
                        }
                        if (p.ParameterType.GetTypeInfo().IsGenericType && p.ParameterType.GetTypeInfo().GetGenericTypeDefinition() == typeof(ITestMatcher<>)) {
                            return TestMatcher<object>.Anything;
                        }

                        return null;
                    }).ToArray();

                try {
                    result.Add(Activator.CreateInstance(type, args));

                } catch (Exception ex) {
                    // TODO Should use Log -- but it may not be wired correctly at this time
                    Console.WriteLine("Can't create subject {0}\n{1}", t, ex.InnerException.Message);
                }
            }
            return result;
        }
    }
}
#endif
