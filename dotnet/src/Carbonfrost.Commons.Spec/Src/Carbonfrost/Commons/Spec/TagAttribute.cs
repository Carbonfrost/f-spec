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
    public class TagAttribute : Attribute, ITestTagProvider {

        public TestTag Tag {
            get;
            private set;
        }

        public TagAttribute(string tag) {
            Tag = TestTag.Parse(tag);
        }

        IEnumerable<TestTag> ITestTagProvider.GetTags(TestContext context) {
            return new [] { Tag };
        }
    }
}
