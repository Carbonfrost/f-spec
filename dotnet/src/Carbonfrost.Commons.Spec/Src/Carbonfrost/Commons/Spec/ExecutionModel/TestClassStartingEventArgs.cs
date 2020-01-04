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
using System.Reflection;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    public class TestClassStartingEventArgs : EventArgs {

        private readonly TestUnitStartingEventArgs _inner;

        public bool Cancel {
            get {
                return _inner.Cancel;
            }
            set {
                _inner.Cancel = value;
            }
        }

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

        internal TestClassStartingEventArgs(TestUnitStartingEventArgs inner) {
            _inner = inner;
        }
    }
}
