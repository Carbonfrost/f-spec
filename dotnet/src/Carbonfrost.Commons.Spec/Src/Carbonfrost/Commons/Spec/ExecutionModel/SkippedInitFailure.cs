//
// Copyright 2017, 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Collections.Generic;
using System.Reflection;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    class SkippedInitFailure : ReflectedTestCase {

        private readonly Exception _err;

        public override TestUnitType Type {
            get {
                return TestUnitType.Case;
            }
        }

        public override string Name {
            get {
                return TestMethod.Name;
            }
        }

        public SkippedInitFailure(MethodInfo mi, Exception err, string reason = null) : base(mi) {
            _err = err;
            ForcePredeterminedStatus(TestUnitFlags.Failed, reason);
        }

        protected override object CoreRunTest(TestExecutionContext context) {
            return null;
        }

        public override int Position {
            get {
                return 0;
            }
        }

        public override IReadOnlyList<object> TestMethodArguments {
            get {
                return Array.Empty<object>();
            }
        }

        public static TestCaseInfo CreateTestUnitFactoryProblem(MethodInfo mi, Exception err) {
            return new SkippedInitFailure(mi, err, string.Format("Problem creating test ({0}: {1})", err.GetType().Name, err.Message));
        }

        public static TestCaseInfo DataProviderProblem(TestUnit item,
            string dataProvider,
            MethodInfo mi,
            Exception err
        ) {
            string reason = string.Format("Problem with data provider {0}", dataProvider);
            return new SkippedInitFailure(mi, err, reason);
        }

        internal override object CreateTestObject() {
            return null;
        }
    }
}
