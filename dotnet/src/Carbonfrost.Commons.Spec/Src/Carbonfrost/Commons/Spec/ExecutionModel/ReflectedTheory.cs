//
// Copyright 2016, 2017 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

    class ReflectedTheory : TestTheory {

        private TestDataProviderCollection _testDataProvidersCache;
        private readonly TestUnitCollection _children;

        public ReflectedTheory(MethodInfo method) : base(method) {
            _children = new TestUnitCollection(this);
        }

        public sealed override TestUnitCollection Children {
            get {
                return _children;
            }
        }

        public override TestDataProviderCollection TestDataProviders {
            get {
                return _testDataProvidersCache ??
                    (_testDataProvidersCache = CreateTestDataProviders());
            }
        }

        private TestDataProviderCollection CreateTestDataProviders() {
            var attrs = TestMethod.GetCustomAttributes(false);
            return new TestDataProviderCollection(attrs.OfType<ITestDataProvider>().ToArray());
        }

        protected override void Initialize(TestContext testContext) {
            IEnumerable<Attribute> attrs = TestMethod.GetCustomAttributes(false).Cast<Attribute>();
            attrs.ApplyMetadata(testContext);

            int index = 0;
            foreach (var attr in TestDataProviders) {
                var apply = attr as ITestCaseMetadataFilter;

                IEnumerable<TestData> data = null;
                try {
                    data = attr.GetData(testContext);
                } catch (Exception ex) {
                    Children.Add(SkippedInitFailure.DataProviderProblem(this, attr.ToString(), TestMethod, ex));
                }

                if (data == null) {
                    continue;
                }
                var allData = data.ToList();
                foreach (var row in allData) {
                    var caseUnit = new ReflectedTheoryCase(TestMethod, index, row);
                    Children.Add(caseUnit);

                    if (apply != null) {
                        apply.Apply(caseUnit);
                    }
                    index++;
                }
            }
        }

    }
}
