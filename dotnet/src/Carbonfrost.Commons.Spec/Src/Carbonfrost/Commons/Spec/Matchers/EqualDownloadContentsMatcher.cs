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
using System.Linq;

using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.Commons.Spec {

    partial class Matchers {

        public static EqualDownloadContentsMatcher EqualDownloadContents(Uri source) {
            return new EqualDownloadContentsMatcher(source);
        }

    }

    static partial class Extensions {

        public static void EqualToDownloadContents(this Expectation<string> e, Uri source) {
            EqualToDownloadContents(e, source, null);
        }

        public static void EqualToDownloadContents(this Expectation<string> e, Uri source, string message, params object[] args) {
            e.Should(Matchers.EqualDownloadContents(source), message, (object[]) args);
        }

    }

    partial class Asserter {

        public void EqualDownloadContents(Uri source, string actual) {
            EqualDownloadContents(source, actual, null);
        }

        public void EqualDownloadContents(Uri source, string actual, string message, params object[] args) {
            That(actual, Matchers.EqualDownloadContents(source), message, (object[]) args);
        }

        public void NotEqualDownloadContents(Uri source, string actual) {
            NotEqualDownloadContents(source, actual, null);
        }

        public void NotEqualDownloadContents(Uri source, string actual, string message, params object[] args) {
            NotThat(actual, Matchers.EqualDownloadContents(source), message, (object[]) args);
        }

    }

    partial class Assert {

        public static void EqualDownloadContents(Uri source, string actual) {
            Global.EqualDownloadContents(source, actual);
        }

        public static void EqualDownloadContents(Uri source, string actual, string message, params object[] args) {
            Global.EqualDownloadContents(source, actual, message, (object[]) args);
        }

        public static void NotEqualDownloadContents(Uri source, string actual) {
            Global.NotEqualDownloadContents(source, actual);
        }

        public static void NotEqualDownloadContents(Uri source, string actual, string message, params object[] args) {
            Global.NotEqualDownloadContents(source, actual, message, (object[]) args);
        }

    }

    partial class Assume {

        public static void EqualDownloadContents(Uri source, string actual) {
            Global.EqualDownloadContents(source, actual);
        }

        public static void EqualDownloadContents(Uri source, string actual, string message, params object[] args) {
            Global.EqualDownloadContents(source, actual, message, (object[]) args);
        }

        public static void NotEqualDownloadContents(Uri source, string actual) {
            Global.NotEqualDownloadContents(source, actual);
        }

        public static void NotEqualDownloadContents(Uri source, string actual, string message, params object[] args) {
            Global.NotEqualDownloadContents(source, actual, message, (object[]) args);
        }

    }


    namespace TestMatchers {

        public class EqualDownloadContentsMatcher : TestMatcher<string> {

            public Uri Source { get; private set; }

            public EqualDownloadContentsMatcher(Uri source) {
                Source = source;
            }

            public override bool Matches(string actual) {
                string expected = StreamContext.FromSource(Source).ReadAllText();
                return string.Equals(expected, actual);
            }

        }
    }

}
