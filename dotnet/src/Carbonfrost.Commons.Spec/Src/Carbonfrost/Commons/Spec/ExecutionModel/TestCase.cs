//
// Copyright 2017, 2018 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

    public abstract class TestCase : TestUnit {

        private readonly List<ITestCaseFilter> _filters = new List<ITestCaseFilter>(0);
        private List<object> _attributesCache;

        internal IList<ITestCaseFilter> Filters {
            get {
                return _filters;
            }
        }

        internal TestStatus PredeterminedStatus {
            get {
                if (Skipped && !IsFocused) { // Skip unless focussed
                    return TestStatus.Skipped;
                }
                if (IsPending) {
                    return TestStatus.Pending;
                }
                return TestStatus.NotRun;
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
        internal IEnumerable<Attribute> Attributes {
            get {
                if (_attributesCache == null) {
                    _attributesCache = new List<object>();
                    _attributesCache.AddAll(TestMethod.GetCustomAttributes(false));

                    if (TestMethod.ReturnParameter != null) {
                        _attributesCache.AddAll(TestMethod.ReturnParameter.GetCustomAttributes(false));
                    }
                    if (TestProperty != null) {
                        _attributesCache.AddAll(TestProperty.GetCustomAttributes(false));
                    }
                }
                return _attributesCache.Cast<Attribute>();
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
                return TestMethod.DeclaringType.GetProperty(TestMethod.Name.Substring(4));
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

        public override TestUnitType Type {
            get {
                return TestUnitType.Case;
            }
        }

        public override string DisplayName {
            get {
                return string.Concat(TestMethod.DeclaringType.FullName, ".", TestMethod.Name);
            }
        }

        protected TestCase(MethodInfo testMethod) {
            if (testMethod == null) {
                throw new ArgumentNullException("testMethod");
            }

            TestMethod = testMethod;
            Children.MakeReadOnly();
        }

        public TestCaseResult RunTest(TestContext testContext) {
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

            var result = new TestCaseResult(this) {
                Status = PredeterminedStatus,
                Reason = Reason,
            };
            result.Done(DateTime.Now);
            return result;
        }

        protected abstract TestCaseResult RunTestCore(TestContext testContext);

    }
}
