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
using System.Linq;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    public abstract class TestTagPredicate : ITestPlanFilter {

        public static readonly TestTagPredicate Anything = new InvariantImpl(true);
        public static readonly TestTagPredicate Nothing = new InvariantImpl(false);

        public static TestTagPredicate Parse(string text) {
            TestTagPredicate result;
            Exception ex = _TryParse(text, out result);
            if (ex == null)
                return result;
            else
                throw ex;
        }

        public static bool TryParse(string text, out TestTagPredicate result) {
            return _TryParse(text, out result) == null;
        }

        private static TestTagPredicate _TryParseExactly(string text) {
            if (TestTag.TryParse(text, out TestTag tag)) {
                return Exactly(tag);
            }
            Console.WriteLine("tryparseexact " + text);
            return null;
        }

        private static TestTagPredicate _TryParseOnePossiblyNegated(string text) {
            text = text.Trim();
            if (text.Length == 0) {
                return null;
            }
            if (text[0] == '~') {
                var exact = _TryParseExactly(text.Substring(1));
                if (exact == null) {
                    return null;
                }
                return Not(exact);
            }
            return _TryParseExactly(text);
        }

        private static Exception _TryParse(string text, out TestTagPredicate result) {
            if (text == null) {
                result = null;
                return new ArgumentException();
            }
            var items = Array.ConvertAll(
                text.Split(','),
                _TryParseOnePossiblyNegated
            );
            if (items.Any(i => i == null)) {
                result = null;
                return new FormatException();
            }
            result = Or(items);
            return null;
        }

        public abstract bool IsMatch(TestUnit unit);

        public static TestTagPredicate And(IEnumerable<TestTagPredicate> items) {
            return Composite(items, (items2, match) => items2.All(match));
        }

        public static TestTagPredicate And(params TestTagPredicate[] items) {
            return And((IEnumerable<TestTagPredicate>) items);
        }

        public static TestTagPredicate Or(IEnumerable<TestTagPredicate> items) {
            return Composite(items, (items2, match) => items2.Any(match));
        }

        public static TestTagPredicate Or(params TestTagPredicate[] items) {
            return Or((IEnumerable<TestTagPredicate>) items);
        }

        public static TestTagPredicate Not(TestTagPredicate item) {
            if (item == null) {
                throw new ArgumentNullException(nameof(item));
            }
            return new NegatedImpl(item);
        }

        public static TestTagPredicate Exactly(TestTag tag) {
            return new ExactlyImpl(tag);
        }

        public static TestTagPredicate Exactly(string name, string value) {
            return Exactly(new TestTag(name, value));
        }

        public static TestTagPredicate Platform(string value) {
            return Exactly(TestTagType.Platform, value);
        }

        private static TestTagPredicate Composite(IEnumerable<TestTagPredicate> items, TestTagPredicateComposer func) {
            if (items == null) {
                return Anything;
            }
            var myItems = items.ToArray();
            if (myItems.Length == 0) {
                return Anything;
            }
            if (myItems.Length == 1) {
                return myItems[0];
            }
            return new CompositeImpl(myItems, func);
        }

        sealed class InvariantImpl : TestTagPredicate {
            private readonly bool _value;
            public InvariantImpl(bool value) {
                _value = value;
            }
            public override bool IsMatch(TestUnit unit) {
                return _value;
            }
        }

        sealed class ExactlyImpl : TestTagPredicate {
            private readonly TestTag _tag;

            public ExactlyImpl(TestTag tag) {
                _tag = tag;
            }

            public override bool IsMatch(TestUnit unit) {
                return unit.Tags.Contains(_tag);
            }

            public override string ToString() {
                return _tag.ToString();
            }
        }

        internal sealed class NegatedImpl : TestTagPredicate {
            private readonly TestTagPredicate _inner;

            public NegatedImpl(TestTagPredicate inner) {
                _inner = inner;
            }

            public override bool IsMatch(TestUnit unit) {
                return !_inner.IsMatch(unit);
            }

            public override string ToString() {
                return "~" + _inner;
            }
        }

        delegate bool TestTagPredicateComposer(
            IEnumerable<TestTagPredicate> items, Func<TestTagPredicate, bool> match
         );

        class CompositeImpl : TestTagPredicate {
            private readonly TestTagPredicate[] _items;
            private readonly TestTagPredicateComposer _predicate;

            protected IEnumerable<TestTagPredicate> Items {
                get {
                    return _items;
                }
            }

            public CompositeImpl(TestTagPredicate[] items, TestTagPredicateComposer predicate) {
                _items = items;
                _predicate = predicate;
            }

            public override bool IsMatch(TestUnit unit) {
                return _predicate(_items, p => p.IsMatch(unit));
            }

            public override string ToString() {
                return base.ToString();
            }
        }
    }
}
