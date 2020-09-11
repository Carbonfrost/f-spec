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
using System.Security.Cryptography;
using System.Text;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    public struct TestId : IEquatable<TestId> {
        private readonly byte[] _data;

        public TestId(byte[] data) {
            if (data == null) {
                _data = Array.Empty<byte>();
            } else {
                _data = (byte[]) data.Clone();
            }
        }

        public static TestId FromTestName(TestName name) {
            return new TestId(Hash(string.Join(
                ":",
                name.Assembly,
                name.Namespace,
                name.Class,
                name.SubjectClassBinding,
                name.Method,
                name.DataName,
                name.Position
            ) + string.Join(
                " ",
                name.Arguments
            )));
        }

        private static byte[] Hash(string text) {
            return SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(text));
        }

        public static TestId Parse(string text) {
            TestId result;
            var ex = _TryParse(text, out result);
            if (ex != null) {
                throw ex;
            }

            return result;
        }

        public static bool TryParse(string text, out TestId result) {
            return _TryParse(text, out result) == null;
        }

        private static Exception _TryParse(string text, out TestId result) {
            result = default(TestId);

            if (text == null) {
                return new ArgumentNullException(nameof(text));
            }
            text = text.Trim();
            if ((text.Length % 2 ) == 0) {
                var bytes = new byte[text.Length / 2];
                int j = 0;
                for (int i = 0; i < text.Length; i += 2) {
                    bytes[j++] = Convert.ToByte(text.Substring(i, 2), 16);
                }
                result = new TestId(bytes);
                return null;
            }
            return SpecFailure.NotParsable(nameof(text), typeof(TestId));
        }

        public bool Equals(TestId other) {
            return other._data.SequenceEqual(_data);
        }

        public override bool Equals(object obj) {
            return obj is TestId tid && Equals(tid);
        }

        public static bool operator ==(TestId x, TestId y) {
            return x.Equals(y);
        }

        public static bool operator !=(TestId x, TestId y) {
            return !x.Equals(y);
        }

        public override string ToString() {
            return string.Concat(_data.Select(d => d.ToString("x2")));
        }

        public override int GetHashCode() {
            return -1945990370 + EqualityComparer<byte[]>.Default.GetHashCode(_data);
        }
    }
}
