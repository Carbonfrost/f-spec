//
// Copyright 2016, 2018, 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    public class TestUnitResults : TestUnitResult {

        private readonly TestUnitResultCollection _children;
        private readonly string _displayName;
        private readonly TestUnitType _type;

        public override TestUnitResultCollection Children {
            get {
                return _children;
            }
        }

        public override TestUnitType Type {
            get {
                return _type;
            }
        }

        internal TestUnitResults(TestUnit node) {
            _displayName = node.DisplayName;
            _type = node.Type;
            _children = new TestUnitResultCollection(this);
        }

        public override string DisplayName {
            get {
                return _displayName;
            }
        }

        internal override JTestUnitResult JResult {
            get {
                return new JTestUnitResult {
                    Status = Status,
                    DisplayName = DisplayName,
                    Attributes = Attributes,
                    Type = _type,
                    Ordinal = Ordinal,
                };
            }
        }

        internal override void ApplyCounts(TestUnitCounts counts) {
            foreach (var c in Children) {
                c.ApplyCounts(counts);
            }
        }

    }
}
