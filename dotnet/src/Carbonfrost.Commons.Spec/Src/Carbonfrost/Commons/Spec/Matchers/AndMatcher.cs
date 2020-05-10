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
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.Commons.Spec {

    partial class Matchers {

        public static AndMatcher<T> And<T>(params ITestMatcher<T>[] matchers) {
            return new AndMatcher<T>(matchers);
        }

        public static AndMatcher<T> And<T>(IEnumerable<ITestMatcher<T>> matchers) {
            return new AndMatcher<T>(matchers);
        }

        public static AndMatcher And(params ITestMatcher[] matchers) {
            return new AndMatcher(matchers);
        }

        public static AndMatcher And(IEnumerable<ITestMatcher> matchers) {
            return new AndMatcher(matchers);
        }
    }

    namespace TestMatchers {

        public class AndMatcher<T> : ITestMatcher<T>, ICompositeTestMatcher {

            private readonly ITestMatcher<T>[] _matchers;

            public AndMatcher(params ITestMatcher<T>[] matchers) : this((IEnumerable<ITestMatcher<T>>) matchers) {
            }

            public AndMatcher(IEnumerable<ITestMatcher<T>> matchers) {
                if (matchers == null) {
                    _matchers = Empty<ITestMatcher<T>>.Array;
                } else {
                    _matchers = matchers.ToArray();
                }
            }

            public bool Matches(ITestActualEvaluation<T> actualFactory) {
                return _matchers.All(t => t.Matches(actualFactory));
            }

            System.Collections.IEnumerable ICompositeTestMatcher.Matchers {
                get {
                    return _matchers;
                }
            }

            string ICompositeTestMatcher.Operator {
                get {
                    return "and";
                }
            }
        }

        public class AndMatcher : ITestMatcher, ICompositeTestMatcher {

            private readonly ITestMatcher[] _matchers;

            public AndMatcher(params ITestMatcher[] matchers) {
                _matchers = (matchers ?? Empty<ITestMatcher>.Array).ToArray();
            }

            public AndMatcher(IEnumerable<ITestMatcher> matchers) {
                _matchers = (matchers ?? Empty<ITestMatcher>.Array).ToArray();
            }

            public bool Matches(ITestActualEvaluation testCode) {
                return _matchers.All(t => t.Matches(testCode));
            }

            System.Collections.IEnumerable ICompositeTestMatcher.Matchers {
                get {
                    return _matchers;
                }
            }

            string ICompositeTestMatcher.Operator {
                get {
                    return "and";
                }
            }
        }
    }

}
