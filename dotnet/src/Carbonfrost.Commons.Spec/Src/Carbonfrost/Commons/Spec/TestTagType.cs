//
// Copyright 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
using System;
using System.Collections.Generic;

namespace Carbonfrost.Commons.Spec {

    public partial struct TestTagType : IEquatable<TestTagType>, IComparable<TestTagType> {

        private static readonly Dictionary<string, string> _canonicalNames = new Dictionary<string, string>(
            StringComparer.OrdinalIgnoreCase
        );
        private readonly string _name;

        public string Name {
            get {
                return _name ?? "unspecified";
            }
        }

        public bool Automatic {
            get {
                return IsAutomaticTag(Name);
            }
        }

        public TestTagType(string name) {
            this = Parse(name);
        }

        private TestTagType(string name, bool dummy) {
            _name = name;
        }

        public static TestTagType Parse(string text) {
            TestTagType result;
            var ex = _TryParse(text, out result);
            if (ex != null) {
                throw ex;
            }

            return result;
        }

        public static bool TryParse(string text, out TestTagType result) {
            return _TryParse(text, out result) == null;
        }

        private static Exception _TryParse(string text, out TestTagType result) {
            result = default(TestTagType);
            if (text == null) {
                return new ArgumentNullException(nameof(text));
            }

            text = text.Trim();
            if (text.Length == 0) {
                return SpecFailure.AllWhitespace(nameof(text));
            }

            if (_canonicalNames.TryGetValue(text, out string canonical)) {
                text = canonical;
            } else {
                _canonicalNames.Add(text, text);
            }
            result = new TestTagType(text, false);
            return null;
        }

        private static bool StaticEquals(TestTagType x, TestTagType y) {
            return StringComparer.OrdinalIgnoreCase.Equals(x.Name, y.Name);
        }

        public bool Equals(TestTagType other) {
            return StaticEquals(this, other);
        }

        public int CompareTo(TestTagType other) {
            return StringComparer.OrdinalIgnoreCase.Compare(Name, other.Name);
        }

        public override string ToString() {
            return Name;
        }

        public static bool operator ==(TestTagType x, TestTagType y) {
            return StaticEquals(x, y);
        }

        public static bool operator !=(TestTagType x, TestTagType y) {
            return !StaticEquals(x, y);
        }

        public static implicit operator TestTagType(string text) {
            return new TestTagType(text);
        }

        public override int GetHashCode() {
            return -1521134295 + Name.GetHashCode();
        }

        public override bool Equals(object obj) {
            return obj is TestTagType y && Equals(y);
        }
    }
}
