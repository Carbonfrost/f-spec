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
using System.Linq;
using System.Collections.Generic;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    public class TestPlanFilter {

        private static readonly Action<TestUnit> SKIP = t => {
            if (IsLeaf(t)) {
                t.Skipped = true;
            }
        };
        private static readonly Action<TestUnit> ACTIVATE = t => t.Skipped = false;
        private static readonly Action<TestUnit> FOCUS =  t => {
            t.Skipped = false;
            t.IsFocused = true;
        };
        private static readonly Action<TestUnit> SKIP_IF_HAS_ANY_TAGS = t => {
            if (t.Tags.HasUserTags) {
                t.Skipped = true;
            }
        };

        private readonly TestPlanFilterPatternCollection _focusPatterns = new TestPlanFilterPatternCollection();
        private readonly TestPlanFilterPatternCollection _includes = new TestPlanFilterPatternCollection();
        private readonly TestPlanFilterPatternCollection _excludes = new TestPlanFilterPatternCollection();
        private readonly TestTagPredicateCollection _tags = new TestTagPredicateCollection();

        public TestPlanFilterPatternCollection FocusPatterns {
            get {
                return _focusPatterns;
            }
        }

        public TestPlanFilterPatternCollection Includes {
            get {
                return _includes;
            }
        }

        public TestPlanFilterPatternCollection Excludes {
            get {
                return _excludes;
            }
        }

        public TestTagPredicateCollection Tags {
            get {
                return _tags;
            }
        }

        internal void MakeReadOnly() {
            _excludes.MakeReadOnly();
            _focusPatterns.MakeReadOnly();
            _includes.MakeReadOnly();
            _tags.MakeReadOnly();
        }

        internal void CopyFrom(TestPlanFilter other) {
            Excludes.AddAll(other.Excludes);
            FocusPatterns.AddAll(other.FocusPatterns);
            Includes.AddAll(other.Includes);
            Tags.AddAll(other.Tags);
        }

        static bool IsLeaf(TestUnit t) {
            return t.Type == TestUnitType.Case
                || t.Type == TestUnitType.Theory
                || t.Type == TestUnitType.Fact;
        }

        internal void Apply(TestRun testRun, TestRunnerOptions normalized) {
            ActivateDefaultTestSet(testRun);

            Excludes.Apply(testRun, SKIP);
            FocusPatterns.Apply(testRun, FOCUS);

            // If any focused nodes, then only run focused nodes
            if (!normalized.IgnoreFocus && testRun.ContainsFocusedUnits) {
                ApplyFocussing(testRun);
            }

            InheritBiasToChildren(testRun);
            SealRecursive(testRun);
        }

        private void ActivateDefaultTestSet(TestRun testRun) {
            // Look for explicit tests
            SkipExplicitTests(testRun);
            bool emptyIncludes = Includes.Count == 0;
            bool emptyTags = Tags.Count == 0;

            if (emptyIncludes && emptyTags) {
                // Default configuration, which is all tests that aren't tagged
                Tags.Apply(testRun, SKIP_IF_HAS_ANY_TAGS);

            } else if (emptyIncludes) {
                // Only run tests that match tag
                Tags.Apply(testRun, ACTIVATE, SKIP);

            } else if (emptyTags) {
                // Only run tests that match includes
                TestPlanFilterPattern.Or(Includes).Apply(testRun, ACTIVATE, SKIP);

            } else {
                // Only run tests that match either
                TestPlanFilterPattern.Or(Includes).Apply(testRun, ACTIVATE, SKIP);
                Tags.Apply(testRun, ACTIVATE);
            }
        }

        private void ApplyFocussing(TestUnit m) {
            if (m.ContainsFocusedUnits) {
                foreach (var c in m.Children) {
                    ApplyFocussing(c);
                }

            } else if (m.IsFocused) {

            } else {
                m.Skipped = true;
            }
        }

        private static void InheritBiasToChildren(TestUnit unit) {
            foreach (var child in unit.Children) {
                child.Skipped |= unit.Skipped;
                child.IsPending |= unit.IsPending;
                child.IsExplicit |= unit.IsExplicit;
                child.PassExplicitly |= unit.PassExplicitly;
                if (unit.Reason != null) {
                    child.Reason = unit.Reason;
                }
                InheritBiasToChildren(child);
            }
        }

        private static void SealRecursive(TestUnit unit) {
            foreach (var c in unit.Children) {
                SealRecursive(c);
            }
            unit.Seal();
        }

        private void SkipExplicitTests(TestUnit m) {
            if (m.IsExplicit) {
                m.Skipped = true;
            }
            foreach (var c in m.Children) {
                SkipExplicitTests(c);
            }
        }
    }

}
