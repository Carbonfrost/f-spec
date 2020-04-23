#if SELF_TEST

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
using Carbonfrost.Commons.Spec;
using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.SelfTest.Spec.ExecutionModel {

    public class FakeTestUnit : TestUnit {

        private readonly string _displayName;

        public FakeTestUnit(string displayName) {
            _displayName = displayName;
        }


        public override string DisplayName {
            get {
                return _displayName;
            }
        }

        public override TestUnitType Type {
            get {
                return TestUnitType.Fact;
            }
        }

        public override TestUnitCollection Children {
            get;
        }

        internal override TestUnitMetadata Metadata {
            get;
        }
    }
}
#endif
