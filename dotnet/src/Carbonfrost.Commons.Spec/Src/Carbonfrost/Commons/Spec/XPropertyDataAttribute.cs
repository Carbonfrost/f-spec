//
// Copyright 2016-2018 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.Commons.Spec {

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class XPropertyDataAttribute : Attribute, ITestDataProvider, ITestCaseMetadataFilter {

        private readonly PropertyDataAttribute _inner;

        public IReadOnlyList<string> Properties {
            get {
                return _inner.Properties;
            }
        }

        public string Name {
            get {
                return _inner.Name;
            }
            set {
                _inner.Name = value;
            }
        }

        public string Reason { get; set; }

        public XPropertyDataAttribute(params string[] properties) {
            _inner = new PropertyDataAttribute(properties);
        }

        public override string ToString() {
            return string.Format("XPropertyData({0})", string.Join(", ", Properties));
        }

        IEnumerable<TestData> ITestDataProvider.GetData(TestContext context) {
            return ((ITestDataProvider) _inner).GetData(context);
        }

        void ITestCaseMetadataFilter.Apply(TestCase testCase) {
            testCase.IsPending = true;
            testCase.Reason = Reason;
        }
    }
}
