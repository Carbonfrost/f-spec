//
// Copyright 2016, 2017 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

    public class TestClassStartedEventArgs : EventArgs {

        private readonly TestUnitStartedEventArgs _inner;

        public TypeInfo Class {
            get {
                return TestClass.TestType.GetTypeInfo();
            }
        }

        public object TestSubject {
            get {
                var s = TestClass as TestClassSubjectBinding;
                if (s == null) {
                    return null;
                }
                return s.TestSubject;
            }
        }

        internal ReflectedTestClass TestClass {
            get {
                return (ReflectedTestClass) _inner.TestUnit;
            }
        }
        internal TestClassStartedEventArgs(TestUnitStartedEventArgs inner) {
            _inner = inner;
        }
    }


}
