//
// Copyright 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

namespace Carbonfrost.Commons.Spec {

    public interface ITestMatcher<in T> {
        bool Matches(ITestActualEvaluation<T> actual);
    }

    public interface ITestMatcher {
        bool Matches(ITestActualEvaluation testCode);
    }

    static partial class Extensions {

        public static bool Matches(this ITestMatcher self, Action testCode) {
            return self.Matches(
                TestActual.Of(Unit.Thunk(testCode))
            );
        }

        internal static bool Matches<T>(this ITestMatcher<T> self, Func<T> actualFactory) {
            return self.Matches(new TestActual<T>(actualFactory));
        }
    }

}
