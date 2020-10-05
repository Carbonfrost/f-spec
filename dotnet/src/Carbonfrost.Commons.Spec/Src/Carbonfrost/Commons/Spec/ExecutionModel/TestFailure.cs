//
// Copyright 2016, 2018, 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Text;
using Carbonfrost.Commons.Spec.Resources;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    public class TestFailure {

        private readonly TestMatcherName _name;
        private readonly TestFailureCollection _children = new TestFailureCollection();
        private AsserterBehavior _behavior;

        public string Message {
            get;
            set;
        }

        internal AsserterBehavior AsserterBehavior {
            get {
                return _behavior ?? ExecutionModel.AsserterBehavior.Default;
            }
        }

        public TestFailureCollection Children {
            get {
                return _children;
            }
        }

        public TestMatcherName Name {
            get {
                return _name;
            }
        }

        public UserDataCollection UserData {
            get;
            private set;
        }

        internal TestFailure(TestMatcherName name, object matcher) {
            _name = name;
            UserData = new UserDataCollection(matcher);
        }

        public TestFailure(TestMatcherName name) {
            _name = name;
            UserData = new UserDataCollection();
        }

        internal Exception ToException() {
            return new AssertException(Message, this, null);
        }

        internal string FormatMessage(string userMessage) {
            var err = new StringBuilder();
            err.Append(userMessage);

            AppendChildren(err, 0);
            return err.ToString();
        }

        private void AppendChildren(StringBuilder err, int level) {
            if (level == 2) {
                return;
            }
            if (Children.Count == 0) {
                return;
            }
            err.AppendLine();

            string prefix = new string(' ', 2 * level) + "- ";
            foreach (var c in Children) {
                err.Append(prefix);
                err.AppendLine(c.Message);
                c.AppendChildren(err, level + 1);
            }
        }

        internal TestFailure UpdateBehavior(AsserterBehavior behavior) {
            _behavior = behavior;
            return this;
        }

        internal TestFailure UpdateGiven(string given) {
            if (!string.IsNullOrEmpty(given)) {
                UserData[SR.LabelGiven()] = given;
            }
            return this;
        }

        internal TestFailure UpdateMessage(string message, object[] args) {
            if (message != null) {
                Message = string.Format(message, (object[]) args ?? Array.Empty<object>());
            }
            return this;
        }

        internal TestFailure UpdateTestSubject() {
            var cur = TestContext.Current;
            if (cur != null) {
                UserData.Add("Subject", cur.TestUnit.FindTestSubject());
            }
            return this;
        }

        internal TestFailure UpdateActual(object actual) {
            UserData.Remove("Actual");
            UserData.Add("Actual", actual);
            return this;
        }
    }
}
