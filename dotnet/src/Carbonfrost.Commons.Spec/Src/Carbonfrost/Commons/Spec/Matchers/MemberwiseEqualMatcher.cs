//
// Copyright 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.Commons.Spec {

    partial class Matchers {

        public static MemberwiseEqualMatcher<T> BeMemberwiseEqualTo<T>(T expected) {
            return new MemberwiseEqualMatcher<T>(expected, TestMemberFilter.Public);
        }

        public static MemberwiseEqualMatcher<T> BeMemberwiseEqualTo<T>(T expected, params TestMemberFilter[] memberFilters) {
            return new MemberwiseEqualMatcher<T>(expected, memberFilters);
        }

        public static MemberwiseEqualMatcher BeMemberwiseEqualTo(IEnumerable<KeyValuePair<string, object>> expected) {
            return new MemberwiseEqualMatcher(expected);
        }

        public static MemberwiseEqualMatcher BeMemberwiseEqualTo(object expected) {
            return new MemberwiseEqualMatcher(true, expected, TestMemberFilter.Public);
        }

    }

    static partial class Extensions {

        public static void MemberwiseEqualTo<T>(this IExpectation<T> e, T expected) {
            MemberwiseEqualTo<T>(e, expected, (string) null);
        }

        public static void MemberwiseEqualTo<T>(this IExpectation<T> e, T expected, string message, params object[] args) {
            e.Like(Matchers.BeMemberwiseEqualTo(expected));
        }

        public static void MemberwiseEqualTo<T>(this IExpectation<T> e, object expected) {
            MemberwiseEqualTo<T>(e, expected, (string) null);
        }

        public static void MemberwiseEqualTo<T>(this IExpectation<T> e, object expected, string message, params object[] args) {
            e.As<object>().Like(Matchers.BeMemberwiseEqualTo(expected));
        }

        public static void MemberwiseEqualTo<T>(this IExpectation<T> e, IEnumerable<KeyValuePair<string, object>> expected) {
            MemberwiseEqualTo<T>(e, expected, (string) null);
        }

        public static void MemberwiseEqualTo<T>(this IExpectation<T> e, IEnumerable<KeyValuePair<string, object>> expected, string message, params object[] args) {
            e.As<object>().Like(Matchers.BeMemberwiseEqualTo(expected));
        }

    }

    partial class Asserter {

        public void MemberwiseEqual<T>(T expected, T actual) {
            That(actual, Matchers.BeMemberwiseEqualTo(expected));
        }

        public void NotMemberwiseEqual<T>(T expected, T actual) {
            NotThat(actual, Matchers.BeMemberwiseEqualTo(expected));
        }

        public void MemberwiseEqual<T>(T expected, T actual, string message, params object[] args) {
            That(actual, Matchers.BeMemberwiseEqualTo(expected), message, args);
        }

        public void NotMemberwiseEqual<T>(T expected, T actual, string message, params object[] args) {
            NotThat(actual, Matchers.BeMemberwiseEqualTo(expected), message, args);
        }

        public void MemberwiseEqual(IEnumerable<KeyValuePair<string, object>> expected, object actual, string message, params object[] args) {
            That(actual, Matchers.BeMemberwiseEqualTo(expected), message, args);
        }

        public void NotMemberwiseEqual(IEnumerable<KeyValuePair<string, object>> expected, object actual, string message, params object[] args) {
            NotThat(actual, Matchers.BeMemberwiseEqualTo(expected), message, args);
        }
    }

    partial class Assert {

        public static void MemberwiseEqual<T>(T expected, T actual) {
            Global.MemberwiseEqual<T>(expected, actual);
        }

        public static void NotMemberwiseEqual<T>(T expected, T actual) {
            Global.NotMemberwiseEqual<T>(expected, actual);
        }

        public static void MemberwiseEqual<T>(T expected, T actual, string message, params object[] args) {
            Global.MemberwiseEqual<T>(expected, actual, message, (object[]) args);
        }

        public static void NotMemberwiseEqual<T>(T expected, T actual, string message, params object[] args) {
            Global.NotMemberwiseEqual<T>(expected, actual, message, (object[]) args);
        }

        public static void MemberwiseEqual(IEnumerable<KeyValuePair<string, object>> expected, object actual, string message, params object[] args) {
            Global.MemberwiseEqual(expected, actual, message, (object[]) args);
        }

        public static void NotMemberwiseEqual(IEnumerable<KeyValuePair<string, object>> expected, object actual, string message, params object[] args) {
            Global.NotMemberwiseEqual(expected, actual, message, (object[]) args);
        }
    }

    partial class Assume {

        public static void MemberwiseEqual<T>(T expected, T actual) {
            Global.MemberwiseEqual<T>(expected, actual);
        }

        public static void NotMemberwiseEqual<T>(T expected, T actual) {
            Global.NotMemberwiseEqual<T>(expected, actual);
        }

        public static void MemberwiseEqual<T>(T expected, T actual, string message, params object[] args) {
            Global.MemberwiseEqual<T>(expected, actual, message, (object[]) args);
        }

        public static void NotMemberwiseEqual<T>(T expected, T actual, string message, params object[] args) {
            Global.NotMemberwiseEqual<T>(expected, actual, message, (object[]) args);
        }

        public static void MemberwiseEqual(IEnumerable<KeyValuePair<string, object>> expected, object actual, string message, params object[] args) {
            Global.MemberwiseEqual(expected, actual, message, (object[]) args);
        }

        public static void NotMemberwiseEqual(IEnumerable<KeyValuePair<string, object>> expected, object actual, string message, params object[] args) {
            Global.NotMemberwiseEqual(expected, actual, message, (object[]) args);
        }
    }

    namespace TestMatchers {

        public class MemberwiseEqualMatcher<T> : TestMatcher<T>, ITestMatcherActualDiff {

            private MemberwiseEqualMatcher _innerMatcherCache;

            public T Expected {
                get;
            }

            public TestMemberFilter MemberFilter {
                get;
            }

            public IList<string> Differences {
                get {
                    return InnerMatcher.Differences;
                }
            }

            private MemberwiseEqualMatcher InnerMatcher {
                get {
                    if (_innerMatcherCache == null) {
                        _innerMatcherCache = new MemberwiseEqualMatcher(
                            false, Expected, MemberFilter
                        );
                    }
                    return _innerMatcherCache;
                }
            }

            public MemberwiseEqualMatcher(T expected, params TestMemberFilter[] memberFilters) {
                Expected = expected;
                MemberFilter = TestMemberFilter.Compose(memberFilters);
            }

            public override bool Matches(T actual) {
                if (actual == null) {
                    return false;
                }
                if (ReferenceEquals(actual, Expected)) {
                    return true;
                }
                return InnerMatcher.Matches(actual);
            }

            Patch ITestMatcherActualDiff.GetPatch(object actual) {
                return ((ITestMatcherActualDiff) InnerMatcher).GetPatch(actual);
            }
        }

        public partial class MemberwiseEqualMatcher : TestMatcher<object>, ITestMatcherActualDiff {

            private PatchResult _patchCache;
            private readonly ExpectedAccessors _expected;

            public IReadOnlyDictionary<string, object> Expected {
                get {
                    return _expected;
                }
            }

            public IList<string> Differences {
                get {
                    if (_patchCache == null) {
                        return Array.Empty<string>();
                    }
                    return _patchCache.Differences;
                }
            }

            public MemberwiseEqualMatcher(IEnumerable<KeyValuePair<string, object>> expected) {
                _expected = new ExpectedAccessors(expected);
            }

            internal MemberwiseEqualMatcher(bool untypedTarget, object expected, TestMemberFilter filter) {
                _expected = new ExpectedAccessors(untypedTarget, expected, filter);
            }

            public override bool Matches(object actual) {
                if (actual == null) {
                    return false;
                }

                return CalcPatch(actual).Result;
            }

            Patch ITestMatcherActualDiff.GetPatch(object actual) {
                return CalcPatch(actual).ToPatch();
            }

            private PatchResult CalcPatch(object actual) {
                if (_patchCache != null && _patchCache.ActualObject == actual) {
                    // Patch calculation happens once on match and once or displaying the
                    // diff, so we cache the result to keep a consistent comparison
                    return _patchCache;
                }

                _patchCache = new PatchResult() {
                    ActualObject = actual,
                    Result = true
                };

                _patchCache.Actual.Add("{");
                _patchCache.Expected.Add("{");

                foreach (var item in _expected.Data) {
                    var actualFieldValue = item.Accessor.GetValue(actual);
                    bool success = item.Matcher.Matches(() => actualFieldValue);
                    string expectedLine = DisplayActual.Create(actualFieldValue, 0).Format(DisplayActualOptions.None);
                    string actualLine = DisplayActual.Create(item.Specified, 0).Format(DisplayActualOptions.None);
                    string prefix = $"  {item.Name} = ";

                    if (success) {
                        // Require them to be equal so that no differences show in patches
                        expectedLine = actualLine;
                    } else {
                        _patchCache.Result = false;
                        // When actual and expected are the same and wouldn't difference
                        if (expectedLine == actualLine) {
                            expectedLine += "*";
                        }
                        Differences.Add(item.Name);
                    }

                    _patchCache.Actual.Add(prefix + actualLine);
                    _patchCache.Expected.Add(prefix + expectedLine);
                }

                _patchCache.Actual.Add("}");
                _patchCache.Expected.Add("}");
                return _patchCache;
            }

            private class PatchResult {
                public readonly List<string> Actual = new List<string>();
                public readonly List<string> Expected = new List<string>();
                public readonly List<string> Differences = new List<string>();
                public bool Result;
                public object ActualObject;

                public Patch ToPatch() {
                    return Patch.StandardTextPatch(Expected, Actual);
                }
            }
        }
    }

}
