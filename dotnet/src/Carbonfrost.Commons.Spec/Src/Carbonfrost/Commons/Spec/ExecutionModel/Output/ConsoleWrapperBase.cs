//
// Copyright 2016 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Linq;

namespace Carbonfrost.Commons.Spec.ExecutionModel.Output {

    abstract class ConsoleWrapperBase : IConsoleWrapper {

        private int _indent;
        private string[] _indentCache = Enumerable.Range(0, 10)
            .Select(t => new string(' ', t * 2))
            .ToArray();


        protected virtual string IndentString {
            get {
                int indent = _indent > 10 ? 10 : _indent;
                return _indentCache[indent];
            }
        }

        protected virtual int Indent {
            get {
                return _indent;
            }
        }

        public virtual void PushIndent() {
            _indent++;
            if (_indent > 10) {
                _indent = 10;
            }
        }

        public virtual void PopIndent() {
            _indent--;
            if (_indent < 0) {
                _indent = 0;
            }
        }

        public abstract void SetForeground(ConsoleColor color);

        public abstract void ResetColor();

        public abstract void Write(string text);

        public void Write(string text, object arg1) {
            Write(string.Format(text, arg1));
        }

        public void Write(string text, object arg1, object arg2) {
            Write(string.Format(text, arg1, arg2));
        }

        public void Write(string text, object arg1, object arg2, object arg3) {
            Write(string.Format(text, arg1, arg2, arg3));
        }

        public void Write(string text, params object[] args) {
            Write(string.Format(text, args));
        }

        public void WriteLine(string text, object arg1) {
            Write(text, arg1);
            WriteLine();
        }

        public void WriteLine(string text, object arg1, object arg2) {
            Write(text, arg1, arg2);
            WriteLine();
        }

        public void WriteLine(string text, object arg1, object arg2, object arg3) {
            Write(text, arg1, arg2, arg3);
            WriteLine();
        }

        public void WriteLine(string text, params object[] args) {
            Write(text, args);
            WriteLine();
        }

        public void WriteLine(string text) {
            Write(text);
            WriteLine();
        }

        public abstract void WriteLine();

        public abstract bool IsUnicodeEncoding { get; }

        public virtual void Bold() {
        }

        public virtual void Underline() {
        }

        public virtual void ResetStyles() {
        }
    }
}
