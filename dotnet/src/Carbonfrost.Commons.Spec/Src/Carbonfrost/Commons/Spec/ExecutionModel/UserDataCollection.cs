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

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    public class UserDataCollection : IDictionary<string, string> {

        private readonly IDictionary<string, string> _dictionary;
        private readonly IDictionary<string, IDisplayActual> _actuals;

        public string this[string key] {
            get {
                return _dictionary.GetValueOrDefault(key);
            }
            set {
                _dictionary[key] = value;
            }
        }

        public int Count {
            get {
                return _dictionary.Count;
            }
        }

        public bool IsReadOnly {
            get {
                return false;
            }
        }

        private bool ActualsOnlyTypeDifferences {
            get {
                return _actuals.TryGetValue("Actual", out IDisplayActual ac)
                    && _actuals.TryGetValue("Expected", out IDisplayActual ex)
                    && DisplayActual.OnlyTypeDifferences(ex, ac);
            }
        }

        public void Clear() {
            _dictionary.Clear();
            _actuals.Clear();
        }

        public bool Remove(string item) {
            _actuals.Remove(item);
            return _dictionary.Remove(item);
        }

        public bool Contains(string item) {
            return _dictionary.ContainsKey(item);
        }

        public void CopyTo(string[] array, int arrayIndex) {
            _dictionary.Values.CopyTo(array, arrayIndex);
        }

        IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        internal void CopyActuals(UserDataCollection from) {
            foreach (var key in new [] { "Actual" }) {
                _dictionary[key] = from._dictionary[key];
                _actuals[key] = from._actuals[key];
            }
        }

        public ICollection<string> Keys {
            get {
                return _dictionary.Keys;
            }
        }

        public ICollection<string> Values {
            get {
                return _dictionary.Values;
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
            return _dictionary.ContainsKey(key);
        }

        public void Add(string key, string value) {
            CoreAdd(key, value);
        }

        public void Add(string key, object value) {
            CoreAdd(key, value);
        }

        private void CoreAdd(string key, object value) {
            var actual = DisplayActual.Create(value);
            _dictionary.Add(key, actual.Format(DisplayActualOptions.None));

            if (IsActualKey(key)) {
                _actuals.Add(key, actual);
            }
        }

        public bool TryGetValue(string key, out string value) {
            return _dictionary.TryGetValue(key, out value);
        }

        void ICollection<KeyValuePair<string, string>>.Add(KeyValuePair<string, string> item) {
            Add(item.Key, item.Value);
        }

        bool ICollection<KeyValuePair<string, string>>.Contains(KeyValuePair<string, string> item) {
            return _dictionary.Contains(item);
        }

        void ICollection<KeyValuePair<string, string>>.CopyTo(KeyValuePair<string, string>[] array, int arrayIndex) {
            _dictionary.CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<string, string>>.Remove(KeyValuePair<string, string> item) {
            throw new NotSupportedException();
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator() {
            return _dictionary.GetEnumerator();
        }

        internal UserDataCollection(object matcher) : this() {
            ExtractUserData(matcher);
        }

        public UserDataCollection() {
            _dictionary = new SortedDictionary<string, string>(new SortOrder());
            _actuals = new Dictionary<string, IDisplayActual>();
        }

        private void ExtractUserData(object matcher) {
            var props = matcher.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(t => t.CanRead && t.GetIndexParameters().Length == 0);

            foreach (var pi in props) {
                var name = pi.Name;
                object val = pi.GetValue(matcher);

                var attr = (MatcherUserDataAttribute) pi.GetCustomAttribute(typeof(MatcherUserDataAttribute));
                if (attr != null) {
                    if (attr.Hidden) {
                        continue;
                    }
                }
                Add(name, val);
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
            if (key[0] == '_') {
                return true;
            }

            return false;
        }

        internal string FormatValue(string key, bool showWhitespace) {
            if (_actuals.TryGetValue(key, out IDisplayActual actual)) {
                var options = showWhitespace ? DisplayActualOptions.ShowWhitespace : DisplayActualOptions.None;
                if (ActualsOnlyTypeDifferences) {
                    options |= DisplayActualOptions.ShowType;
                }
                return actual.Format(options);
            }

            string text = this[key];
            if (showWhitespace) {
                return TextUtility.ShowWhitespace(text);
            }
            return text;
        }

        private static bool IsActualKey(string key) {
            return Array.IndexOf(ActualOrder, key) >= 0;
        }

        static readonly string[] ActualOrder = {
            "Subject",
            "Given",
            "Property",
            "Actual",
            "Expected",
        };

        class SortOrder : IComparer<string> {

            public int Compare(string x, string y) {
                // Subject, Given, then Expected, Actual, then everything else alphabetically
                return string.Compare(CheckNames(x),
                                      CheckNames(y),
                                      StringComparison.OrdinalIgnoreCase);
            }

            static string CheckNames(string x) {
                if (IsActualKey(x)) {
                    return " " + Array.IndexOf(ActualOrder, x);
                }
                return x;
            }
        }
    }
}
