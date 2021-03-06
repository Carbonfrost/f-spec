//
// Copyright 2016, 2017, 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.Commons.Spec {

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class TimeoutAttribute : Attribute, ITestUnitMetadataProvider {

        public TimeSpan? Timeout { get; private set; }

        public TimeoutAttribute(int timeout) {
            if (timeout == 0 || timeout == -1) {
                return;
            }
            if (timeout < 0) {
                throw SpecFailure.NegativeTimeout(nameof(timeout));
            }
            Timeout = TimeSpan.FromMilliseconds(timeout);
        }

        void ITestUnitMetadataProvider.Apply(TestContext testContext) {
            testContext.TestUnit.Timeout = Timeout;
        }

    }
}
