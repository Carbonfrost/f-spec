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

    class SetEqualOperatorTests : SequenceComparisonOperatorTestBase<SetEqualOperator> {

        public override string[] Input {
            get {
                return new [] {
                    "a", "b", "c",
                };
            }
        }

        public override ExampleData[] Examples {
            get {
                Comparison<string> comparison = (x, y) => x.CompareTo(y);
                return new[] {
                    Example(new [] { "a", "b", "c" }),
                    Example(new [] { "C", "A", "B" }, StringComparer.OrdinalIgnoreCase),
                    Example(new [] { "c", "b", "a" }, comparison),
                };
            }
        }

        public override ExampleData[] CounterExamples {
            get {
                return new[] {
                    Example(new [] { "z" }),
                    Example(new string [0]),
                };
            }
        }

        public override bool HasStringExpectations {
            get {
                return true;
            }
        }

        public override string APIName {
            get {
                // To(BeSetEqualTo()), ToBe.SetEqualTo()
                return "SetEqualTo";
            }
        }

    }
}

#endif
