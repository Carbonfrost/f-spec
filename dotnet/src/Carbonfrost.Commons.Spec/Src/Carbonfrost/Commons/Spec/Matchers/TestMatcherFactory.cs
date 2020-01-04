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

namespace Carbonfrost.Commons.Spec.TestMatchers {

    public static class TestMatcherFactory {

        internal static string GetMessage(object o) {
            var pi = o.GetType().GetTypeInfo().GetProperty("Message");
            if (pi == null) {
                return null;
            }
            return Convert.ToString(pi.GetValue(o));
        }

        public static ITestMatcherFactory<string> TestFileContents(string pathPattern) {
            return TestFileContents(pathPattern, null);
        }

        public static ITestMatcherFactory<string> TestFileContents(string pathPattern, string message, params object[] args) {
            return new TestFileContentsAttribute(pathPattern) {
                Message = string.Format(message, (object[]) args),
            };
        }

        public static ITestMatcherFactory<object> Null() {
            return Null(null);
        }

        public static ITestMatcherFactory<object> Null(string message, params object[] args) {
            return new NullAttribute {
                Message = string.Format(message, args),
            };
        }

        public static ITestMatcherFactory<object> NotNull() {
            return NotNull(null);
        }

        public static ITestMatcherFactory<object> NotNull(string message, params object[] args) {
            return new NotNullAttribute {
                Message = string.Format(message, args),
            };
        }

        public static ITestMatcherFactory<bool> False() {
            return False(null);
        }

        public static ITestMatcherFactory<bool> False(string message, params object[] args) {
            return new FalseAttribute {
                Message = string.Format(message, args),
            };
        }

        public static ITestMatcherFactory<bool> True() {
            return True(null);
        }

        public static ITestMatcherFactory<bool> True(string message, params object[] args) {
            return new TrueAttribute {
                Message = string.Format(message, args),
            };
        }

        public static ITestMatcherFactory ExpectedException(Type exceptionType) {
            return ExpectedException(exceptionType, null);
        }

        public static ITestMatcherFactory ExpectedException(Type exceptionType, string message, params object[] args) {
            return new ExpectedExceptionAttribute(exceptionType) {
                Message = string.Format(message, (object[])args),
            };
        }

        public static ITestMatcherFactory Throws(Type exceptionType) {
            return Throws(exceptionType, null);
        }

        public static ITestMatcherFactory Throws(Type exceptionType, string message, params object[] args) {
            return new ExpectedExceptionAttribute(exceptionType) {
                Message = string.Format(message, (object[])args),
            };
        }
    }
}
