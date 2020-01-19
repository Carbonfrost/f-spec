//
// Copyright 2017 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Linq;
using System.Reflection;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    class SkippedInitFailure : TestCase {

        private readonly Exception _err;
        private string _reason;

        public SkippedInitFailure(MethodInfo mi, Exception err) : base(mi) {
            _err = err;
            _reason = string.Format("Problem setting up test ({0}: {1})", err.GetType().Name, err.Message);
        }

        protected override TestCaseResult RunTestCore(TestContext testContext) {
            var result = new TestCaseResult(this);
            result.Reason = _reason;
            result.SetFailed(_err);
            return result;
        }

        public override int Position {
            get {
                return 0;
            }
        }

        public override IReadOnlyList<object> TestMethodArguments {
            get {
                return Empty<object>.Array;
            }
        }

        public static TestCase DataProviderProblem(TestUnit item,
                                                   string dataProvider,
                                                   MethodInfo mi,
                                                   Exception err) {
            string reason = string.Format("Problem with data provider {0}", dataProvider);
            return new SkippedInitFailure(mi, err) {
                _reason = reason,
            };
        }
    }
}
