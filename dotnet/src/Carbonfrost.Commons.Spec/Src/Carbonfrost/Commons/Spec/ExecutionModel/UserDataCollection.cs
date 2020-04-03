//
// Copyright 2018, 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Reflection;
using System.Linq;
using System;

using Carbonfrost.Commons.Spec.TestMatchers;
using Carbonfrost.Commons.Spec.Resources;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    public class UserDataCollection : IDictionary<string, string> {

        private readonly IDictionary<string, string> Dictionary;

        public string this[string key] {
            get {
                return Dictionary.GetValueOrDefault(key);
            }
            set {
                Dictionary[key] = value;
            }
        }

        public int Count {
            get {
                return Dictionary.Count;
            }
        }

        public bool IsReadOnly {
            get {
                return false;
            }
        }

        public void Clear() {
            Dictionary.Clear();
        }

        public bool Remove(string item) {
            return Dictionary.Remove(item);
        }

        public bool Contains(string item) {
            return Dictionary.ContainsKey(item);
        }

        public void CopyTo(string[] array, int arrayIndex) {
            Dictionary.Values.CopyTo(array, arrayIndex);
        }

        IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public ICollection<string> Keys {
            get {
                return Dictionary.Keys;
            }
        }

        public ICollection<string> Values {
            get {
                return Dictionary.Values;
            }
        }

        internal bool ExpectedConsumedInMessage {
            get;
            set;
        }

        internal Patch Diff {
            get;
            set;
        }

        public bool ContainsKey(string key) {
            return Dictionary.ContainsKey(key);
        }

        public void Add(string key, string value) {
            Dictionary.Add(key, value);
        }

        public bool TryGetValue(string key, out string value) {
            return Dictionary.TryGetValue(key, out value);
        }

        void ICollection<KeyValuePair<string, string>>.Add(KeyValuePair<string, string> item) {
            Dictionary.Add(item);
        }

        bool ICollection<KeyValuePair<string, string>>.Contains(KeyValuePair<string, string> item) {
            return Dictionary.Contains(item);
        }

        void ICollection<KeyValuePair<string, string>>.CopyTo(KeyValuePair<string, string>[] array, int arrayIndex) {
            Dictionary.CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<string, string>>.Remove(KeyValuePair<string, string> item) {
            throw new NotSupportedException();
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator() {
            return Dictionary.GetEnumerator();
        }

        internal UserDataCollection(object matcher) : this() {
            ExtractUserData(matcher);
        }

        public UserDataCollection() {
            Dictionary = new SortedDictionary<string, string>(new SortOrder());
        }

        private void ExtractUserData(object matcher) {
            var props = matcher.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(t => t.CanRead && t.GetIndexParameters().Length == 0);

            foreach (var pi in props) {
                var name = pi.Name;
                string value = null;
                object val = pi.GetValue(matcher);

                var attr = (MatcherUserDataAttribute) pi.GetCustomAttribute(typeof(MatcherUserDataAttribute));
                if (attr != null) {
                    if (attr.Hidden) {
                        continue;
                    }
                }

                if (val is StringComparer) {
                    value = GetStringComparerText(val);
                } else {
                    value = TextUtility.ConvertToString(val);
                }
                Dictionary[name] = value;
            }
        }

        internal bool IsHiddenFromTable(string key) {
            if (key == "Comparer") {
                return this.GetValueOrDefault(key) == "<default>";
            }

            if (key == "Subject") {
                return this.GetValueOrDefault(key) == "<null>";
            }

            if (key == "Expected" && ExpectedConsumedInMessage) {
                return true;
            }

            return false;
        }

        static string GetStringComparerText(object comparison) {
            if (comparison == null) {
                return "<null>";
            }

            var str = comparison.ToString();

            if (comparison == StringComparer.OrdinalIgnoreCase) {
                str = "ordinal (ignore case)";

            } else if (comparison == StringComparer.Ordinal) {
                str = "ordinal";
            }

            else if (comparison == StringComparer.InvariantCulture) {
                str = "invariant culture";
            }
            else if (comparison == StringComparer.InvariantCultureIgnoreCase) {
                str = "invariant culture (ignore case)";
            }

            return str;
        }

        class SortOrder : IComparer<string> {

            public int Compare(string x, string y) {
                // Expected, Actual, then everything else alphabetically
                return string.Compare(CheckNames(x),
                                      CheckNames(y),
                                      StringComparison.OrdinalIgnoreCase);
            }

            static string CheckNames(string x) {
                if (string.Equals(x, SR.LabelActual()) || string.Equals(x, SR.LabelExpected())) {
                    return " " + x;
                }
                return x;
            }
        }
    }
}
