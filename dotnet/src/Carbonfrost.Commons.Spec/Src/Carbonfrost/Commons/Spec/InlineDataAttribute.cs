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
    public sealed class InlineDataAttribute : Attribute, ITestDataApiAttributeConventions {

        private readonly object[] _data;
        private readonly TestTagCache _tags = new TestTagCache();

        public string[] Tags {
            get {
                return _tags.Tags;
            }
            set {
                _tags.Tags = value;
            }
        }

        public string Tag {
            get {
                return _tags.Tag;
            }
            set {
                _tags.Tag = value;
            }
        }

        public IReadOnlyList<object> Data {
            get {
                return _data;
            }
        }

        public string Name {
            get;
            set;
        }

        public string Reason {
            get;
            set;
        }

        public bool Explicit {
            get;
            set;
        }

        public InlineDataAttribute(params object[] data) {
            _data = data;
        }

        public InlineDataAttribute(object data) {
            _data = new [] { data };
        }

        public InlineDataAttribute(object data1, object data2) {
            _data = new [] { data1, data2 };
        }

        public InlineDataAttribute(object data1, object data2, object data3) {
            _data = new [] { data1, data2, data3 };
        }

        IEnumerable<TestData> ITestDataProvider.GetData(TestContext context) {
            yield return new TestData(_data).Update(
                Name, Reason, Explicit ? TestUnitFlags.Explicit : TestUnitFlags.None
            ).WithTags(_tags);
        }

    }
}
