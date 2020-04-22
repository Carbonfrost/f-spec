//
// Copyright 2016, 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

    public class TestNamespaceStartingEventArgs : EventArgs, ITestUnitStartingEventArgs {

        private readonly TestUnitStartingEventArgs _inner;

        public bool Cancel {
            get {
                return _inner.Cancel;
            }
            set {
                _inner.Cancel = value;
            }
        }

        public string Reason {
            get {
                return _inner.Reason;
            }
            set {
                _inner.Reason = value;
            }
        }

        internal TestNamespace TestNamespace {
            get {
                return (TestNamespace) _inner.TestUnit;
            }
        }

        public string Namespace {
            get {
                return TestNamespace.Namespace;
            }
        }

        public Assembly Assembly {
            get {
                return TestNamespace.Assembly;
            }
        }

        internal TestNamespaceStartingEventArgs(TestUnitStartingEventArgs inner) {
            _inner = inner;
        }
    }
}
