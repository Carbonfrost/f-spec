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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Carbonfrost.Commons.Spec {

    public struct TestMemberFilter {

        private readonly Func<TypeInfo, IEnumerable<MemberInfo>> _filter;
        private readonly string _text;

        public string Label {
            get {
                return _text;
            }
        }

        public static TestMemberFilter All {
            get {
                return Compose("all", new [] { Properties, Fields });
            }
        }

        public static TestMemberFilter Public {
            get {
                return Compose("public", new [] { PublicProperties, PublicFields });
            }
        }

        public static TestMemberFilter Properties {
            get {
                return new TestMemberFilter(t => t.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic), "properties");
            }
        }

        public static TestMemberFilter PublicProperties {
            get {
                return new TestMemberFilter(t => t.GetProperties(BindingFlags.Instance | BindingFlags.Public), "public properties");
            }
        }

        public static TestMemberFilter PrivateProperties {
            get {
                return new TestMemberFilter(t => t.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic), "private properties");
            }
        }

        public static TestMemberFilter Fields {
            get {
                return new TestMemberFilter(t => t.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic), "fields");
            }
        }

        public static TestMemberFilter PublicFields {
            get {
                return new TestMemberFilter(t => t.GetFields(BindingFlags.Instance | BindingFlags.Public), "public fields");
            }
        }

        public static TestMemberFilter PrivateFields {
            get {
                return new TestMemberFilter(t => t.GetFields(BindingFlags.Instance | BindingFlags.NonPublic), "private fields");
            }
        }

        internal TestMemberFilter(Func<TypeInfo, IEnumerable<MemberInfo>> p, string text) {
            _filter = p;
            _text = text;
        }

        public IEnumerable<MemberInfo> GetMembers(TypeInfo type) {
            if (type is null) {
                throw new ArgumentNullException(nameof(type));
            }
            return _filter(type).Where(p => !p.IsDefined(typeof(CompilerGeneratedAttribute))).Distinct();
        }

        public static TestMemberFilter Property(string name) {
            return new TestMemberFilter(t => Item(t.GetProperty(name)), name);
        }

        public static TestMemberFilter Field(string name) {
            return new TestMemberFilter(t => Item(t.GetField(name)), name);
        }

        public static TestMemberFilter Compose(IEnumerable<TestMemberFilter> items) {
            return Compose(items.ToArray());
        }

        public static TestMemberFilter Compose(params TestMemberFilter[] items) {
            if (items == null || items.Length == 0) {
                return All;
            }
            if (items.Length == 1) {
                return items[0];
            }
            var label = new StringBuilder();
            var comma = false;
            foreach (var i in items) {
                if (comma) {
                    label.Append(", ");
                }
                label.Append(i.Label);
                comma = true;
            }

            return Compose(label.ToString(), items);
        }

        private static TestMemberFilter Compose(string label, TestMemberFilter[] items) {
            if (items == null || items.Length == 0) {
                return All;
            }
            if (items.Length == 1) {
                return items[0];
            }
            return new TestMemberFilter(
                type => items.SelectMany(i => i.GetMembers(type)),
                label
            );
        }

        public override string ToString() {
            return _text;
        }

        private static MemberInfo[] Item(MemberInfo item) {
            if (item == null) {
                return Array.Empty<MemberInfo>();
            }
            return new [] { item };
        }
    }

    partial class Matchers {

        public static TestMemberFilter All {
            get {
                return TestMemberFilter.All;
            }
        }
        public static TestMemberFilter Public {
            get {
                return TestMemberFilter.Public;
            }
        }

        public static TestMemberFilter Properties {
            get {
                return TestMemberFilter.Properties;
            }
        }

        public static TestMemberFilter PublicProperties {
            get {
                return TestMemberFilter.PublicProperties;
            }
        }

        public static TestMemberFilter PrivateProperties {
            get {
                return TestMemberFilter.PrivateProperties;
            }
        }

        public static TestMemberFilter Fields {
            get {
                return TestMemberFilter.Fields;
            }
        }

        public static TestMemberFilter PublicFields {
            get {
                return TestMemberFilter.PublicFields;
            }
        }

        public static TestMemberFilter PrivateFields {
            get {
                return TestMemberFilter.PrivateFields;
            }
        }

        public static TestMemberFilter Property(string name) {
            return TestMemberFilter.Property(name);
        }

        public static TestMemberFilter Field(string name) {
            return TestMemberFilter.Field(name);
        }
    }

}
