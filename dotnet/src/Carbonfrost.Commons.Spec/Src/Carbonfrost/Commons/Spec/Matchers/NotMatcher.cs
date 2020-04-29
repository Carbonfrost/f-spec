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
using System.Linq;
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.Commons.Spec {

    partial class Matchers {

        public static NotMatcher<T> Not<T>(ITestMatcher<T> matcher) {
            return new NotMatcher<T>(matcher);
        }

        public static NotMatcher Not(ITestMatcher matcher) {
            return new NotMatcher(matcher);
        }
    }

    namespace TestMatchers {

        public class NotMatcher<T> : ITestMatcher<T>, INotMatcher {

            private readonly ITestMatcher<T> _matcher;

            public NotMatcher(ITestMatcher<T> matcher) {
                if (matcher == null) {
                    throw new ArgumentNullException("matcher");
                }
                _matcher = matcher;
            }

            public bool Matches(ITestActualEvaluation<T> actualFactory) {
                return !_matcher.Matches(actualFactory);
            }

            object INotMatcher.InnerMatcher {
                get {
                    return _matcher;
                }
            }
        }

        public class NotMatcher : ITestMatcher, INotMatcher {

            private readonly ITestMatcher _matcher;

            public NotMatcher(ITestMatcher matcher) {
                if (matcher == null) {
                    throw new ArgumentNullException("matcher");
                }
                _matcher = matcher;
            }

            public bool Matches(ITestActualEvaluation testCode) {
                return !_matcher.Matches(testCode);
            }

            object INotMatcher.InnerMatcher {
                get {
                    return _matcher;
                }
            }
        }
    }

}
