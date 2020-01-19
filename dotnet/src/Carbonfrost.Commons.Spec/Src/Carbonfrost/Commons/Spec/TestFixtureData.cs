//
// Copyright 2016, 2018 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

using System.IO;
using System.Linq;

namespace Carbonfrost.Commons.Spec {

    public class TestFixtureData : IEnumerable<KeyValuePair<string, string>> {

        private readonly Dictionary<string, string> _values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        internal Uri BaseUri { get; set; }

        public string this[string name] {
            get {
                return Values.GetValueOrDefault(name);
            }
        }

        public IDictionary<string, string> Values {
            get {
                return _values;
            }
        }


        public int GetInt32(string name) {
            return Int32.Parse(Values[name]);
        }

        public long GetInt64(string name) {
            return Int64.Parse(Values[name]);
        }

        public short GetInt16(string name) {
            return Int16.Parse(Values[name]);
        }

        public bool GetBoolean(string name) {
            return Boolean.Parse(Values[name]);
        }

        public string GetString(string name) {
            return Values[name];
        }

        public double GetDouble(string name) {
            return Double.Parse(Values[name]);
        }

        public float GetSingle(string name) {
            return Single.Parse(Values[name]);
        }

        public IStreamContext GetStreamContext(string name) {
            return new FixtureWrapper(BaseUri, name, StreamContext.FromText(Values[name]));
        }

        public static TestFixtureData Parse(string text) {
            var items = new FixtureParser(null).Parse(text);
            if (items == null) {
                throw SpecFailure.NotParsable("text", typeof(TestFixtureData));
            }
            return items.Single();
        }

        public static TestFixtureData FromSource(Uri url) {
            return FromStreamContext(StreamContext.FromSource(url));
        }

        public static TestFixtureData FromStreamContext(IStreamContext input) {
            if (input == null) {
                throw new ArgumentNullException("input");
            }
            var result = Parse(input.ReadAllText());
            result.BaseUri = input.Uri;
            return result;
        }

        public static TestFixtureData FromFile(string fileName) {
            return FromStreamContext(StreamContext.FromFile(fileName));
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator() {
            return Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        private class FixtureWrapper : StreamContext {

            private readonly Uri _uri;
            private readonly StreamContext _inner;

            public FixtureWrapper(Uri baseUri, string fileName, StreamContext inner) {
                _inner = inner;
                _uri = new Uri(baseUri + "#" + fileName, UriKind.RelativeOrAbsolute);
            }

            public override Uri Uri {
                get {
                    return this._uri;
                }
            }

            public override StreamContext ChangePath(string relativePath) {
                throw new NotImplementedException();
            }

            public override Stream Open() {
                return _inner.Open();
            }

            public override Stream OpenRead() {
                return _inner.OpenRead();
            }
        }
    }
}
