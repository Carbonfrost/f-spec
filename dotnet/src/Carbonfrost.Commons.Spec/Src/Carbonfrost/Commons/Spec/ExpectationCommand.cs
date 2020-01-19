//
// Copyright 2017, 2018 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.Commons.Spec {

    static partial class ExpectationCommand {

    }

    abstract class ExpectationCommand<T> {

        public virtual IExpectationCommand Untyped() {
            throw new NotImplementedException();
        }

        public abstract TestFailure Should(ITestMatcher<T> matcher);

        public abstract ExpectationCommand<T> Negated();

        public abstract ExpectationCommand<TBase> As<TBase>();

        // Given that T is IEnumerable -- convert to accumulator commands.
        // We can't know the type of the value (e.g. if T == IEnumerable<TValue>,
        // then TValue is unknown to us).  So we use object here, and implementations
        // should implicitly apply As<TValue>()
        public virtual ExpectationCommand<object> ToAll() {
            throw new NotImplementedException();
        }

        public virtual ExpectationCommand<object> ToAny() {
            throw new NotImplementedException();
        }

        public virtual ExpectationCommand<object> Cardinality(int? min, int? max) {
            throw new NotImplementedException();
        }

        public virtual ExpectationCommand<T> Eventually(TimeSpan duration) {
            throw new NotImplementedException();
        }

        public virtual ExpectationCommand<T> Consistently(TimeSpan duration) {
            throw new NotImplementedException();
        }

        public virtual void Implies(CommandCondition c) {
        }
    }

    enum CommandCondition {
        ExactlyOne,
        NotOneButZeroOrMore,
    }
}
