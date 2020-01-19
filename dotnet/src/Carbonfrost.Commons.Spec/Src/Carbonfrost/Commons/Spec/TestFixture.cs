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

namespace Carbonfrost.Commons.Spec{

    public class TestFixture {

        private readonly TestFixtureDataCollection _items = new TestFixtureDataCollection();

        public TestFixtureDataCollection Items {
            get {
                return _items;
            }
        }

        public static TestFixture Parse(string text) {
            var items = new FixtureParser(null).Parse(text);
            if (items == null) {
                throw SpecFailure.NotParsable("text", typeof(TestFixture));
            }
            var result = new TestFixture();
            result.Items.AddAll(items);
            return result;
        }

        public static TestFixture FromSource(Uri url) {
            return FromStreamContext(StreamContext.FromSource(url));
        }

        internal static TestFixture FromStreamContext(StreamContext input) {
            if (input == null) {
                throw new ArgumentNullException("input");
            }
            var result = Parse(input.ReadAllText());
            result.CopyBaseUri(input.Uri);
            return result;
        }

        public static TestFixture FromFile(string fileName) {
            return FromStreamContext(StreamContext.FromFile(fileName));
        }

        private void CopyBaseUri(Uri uri) {
            foreach (var item in Items) {
                item.BaseUri = uri;
            }
        }
    }
}
