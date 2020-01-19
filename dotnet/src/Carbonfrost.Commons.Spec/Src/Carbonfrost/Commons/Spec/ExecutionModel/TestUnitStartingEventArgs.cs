//
// Copyright 2018 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Linq;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    public class TestUnitStartingEventArgs : EventArgs {

        private readonly TestUnit _unit;

        public bool Cancel {
            get;
            set;
        }

        public TestUnit TestUnit {
            get {
                return _unit;
            }
        }

        internal TestUnitStartingEventArgs(TestUnit unit) {
            _unit = unit;
        }
    }
}
