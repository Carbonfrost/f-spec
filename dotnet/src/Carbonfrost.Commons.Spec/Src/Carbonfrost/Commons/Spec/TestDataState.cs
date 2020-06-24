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
using System.Collections.Generic;
using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.Commons.Spec {

    readonly struct TestDataState {

        public static readonly TestDataState Empty = new TestDataState();
        public static readonly TestDataState F = new TestDataState(
            null,
            null,
            TestUnitFlags.Focus,
            null
        );
        public static readonly TestDataState X = new TestDataState(
            null,
            null,
            TestUnitFlags.Pending,
            null
        );

        private readonly string _name;
        private readonly string _reason;
        private readonly TestTagCollection _tags;
        private readonly TestUnitFlags _flags;

        public TestTagCollection Tags {
            get {
                return _tags;
            }
        }

        internal TestUnitFlags Flags {
            get {
                return _flags;
            }
        }

        public string Name {
            get {
                return _name;
            }
        }

        public string Reason {
            get {
                return _reason;
            }
        }

        public bool IsExplicit {
            get {
                return _flags.HasFlag(TestUnitFlags.Explicit);
            }
        }

        public bool IsFocused {
            get {
                return _flags.HasFlag(TestUnitFlags.Focus);
            }
        }

        public bool IsPending {
            get {
                return _flags.HasFlag(TestUnitFlags.Pending);
            }
        }

        public bool PassExplicitly {
            get {
                return _flags.HasFlag(TestUnitFlags.PassExplicitly);
            }
        }

        public bool Skipped {
            get {
                return _flags.HasFlag(TestUnitFlags.Skip);
            }
        }

        public bool Failed {
            get {
                return _flags.HasFlag(TestUnitFlags.Failed);
            }
        }

        internal TestDataState(string name, string reason, TestUnitFlags flags, IEnumerable<TestTag> tags) {
            _name = name;
            _reason = reason;
            _flags = flags;
            _tags = TestTagCollection.Create(tags);
            _tags.MakeReadOnly();
        }

        public TestDataState WithName(string name) {
            return Update(name, Reason, _flags);
        }

        public TestDataState WithReason(string reason) {
            return Update(Name, reason, _flags);
        }

        public TestDataState WithTags(IEnumerable<TestTag> tags) {
            return new TestDataState(Name, Reason, _flags, tags);
        }

        public TestDataState Skip() {
            return Skip(null);
        }

        public TestDataState Skip(string reason) {
            return Update(Name, reason ?? Reason, _flags | TestUnitFlags.Skip);
        }

        public TestDataState Fail() {
            return Fail(null);
        }

        public TestDataState Fail(string reason) {
            return Update(Name, reason ?? Reason, _flags | TestUnitFlags.Failed);
        }

        public TestDataState Focus() {
            return Update(Name, Reason, _flags | TestUnitFlags.Focus);
        }

        public TestDataState Focus(string reason) {
            return Update(Name, reason, _flags | TestUnitFlags.Focus);
        }

        public TestDataState Pending() {
            return Update(Name, Reason, _flags | TestUnitFlags.Pending);
        }

        public TestDataState Pending(string reason) {
            return Update(Name, reason, _flags | TestUnitFlags.Pending);
        }

        public TestDataState Explicit() {
            return Explicit(null);
        }

        public TestDataState Explicit(string reason) {
            return Update(Name, reason ?? Reason, _flags | TestUnitFlags.Explicit);
        }

        private TestDataState Update(string name, string reason, TestUnitFlags flags) {
            return new TestDataState(name, reason, flags, _tags);
        }

    }
}
