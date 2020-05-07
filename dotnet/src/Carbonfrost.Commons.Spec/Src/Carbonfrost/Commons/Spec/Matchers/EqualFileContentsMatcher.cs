//
// Copyright 2018-2019 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.IO;
using System.Linq;
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.Commons.Spec {

    partial class Matchers {

        public static EqualFileContentsMatcher EqualFileContents(string fileName) {
            return new EqualFileContentsMatcher(fileName);
        }

    }

    static partial class Extensions {

        public static void EqualToFileContents(this IExpectation<string> e, string fileName) {
            EqualToFileContents(e, fileName, null);
        }

        public static void EqualToFileContents(this IExpectation<string> e, string fileName, string message, params object[] args) {
            e.Like(Matchers.EqualFileContents(fileName), message, (object[]) args);
        }

    }

    partial class Asserter {

        public void EqualFileContents(string fileName, string actual) {
            EqualFileContents(fileName, actual, null);
        }

        public void EqualFileContents(string fileName, string actual, string message, params object[] args) {
            That(actual, Matchers.EqualFileContents(fileName), message, (object[]) args);
        }

        public void NotEqualFileContents(string fileName, string actual) {
            NotEqualFileContents(fileName, actual, null);
        }

        public void NotEqualFileContents(string fileName, string actual, string message, params object[] args) {
            NotThat(actual, Matchers.EqualFileContents(fileName), message, (object[]) args);
        }

    }

    partial class Assert {

        public static void EqualFileContents(string fileName, string actual) {
            Global.EqualFileContents(fileName, actual);
        }

        public static void EqualFileContents(string fileName, string actual, string message, params object[] args) {
            Global.EqualFileContents(fileName, actual, message, (object[]) args);
        }

        public static void NotEqualFileContents(string fileName, string actual) {
            Global.NotEqualFileContents(fileName, actual);
        }

        public static void NotEqualFileContents(string fileName, string actual, string message, params object[] args) {
            Global.NotEqualFileContents(fileName, actual, message, (object[]) args);
        }

    }

    partial class Assume {

        public static void EqualFileContents(string fileName, string actual) {
            Global.EqualFileContents(fileName, actual);
        }

        public static void EqualFileContents(string fileName, string actual, string message, params object[] args) {
            Global.EqualFileContents(fileName, actual, message, (object[]) args);
        }

        public static void NotEqualFileContents(string fileName, string actual) {
            Global.NotEqualFileContents(fileName, actual);
        }

        public static void NotEqualFileContents(string fileName, string actual, string message, params object[] args) {
            Global.NotEqualFileContents(fileName, actual, message, (object[]) args);
        }

    }


    namespace TestMatchers {

        public class EqualFileContentsMatcher : TestMatcher<string> {

            public string FileName { get; private set; }

            public EqualFileContentsMatcher(string fileName) {
                FileName = fileName;
            }

            public override bool Matches(string actual) {
                string expected = File.ReadAllText(FileName);
                return string.Equals(expected, actual);
            }

        }
    }

}
