//
// Copyright 2017 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

        public static SatisfyAllMatcher<T> SatisfyAll<T>(params ITestMatcher<T>[] matchers) {
            return new SatisfyAllMatcher<T>(matchers);
        }

        public static SatisfyAllMatcher SatisfyAll(params ITestMatcher[] matchers) {
            return new SatisfyAllMatcher(matchers);
        }

    }

    namespace TestMatchers {

        public class SatisfyAllMatcher<T> : ITestMatcher<T>, ICompositeTestMatcher {

            private readonly ITestMatcher<T>[] _matchers;

            [MatcherUserData(Hidden = true)]
            public IReadOnlyList<ITestMatcher<T>> Matchers {
                get {
                    return _matchers;
                }
            }

            public SatisfyAllMatcher(params ITestMatcher<T>[] matchers) {
                _matchers = matchers ?? Empty<ITestMatcher<T>>.Array;
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
                    return null;
                }
            }
        }

        public class SatisfyAllMatcher : ITestMatcher, ICompositeTestMatcher {

            private readonly ITestMatcher[] _matchers;

            public IReadOnlyList<ITestMatcher> Matchers {
                get {
                    return _matchers;
                }
            }

            public SatisfyAllMatcher(params ITestMatcher[] matchers) {
                _matchers = matchers ?? Empty<ITestMatcher>.Array;
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
                    return null;
                }
            }
        }
    }
}
