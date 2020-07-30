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
using System.Linq;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    sealed class BespokeTheoryCase : ReflectedTestCase {

        private readonly Func<TestExecutionContext, object> _func;
        private readonly TestData _data;
        private readonly int _index;
        private readonly TestName _testName;

        public override int Position {
            get {
                return _index;
            }
        }

        public override string Name {
            get {
                return null;
            }
        }

        public override TestUnitType Type {
            get {
                return TestUnitType.Case;
            }
        }

        public override TestName TestName {
            get {
                return _testName;
            }
        }

        public override TestData TestData {
            get {
                return _data;
            }
        }

        internal override object CreateTestObject() {
            return null;
        }

        public BespokeTheoryCase(Func<TestExecutionContext, object> func, TestName baseName, TestDataInfo info) : base(func.Method) {
            _func = func;
            _data = info.TestData;
            _index = info.Index;
            Reason = _data.Reason;
            CopyFlags(_data.Flags);
            _testName = baseName.WithIndex(_index).WithArguments(_data.Select(t => t.ToString()));
        }

        protected override object CoreRunTest(TestExecutionContext context) {
            return _func(context);
        }
    }
}
