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

namespace Carbonfrost.Commons.Spec {

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class FixtureDataAttribute : Attribute, ITestDataApiAttributeConventions {

        private readonly TestFileInput _input;
        private readonly TestTagCache _tags = new TestTagCache();

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

        RetargetDelegates ITestDataApiAttributeConventions.RetargetDelegates {
            get {
                return RetargetDelegates.Unspecified;
            }
            set {
                throw new NotSupportedException();
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

        public string[] Tags {
            get {
                return _tags.Tags;
            }
            set {
                _tags.Tags = value;
            }
        }

        public string Tag {
            get {
                return _tags.Tag;
            }
            set {
                _tags.Tag = value;
            }
        }

        public FixtureDataAttribute(string pathPattern) {
            _input = new TestFileInput(pathPattern);
        }

        IEnumerable<TestData> ITestDataProvider.GetData(TestContext context) {
            TestUnit unit = context.TestUnit;
            var rt = (TestTheory) unit;
            return _input.ReadInputs(
                context,
                u => CoreLoadFixture(Url, url => context.DownloadFixture(url), rt, null),
                f => CoreLoadFixture(f.FileName, fn => context.LoadFixture(fn), rt, f.Data)
            ).SelectMany(t => t);
        }

        IEnumerable<TestData> CoreLoadFixture<TLocation>(
            TLocation location,
            Func<TLocation, TestFixture> fixtureFunc,
            TestTheory rt,
            IEnumerable<KeyValuePair<string, string>> fixturePatternVariables
        ) {
            TestFixture fixture;
            try {
                fixture = fixtureFunc(location);

            } catch (Exception ex) {
                string message = ex is ParserException
                    ? ex.Message
                    : "parser error";
                return new [] {
                    new TestData().WithTags(_tags).VerifiableProblem(
                        false,
                        string.Format("Failed to load fixture {0} ({1})", TextUtility.FormatLocation(location), message)
                    )
                };
            }

            var items = fixture.Items;
            if (items.Count == 0) {
                return new [] {
                    new TestData().WithTags(_tags).VerifiableProblem(
                        false,
                        string.Format("Empty fixture {0}", TextUtility.FormatLocation(location))
                    )
                };
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
            var binder = FixtureTestDataBinder.Create(rt.TestMethod, keySet);
            var results = new List<TestData>(items.Count);
            foreach (var t in items) {
                results.Add(
                    new TestData(
                        new TestDataState(Name, Reason, Explicit ? TestUnitFlags.Explicit : TestUnitFlags.None, _tags),
                        binder.Bind(t.Values)
                    )
                );
            }
            return results;
        }
    }

}
