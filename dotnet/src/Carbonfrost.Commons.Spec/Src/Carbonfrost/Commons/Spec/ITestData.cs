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
using System.Collections.Generic;
using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.Commons.Spec {
    interface ITestData : IReadOnlyList<object>, ITestUnitStateApiConventions<TestData> {
        string Name { get; }
        string Reason { get; }
        TestData WithName(string name);
        TestData WithReason(string reason);
    }

    interface ITestDataHelper : ITestUnitStateApiConventions<TestData> {
        TestData WithName(string name);
        TestData WithReason(string reason);
    }

    interface ITestDataHelper<T> : ITestUnitStateApiConventions<TestData<T>>, ITestDataUntyped {
        TestData<T> WithName(string name);
        TestData<T> WithReason(string reason);
    }

    interface ITestDataUntyped {
        // This interface is split from ITestData<T> to make it simpler to cast
        // when we don't want to deal with the generics
        TestData Untyped();
    }

    interface ITestData<T> : IReadOnlyList<T>, ITestUnitStateApiConventions<TestData<T>>, ITestDataUntyped {
        string Name { get; }
        string Reason { get; }
        TestData<T> WithName(string name);
        TestData<T> WithReason(string reason);
    }
}
