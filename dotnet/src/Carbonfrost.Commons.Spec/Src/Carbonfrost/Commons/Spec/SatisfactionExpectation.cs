//
// Copyright 2017, 2018, 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
namespace Carbonfrost.Commons.Spec {

    public struct SatisfactionExpectation {

        private readonly ExpectationCommand<Unit> _cmd;

        private Expectation Expectation {
            get {
                return new Expectation(_cmd);
            }
        }

        internal SatisfactionExpectation(ExpectationCommand<Unit> cmd) {
            _cmd = cmd;
        }

        public void All(params ITestMatcher[] matchers) {
            Expectation.Should(Matchers.SatisfyAll(matchers));
        }

        public void Any(params ITestMatcher[] matchers) {
            Expectation.Should(Matchers.SatisfyAny(matchers));
        }
    }

    public struct SatisfactionExpectation<T> {

        private readonly ExpectationCommand<T> _cmd;

        private Expectation<T> Expectation {
            get {
                return new Expectation<T>(_cmd);
            }
        }

        internal SatisfactionExpectation(ExpectationCommand<T> cmd) {
            _cmd = cmd;
        }

        public void All(params ITestMatcher<T>[] matchers) {
            Expectation.Should(Matchers.SatisfyAll(matchers));
        }

        public void All(params ITestMatcher[] matchers) {
            Expectation.Untyped().Should(Matchers.SatisfyAll(matchers));
        }

        public void Any(params ITestMatcher<T>[] matchers) {
            Expectation.Should(Matchers.SatisfyAny(matchers));
        }

        public void Any(params ITestMatcher[] matchers) {
            Expectation.Untyped().Should(Matchers.SatisfyAny(matchers));
        }
    }
}
