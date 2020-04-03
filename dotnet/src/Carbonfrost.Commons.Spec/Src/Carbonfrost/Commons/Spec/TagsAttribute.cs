//
// Copyright 2016, 2017, 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.Commons.Spec {

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Assembly, AllowMultiple = false)]
    public sealed class TagsAttribute : Attribute, ITestTagProvider {

        private readonly List<string> _tags;

        public IReadOnlyList<string> Tags {
            get {
                return _tags;
            }
        }

        public TagsAttribute(params string[] tags) {
            if (tags == null) {
                throw new ArgumentNullException(nameof(tags));
            }
            if (tags.Length == 0) {
                throw SpecFailure.EmptyCollection(nameof(tags));
            }
            foreach (var t in tags) {
                TagAttribute.RequireValidTag(t);
            }
            _tags = new List<string>(tags);
        }

        public TagsAttribute(string tag) : this(new [] { tag }) {
        }

        public TagsAttribute(string tag1, string tag2) : this(new [] { tag1, tag2 }) {
        }

        public TagsAttribute(string tag1, string tag2, string tag3) : this(new [] { tag1, tag2, tag3 }) {
        }

        IEnumerable<string> ITestTagProvider.GetTags(TestContext context) {
            return Tags;
        }
    }
}
