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
using System.Text.RegularExpressions;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    public abstract class TestPlanFilterPattern {

        public abstract bool IsMatch(TestUnit test);

        public static TestPlanFilterPattern Parse(string text) {
            TestPlanFilterPattern result;
            Exception ex = _TryParse(text, out result);
            if (ex == null)
                return result;
            else
                throw ex;
        }

        public static bool TryParse(string text, out TestPlanFilterPattern value) {
            return _TryParse(text, out value) == null;
        }

        private static Exception _TryParse(string text, out TestPlanFilterPattern result) {
            if (text == null) {
                result = null;
                return new ArgumentNullException();
            }

            text = text.Trim();
            if (text.Length == 0) {
                result = null;
                return SpecFailure.AllWhitespace(nameof(text));
            }
            string[] nv = Array.ConvertAll(
                text.Split(new [] { ':' }, 3), t => t.Trim()
            );
            if (nv.Length == 2 && nv[0] == "regex") {
                return _TryParseRegex(nv[1], out result);

            } else {
                result = TestPlanFilterPattern.Wildcard(text);
                return null;
            }
        }

        private static Exception _TryParseRegex(string v, out TestPlanFilterPattern result) {
            try {
                result = TestPlanFilterPattern.Pattern(new Regex(v));
                return null;
            } catch {
            }
            result = null;
            return new FormatException();
        }

        public static TestPlanFilterPattern Pattern(Regex regex) {
            if (regex == null) {
                throw new ArgumentNullException(nameof(regex));
            }
            return new RegexImpl(regex);
        }

        public static TestPlanFilterPattern Wildcard(string pattern) {
            if (pattern == null) {
                throw new ArgumentNullException(nameof(pattern));
            }
            return new WildcardImpl(WildcardPattern.Containing(pattern));
        }

        private class WildcardImpl : TestPlanFilterPattern {
            private readonly WildcardPattern _wildcardPattern;

            public WildcardImpl(WildcardPattern wildcardPattern) {
                _wildcardPattern = wildcardPattern;
            }

            public override bool IsMatch(TestUnit test) {
                return _wildcardPattern.IsMatch(test.DisplayName);
            }
        }

        private class RegexImpl : TestPlanFilterPattern {
            private Regex _regex;

            public RegexImpl(Regex regex) {
                _regex = regex;
            }

            public override bool IsMatch(TestUnit test) {
                return _regex.IsMatch(test.DisplayName);
            }
        }
    }

}
