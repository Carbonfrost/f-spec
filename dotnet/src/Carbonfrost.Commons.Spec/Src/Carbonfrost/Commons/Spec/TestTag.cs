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
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Carbonfrost.Commons.Spec {

    public struct TestTag : IEquatable<TestTag> {

        static readonly Regex VALID_NAME = new Regex(@"\A[a-z0-9_.-]+\Z", RegexOptions.IgnoreCase);

        static readonly IDictionary<TestTag, TestTag> _aliases = new Dictionary<TestTag, TestTag>();

        public static readonly TestTag WindowsPlatform = Platform("windows");
        public static readonly TestTag macOSPlatform = Platform("macos");

        private static readonly TestTag OSXPlatform = Alias(Platform("osx"), macOSPlatform);
        public static readonly TestTag Slow = "slow";

        private readonly TestTagType _type;
        private readonly string _value;

        public TestTagType Type {
            get {
                return _type;
            }
        }

        public string Value {
            get {
                return _value;
            }
        }

        public TestTag? AliasTo {
            get {
                TestTag result;
                if (_aliases.TryGetValue(this, out result)) {
                    return result;
                }
                return null;
            }
        }

        public bool IsAlias {
            get {
                return _aliases.ContainsKey(this);
            }
        }

        public TestTag(TestTagType name) : this(name, null) {}

        public TestTag(TestTagType type, string value) {
            _type = RequireType(type);
            _value = string.IsNullOrEmpty(value) ? null : value;
        }

        public override string ToString() {
            if (string.IsNullOrEmpty(Value)) {
                return Type.ToString();
            }
            return string.Concat(Type, ":", Value);
        }

        public override bool Equals(object obj) {
            return obj is TestTag tt && Equals(tt);
        }

        public bool Equals(TestTag other) {
            return other.Type == Type && other.Value == Value;
        }

        public static TestTag Platform(string value) {
            return new TestTag(TestTagType.Platform, value);
        }

        public static TestTag Previously(TestStatus status) {
            return new TestTag(TestTagType.Previously, status.ToString());
        }

        public static TestTag Parse(string text) {
            var ex = _TryParse(text, out TestTag result);
            if (ex != null) {
                throw ex;
            }
            return result;
        }

        public static bool TryParse(string text, out TestTag result) {
            return _TryParse(text, out result) == null;
        }

        private static Exception _TryParse(string text, out TestTag result) {
            if (text == null) {
                result = new TestTag();
                return new ArgumentNullException();
            }

            text = text ?? string.Empty;
            if (text.Length == 0) {
                result = new TestTag();
                return SpecFailure.AllWhitespace(nameof(text));
            }
            string[] nv = Array.ConvertAll(
                text.Split(new [] { ':' }, 3), t => t.Trim()
            );
            if (nv.Length == 1 && ValidName(nv[0])) {
                result = new TestTag(nv[0].Trim());
                return null;
            }
            if (nv.Length == 2 && ValidName(nv[0]) && ValidName(nv[1])) {
                result = new TestTag(nv[0].Trim(), nv[1].Trim());
                return null;
            }

            result = new TestTag();
            return new FormatException();
        }

        internal static bool ValidName(string name) {
            return VALID_NAME.IsMatch(name);
        }

        internal static TestTagType RequireType(TestTagType type) {
            if (type == TestTagType.Unspecified) {
                throw new ArgumentException();
            }

            return type;
        }

        public TestTag Resolve() {
            TestTag result;
            if (_aliases.TryGetValue(this, out result)) {
                return result;
            }
            return this;
        }

        public static TestTag Alias(TestTag from, TestTag to) {
            if (from.Type != to.Type) {
                throw SpecFailure.CannotAliasDifferentTagTypes();
            }
            if (from != to) {
                _aliases[from] = to;
            }
            return from;
        }

        public static bool AreEquivalent(TestTag x, TestTag y) {
            return x.IsEquivalentTo(y);
        }

        public bool IsEquivalentTo(TestTag other) {
            return other.Equals(this) || (
                other.IsAlias && other.AliasTo.Equals(this)
            ) || (
                IsAlias && AliasTo.Equals(other)
            );
        }

        public static implicit operator TestTag(string value) {
            return Parse(value);
        }

        public static bool operator ==(TestTag x, TestTag y) {
            return x.Equals(y);
        }

        public static bool operator !=(TestTag x, TestTag y) {
            return !x.Equals(y);
        }

        public override int GetHashCode() {
            int hashCode = 1761208509;
            hashCode = hashCode * -1521134295 + Type.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Value);
            return hashCode;
        }
    }
}
