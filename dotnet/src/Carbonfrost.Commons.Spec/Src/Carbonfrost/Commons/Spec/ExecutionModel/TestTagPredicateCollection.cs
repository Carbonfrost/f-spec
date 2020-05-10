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

using System.Collections.ObjectModel;
using System.Linq;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    public class TestTagPredicateCollection : Collection<TestTagPredicate>, ITestPlanFilter {

        public TestTagPredicateCollection() : base(new MakeReadOnlyList<TestTagPredicate>()) {
        }

        public void AddNew(string text) {
            Add(TestTagPredicate.Parse(text));
        }

        internal void CopyFrom(TestTagPredicateCollection other) {
            Items.AddAll(other.Items);
        }

        internal void MakeReadOnly() {
            ((MakeReadOnlyList<TestTagPredicate>) Items).MakeReadOnly();
        }

        bool ITestPlanFilter.IsMatch(TestUnit unit) {
            return Items.Any(tag => tag.IsMatch(unit));
        }
    }

}
