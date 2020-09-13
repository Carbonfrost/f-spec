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

        public static readonly TestTagPredicate Anything = Invariant(true);
        public static readonly TestTagPredicate Nothing = Invariant(false);
        public static readonly TestTagPredicate Dynamic = Exactly(TestTag.Dynamic);

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
            return null;
        }

        private static TestTagPredicate _TryParseOnePossiblyNegated(string text) {
            text = text.Trim();
            if (text.Length == 0) {
                return null;
            }
            if (text == "*") {
                return TestTagPredicate.Anything;
            }
            if (text == "~") {
                return TestTagPredicate.Nothing;
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
        public abstract override bool Equals(object obj);
        public abstract override int GetHashCode();

        public static bool operator ==(TestTagPredicate x, TestTagPredicate y) {
            return x.Equals(y);
        }

        public static bool operator !=(TestTagPredicate x, TestTagPredicate y) {
            return !x.Equals(y);
        }

        public static TestTagPredicate And(IEnumerable<TestTagPredicate> items) {
            return Composite(items, items2 => new AndImpl(items2));
        }

        public static TestTagPredicate And(params TestTagPredicate[] items) {
            return And((IEnumerable<TestTagPredicate>) items);
        }

        public static TestTagPredicate Or(IEnumerable<TestTagPredicate> items) {
            return Composite(items, items2 => new OrImpl(items2));
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

        public static TestTagPredicate Invariant(bool invariant) {
            return new InvariantImpl(invariant);
        }

        public static TestTagPredicate Exactly(TestTag tag) {
            return new ExactlyImpl(tag);
        }

        public static TestTagPredicate Exactly(TestTagType type, string value) {
            return Exactly(new TestTag(type, value));
        }

        public static TestTagPredicate Platform(string value) {
            return Exactly(TestTagType.Platform, value);
        }

        public static TestTagPredicate Previously(TestStatus value) {
            return Exactly(TestTag.Previously(value));
        }

        private static TestTagPredicate Composite(IEnumerable<TestTagPredicate> items, Func<TestTagPredicate[], CompositeImpl> factory) {
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
            return factory(myItems);
        }

        sealed class InvariantImpl : TestTagPredicate {
            private readonly bool _value;
            public InvariantImpl(bool value) {
                _value = value;
            }
            public override bool IsMatch(TestUnit unit) {
                return _value;
            }

            public override bool Equals(object obj) {
                return obj is InvariantImpl inv && _value == inv._value;
            }

            public override int GetHashCode() {
                return _value.GetHashCode();
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

            public override bool Equals(object obj) {
                return obj is ExactlyImpl tt && _tag == tt._tag;
            }

            public override int GetHashCode() {
                return _tag.GetHashCode();
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

            public override bool Equals(object obj) {
                return obj is NegatedImpl negated && _inner.Equals(negated._inner);
            }

            public override int GetHashCode() {
                return 37 * _inner.GetHashCode();
            }
        }

        abstract class CompositeImpl : TestTagPredicate {
            private readonly TestTagPredicate[] _items;

            protected IReadOnlyList<TestTagPredicate> Items {
                get {
                    return _items;
                }
            }

            protected CompositeImpl(TestTagPredicate[] items) {
                _items = items;
            }

            public override string ToString() {
                return base.ToString();
            }

            public sealed override bool Equals(object obj) {
                if (obj == null) {
                    return false;
                }
                return GetType() == obj.GetType()
                    && obj is CompositeImpl ci
                    && ci.Items.Count == Items.Count
                    && ci.Items.SequenceEqual(Items);
            }

            public sealed override int GetHashCode() {
                unchecked {
                    long result = GetType().GetHashCode();
                    foreach (var i in Items) {
                        result = ((result << 5) + result) ^ i.GetHashCode();
                    }

                    return (int) result;
                }
            }
        }

        class AndImpl : CompositeImpl {
            public AndImpl(TestTagPredicate[] items) : base(items) {
            }

            public override bool IsMatch(TestUnit unit) {
                return Items.All( p => p.IsMatch(unit));
            }
        }

        class OrImpl : CompositeImpl {
            public OrImpl(TestTagPredicate[] items) : base(items) {
            }

            public override bool IsMatch(TestUnit unit) {
                return Items.Any( p => p.IsMatch(unit));
            }
        }
    }
}
