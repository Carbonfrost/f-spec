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

namespace Carbonfrost.Commons.Spec {

    public abstract partial class TestConsole {

        public abstract TextWriter Error { get; }
        public abstract TextReader In { get; }
        public abstract Encoding InputEncoding { get; set; }
        public abstract bool IsErrorRedirected { get; }
        public abstract bool IsInputRedirected { get; }
        public abstract bool IsOutputRedirected { get; }
        public abstract TextWriter Out { get; }
        public abstract Encoding OutputEncoding { get; set; }

        public abstract Stream OpenStandardError(int bufferSize);
        public abstract Stream OpenStandardError();
        public abstract Stream OpenStandardInput(int bufferSize);
        public abstract Stream OpenStandardInput();
        public abstract Stream OpenStandardOutput(int bufferSize);
        public abstract Stream OpenStandardOutput();
        public abstract int Read();
        public abstract string ReadLine();
        public abstract void SetError(TextWriter newError);
        public abstract void SetIn(TextReader newIn);
        public abstract void SetOut(TextWriter newOut);

        [CLSCompliant(false)]
        public void Write(ulong value) {
            Out.Write(value);
        }

        public void Write(bool value) {
            Out.Write(value);
        }

        public void Write(char value) {
            Out.Write(value);
        }

        public void Write(char[] buffer) {
            Out.Write(buffer);
        }

        public void Write(char[] buffer, int index, int count) {
            Out.Write(buffer, index, count);
        }

        public void Write(double value) {
            Out.Write(value);
        }

        public void Write(long value) {
            Out.Write(value);
        }

        public void Write(object value) {
            Out.Write(value);
        }

        public void Write(float value) {
            Out.Write(value);
        }

        public void Write(string value) {
            Out.Write(value);
        }

        public void Write(string format, object arg0) {
            Out.Write(format, arg0);
        }

        public void Write(string format, object arg0, object arg1) {
            Out.Write(format, arg0, arg1);
        }

        public void Write(string format, object arg0, object arg1, object arg2) {
            Out.Write(format, arg0, arg1, arg2);
        }

        public void Write(string format, params object[] arg) {
            Out.Write(format, arg);
        }

        [CLSCompliant(false)]
        public void Write(uint value) {
            Out.Write(value);
        }

        public void Write(decimal value) {
            Out.Write(value);
        }

        public void Write(int value) {
            Out.Write(value);
        }

        [CLSCompliant(false)]
        public void WriteLine(ulong value) {
            Out.WriteLine(value);
        }

        public void WriteLine(string format, params object[] arg) {
            Out.WriteLine(format, arg);
        }

        public void WriteLine() {
            Out.WriteLine();
        }

        public void WriteLine(bool value) {
            Out.WriteLine(value);
        }

        public void WriteLine(char[] buffer) {
            Out.WriteLine(buffer);
        }

        public void WriteLine(char[] buffer, int index, int count) {
            Out.WriteLine(buffer, index, count);
        }

        public void WriteLine(decimal value) {
            Out.WriteLine(value);
        }

        public void WriteLine(double value) {
            Out.WriteLine(value);
        }

        [CLSCompliant(false)]
        public void WriteLine(uint value) {
            Out.WriteLine(value);
        }

        public void WriteLine(int value) {
            Out.WriteLine(value);
        }

        public void WriteLine(object value) {
            Out.WriteLine(value);
        }

        public void WriteLine(float value) {
            Out.WriteLine(value);
        }

        public void WriteLine(string value) {
            Out.WriteLine(value);
        }

        public void WriteLine(string format, object arg0) {
            Out.WriteLine(format, arg0);
        }

        public void WriteLine(string format, object arg0, object arg1) {
            Out.WriteLine(format, arg0, arg1);
        }

        public void WriteLine(string format, object arg0, object arg1, object arg2) {
            Out.WriteLine(format, arg0, arg1, arg2);
        }

        public void WriteLine(long value) {
            Out.WriteLine(value);
        }

        public void WriteLine(char value) {
            Out.WriteLine(value);
        }
    }
}
