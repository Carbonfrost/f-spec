//
// Copyright 2016, 2017, 2018, 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Collections;
using System.Collections.Generic;

namespace Carbonfrost.Commons.Spec {

    class TestTagCache : ITestTagApiConventions, IEnumerable<TestTag> {

        private TestTag[] _cache;
        private string[] _tags;
        private string _tag;

        public string[] Tags {
            get {
                return _tags ?? Array.Empty<string>();
            }
            set {
                _tags = value;
                ClearCache();
            }
        }

        public string Tag {
            get {
                return _tag;
            }
            set {
                _tag = value;
                ClearCache();
            }
        }

        public TestTag[] TestTags {
            get {
                return EnsureCache();
            }
        }

        public IEnumerator<TestTag> GetEnumerator() {
            return ((IEnumerable<TestTag>) TestTags).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        private TestTag[] EnsureCache() {
            if (_cache == null) {
                int count = Tags.Length + (string.IsNullOrEmpty(Tag) ? 0 : 1);
                string[] tags;
                if (string.IsNullOrEmpty(Tag)) {
                    tags = Tags;
                } else {
                    tags = new string[Tags.Length + 1];
                    tags[0] = Tag;
                    Array.Copy(Tags, 0, tags, 1, Tags.Length);
                }
                _cache = Array.ConvertAll(tags, TestTag.Parse);
            }
            return _cache;
        }

        private void ClearCache() {
            _cache = null;
        }

    }

}
