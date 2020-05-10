#if SELF_TEST

//
// Copyright 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using System;
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.SelfTest.Spec.TestMatchers {

    class LessThanOperatorTests : ComparisonOperatorTestBase<LessThanOperator> {

        public override string Input {
            get {
                return "ghost";
            }
        }

        public override ExampleData[] Examples {
            get {
                Comparison<string> comparison = (x, y) => x.CompareTo(y);
                return new[] {
                    Example("mansion"),
                    Example("Mansion", StringComparer.OrdinalIgnoreCase),
                    Example("mansion", comparison),
                };
            }
        }

        public override ExampleData[] CounterExamples {
            get {
                return new[] {
                    Example("bear"),
                    Example("ghost"),
                    Example(""),
                };
            }
        }

        public override string APIName {
            get {
                // To(BeLessThan()), ToBe.LessThan()
                return "LessThan";
            }
        }

    }
}

#endif
