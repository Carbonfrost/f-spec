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
using System.Reflection;

using Carbonfrost.Commons.Spec.ExecutionModel;
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.Commons.Spec {

    public abstract class TestMatcher {

        static readonly MethodInfo AdapterMethod = typeof(TestMatcher).GetMethod("Adapter_", BindingFlags.NonPublic | BindingFlags.Static);

        public static readonly ITestMatcher Anything = new InvariantMatcher(true);
        public static readonly ITestMatcher Nothing = new InvariantMatcher(false);

        internal static ITestMatcher<Unit> UnitWrapper(ITestMatcher matcher) {
            return new DispatchWrapper(matcher);
        }

        internal static object AllowingNullActualValue(object matcher) {
            if (matcher is ITestMatcherValidations val) {
                return val.AllowingNullActualValue();
            }
            return matcher;
        }

        internal interface IInvariantTestMatcher {
            bool Matches();
        }

        internal static object Adapter(Type fromType, Type toType, object instance) {
            return AdapterMethod.MakeGenericMethod(fromType, toType).Invoke(null, new[] { instance });
        }

        internal static ITestMatcher<TTo> Adapter_<TFrom, TTo>(ITestMatcher<TFrom> instance) where TFrom : TTo {
            return new AdapterImpl<TFrom, TTo>(instance);
        }

        class InvariantMatcher : ITestMatcher, IInvariantTestMatcher {

            private readonly bool _answer;

            public InvariantMatcher(bool answer) {
                _answer = answer;
            }

            public bool Matches(ITestActualEvaluation testCode) {
                return _answer;
            }

            public bool Matches() {
                return _answer;
            }
        }

        struct DispatchWrapper : ITestMatcher<Unit>, ISupportTestMatcher {
            private readonly ITestMatcher _matcher;

            [MatcherUserData(Hidden = true)]
            public object RealMatcher {
                get {
                    return _matcher;
                }
            }

            public DispatchWrapper(ITestMatcher matcher) {
                _matcher = matcher;
            }

            public bool Matches(ITestActualEvaluation<Unit> actualFactory) {
                return _matcher.Matches(actualFactory);
            }
        }

        struct AdapterImpl<TFrom, TTo> : ITestMatcher<TTo>, ISupportTestMatcher
            where TFrom : TTo {
            private readonly ITestMatcher<TFrom> _matcher;

            [MatcherUserData(Hidden = true)]
            public object RealMatcher {
                get {
                    return _matcher;
                }
            }

            public AdapterImpl(ITestMatcher<TFrom> matcher) {
                _matcher = matcher;
            }

            public bool Matches(ITestActualEvaluation<TTo> actualFactory) {
                return _matcher.Matches(new WrapperEvaluation(actualFactory));
            }

            struct WrapperEvaluation : ITestActualEvaluation<TFrom> {
                private readonly ITestActualEvaluation<TTo> _inner;

                public WrapperEvaluation(ITestActualEvaluation<TTo> inner) {
                    _inner = inner;
                }

                public TFrom Value {
                    get {
                        return (TFrom) _inner.Value;
                    }
                }

                public Exception Exception {
                    get {
                        return _inner.Exception;
                    }
                }
            }
        }
    }

    public abstract class TestMatcher<T> : ITestMatcher<T> {

        public static readonly ITestMatcher<T> Anything = new InvariantMatcher(true);
        public static readonly ITestMatcher<T> Nothing = new InvariantMatcher(false);

        public abstract bool Matches(T actual);

        public virtual bool Matches(ITestActualEvaluation<T> actual) {
            if (actual == null) {
                return false;
            }
            return Matches(actual.Value);
        }

        internal int CompareSafely(IComparer<T> comparer, T actual, T expected) {
            try {
                return comparer.Compare(actual, expected);

            } catch (Exception e) when (e is ArgumentException || e is InvalidCastException) {
                var name = TestMatcherName.FromType(GetType());
                throw SpecFailure.UnusableComparer(name, comparer, e);
            }
        }

        class InvariantMatcher : ITestMatcher<T> {

            private readonly bool _answer;

            public InvariantMatcher(bool answer) {
                _answer = answer;
            }

            public bool Matches(ITestActualEvaluation<T> actualFactory) {
                return _answer;
            }
        }
    }
}
