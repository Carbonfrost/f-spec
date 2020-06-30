//
// Copyright 2016-2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.Commons.Spec {

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class FFixtureDataAttribute : Attribute, ITestDataApiAttributeConventions, ITestCaseMetadataFilter {

        private readonly FixtureDataAttribute _inner;

        public string[] Tags {
            get {
                return _inner.Tags;
            }
            set {
                _inner.Tags = value;
            }
        }

        public string Tag {
            get {
                return _inner.Tag;
            }
            set {
                _inner.Tag = value;
            }
        }

        public string PathPattern {
            get {
                return _inner.PathPattern;
            }
        }

        public Uri Url {
            get {
                return _inner.Url;
            }
        }

        RetargetDelegates ITestDataApiAttributeConventions.RetargetDelegates {
            get {
                return RetargetDelegates.Unspecified;
            }
            set {
                throw new NotSupportedException();
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

        public string Reason {
            get {
                return _inner.Reason;
            }
            set {
                _inner.Reason = value;
            }
        }

        public bool Explicit {
            get {
                return _inner.Explicit;
            }
            set {
                _inner.Explicit = value;
            }
        }

        public FFixtureDataAttribute(string pathPattern) {
            _inner = new FixtureDataAttribute(pathPattern);
        }

        IEnumerable<TestData> ITestDataProvider.GetData(TestContext context) {
            return ((ITestDataProvider) _inner).GetData(context);
        }

        void ITestCaseMetadataFilter.Apply(TestCaseInfo testCase) {
            testCase.IsFocused = true;
        }
    }
}
