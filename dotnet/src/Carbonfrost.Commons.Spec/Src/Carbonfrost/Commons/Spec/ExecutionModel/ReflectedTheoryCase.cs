//
// Copyright 2016, 2018, 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
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
using System.Linq;
using System.Reflection;
using System.Text;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    class ReflectedTheoryCase : ReflectedTestCase {

        private readonly TestData _data;
        private readonly int _index;
        private object[] _args;
        private readonly string _name;

        public ReflectedTheoryCase(MethodInfo method, int index, TestData data) : base(method) {
            _data = data;
            _index = index;
            _name = string.IsNullOrEmpty(data.Name) ? ("#" + _index) : (" @ " + data.Name);
            Reason = data.Reason;
            CopyFlags(data.Flags);
        }

        public override string DisplayName {
            get {
                var sb = new StringBuilder();
                sb.Append(base.DisplayName);
                sb.Append(_name);
                sb.Append(" (");
                sb.Append(TextUtility.FormatArgs(_data));
                sb.Append(")");
                return sb.ToString();
            }
        }

        public override IReadOnlyList<object> TestMethodArguments {
            get {
                return _args;
            }
        }

        public override int Position {
            get {
                return _index;
            }
        }

        protected override void InitializeOverride(TestContext testContext) {
            // If items in the test data have to be evaluated before they can be
            // used, do it now
            _args = _data.Evaluate(testContext).ToArray();
        }

        protected override object CoreRunTest(TestContext context) {
            try {
                return InvokeMethodHelper(context.TestObject, TestMethodArguments.ToArray());
            }
            catch (TargetParameterCountException) {
                throw SpecFailure.WrongNumberOfTheoryArguments(TypeName, MethodName, _index);
            }
        }
    }
}
