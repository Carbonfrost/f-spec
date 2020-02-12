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
using System.Reflection;
using System;
using System.Text.RegularExpressions;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    public struct TestMatcherName {

        internal static readonly Regex NAME = new Regex(@"^(.+)Matcher(`\d+)?$");
        private readonly string _name;
        private readonly bool _negated;

        public string Name {
            get {
                return _name;
            }
        }

        public TestMatcherName(string name) {
            _name = name;
            _negated = false;
        }

        internal TestMatcherName(string name, bool negated) {
            _name = name;
            _negated = negated;
        }

        public override string ToString() {
            return Name + (_negated ? ".not" : "");
        }

        public TestMatcherName Negated() {
            return new TestMatcherName(_name, !_negated);
        }

        public static TestMatcherName FromType(Type type) {
            return new TestMatcherName(Code(type.GetTypeInfo()));
        }

        public static implicit operator TestMatcherName(string name) {
            return new TestMatcherName(name);
        }

        internal static string Code(TypeInfo type) {
            string name = NAME.Replace(type.Name, "$1");
            return "spec." + char.ToLowerInvariant(name[0]) + name.Substring(1);
        }

    }
}
