#if SELF_TEST

//
// Copyright 2018, 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Carbonfrost.Commons.Spec;
using Carbonfrost.Commons.Spec.ExecutionModel;
using Extensions = Carbonfrost.Commons.Spec.Extensions;

namespace Carbonfrost.SelfTest.Spec {

    public class TestUnitEventArgsConsistencyTests : TestClass {

        public IEnumerable<TypeInfo> EventArgsTypes {
            get {
                return typeof(Extensions).GetTypeInfo().Assembly.DefinedTypes.Where(t => t.IsClass && t.Name.EndsWith("EventArgs"));
            }
        }

        public IEnumerable<TypeInfo> StartingEventArgsTypes {
            get {
                return Candidates("Starting");
            }
        }

        public IEnumerable<TypeInfo> StartedEventArgsTypes {
            get {
                return Candidates("Started");
            }
        }


        public IEnumerable<TypeInfo> FinishedEventArgsTypes {
            get {
                return Candidates("Finished");
            }
        }

        static IEnumerable<TypeInfo> Candidates(string suffix) {
            var self = "TestUnit" + suffix + "EventArgs";
            Func<TypeInfo, bool> pred = t =>
                t.Name != self
                && t.IsClass
                && !t.Name.Contains("Runner")
                && t.Name.EndsWith(suffix + "EventArgs");

            return typeof(Extensions).GetTypeInfo().Assembly.DefinedTypes.Where(pred);
        }

        [Theory]
        [PropertyData("EventArgsTypes")]
        public void EventArgs_should_have_no_public_constructor(TypeInfo type) {
            Expect(type.DeclaredConstructors.ToArray()).ToHave.Count(
                0, t => t.IsPublic
            );
        }

        [Theory]
        [PropertyData("StartedEventArgsTypes")]
        public void StartedEventArgs_should_have_only_unit_constructor(TypeInfo type) {
            var ctors = type.DeclaredConstructors;
            Assert.Single(ctors);
            Assert.Single(ctors.First().GetParameters());
            Assert.Equal("TestUnitStartedEventArgs", ctors.First().GetParameters()[0].ParameterType.Name);
        }

        [Theory]
        [PropertyData("StartingEventArgsTypes")]
        public void StartingEventArgs_should_have_only_unit_constructor(TypeInfo type) {
            var ctors = type.DeclaredConstructors;
            Assert.Single(ctors);
            Assert.Single(ctors.First().GetParameters());
            Assert.Equal("TestUnitStartingEventArgs", ctors.First().GetParameters()[0].ParameterType.Name);
        }

        [Theory]
        [PropertyData("StartingEventArgsTypes")]
        public void StartingEventArgs_should_have_Cancel_property(TypeInfo type) {
            var cancel = type.GetDeclaredProperty("Cancel");
            Assert.NotNull(cancel);
            Assert.NotNull(cancel.GetMethod);
            Assert.NotNull(cancel.SetMethod);
            Assert.True(cancel.GetMethod.IsPublic);
            Assert.True(cancel.SetMethod.IsPublic);
        }

        [Theory]
        [PropertyData("StartingEventArgsTypes")]
        public void StartingEventArgs_should_redirect_Cancel(TypeInfo type) {
            var cancel = type.GetDeclaredProperty("Cancel");
            Assert.NotNull(cancel);

            var unit = new TestUnitStartingEventArgs(null);
            var ctor = type.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance).First();
            var inst = ctor.Invoke(new object[] { unit });
            cancel.SetValue(inst, true);

            // Redirected to the underlying unit
            Assert.True(unit.Cancel);
        }

        [Theory]
        [PropertyData("FinishedEventArgsTypes")]
        public void FinishedEventArgs_should_have_only_unit_constructor(TypeInfo type) {
            var ctors = type.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.Single(ctors);
            Assert.Single(ctors.First().GetParameters());
            Assert.Equal("TestUnitFinishedEventArgs", ctors.First().GetParameters()[0].ParameterType.Name);
        }

        [Fact]
        public void Should_have_corresponding_event_args() {
            Assert.Equal(StartedEventArgsTypes.Count(), FinishedEventArgsTypes.Count());
            Assert.Equal(StartedEventArgsTypes.Count(), StartingEventArgsTypes.Count());
        }

    }
}
#endif
