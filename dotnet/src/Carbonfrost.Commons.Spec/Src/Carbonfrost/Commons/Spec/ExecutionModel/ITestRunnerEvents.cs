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
        event EventHandler<TestClassStartingEventArgs> ClassStarting;
        event EventHandler<TestClassStartedEventArgs> ClassStarted;
        event EventHandler<TestClassFinishedEventArgs> ClassFinished;
        event EventHandler<TestSubjectClassBindingStartingEventArgs> SubjectClassBindingStarting;
        event EventHandler<TestSubjectClassBindingStartedEventArgs> SubjectClassBindingStarted;
        event EventHandler<TestSubjectClassBindingFinishedEventArgs> SubjectClassBindingFinished;
        event EventHandler<TestAssemblyStartingEventArgs> AssemblyStarting;
        event EventHandler<TestAssemblyStartedEventArgs> AssemblyStarted;
        event EventHandler<TestAssemblyFinishedEventArgs> AssemblyFinished;
        event EventHandler<TestNamespaceStartingEventArgs> NamespaceStarting;
        event EventHandler<TestNamespaceStartedEventArgs> NamespaceStarted;
        event EventHandler<TestNamespaceFinishedEventArgs> NamespaceFinished;
        event EventHandler<TestCaseStartingEventArgs> CaseStarting;
        event EventHandler<TestCaseStartedEventArgs> CaseStarted;
        event EventHandler<TestCaseFinishedEventArgs> CaseFinished;
        event EventHandler<TestRunnerStartingEventArgs> RunnerStarting;
        event EventHandler<TestRunnerStartedEventArgs> RunnerStarted;
        event EventHandler<TestRunnerFinishedEventArgs> RunnerFinished;
        event EventHandler<TestUnitStartingEventArgs> UnitStarting;
        event EventHandler<TestUnitStartedEventArgs> UnitStarted;
        event EventHandler<TestUnitFinishedEventArgs> UnitFinished;
        event EventHandler<TestTheoryStartingEventArgs> TheoryStarting;
        event EventHandler<TestTheoryStartedEventArgs> TheoryStarted;
        event EventHandler<TestTheoryFinishedEventArgs> TheoryFinished;
    }
}
