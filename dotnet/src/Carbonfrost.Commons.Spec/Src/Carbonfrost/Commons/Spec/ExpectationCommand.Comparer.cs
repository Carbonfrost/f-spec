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

namespace Carbonfrost.Commons.Spec {

    partial class ExpectationCommand {

        public static ExpectationCommand<T> Comparer<T>(IComparer<T> comparer, ExpectationCommand<T> inner) {
            return new ComparerCommand<T>(comparer, inner);
        }

        class ComparerCommand<T> : ExpectationCommand<T> {

            private readonly IComparer<T> _comparer;
            private readonly ExpectationCommand<T> _inner;

            public ComparerCommand(IComparer<T> comparer, ExpectationCommand<T> inner) {
                _inner = inner;
                _comparer = comparer;
            }

            public override ExpectationCommand<T> Negated() {
                return new ComparerCommand<T>(_comparer, _inner.Negated());
            }

            internal override void Implies(CommandCondition c) {
                _inner.Implies(c);
            }

            public override ExpectationCommand<T> Given(string given) {
                return new ComparerCommand<T>(_comparer, _inner.Given(given));
            }

            public override TestFailure Should(ITestMatcher<T> matcher) {
                if (_comparer != null) {
                    matcher = ((ITestMatcherWithComparer<T>) matcher).WithComparer(_comparer);
                }
                return _inner.Should(matcher);
            }
        }
    }
}
