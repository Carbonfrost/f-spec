//
// Copyright 2016, 2017, 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Reflection;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    public class TestClassFinishedEventArgs : EventArgs {

        private readonly TestUnitFinishedEventArgs _inner;

        public TypeInfo Class {
            get {
                return TestClass.TestClass.GetTypeInfo();
            }
        }

        public TestClassInfo TestClass {
            get {
                return (TestClassInfo) _inner.TestUnit;
            }
        }

        public TestUnitResult Results {
            get {
                return (TestUnitResult) _inner.Result;
            }
        }

        internal TestClassFinishedEventArgs(TestUnitFinishedEventArgs inner) {
            _inner = inner;
        }
    }
}
