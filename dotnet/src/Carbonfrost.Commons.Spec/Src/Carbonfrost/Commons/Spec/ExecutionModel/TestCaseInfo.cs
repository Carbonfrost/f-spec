//
// Copyright 2017, 2018, 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    public abstract partial class TestCaseInfo : TestUnit {

        private readonly List<ITestCaseFilter> _filters = new List<ITestCaseFilter>(0);
        private IReadOnlyList<Attribute> _attributesCache;

        internal IList<ITestCaseFilter> Filters {
            get {
                return _filters;
            }
        }

        private protected TestStatus PredeterminedStatus {
            get {
                return TestUnit.ConvertToStatus(this).GetValueOrDefault(TestStatus.NotRun);
            }
        }

        public abstract int Position { get; }

        public sealed override TestUnitCollection Children {
            get {
                return TestUnitCollection.Empty;
            }
        }

        // These are the attributes that control the test case.  They
        // come from the method, its return parameter, and the corresponding
        // property
        private protected IReadOnlyList<Attribute> Attributes {
            get {
                if (_attributesCache == null) {
                    var cache = new List<object>();
                    cache.AddAll(TestMethod.GetCustomAttributes(false));

                    if (TestMethod.ReturnParameter != null) {
                        cache.AddAll(TestMethod.ReturnParameter.GetCustomAttributes(false));
                    }
                    if (TestProperty != null) {
                        cache.AddAll(TestProperty.GetCustomAttributes(false));
                    }
                    _attributesCache = cache.Cast<Attribute>().ToArray();
                }
                return _attributesCache;
            }
        }

        internal override TestUnitMetadata Metadata {
            get {
                return new TestUnitMetadata(Attributes);
            }
        }

        public MethodInfo TestMethod {
            get;
            private set;
        }

        public PropertyInfo TestProperty {
            get {
                if (TestMethod == null) {
                    return null;
                }
                if (TestMethod.Name.StartsWith("get_")) {
                    return TestMethod.DeclaringType.GetProperty(TestMethod.Name.Substring(4));
                }

                return null;
            }
        }

        public string MethodName {
            get {
                return TestMethod.Name;
            }
        }

        public virtual IReadOnlyList<object> TestMethodArguments {
            get {
                return Empty<object>.Array;
            }
        }

        public sealed override bool ContainsFocusedUnits {
            get {
                return false;
            }
        }

        public override string DisplayName {
            get {
                return string.Concat(Parent.DisplayName, ".", TestMethod.Name);
            }
        }

        private protected TestCaseInfo(MethodInfo testMethod) {
            if (testMethod == null) {
                throw new ArgumentNullException(nameof(testMethod));
            }

            TestMethod = testMethod;
            Children.MakeReadOnly();
        }

        public TestCaseResult RunTest(TestExecutionContext testContext) {
            if (PredeterminedStatus == TestStatus.NotRun) {
                var startedAt = DateTime.Now;
                try {
                    using (testContext.ApplyingContext()) {
                        return RunTestCore(testContext);
                    }

                } catch (Exception ex) {
                    var unhandled = new TestCaseResult(this);
                    unhandled.SetFailed(ex);
                    unhandled.Done(startedAt);
                    return unhandled;
                }
            }

            var result = new TestCaseResult(this, PredeterminedStatus) {
                Reason = Reason,
            };
            result.Done(DateTime.Now);
            return result;
        }

        internal abstract TestExecutionContext CreateExecutionContext(DefaultTestRunner runner);

        protected abstract TestCaseResult RunTestCore(TestExecutionContext testContext);

    }
}
