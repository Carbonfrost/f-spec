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

namespace Carbonfrost.Commons.Spec.ExecutionModel.Output {

    interface IConsoleWrapper {
        bool IsUnicodeEncoding { get; }
        void SetForeground(ConsoleColor color);
        void ResetColor();
        void ResetStyles();
        void PushIndent();
        void PopIndent();
        void Bold();
        void Underline();
        void Write(string text);
        void Write(string text, object arg1);
        void Write(string text, object arg1, object arg2);
        void Write(string text, object arg1, object arg2, object arg3);
        void Write(string text, params object[] args);
        void WriteLine(string text);
        void WriteLine(string text, object arg1);
        void WriteLine(string text, object arg1, object arg2);
        void WriteLine(string text, object arg1, object arg2, object arg3o);
        void WriteLine(string text, params object[] args);
        void WriteLine();
    }
}
