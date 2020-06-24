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
using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.Commons.Spec {

    public readonly partial struct TestData : ITestData, ITestUnitState, ITestUnitStateApiConventions<TestData> {

        private readonly object[] _data;
        private readonly TestDataState _state;

        public TestTagCollection Tags {
            get {
                return _state.Tags;
            }
        }

        internal TestUnitFlags Flags {
            get {
                return _state.Flags;
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

        internal TestData(TestDataState state, object[] data) {
            _state = state;
            _data = data ?? Array.Empty<object>();
        }

        public TestData(params object[] data)
            : this(TestDataState.Empty, data) {
        }

        public static TestData Create(params object[] data) {
            return new TestData(data);
        }

        public static TestData FCreate(params object[] data) {
            return new TestData(TestDataState.F, data);
        }

        public static TestData XCreate(params object[] data) {
            return new TestData(TestDataState.X, data);
        }

        public static TestData<T> Create<T>(params T[] data) {
            return new TestData<T>(data);
        }

        public static TestData<T> FCreate<T>(params T[] data) {
            return new TestData<T>(TestDataState.F, data);
        }

        public static TestData<T> XCreate<T>(params T[] data) {
            return new TestData<T>(TestDataState.X, data);
        }

        public TestData WithName(string name) {
            return Update(_state.WithName(name));
        }

        public TestData WithReason(string reason) {
            return Update(_state.WithReason(reason));
        }

        public TestData WithTags(IEnumerable<TestTag> tags) {
            return Update(_state.WithTags(tags));
        }

        public TestData Skip() {
            return Update(_state.Skip());
        }

        public TestData Skip(string reason) {
            return Update(_state.Skip(reason));
        }

        public TestData Fail() {
            return Update(_state.Fail());
        }

        public TestData Fail(string reason) {
            return Update(_state.Fail(reason));
        }

        public TestData Focus() {
            return Update(_state.Focus());
        }

        public TestData Focus(string reason) {
            return Update(_state.Focus(reason));
        }

        public TestData Pending() {
            return Update(_state.Pending());
        }

        public TestData Pending(string reason) {
            return Update(_state.Pending(reason));
        }

        public TestData Explicit() {
            return Update(_state.Explicit());
        }

        public TestData Explicit(string reason) {
            return Update(_state.Explicit(reason));
        }

        internal TestData Update(TestDataState state) {
            return new TestData(state, _data);
        }

        public object this[int index] {
            get {
                return _data[index];
            }
        }

        public int Count {
            get {
                return _data.Length;
            }
        }

        public IEnumerator<object> GetEnumerator() {
            return ((IEnumerable<object>) _data).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        internal IEnumerable<object> Evaluate(TestContext testContext) {
            var method = ((TestCaseInfo) testContext.CurrentTest).TestMethod;
            var pms = method.GetParameters();
            int index = 0;

            foreach (var o in _data) {
                var convert = o as ITestDataConversion;

                if (convert != null) {
                    yield return convert.Convert(testContext, pms[index]);
                } else {
                    yield return o;
                }
                index++;
            }
        }

    }
}
