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
using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.Commons.Spec {

    public partial class TestCaseBuilder {

        private static TestCaseBuilder Current {
            get {
                return null;
            }
        }

        public static TestCaseBuilder Before(Action action) {
            return Current.AddBefore(action);
        }

        public static TestCaseBuilder After(Action action) {
            return Current.AddAfter(action);
        }

        public static TestCaseBuilder Context(string text, Action body) {
            return Current.PushContainer(text, body, TestUnitFlags.None);
        }

        public static TestCaseBuilder FContext(string text, Action body) {
            return Current.PushContainer(text, body, TestUnitFlags.Focus);
        }

        public static TestCaseBuilder XContext(string text, Action body) {
            return Current.PushContainer(text, body, TestUnitFlags.Pending);
        }

        public static TestCaseBuilder It(string text, Action body) {
            return Current.PushIt(text, body, TestUnitFlags.None);
        }

        public static TestCaseBuilder FIt(string text, Action body) {
            return Current.PushIt(text, body, TestUnitFlags.Focus);
        }

        public static TestCaseBuilder XIt(string text, Action body) {
            return Current.PushIt(text, body, TestUnitFlags.Pending);
        }

        public static TestCaseBuilder Describe(string text, Action body) {
            return Current.PushContainer(text, body, TestUnitFlags.None);
        }

        public static TestCaseBuilder FDescribe(string text, Action body) {
            return Current.PushContainer(text, body, TestUnitFlags.Focus);
        }

        public static TestCaseBuilder XDescribe(string text, Action body) {
            return Current.PushContainer(text, body, TestUnitFlags.Pending);
        }

        public static TestCaseBuilder When(string text, Action body) {
            return Current.PushContainer("When " + text, body, TestUnitFlags.None);
        }

        public static TestCaseBuilder FWhen(string text, Action body) {
            return Current.PushContainer("When " + text, body, TestUnitFlags.Focus);
        }

        public static TestCaseBuilder XWhen(string text, Action body) {
            return Current.PushContainer("When " + text, body, TestUnitFlags.Pending);
        }

        private TestCaseBuilder PushContainer(string text, Action body, TestUnitFlags none) {
            // FIXME Push container
            return Current;
        }

        private TestCaseBuilder PushIt(string text, Action body, TestUnitFlags none) {
            // FIXME Push it
            return Current;
        }

        private TestCaseBuilder AddBefore(Action body) {
            // FIXME Update container
            return Current;
        }

        private TestCaseBuilder AddAfter(Action body) {
            // FIXME Update container
            return Current;
        }


// • SPEC public static void It(string text, Action body)
//    // FIt,XIT, Specify
//       TestCaseBuilder.Current.PushIt("text,body,TestUnitFlags.None,codelocation);
    }
}
