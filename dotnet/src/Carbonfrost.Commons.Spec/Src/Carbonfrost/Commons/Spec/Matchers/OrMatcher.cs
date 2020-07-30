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

        public static OrMatcher<T> Or<T>(params ITestMatcher<T>[] matchers) {
            return new OrMatcher<T>(matchers);
        }

        public static OrMatcher<T> Or<T>(IEnumerable<ITestMatcher<T>> matchers) {
            return new OrMatcher<T>(matchers);
        }

        public static OrMatcher Or(params ITestMatcher[] matchers) {
            return new OrMatcher(matchers);
        }

        public static OrMatcher Or(IEnumerable<ITestMatcher> matchers) {
            return new OrMatcher(matchers);
        }
    }

    namespace TestMatchers {

        public class OrMatcher<T> : ITestMatcher<T>, ICompositeTestMatcher {

            private readonly ITestMatcher<T>[] _matchers;

            public OrMatcher(params ITestMatcher<T>[] matchers) : this((IEnumerable<ITestMatcher<T>>) matchers) {
            }

            public OrMatcher(IEnumerable<ITestMatcher<T>> matchers) {
                if (matchers == null) {
                    _matchers = Array.Empty<ITestMatcher<T>>();
                } else {
                    _matchers = matchers.ToArray();
                }
            }

            public bool Matches(ITestActualEvaluation<T> actual) {
                return _matchers.Any(t => t.Matches(actual));
            }

            System.Collections.IEnumerable ICompositeTestMatcher.Matchers {
                get {
                    return _matchers;
                }
            }

            string ICompositeTestMatcher.Operator {
                get {
                    return "or";
                }
            }
        }

        public class OrMatcher : ITestMatcher, ICompositeTestMatcher {

            private readonly ITestMatcher[] _matchers;

            public OrMatcher(params ITestMatcher[] matchers) {
                _matchers = (matchers ?? Array.Empty<ITestMatcher>()).ToArray();
            }

            public OrMatcher(IEnumerable<ITestMatcher> matchers) {
                _matchers = (matchers ?? Array.Empty<ITestMatcher>()).ToArray();
            }

            public bool Matches(ITestActualEvaluation testCode) {
                return _matchers.Any(t => t.Matches(testCode));
            }

            System.Collections.IEnumerable ICompositeTestMatcher.Matchers {
                get {
                    return _matchers;
                }
            }

            string ICompositeTestMatcher.Operator {
                get {
                    return "or";
                }
            }
        }
    }

}
