//
// Copyright 2016, 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
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

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    public interface ITestRunnerEvents {
        event EventHandler<TestMessageEventArgs> Message;
        event EventHandler<TestClassStartingEventArgs> TestClassStarting;
        event EventHandler<TestClassStartedEventArgs> TestClassStarted;
        event EventHandler<TestClassFinishedEventArgs> TestClassFinished;
        event EventHandler<TestAssemblyStartingEventArgs> TestAssemblyStarting;
        event EventHandler<TestAssemblyStartedEventArgs> TestAssemblyStarted;
        event EventHandler<TestAssemblyFinishedEventArgs> TestAssemblyFinished;
        event EventHandler<TestNamespaceStartingEventArgs> TestNamespaceStarting;
        event EventHandler<TestNamespaceStartedEventArgs> TestNamespaceStarted;
        event EventHandler<TestNamespaceFinishedEventArgs> TestNamespaceFinished;
        event EventHandler<TestCaseStartingEventArgs> TestCaseStarting;
        event EventHandler<TestCaseStartedEventArgs> TestCaseStarted;
        event EventHandler<TestCaseFinishedEventArgs> TestCaseFinished;
        event EventHandler<TestRunnerStartingEventArgs> TestRunnerStarting;
        event EventHandler<TestRunnerStartedEventArgs> TestRunnerStarted;
        event EventHandler<TestRunnerFinishedEventArgs> TestRunnerFinished;
        event EventHandler<TestUnitStartingEventArgs> TestUnitStarting;
        event EventHandler<TestUnitStartedEventArgs> TestUnitStarted;
        event EventHandler<TestUnitFinishedEventArgs> TestUnitFinished;
        event EventHandler<TestTheoryStartingEventArgs> TestTheoryStarting;
        event EventHandler<TestTheoryStartedEventArgs> TestTheoryStarted;
        event EventHandler<TestTheoryFinishedEventArgs> TestTheoryFinished;
    }
}
