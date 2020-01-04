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

    public class TestNamespaceStartedEventArgs : EventArgs {

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

        private readonly TestUnitStartedEventArgs _inner;

        internal TestNamespaceStartedEventArgs(TestUnitStartedEventArgs inner) {
            _inner = inner;
        }
    }
}
