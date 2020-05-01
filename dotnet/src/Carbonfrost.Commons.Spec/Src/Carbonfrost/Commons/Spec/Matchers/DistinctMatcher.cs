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

        public static DistinctMatcher BeDistinct() {
            return new DistinctMatcher();
        }

        public static DistinctMatcher<TSource> BeDistinct<TSource>() {
            return new DistinctMatcher<TSource>(EqualityComparer<TSource>.Default);
        }

        public static DistinctMatcher<TSource> BeDistinct<TSource>(IEqualityComparer<TSource> comparer) {
            return new DistinctMatcher<TSource>(comparer);
        }

        public static DistinctMatcher<TSource> BeDistinct<TSource>(Comparison<TSource> comparison) {
            return new DistinctMatcher<TSource>(new Assert.EqualityComparisonAdapter<TSource>(comparison));
        }

        public static DistinctMatcher<string> BeDistinct(StringComparison comparison) {
            return new DistinctMatcher<string>(comparison.MapComparer());
        }

    }

    static partial class Extensions {

        public static void Distinct<TSource>(this Expectation<IEnumerable<TSource>> e) {
            Distinct(e, (string) null);
        }

        // public static void Distinct<TSource>(this Expectation<TSource[]> e) {
        //     Distinct<TSource>(e.As<IEnumerable<TSource>>(), (string) null);
        // }

        // FIXME Missing
        public static void Distinct<TSource>(this Expectation<IEnumerable<TSource>> e, Comparison<TSource> comparison) {
            Distinct(e, comparison, null);
        }

        public static void Distinct(this Expectation<IEnumerable<string>> e, StringComparison comparison) {
            Distinct(e, comparison, null);
        }

        public static void Distinct<TSource>(this Expectation<IEnumerable<TSource>> e, IEqualityComparer<TSource> comparer) {
            Distinct(e, comparer, null);
        }

        public static void Distinct<TSource>(this Expectation<IEnumerable<TSource>> e, string message, params object[] args) {
            e.As<IEnumerable<TSource>>().Should(Matchers.BeDistinct<TSource>(), message, (object[]) args);
        }

        public static void Distinct<TSource>(this Expectation<IEnumerable<TSource>> e, Comparison<TSource> comparison, string message, params object[] args) {
            e.As<IEnumerable<TSource>>().Should(Matchers.BeDistinct(comparison), message, (object[]) args);
        }

        public static void Distinct<TSource, TValue>(this Expectation<TSource> e, string message, params object[] args)
            where TSource: IEnumerable<TValue> {
            e.As<IEnumerable<TSource>>().Should(Matchers.BeDistinct<TSource>(), message, (object[]) args);
        }

        // FIXME Missing
        // public static void Distinct<TSource>(this Expectation<IEnumerable<TSource>> e, Comparison<TSource> comparison, string message, params object[] args) {
        //     e.As<IEnumerable<TSource>>().Should(Matchers.BeDistinct(comparison), message, (object[]) args);
        // }

        public static void Distinct(this Expectation<IEnumerable<string>> e, StringComparison comparison, string message, params object[] args) {
            e.As<IEnumerable<string>>().Should(Matchers.BeDistinct(comparison), message, (object[]) args);
        }

        public static void Distinct<TSource>(this Expectation<IEnumerable<TSource>> e, IEqualityComparer<TSource> comparer, string message, params object[] args) {
            e.As<IEnumerable<TSource>>().Should(Matchers.BeDistinct(comparer), message, (object[]) args);
        }
    }

    partial class Asserter {

        public void Distinct<TSource>(IEnumerable<TSource> actual) {
            Distinct(actual, EqualityComparer<TSource>.Default);
        }

        public void Distinct(IEnumerable<string> actual, StringComparison comparison) {
            Distinct<string>(actual, comparison.MapComparer());
        }

        public void Distinct<TSource>(IEnumerable<TSource> actual, Comparison<TSource> comparison) {
            Distinct<TSource>(actual, new Assert.EqualityComparisonAdapter<TSource>(comparison));
        }

        public void Distinct<TSource>(IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer) {
            That(actual, Matchers.BeDistinct(comparer));
        }

        public void NotDistinct<TSource>(IEnumerable<TSource> actual) {
            NotDistinct<TSource>(actual, EqualityComparer<TSource>.Default);
        }

        public void NotDistinct(IEnumerable<string> actual, StringComparison comparison) {
            NotDistinct<string>(actual, comparison.MapComparer());
        }

        public void NotDistinct<TSource>(IEnumerable<TSource> actual, Comparison<TSource> comparison) {
            NotDistinct<TSource>(actual, new Assert.EqualityComparisonAdapter<TSource>(comparison));
        }

        public void NotDistinct<TSource>(IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer) {
            NotThat(actual, Matchers.BeDistinct(comparer));
        }

        public void Distinct<TSource>(IEnumerable<TSource> actual, string message, params object[] args) {
            Distinct(actual, EqualityComparer<TSource>.Default, message, args);
        }

        public void Distinct(IEnumerable<string> actual, StringComparison comparison, string message, params object[] args) {
            Distinct<string>(actual, comparison.MapComparer(), message, args);
        }

        public void Distinct<TSource>(IEnumerable<TSource> actual, Comparison<TSource> comparison, string message, params object[] args) {
            Distinct<TSource>(actual, new Assert.EqualityComparisonAdapter<TSource>(comparison), message, args);
        }

        public void Distinct<TSource>(IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer, string message, params object[] args) {
            That(actual, Matchers.BeDistinct(comparer), message, args);
        }

        public void NotDistinct<TSource>(IEnumerable<TSource> actual, string message, params object[] args) {
            NotDistinct<TSource>(actual, EqualityComparer<TSource>.Default, message, args);
        }

        public void NotDistinct(IEnumerable<string> actual, StringComparison comparison, string message, params object[] args) {
            NotDistinct<string>(actual, comparison.MapComparer(), message, args);
        }

        public void NotDistinct<TSource>(IEnumerable<TSource> actual, Comparison<TSource> comparison, string message, params object[] args) {
            NotDistinct<TSource>(actual, new Assert.EqualityComparisonAdapter<TSource>(comparison), message, args);
        }

        public void NotDistinct<TSource>(IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer, string message, params object[] args) {
            NotThat(actual, Matchers.BeDistinct(comparer), message, args);
        }
    }

    partial class Assert {

        public static void Distinct<TSource>(IEnumerable<TSource> actual) {
            Global.Distinct<TSource>(actual);
        }

        public static void Distinct(IEnumerable<string> actual, StringComparison comparison) {
            Global.Distinct(actual, comparison);
        }

        public static void Distinct<TSource>(IEnumerable<TSource> actual, Comparison<TSource> comparison) {
            Global.Distinct<TSource>(actual, comparison);
        }

        public static void Distinct<TSource>(IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer) {
            Global.Distinct<TSource>(actual, comparer);
        }

        public static void NotDistinct<TSource>(IEnumerable<TSource> actual) {
            Global.NotDistinct<TSource>(actual);
        }

        public static void NotDistinct(IEnumerable<string> actual, StringComparison comparison) {
            Global.NotDistinct(actual, comparison);
        }

        public static void NotDistinct<TSource>(IEnumerable<TSource> actual, Comparison<TSource> comparison) {
            Global.NotDistinct<TSource>(actual, comparison);
        }

        public static void NotDistinct<TSource>(IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer) {
            Global.NotDistinct<TSource>(actual, comparer);
        }

        public static void Distinct<TSource>(IEnumerable<TSource> actual, string message, params object[] args) {
            Global.Distinct<TSource>(actual, message, (object[]) args);
        }

        public static void Distinct(IEnumerable<string> actual, StringComparison comparison, string message, params object[] args) {
            Global.Distinct(actual, comparison, message, (object[]) args);
        }

        public static void Distinct<TSource>(IEnumerable<TSource> actual, Comparison<TSource> comparison, string message, params object[] args) {
            Global.Distinct<TSource>(actual, comparison, message, (object[]) args);
        }

        public static void Distinct<TSource>(IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer, string message, params object[] args) {
            Global.Distinct<TSource>(actual, comparer, message, (object[]) args);
        }

        public static void NotDistinct<TSource>(IEnumerable<TSource> actual, string message, params object[] args) {
            Global.NotDistinct<TSource>(actual, message, (object[]) args);
        }

        public static void NotDistinct(IEnumerable<string> actual, StringComparison comparison, string message, params object[] args) {
            Global.NotDistinct(actual, comparison, message, (object[]) args);
        }

        public static void NotDistinct<TSource>(IEnumerable<TSource> actual, Comparison<TSource> comparison, string message, params object[] args) {
            Global.NotDistinct<TSource>(actual, comparison, message, (object[]) args);
        }

        public static void NotDistinct<TSource>(IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer, string message, params object[] args) {
            Global.NotDistinct<TSource>(actual, comparer, message, (object[]) args);
        }
    }

    partial class Assume {

        public static void Distinct<TSource>(IEnumerable<TSource> actual) {
            Global.Distinct<TSource>(actual);
        }

        public static void Distinct(IEnumerable<string> actual, StringComparison comparison) {
            Global.Distinct(actual, comparison);
        }

        public static void Distinct<TSource>(IEnumerable<TSource> actual, Comparison<TSource> comparison) {
            Global.Distinct<TSource>(actual, comparison);
        }

        public static void Distinct<TSource>(IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer) {
            Global.Distinct<TSource>(actual, comparer);
        }

        public static void NotDistinct<TSource>(IEnumerable<TSource> actual) {
            Global.NotDistinct<TSource>(actual);
        }

        public static void NotDistinct(IEnumerable<string> actual, StringComparison comparison) {
            Global.NotDistinct(actual, comparison);
        }

        public static void NotDistinct<TSource>(IEnumerable<TSource> actual, Comparison<TSource> comparison) {
            Global.NotDistinct<TSource>(actual, comparison);
        }

        public static void NotDistinct<TSource>(IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer) {
            Global.NotDistinct<TSource>(actual, comparer);
        }

        public static void Distinct<TSource>(IEnumerable<TSource> actual, string message, params object[] args) {
            Global.Distinct<TSource>(actual, message, (object[]) args);
        }

        public static void Distinct(IEnumerable<string> actual, StringComparison comparison, string message, params object[] args) {
            Global.Distinct(actual, comparison, message, (object[]) args);
        }

        public static void Distinct<TSource>(IEnumerable<TSource> actual, Comparison<TSource> comparison, string message, params object[] args) {
            Global.Distinct<TSource>(actual, comparison, message, (object[]) args);
        }

        public static void Distinct<TSource>(IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer, string message, params object[] args) {
            Global.Distinct<TSource>(actual, comparer, message, (object[]) args);
        }

        public static void NotDistinct<TSource>(IEnumerable<TSource> actual, string message, params object[] args) {
            Global.NotDistinct<TSource>(actual, message, (object[]) args);
        }

        public static void NotDistinct(IEnumerable<string> actual, StringComparison comparison, string message, params object[] args) {
            Global.NotDistinct(actual, comparison, message, (object[]) args);
        }

        public static void NotDistinct<TSource>(IEnumerable<TSource> actual, Comparison<TSource> comparison, string message, params object[] args) {
            Global.NotDistinct<TSource>(actual, comparison, message, (object[]) args);
        }

        public static void NotDistinct<TSource>(IEnumerable<TSource> actual, IEqualityComparer<TSource> comparer, string message, params object[] args) {
            Global.NotDistinct<TSource>(actual, comparer, message, (object[]) args);
        }
    }

    namespace TestMatchers {

        public class DistinctMatcher : TestMatcher<System.Collections.IEnumerable> {

            public DistinctMatcher<TSource> WithComparer<TSource>(IEqualityComparer<TSource> comparer) {
                return new DistinctMatcher<TSource>(comparer);
            }

            public DistinctMatcher<TSource> WithComparer<TSource>(IComparer<TSource> comparer) {
                if (comparer == null) {
                    return new DistinctMatcher<TSource>();
                }
                return new DistinctMatcher<TSource>(new Assert.EqualityComparerAdapter<TSource>(comparer));
            }

            public DistinctMatcher<TSource> WithComparison<TSource>(IEqualityComparer<TSource> comparer) {
                return new DistinctMatcher<TSource>(comparer);
            }

            public override bool Matches(System.Collections.IEnumerable actual) {
                var comparer = EqualityComparer<object>.Default;
                var tally = new HashSet<object>(comparer);
                foreach (var a in actual) {
                    if (!tally.Add(a)) {
                        return false;
                    }
                }
                return true;
            }
        }

        public class DistinctMatcher<TSource> : TestMatcher<IEnumerable<TSource>>, ITestMatcherWithEqualityComparerApiConventions<DistinctMatcher<TSource>, TSource> {

            public IEqualityComparer<TSource> Comparer {
                get;
                private set;
            }

            public DistinctMatcher() {
            }

            public DistinctMatcher(IEqualityComparer<TSource> comparer) {
                Comparer = comparer;
            }

            public DistinctMatcher(Comparison<TSource> comparison) {
                if (comparison != null) {
                    Comparer = new Assert.EqualityComparisonAdapter<TSource>(comparison);
                }
            }

            public DistinctMatcher<TSource> WithComparer(IEqualityComparer<TSource> comparer) {
                return new DistinctMatcher<TSource>(comparer);
            }

            public DistinctMatcher<TSource> WithComparer(IComparer<TSource> comparer) {
                if (comparer == null) {
                    return new DistinctMatcher<TSource>();
                }
                return new DistinctMatcher<TSource>(new Assert.EqualityComparerAdapter<TSource>(comparer));
            }

            public DistinctMatcher<TSource> WithComparison(Comparison<TSource> comparison) {
                return new DistinctMatcher<TSource>(comparison);
            }

            public override bool Matches(IEnumerable<TSource> actual) {
                var comparer = Comparer ?? EqualityComparer<TSource>.Default;
                var tally = new HashSet<TSource>(comparer);
                foreach (var a in actual) {
                    if (!tally.Add(a)) {
                        return false;
                    }
                }
                return true;
            }
        }
    }

}
