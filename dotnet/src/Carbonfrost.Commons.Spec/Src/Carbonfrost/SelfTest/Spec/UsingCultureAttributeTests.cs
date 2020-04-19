#if SELF_TEST

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
using System.Threading;

using Carbonfrost.Commons.Spec;

namespace Carbonfrost.SelfTest.Spec {

    public class UsingCultureAttributeTests {

        [UsingCulture("fr-FR")]
        [Fact]
        public void UsingCultureAttribute_should_set_up_culture() {
            Assert.Equal("fr-FR", Thread.CurrentThread.CurrentCulture.Name);
        }
    }

    [UsingCulture("fr-FR")]
    public class UsingCultureAttributeClassLevelTests {

        [Fact]
        public void UsingCultureAttribute_should_set_up_culture_when_used_on_class() {
            Assert.Equal("fr-FR", Thread.CurrentThread.CurrentCulture.Name);
        }
    }
}
#endif
