//
// Copyright 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using System.Threading;
using System.Globalization;

namespace Carbonfrost.Commons.Spec.ExecutionModel.TestFilters {

    class UsingCultureFilter : TestFilter {

        private CultureInfo _resetCulture;
        public CultureInfo Culture { get; private set; }

        public UsingCultureFilter(CultureInfo culture) {
            Culture = culture;
        }

        public override void BeforeExecuting(TestContext testContext) {
            _resetCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = Culture;
        }

        public override void AfterExecuting(TestContext testContext) {
            Thread.CurrentThread.CurrentCulture = _resetCulture;
        }
    }
}