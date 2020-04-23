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

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    public class TestEnvironment : ITestTagProvider {

        private TestTagCollection _tagsCache;

        public string Platform {
            get {
                switch (Environment.OSVersion.Platform) {
                    case PlatformID.MacOSX:
                        return "darwin";
                    case PlatformID.Unix:
                        return "unix";
                    default:
                        return "windows";
                }
            }
        }

        public TestTagCollection Tags {
            get {
                if (_tagsCache == null) {
                    _tagsCache = new TestTagCollection();
                    _tagsCache.Add(TestTagType.Platform, Platform);
                }
                return _tagsCache;
            }
        }

        IEnumerable<TestTag> ITestTagProvider.GetTags(TestContext context) {
            return Tags;
        }
    }
}
