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

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    [Flags]
    internal enum JTestAttributes {
        None = 0,
        Slow = 1 << 0,
        ContainsFocusedUnits = 1 << 1,
        Failed = 1 << 2,
        Pending = 1 << 3,
        Running = 1 << 4,
        StatusExplicit = 1 << 5,
        Passed = 1 << 6,
        Skipped = 1 << 7,
        StrictlyPassed = 1 << 8,
        Focused = 1 << 9,
    }
}
