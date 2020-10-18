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

using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.Commons.Spec {

    partial class TestContext {

        public void Pass(string message) {
            TestUnit.ForcePredeterminedStatus(TestUnitFlags.PassExplicitly, message);
        }

        public void Pass() {
            Pass(null);
        }

        public void Pass(IFormatProvider formatProvider, string format, params object[] args) {
            Pass(string.Format(formatProvider, format, args));
        }

        public void Pass(string format, params object[] args) {
            Pass(string.Format(format, args));
        }

        public void Skip() {
            Skip(null);
        }

        public void Skip(string reason) {
            TestUnit.ForcePredeterminedStatus(TestUnitFlags.Skip, reason);
        }

        public void Pending() {
            Pending(null);
        }

        public void Pending(string reason) {
            TestUnit.ForcePredeterminedStatus(TestUnitFlags.Pending, reason);
        }

        public void Pending(IFormatProvider formatProvider, string format, params object[] args) {
            Pending(string.Format(formatProvider, format, args));
        }

        public void Pending(string format, params object[] args) {
            Pending(string.Format(format, args));
        }

        public void Fail() {
            Fail(null);
        }

        public void Fail(string reason) {
            TestUnit.ForcePredeterminedStatus(TestUnitFlags.Failed, reason);
        }

        public void Fail(IFormatProvider formatProvider, string format, params object[] args) {
            Fail(string.Format(formatProvider, format, args));
        }

        public void Fail(string format, params object[] args) {
            Fail(string.Format(format, args));
        }

        internal void VerifiableProblem(string reason) {
            if (ShouldVerify) {
                Fail(reason);
            } else {
                Pending(reason);
            }
        }
    }
}
