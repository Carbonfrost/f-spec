//
// Copyright 2016, 2018 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Text;
using Carbonfrost.Commons.Spec.Resources;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    public class TestFailure {

        const int bufferWidth = 6;
        private readonly string _name;
        private readonly TestFailureCollection _children = new TestFailureCollection();

        public string Message { get; set; }

        public TestFailureCollection Children {
            get {
                return _children;
            }
        }

        public string Name {
            get {
                return _name;
            }
        }

        public IDictionary<string, string> UserData {
            get;
            private set;
        }

        public TestFailure(string name) {
            _name = name;
            UserData = new SortedDictionary<string, string>(new SortOrder());
        }

        public override string ToString() {
            return string.Format("<TestFailure ({0}), {1}, UserData={2}>",
                                 _name, Message, TextUtility.ConvertToString(UserData));
        }


        class SortOrder : IComparer<string> {

            public int Compare(string x, string y) {
                // Expected, Actual, then everything else alphabetically
                return string.Compare(CheckNames(x),
                                      CheckNames(y),
                                      StringComparison.OrdinalIgnoreCase);
            }

            static string CheckNames(string x) {
                if (string.Equals(x, SR.LabelActual()) || string.Equals(x, SR.LabelExpected())) {
                    return " " + x;
                }
                return x;
            }
        }


        internal Exception ToException() {
            return new AssertException(Message, this, null);
        }

        internal string FormatMessage(string userMessage) {
            // Format actual and expected together
            int maxLength = 0;
            var err = new StringBuilder();

            if (!string.IsNullOrEmpty(userMessage)) {
                err.AppendLine(userMessage);
            }
            AppendChildren(err, 0);

            if (UserData.Keys.Count > 0) {
                maxLength = UserData.Keys.Max(t => t.Length);

                err.AppendLine();
                foreach (var kvp in UserData) {
                    string line = LineItem(kvp.Key, kvp.Value, maxLength);
                    err.AppendLine(line);
                }
            }

            return err.ToString();
        }

        private void AppendChildren(StringBuilder err, int level) {
            if (level == 2) {
                return;
            }

            string prefix = new string(' ', 2 * level) + "- ";
            foreach (var c in Children) {
                err.Append(prefix);
                err.AppendLine(c.Message);
                c.AppendChildren(err, level + 1);
            }
        }

        internal TestFailure UpdateGiven(string given) {
            if (!string.IsNullOrEmpty(given)) {
                UserData[SR.LabelGiven()] = given;
            }
            return this;
        }

        internal TestFailure UpdateMessage(string message, object[] args) {
            if (message != null) {
                Message = string.Format(message, (object[]) args);
            }
            return this;
        }

        static string LineItem(string caption, string data, int length) {
            caption = TestMatcherLocalizer.Caption(caption);
            return string.Format("{0," + (bufferWidth + length) + "}: {1}", caption, data);
        }

        internal TestFailure UpdateTestSubject() {
            var cur = TestContext.Current;
            if (cur != null) {
                var subj = TextUtility.ConvertToString(cur.CurrentTest.FindTestSubject());
                UserData[SR.LabelSubject()] = subj;
            }
            return this;
        }
    }
}
