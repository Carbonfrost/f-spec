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

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Carbonfrost.Commons.Spec.ExecutionModel.TestFilters;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    public abstract class TestFilter : ITestExecutionFilter {

        public static readonly TestFilter Null = new NullImpl();

        public virtual void BeforeExecuting(TestExecutionContext testContext) {
        }

        public virtual void AfterExecuting(TestExecutionContext testContext) {
        }

        public static TestFilter UsingCulture(CultureInfo cultureInfo) {
            if (cultureInfo == null) {
                throw new ArgumentNullException(nameof(cultureInfo));
            }

            return new UsingCultureFilter(cultureInfo);
        }

        public static TestFilter Compose(params TestFilter[] items) {
            return Compose((IEnumerable<TestFilter>) items);
        }

        public static TestFilter Compose(IEnumerable<TestFilter> items) {
            if (items == null) {
                return Null;
            }
            var all = items.Where(i => i != null && !(i is NullImpl)).ToArray();
            if (all.Length == 1) {
                return all[0];
            }
            return new CompositeTestFilter(all);
        }

        internal static TestFilter Upgrade(ITestExecutionFilter filter) {
            if (filter is TestFilter f) {
                return f;
            }

            return new ExecutionFilterWrapper(filter);
        }

        private class NullImpl : TestFilter {}

        private class ExecutionFilterWrapper : TestFilter {
            private readonly ITestExecutionFilter _filter;

            public ExecutionFilterWrapper(ITestExecutionFilter filter) {
                _filter = filter;
            }

            public override void BeforeExecuting(TestExecutionContext testContext) {
                _filter.BeforeExecuting(testContext);
            }

            public override void AfterExecuting(TestExecutionContext testContext) {
                _filter.AfterExecuting(testContext);
            }
        }

        private class CompositeTestFilter : TestFilter {
            private readonly TestFilter[] _all;

            public CompositeTestFilter(TestFilter[] all) {
                _all = all;
            }

            public override void BeforeExecuting(TestExecutionContext testContext) {
                foreach (var f in _all) {
                    f.BeforeExecuting(testContext);
                }
            }

            public override void AfterExecuting(TestExecutionContext testContext) {
                foreach (var f in _all) {
                    f.AfterExecuting(testContext);
                }
            }
        }
    }
}
