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

namespace Carbonfrost.Commons.Spec {

    static class TestActual {

        public static ITestActualEvaluation<T> Value<T>(T value) {
            return new DiscreteValue<T>(value);
        }

        public static ITestActualEvaluation<T> Of<T>(Func<T> factory) {
            return new TestActual<T>(factory);
        }

        struct DiscreteValue<T> : ITestActualEvaluation<T> {
            private readonly T _value;

            public Exception Exception {
                get {
                    return null;
                }
            }

            public T Value {
                get {
                    return _value;
                }
            }

            public DiscreteValue(T value) {
                _value = value;
            }
        }
    }

    struct TestActual<T> : ITestActualEvaluation<T> {
        private readonly Func<T> _value;
        private T _actual;
        private TestCodeDispatchInfo _dispatch;

        public Exception Exception {
            get {
                EnsureValues();
                return _dispatch?.Exception;
            }
        }

        public T Value {
            get {
                EnsureValues();
                return _actual;
            }
        }

        public TestActual(Func<T> value) {
            _value = value;
            _actual = default(T);
            _dispatch = null;
        }

        private void EnsureValues() {
            if (_dispatch != null) {
                return;
            }

            var value = _value;
            T actual = default(T);

            _dispatch = Record.DispatchInfo(
                () => actual = value(),
                Assert.UseStrictMode
                    ? RecordExceptionFlags.StrictVerification
                    : RecordExceptionFlags.IgnoreAssertExceptions
            );
            if (typeof(T) != typeof(Unit)) {
                _actual = actual;
            }
        }
    }

}
