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

namespace Carbonfrost.Commons.Spec {

    public readonly struct XTestData<T> : ITestDataHelper<T> {

        private readonly TestData<T> _self;

        private TestData<T> Copy {
            get {
                return _self.Pending();
            }
        }

        public XTestData(params T[] data) {
            _self = new TestData<T>(data);
        }

        public static TestData<T> Create(params T[] data) {
            return TestData.XCreate(data);
        }

        public TestData<T> Explicit() {
            return Copy.Explicit();
        }

        public TestData<T> Explicit(string reason) {
            return Copy.Explicit(reason);
        }

        public TestData<T> Fail() {
            return Copy.Fail(null);
        }

        public TestData<T> Fail(string reason) {
            return Copy.Fail(reason);
        }

        public TestData<T> Focus() {
            return Copy.Focus();
        }

        public TestData<T> Focus(string reason) {
            return Copy.Focus(reason);
        }

        public TestData<T> Pending() {
            return Copy.Pending();
        }

        public TestData<T> Pending(string reason) {
            return Copy.Pending(reason);
        }

        public TestData<T> Skip() {
            return Copy.Skip();
        }

        public TestData<T> Skip(string reason) {
            return Copy.Skip(reason);
        }

        public TestData<T> WithName(string name) {
            return Copy.WithName(name);
        }

        public TestData<T> WithReason(string reason) {
            return Copy.WithReason(reason);
        }

        public TestData Untyped() {
            return Copy.Untyped();
        }

        public static implicit operator TestData(XTestData<T> data) {
            return data.Untyped();
        }
    }
}
