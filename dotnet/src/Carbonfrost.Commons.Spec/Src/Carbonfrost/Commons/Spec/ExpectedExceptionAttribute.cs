//
// Copyright 2018, 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
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
using Carbonfrost.Commons.Spec.TestMatchers;

namespace Carbonfrost.Commons.Spec {

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class ExpectedExceptionAttribute : Attribute, ITestMatcherFactory {

        private readonly ThrowsAttribute _ta;

        public string Message {
            get {
                return _ta.Message;
            }
            set {
                _ta.Message = value;
            }
        }

        public Type ExceptionType {
            get {
                return _ta.ExceptionType;
            }
        }

        public ExpectedExceptionAttribute(Type exceptionType) {
            _ta = new ThrowsAttribute(exceptionType);
        }

        ITestMatcher ITestMatcherFactory.CreateMatcher(TestContext testContext) {
            return ((ITestMatcherFactory) _ta).CreateMatcher(testContext);
        }
    }

}
