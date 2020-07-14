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
using System.Collections.Generic;
using System.Linq;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    public struct TestName : IFormattable {

        public string DataName {
            get;
        }

        public string Assembly {
            get;
        }

        public string Namespace {
            get;
        }

        public string Class {
            get;
        }

        public string SubjectClassBinding {
            get;
        }

        public string Method {
            get;
        }

        public IReadOnlyList<string> Arguments {
            get;
        }

        public int Position {
            get;
        }

        public string DisplayName {
            get {
                var parts = new [] {
                    Namespace,
                    Class,
                    Method,
                };

                return string.Concat(
                    string.Join(
                        ".",
                        parts.Where(p => !string.IsNullOrEmpty(p))
                    ),
                    Position >= 0 ? (" #" + Position) : null,
                    Arguments.Count > 0 ? "(" + string.Join(",", Arguments) + ")" : null
                );
            }
        }

        public TestName(
            string assembly,
            string @namespace,
            string @class,
            string subjectClassBinding,
            string method,
            int position,
            string dataName,
            IEnumerable<string> arguments
        ) {
            Assembly = assembly;
            Namespace = @namespace;
            Class = @class;
            SubjectClassBinding = subjectClassBinding;
            Method = method;
            Arguments = (arguments ?? Enumerable.Empty<string>()).ToArray();
            Position = position;
            DataName = dataName;
        }

        internal static TestName Create(TestCaseInfo caze) {
            string assembly = null;
            string ns = null;
            string clazz = null;
            string binding = null;
            IEnumerable<string> arguments = caze.TestMethodArguments.Select(
                TextUtility.ConvertToString
            );

            foreach (var a in caze.Ancestors()) {
                switch (a) {
                    case TestSubjectClassBinding m:
                        binding = TextUtility.ConvertToSimpleTypeName(m.TestSubject.GetType(), qualified: true);
                        break;
                    case TestAssembly m:
                        assembly = m.Assembly.FullName;
                        break;
                    case TestNamespace m:
                        ns = m.Namespace;
                        break;
                    case TestClassInfo m:
                        clazz = m.TestClass.Name;
                        break;
                }
            }
            return new TestName(
                assembly, ns, clazz, binding, caze.MethodName, caze.Position, caze.TestData.Name, arguments
            );
        }

        internal TestName InstanceNamed(string name) {
            return new TestName(
                Assembly, Namespace, Class, SubjectClassBinding, Method, -1, name, null
            );
        }

        public override string ToString() {
            return DisplayName;
        }

        public string ToString(string format) {
            if (string.IsNullOrEmpty(format)) {
                return Method;
            }
            if (format.Length == 1) {
                switch (char.ToLowerInvariant(format[0])) {
                    case 'g':
                    case 'r':
                        return Method;
                }
            }
            throw new FormatException();
        }

        string IFormattable.ToString(string format, IFormatProvider formatProvider) {
            return ToString(format);
        }

    }
}
