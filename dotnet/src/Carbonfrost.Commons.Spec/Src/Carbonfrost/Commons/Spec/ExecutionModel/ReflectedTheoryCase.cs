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

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    class ReflectedTheoryCase : ReflectedTestCase {

        private readonly TestData _data;
        private readonly int _index;
        private object[] _args;
        private readonly string _dataProviderName;

        public ReflectedTheoryCase(MethodInfo method, TestDataInfo info) : base(method) {
            _data = info.TestData;
            _index = info.Index;
            _dataProviderName = info.ProviderName;
            Reason = _data.Reason;
            CopyFlags(_data.Flags);
        }

        public override TestData TestData {
            get {
                return _data;
            }
        }

        public override TestUnitType Type {
            get {
                return TestUnitType.Case;
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

        public override string DataProviderName {
            get {
                return _dataProviderName;
            }
        }

        public override string Name {
            get {
                return TestData.Name;
            }
        }

        protected override void InitializeOverride(TestContext testContext) {
            // If items in the test data have to be evaluated before they can be
            // used, do it now
            _args = _data.Evaluate(testContext).ToArray();
        }

        protected override object CoreRunTest(TestExecutionContext context) {
            try {
                var args = TestMethodArguments.Select(a => RebindDelegates(context.TestObject, a)).ToArray();

                return InvokeMethodHelper(context.TestObject, args);
            }
            catch (TargetParameterCountException) {
                throw SpecFailure.WrongNumberOfTheoryArguments(TypeName, MethodName, _index);
            }
        }

        private object RebindDelegates(object instance, object input) {
            if (input is Delegate action && ShouldRetarget(instance.GetType(), action)) {
                return Delegate.CreateDelegate(
                    action.GetType(), instance, action.Method, false
                );
            }
            return input;
        }

        private bool ShouldRetarget(Type instanceType, Delegate action) {
            return action.Target != null
                && instanceType.IsInstanceOfType(action.Target)
                && (RetargetAttribute.IsRetargeted(action.GetType()) || DemandRetargetDelegates());
        }

        private bool DemandRetargetDelegates() {
            switch (RetargetDelegates) {
                case RetargetDelegates.Enabled:
                    return true;

                case RetargetDelegates.Disabled:
                    return false;

                case RetargetDelegates.Unspecified:
                default:
                    if (Assert.UseStrictMode) {
                        throw SpecFailure.PossibleDelegateRetargeting();
                    }
                    return true;
            }
        }
    }
}
