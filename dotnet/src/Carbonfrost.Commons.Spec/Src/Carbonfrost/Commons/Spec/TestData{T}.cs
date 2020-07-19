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
using System;
using System.Collections;
using System.Collections.Generic;

namespace Carbonfrost.Commons.Spec {

    public readonly struct TestData<T> : ITestData<T>, ITestDataUntyped, ITestDataProvider {

        private readonly T[] _data;
        private readonly TestDataState _state;

        public TestTagCollection Tags {
            get {
                return _state.Tags;
            }
        }

        public string Name {
            get {
                return _state.Name;
            }
        }

        public string Reason {
            get {
                return _state.Reason;
            }
        }

        public bool IsExplicit {
            get {
                return _state.IsExplicit;
            }
        }

        public bool IsFocused {
            get {
                return _state.IsFocused;
            }
        }

        public bool IsPending {
            get {
                return _state.IsPending;
            }
        }

        public bool PassExplicitly {
            get {
                return _state.PassExplicitly;
            }
        }

        public bool Skipped {
            get {
                return _state.Skipped;
            }
        }

        public bool Failed {
            get {
                return _state.Failed;
            }
        }

        internal TestData(TestDataState state, T[] data) {
            _state = state;
            _data = data ?? Array.Empty<T>();
        }

        public TestData(params T[] data)
            : this(TestDataState.Empty, data) {
        }

        public static TestData<T> Create(params T[] data) {
            return new TestData<T>(data);
        }

        public static TestData<T> FCreate(params T[] data) {
            return new TestData<T>(TestDataState.F, data);
        }

        public static TestData<T> XCreate(params T[] data) {
            return new TestData<T>(TestDataState.X, data);
        }

        public TestData<T> WithName(string name) {
            return Update(_state.WithName(name));
        }

        public TestData<T> WithReason(string reason) {
            return Update(_state.WithReason(reason));
        }

        public TestData<T> WithTags(IEnumerable<TestTag> tags) {
            return Update(_state.WithTags(tags));
        }

        public TestData<T> Skip() {
            return Update(_state.Skip());
        }

        public TestData<T> Skip(string reason) {
            return Update(_state.Skip(reason));
        }

        public TestData<T> Fail() {
            return Update(_state.Fail());
        }

        public TestData<T> Fail(string reason) {
            return Update(_state.Fail(reason));
        }

        public TestData<T> Focus() {
            return Update(_state.Focus());
        }

        public TestData<T> Focus(string reason) {
            return Update(_state.Focus(reason));
        }

        public TestData<T> Pending() {
            return Update(_state.Pending());
        }

        public TestData<T> Pending(string reason) {
            return Update(_state.Pending(reason));
        }

        public TestData<T> Explicit() {
            return Update(_state.Explicit());
        }

        public TestData<T> Explicit(string reason) {
            return Update(_state.Explicit(reason));
        }

        private TestData<T> Update(TestDataState state) {
            return new TestData<T>(state, _data);
        }

        public T this[int index] {
            get {
                return _data[index];
            }
        }

        public int Count {
            get {
                return _data.Length;
            }
        }

        public IEnumerator<T> GetEnumerator() {
            return ((IEnumerable<T>) _data).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        IEnumerable<TestData> ITestDataProvider.GetData(TestContext context) {
            return new [] {
                Untyped()
            };
        }

        public TestData Untyped() {
            var clone = new object[_data.Length];
            for (int i = 0; i < _data.Length; i++) {
                clone[i] = _data[i];
            }
            return new TestData(_state, clone);
        }
    }
}
