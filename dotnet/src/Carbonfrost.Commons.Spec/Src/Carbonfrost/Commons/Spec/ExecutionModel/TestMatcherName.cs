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
using System.Reflection;
using System.Text.RegularExpressions;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    public struct TestMatcherName {

        internal static readonly Regex NAME = new Regex(@"^(.+)Matcher(`\d+)?$");
        private readonly string _name;
        private readonly Flags _flags;

        public TestMatcherName Base {
            get {
                return new TestMatcherName(_name, Flags.None);
            }
        }

        public string Name {
            get {
                string name = "spec." + char.ToLowerInvariant(_name[0]) + _name.Substring(1);
                return name + (IsPredicate ? ".predicate" : "") + (IsNegated ? ".not" : "");
            }
        }

        internal bool IsAndOr {
            get {
                return _name == "And" || _name == "Or";
            }
        }

        internal bool IsInvariant {
            get {
                return _name == "Anything" || _name == "Nothing";
            }
        }

        public bool IsNegated {
            get {
                return _flags.HasFlag(Flags.Negated);
            }
        }

        public bool IsPredicate {
            get {
                return _flags.HasFlag(Flags.Predicate);
            }
        }

        public TestMatcherName(string name) {
            _name = name;
            _flags = Flags.None;
        }

        private TestMatcherName(string name, Flags flags) {
            _name = name;
            _flags = flags;
        }

        public override string ToString() {
            return Name;
        }

        public TestMatcherName Negated() {
            if (IsNegated) {
                return new TestMatcherName(_name, _flags & ~Flags.Negated);
            }
            return new TestMatcherName(_name, _flags | Flags.Negated);
        }

        public TestMatcherName Predicate() {
            return new TestMatcherName(_name, _flags | Flags.Predicate);
        }

        public static TestMatcherName FromType(Type type) {
            string name = NAME.Replace(type.Name, "$1");
            return new TestMatcherName(name);
        }

        public static implicit operator TestMatcherName(string name) {
            return new TestMatcherName(name);
        }

        internal string ToSRKey() {
            var prefix = "Expected";
            if (IsPredicate) {
                prefix = "Predicate";
            }

            var msgCode = prefix + _name;
            if (IsNegated) {
                msgCode = "Not" + msgCode;
            }
            return msgCode;
        }

        public static TestMatcherName For(object matcher) {
            if (matcher is TestMatcher.IInvariantTestMatcher invariant) {
                return invariant.Matches() ? "Anything" : "Nothing";
            }
            if (matcher is ISupportTestMatcher support) {
                return For(support.RealMatcher);
            }
            if (matcher is INotMatcher nt) {
                return For(nt.InnerMatcher).Negated();
            }

            var matcherType = matcher.GetType().GetTypeInfo();
            if (matcherType.IsGenericType) {
                matcherType = matcherType.GetGenericTypeDefinition().GetTypeInfo();
            }

            return TestMatcherName.FromType(matcherType);
        }

        [Flags]
        enum Flags {
            None,
            Negated = 1 << 0,
            Predicate = 1 << 1,
        }

    }
}
