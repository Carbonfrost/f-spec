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
using System.Collections.Generic;
using System.Linq;

using Carbonfrost.Commons.Spec.ExecutionModel;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.Commons.Spec {

    public class TestOptions {

        private bool _passExplicitly;
        private bool _seal;
        private TimeSpan? _timeout;
        private readonly MakeReadOnlyList<ITestCaseFilter> _filters = new MakeReadOnlyList<ITestCaseFilter>();
        private string _displayName;

        public string DisplayName {
            get {
                return _displayName;
            }
            set {
                WritePreamble();
                _displayName = value;
            }
        }

        public bool PassExplicitly {
            get {
                return _passExplicitly;
            }
            set {
                WritePreamble();
                _passExplicitly = value;
            }
        }

        public TimeSpan? Timeout {
            get {
                return _timeout;
            }
            set {
                WritePreamble();
                _timeout = value;
            }
        }

        internal IList<ITestCaseFilter> Filters {
            get {
                return _filters;
            }
        }

        internal void MakeReadOnly() {
            _seal = true;
            _filters.MakeReadOnly();
        }

        private void WritePreamble() {
            if (_seal) {
                throw SpecFailure.Sealed();
            }
        }
    }
}
