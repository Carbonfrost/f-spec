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
using Carbonfrost.Commons.Spec;
using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.SelfTest.ExecutionModel.Spec {

    public class TestRunnerOptionsTests : TestClass {

        [Fact]
        public void Constructor_should_copy_Boolean_property() {
            var tro = new TestRunnerOptions {
                FailFast = true,
                IgnoreFocus = true,
                RandomizeSpecs = true,
                ShowPassExplicitly = true,
                ShowTestNames = true,
                SuppressSummary = true,
            };
            var clone = new TestRunnerOptions(tro);

            Assert.True(clone.FailFast);
            Assert.True(clone.IgnoreFocus);
            Assert.True(clone.RandomizeSpecs);
            Assert.True(clone.ShowPassExplicitly);
            Assert.True(clone.ShowTestNames);
            Assert.True(clone.SuppressSummary);
        }

        [Fact]
        public void Constructor_does_not_clone_ReadOnly_property() {
            var tro = new TestRunnerOptions();
            tro.MakeReadOnly();

            var clone = new TestRunnerOptions(tro);
            Assert.False(clone.IsReadOnly);
        }

        [Fact]
        public void Constructor_should_copy_TimeSpan_property() {
            var two = TimeSpan.FromSeconds(2);
            var three = TimeSpan.FromSeconds(3);
            var four = TimeSpan.FromSeconds(4);
            var tro = new TestRunnerOptions {
                PlanTimeout = two,
                TestTimeout = three,
                SlowTestThreshold = four,
            };
            var clone = new TestRunnerOptions(tro);

            Assert.Equal(two, clone.PlanTimeout);
            Assert.Equal(three, clone.TestTimeout);
            Assert.Equal(four, clone.SlowTestThreshold);
        }

        [Fact]
        public void Constructor_should_copy_other_property() {
            var tro = new TestRunnerOptions {
                ContextLines = 20,
                RandomSeed = 100,
                AssertionMessageFormatMode = AssertionMessageFormatModes.UseUnifiedDiff,
            };
            var clone = new TestRunnerOptions(tro);

            Assert.Equal(20, clone.ContextLines);
            Assert.Equal(100, clone.RandomSeed);
            Assert.Equal(AssertionMessageFormatModes.UseUnifiedDiff, clone.AssertionMessageFormatMode);
        }

        [Fact]
        public void Normalize_should_set_default_value() {
            var tro = new TestRunnerOptions();
            var clone = tro.Normalize();

            Assert.Equal(500, clone.SlowTestThreshold.Value.TotalMilliseconds);
        }
    }
}

#endif
