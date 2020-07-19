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

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    class BespokeFact : ReflectedTestCase {

        private readonly TestName _baseName;
        private readonly string _name;
        private readonly Func<TestExecutionContext, object> _func;

        public override TestName TestName {
            get {
                return _baseName.WithName(_name);
            }
        }

        public override int Position {
            get {
                return -1;
            }
        }

        public override string Name {
            get {
                return _name;
            }
        }

        public override TestUnitType Type {
            get {
                return TestUnitType.Fact;
            }
        }

        protected override object CoreRunTest(TestExecutionContext context) {
            return _func(context);
        }

        internal override object CreateTestObject() {
            return null;
        }

        public BespokeFact(Func<TestExecutionContext, object> func, TestName baseName, string name) : base(func.Method) {
            _baseName = baseName;
            _func = func;
            _name = name;
        }
    }
}
