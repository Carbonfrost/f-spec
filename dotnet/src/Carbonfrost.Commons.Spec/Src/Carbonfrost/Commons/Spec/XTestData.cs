//
// Copyright 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

using System.Collections;
using System.Collections.Generic;

namespace Carbonfrost.Commons.Spec {

    public readonly struct XTestData : ITestData {

        private readonly TestData _self;

        public object this[int index] {
            get {
                return _self[index];
            }
        }

        public string Name {
            get {
                return _self.Name;
            }
        }

        public string Reason {
            get {
                return _self.Reason;
            }
        }

        public int Count {
            get {
                return _self.Count;
            }
        }

        public bool IsExplicit {
            get {
                return _self.IsExplicit;
            }
        }

        public bool IsFocused {
            get {
                return _self.IsFocused;
            }
        }

        public bool IsPending {
            get {
                return true;
            }
        }

        public bool PassExplicitly {
            get {
                return _self.PassExplicitly;
            }
        }

        public bool Skipped {
            get {
                return _self.Skipped;
            }
        }

        private TestData Copy {
            get {
                return _self.Pending();
            }
        }

        public XTestData(params object[] data) {
            _self = new TestData(data);
        }

        public static TestData Create(params object[] data) {
            return TestData.XCreate((object[]) data);
        }

        public TestData Explicit() {
            return Copy.Explicit();
        }

        public TestData Explicit(string reason) {
            return Copy.Explicit(reason);
        }

        public TestData Focus() {
            return Copy.Focus();
        }

        public TestData Focus(string reason) {
            return Copy.Focus(reason);
        }

        public IEnumerator<object> GetEnumerator() {
            return _self.GetEnumerator();
        }

        public TestData Pending() {
            return Copy.Pending();
        }

        public TestData Pending(string reason) {
            return Copy.Pending(reason);
        }

        public TestData Skip() {
            return Copy.Skip();
        }

        public TestData Skip(string reason) {
            return Copy.Skip(reason);
        }

        public TestData WithName(string name) {
            return Copy.WithName(name);
        }

        public TestData WithReason(string reason) {
            return Copy.WithReason(reason);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public static implicit operator TestData(XTestData data) {
            return data.Copy;
        }
    }
}
