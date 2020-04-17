//
// Copyright 2016, 2017, 2018, 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Collections.Generic;
using System.Linq;
using Carbonfrost.Commons.Spec.ExecutionModel;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.Commons.Spec {

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class FixtureDataAttribute : Attribute, ITestDataApiAttributeConventions {

        private readonly TestFileInput _input;

        public string PathPattern {
            get {
                return _input.PathPattern;
            }
        }

        public Uri Url {
            get {
                return _input.Url;
            }
        }

        public string Name {
            get;
            set;
        }

        public string Reason {
            get;
            set;
        }

        public bool Explicit {
            get;
            set;
        }

        public FixtureDataAttribute(string pathPattern) {
            _input = new TestFileInput(pathPattern);
        }

        IEnumerable<TestData> ITestDataProvider.GetData(TestContext context) {
            TestUnit unit = context.CurrentTest;
            var rt = (TestTheory) unit;
            return _input.ReadInputs(
                context,
                u => CoreLoadFixture(context.DownloadFixture(Url), rt, null),
                f => CoreLoadFixture(context.LoadFixture(f.FileName), rt, f.Data))
                .SelectMany(t => t);
        }

        IEnumerable<TestData> CoreLoadFixture(TestFixture fixture,
                                              TestTheory rt,
                                              IEnumerable<KeyValuePair<string, string>> fixturePatternVariables) {
            var items = fixture.Items;
            if (items.Count == 0) {
                return Empty<TestData>.Array;
            }
            if (fixturePatternVariables != null) {
                foreach (var t in items) {
                    foreach (var kvp in fixturePatternVariables) {
                        // Variables captured from the fixture pattern should
                        // lose to ones defined in the fixture itself
                        if (!t.Values.ContainsKey(kvp.Key)) {
                            t.Values.Add(kvp);
                        }
                    }
                }
            }
            // TODO Assume that the fixture has homogeneous records -- if it doesn't,
            // then we end up with the wrong TestDataBinder
            var keySet = items[0].Values.Keys;
            var binder = TestDataBinder.Create(rt.TestMethod, keySet);
            var results = new List<TestData>(items.Count);
            foreach (var t in items) {
                results.Add(new TestData(binder.Bind(t.Values)).WithNameAndReason(
                    Name, Reason, Explicit ? TestUnitFlags.Explicit : TestUnitFlags.None
                ));
            }
            return results;
        }
    }

}
