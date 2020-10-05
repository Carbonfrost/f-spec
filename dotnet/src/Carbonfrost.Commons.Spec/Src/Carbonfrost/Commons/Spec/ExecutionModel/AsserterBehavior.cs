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

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    abstract class AsserterBehavior {

        public static readonly AsserterBehavior Default = new DefaultBehavior();
        public static readonly AsserterBehavior Assumption = new AssumptionBehavior();
        public static readonly AsserterBehavior Disabled = new DisabledBehavior();

        public abstract void Assert(TestFailure failure);

        public virtual void Fail(Exception exception) {
            throw exception;
        }

        class DefaultBehavior : AsserterBehavior {
            public override void Assert(TestFailure failure) {
                if (failure != null) {
                    Fail(failure.ToException());
                }
            }
        }

        class AssumptionBehavior : AsserterBehavior {
            public override void Assert(TestFailure failure) {
                if (failure != null) {
                    failure.UserData.IsAssumption = true;
                }
            }
        }

        class DisabledBehavior : AsserterBehavior {
            public override void Assert(TestFailure failure) {
                if (failure != null) {
                    Fail(failure.ToException());
                }
            }
            public override void Fail(Exception exception) {
                TestContext.Current.Log.AsserterDisabled(exception);
            }
        }
    }
}
