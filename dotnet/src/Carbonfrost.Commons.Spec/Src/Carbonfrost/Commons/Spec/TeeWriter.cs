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
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Carbonfrost.Commons.Spec {

    class TeeWriter : TextWriter {
        private readonly TextWriter _left;
        private readonly TextWriter _right;

        public override Encoding Encoding {
            get;
        }

        public override void Close() {
            Do(w => w.Close());
        }

        public override void Flush() {
            Do(w => w.Flush());
        }

        public override Task FlushAsync() {
            return DoAsync(w => w.FlushAsync());
        }

        public override void Write(ulong value) {
            Do(w => w.Write(value));
        }

        public override void Write(uint value) {
            Do(w => w.Write(value));
        }

        public override void Write(string format, params object[] arg) {
            Do(w => w.Write(format, arg));
        }

        public override void Write(string format, object arg0, object arg1, object arg2) {
            Do(w => w.Write(format, arg0, arg1, arg2));
        }

        public override void Write(string format, object arg0, object arg1) {
            Do(w => w.Write(format, arg0, arg1));
        }

        public override void Write(string format, object arg0) {
            Do(w => w.Write(format, arg0));
        }

        public override void Write(string value) {
            Do(w => w.Write(value));
        }

        public override void Write(object value) {
            Do(w => w.Write(value));
        }

        public override void Write(long value) {
            Do(w => w.Write(value));
        }

        public override void Write(int value) {
            Do(w => w.Write(value));
        }

        public override void Write(double value) {
            Do(w => w.Write(value));
        }

        public override void Write(decimal value) {
            Do(w => w.Write(value));
        }

        public override void Write(char[] buffer, int index, int count) {
            Do(w => w.Write(buffer, index, count));
        }

        public override void Write(char[] buffer) {
            Do(w => w.Write(buffer));
        }

        public override void Write(char value) {
            Do(w => w.Write(value));
        }

        public override void Write(bool value) {
            Do(w => w.Write(value));
        }

        public override void Write(float value) {
            Do(w => w.Write(value));
        }

        public override Task WriteAsync(string value) {
            return DoAsync(w => w.WriteAsync(value));
        }

        public override Task WriteAsync(char value) {
            return DoAsync(w => w.WriteAsync(value));
        }

        public override Task WriteAsync(char[] buffer, int index, int count) {
            return DoAsync(w => w.WriteAsync(buffer, index, count));
        }

        public override void WriteLine(string format, object arg0) {
            Do(w => w.WriteLine(format, arg0));
        }

        public override void WriteLine(ulong value) {
            Do(w => w.WriteLine(value));
        }

        public override void WriteLine(uint value) {
            Do(w => w.WriteLine(value));
        }

        public override void WriteLine(string format, params object[] arg) {
            Do(w => w.WriteLine(format, (object[]) arg));
        }

        public override void WriteLine(string format, object arg0, object arg1, object arg2) {
            Do(w => w.WriteLine(format, arg0, arg1, arg2));
        }

        public override void WriteLine(string format, object arg0, object arg1) {
            Do(w => w.WriteLine(format, arg0, arg1));
        }

        public override void WriteLine(string value) {
            Do(w => w.WriteLine(value));
        }

        public override void WriteLine(float value) {
            Do(w => w.WriteLine(value));
        }

        public override void WriteLine() {
            Do(w => w.WriteLine());
        }

        public override void WriteLine(long value) {
            Do(w => w.WriteLine(value));
        }

        public override void WriteLine(int value) {
            Do(w => w.WriteLine(value));
        }

        public override void WriteLine(double value) {
            Do(w => w.WriteLine(value));
        }

        public override void WriteLine(decimal value) {
            Do(w => w.WriteLine(value));
        }

        public override void WriteLine(char[] buffer, int index, int count) {
            Do(w => w.WriteLine(buffer, index, count));
        }

        public override void WriteLine(char[] buffer) {
            Do(w => w.WriteLine(buffer));
        }

        public override void WriteLine(char value) {
            Do(w => w.WriteLine(value));
        }

        public override void WriteLine(bool value) {
            Do(w => w.WriteLine(value));
        }

        public override void WriteLine(object value) {
            Do(w => w.WriteLine(value));
        }

        public override Task WriteLineAsync() {
            return DoAsync(w => w.WriteLineAsync());
        }

        public override Task WriteLineAsync(char value) {
            return DoAsync(w => w.WriteLineAsync(value));
        }

        public override Task WriteLineAsync(char[] buffer, int index, int count) {
            return DoAsync(w => w.WriteLineAsync(buffer, index, count));
        }

        public override Task WriteLineAsync(string value) {
            return DoAsync(w => w.WriteLineAsync(value));
        }

        private void Do(Action<TextWriter> action)  {
            throw new NotImplementedException();
        }

        private async Task DoAsync(Func<TextWriter, Task> action) {
            throw new NotImplementedException();
        }
    }
}
