//
// Copyright 2018, 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.Commons.Spec {

    public class TestOptions {

        private bool _passExplicitly;
        private bool _seal;
        private TimeSpan? _timeout;
        private string _reason;
        private readonly MakeReadOnlyList<ITestCaseFilter> _filters = new MakeReadOnlyList<ITestCaseFilter>();
        private string _name;

        public static readonly TestOptions Empty = ReadOnly(new TestOptions());

        public string Name {
            get {
                return _name;
            }
            set {
                WritePreamble();
                _name = value;
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

        public string Reason {
            get {
                return _reason;
            }
            set {
                WritePreamble();
                _reason = value;
            }
        }

        internal IList<ITestCaseFilter> Filters {
            get {
                return _filters;
            }
        }

        public TestOptions() {
        }

        public TestOptions(TestOptions copyFrom) {
            if (copyFrom != null) {
                PassExplicitly = copyFrom.PassExplicitly;
                Name = copyFrom.Name;
                Reason = copyFrom.Reason;
                Timeout = copyFrom.Timeout;
                Filters.AddAll(copyFrom.Filters);
            }
        }

        private static TestOptions ReadOnly(TestOptions options) {
            options.MakeReadOnly();
            return options;
        }

        internal void MakeReadOnly() {
            _seal = true;
            _filters.MakeReadOnly();
        }

        internal static TestOptions Named(string name) {
            return new TestOptions {
                Name = name
            };
        }

        public TestOptions Clone() {
            return new TestOptions(this);
        }

        private void WritePreamble() {
            if (_seal) {
                throw SpecFailure.Sealed();
            }
        }
    }

    partial class Extensions {

        internal static TestOptions SafeWithName(this TestOptions opts, string name) {
            if (opts == null || ReferenceEquals(opts, TestOptions.Empty)) {
                return TestOptions.Named(name);
            };
            var c = opts.Clone();
            c.Name = name;
            return c;
        }
    }
}
