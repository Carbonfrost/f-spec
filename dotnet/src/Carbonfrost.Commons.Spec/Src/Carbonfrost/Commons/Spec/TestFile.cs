//
// Copyright 2018 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Reflection;

using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.Commons.Spec {

    public class TestFile : TestFileBase, ITestDataConversion {

        private static readonly MethodInfo OfMethod =
            typeof(TestFile).GetTypeInfo().GetMethod("Of");

        internal TestFile(string fileName) : base(fileName) {
        }

        public TestFile<T> Of<T>() {
            return new TestFile<T>(FileName);
        }

        internal TestFile Typed(Type type) {
            var result = OfMethod.MakeGenericMethod(type).Invoke(this, null);
            return (TestFile) result;
        }

        internal object ReadValue(Type type) {
            return Activation.FromText(type, TextContents);
        }

        object ITestDataConversion.Convert(TestContext context, ParameterInfo param) {
            if (param.ParameterType == typeof(string)) {
                return TextContents;
            }

            if (param.ParameterType == typeof(TestFile)) {
                return this;
            }

            return ReadValue(param.ParameterType);
        }
    }

    public class TestFile<T> : TestFile {

        private readonly Lazy<T> _value;

        public T Value {
            get {
                return _value.Value;
            }
        }

        internal TestFile(string fileName) : base(fileName) {
            _value = new Lazy<T>(() => (T) ReadValue(typeof(T)));
        }
    }
}
