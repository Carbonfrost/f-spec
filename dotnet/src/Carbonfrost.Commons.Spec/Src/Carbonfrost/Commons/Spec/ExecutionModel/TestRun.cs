//
// Copyright 2018, 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Reflection;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    public class TestRun : TestUnit {

        private readonly TestUnitCollection _children;

        public override TestUnitType Type {
            get {
                return TestUnitType.Run;
            }
        }

        public override TestUnitCollection Children {
            get {
                return _children;
            }
        }

        internal override TestUnitMetadata Metadata {
            get {
                return TestUnitMetadata.Empty;
            }
        }

        public TestRun() {
            _children = new TestUnitCollection(this);
        }

        public void AddAssembly(Assembly assembly) {
            Children.AddAssembly(assembly);
        }

        public override string DisplayName {
            get {
                return "<TestRun>";
            }
        }

        public override string Name {
            get {
                return DisplayName;
            }
        }

        public void AddSelfTests() {
            SpecLog.ActivatedSelfTestMode();

            if (!TestClass.HasSelfTests) {
                throw SpecFailure.NoSelfTestsAvailable();
            }
            AddAssembly(typeof(TestMatcher).GetTypeInfo().Assembly);
        }
    }
}
