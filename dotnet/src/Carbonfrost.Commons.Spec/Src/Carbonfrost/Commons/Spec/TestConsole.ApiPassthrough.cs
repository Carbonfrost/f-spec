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

    // These methods and properties are present for ergonomics and they always forward to System.Console
    partial class TestConsole {

        public static event ConsoleCancelEventHandler CancelKeyPress {
            add {
                Console.CancelKeyPress += value;
            }
            remove {
                Console.CancelKeyPress -= value;
            }
        }

        public static ConsoleColor BackgroundColor {
            get { return Console.BackgroundColor; }
            set { Console.BackgroundColor = value; }
        }

        public int BufferHeight {
            get { return Console.BufferHeight; }
            set { Console.BufferHeight = value; }
        }

        public int BufferWidth {
            get { return Console.BufferWidth; }
            set { Console.BufferWidth = value; }
        }

        public bool CapsLock {
            get { return Console.CapsLock; }
        }

        public int CursorLeft {
            get { return Console.CursorLeft; }
            set { Console.CursorLeft = value; }
        }

        public int CursorSize {
            get { return Console.CursorSize; }
            set { Console.CursorSize = value; }
        }

        public int CursorTop {
            get { return Console.CursorTop; }
            set { Console.CursorTop = value; }
        }

        public bool CursorVisible {
            get { return Console.CursorVisible; }
            set { Console.CursorVisible = value; }
        }

        public ConsoleColor ForegroundColor {
            get { return Console.ForegroundColor; }
            set { Console.ForegroundColor = value; }
        }

        public bool KeyAvailable {
            get { return Console.KeyAvailable; }
        }

        public int LargestWindowHeight {
            get { return Console.LargestWindowHeight; }
        }

        public int LargestWindowWidth {
            get { return Console.LargestWindowWidth; }
        }

        public bool NumberLock {
            get { return Console.NumberLock; }
        }

        public string Title {
            get { return Console.Title; }
            set { Console.Title = value; }
        }

        public bool TreatControlCAsInput {
            get { return Console.TreatControlCAsInput; }
            set { Console.TreatControlCAsInput = value; }
        }

        public int WindowHeight {
            get { return Console.WindowHeight; }
            set { Console.WindowHeight = value; }
        }

        public int WindowLeft {
            get { return Console.WindowLeft; }
            set { Console.WindowLeft = value; }
        }

        public int WindowTop {
            get { return Console.WindowTop; }
            set { Console.WindowTop = value; }
        }

        public int WindowWidth {
            get { return Console.WindowWidth; }
            set { Console.WindowWidth = value; }
        }

        public void Beep() {
            Console.Beep();
        }

        public void Beep(int frequency, int duration) {
            Console.Beep(frequency, duration);
        }

        public void Clear() {
            Console.Clear();
        }

        public void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft, int targetTop) {
            Console.MoveBufferArea(sourceLeft, sourceTop, sourceWidth, sourceHeight, targetLeft, targetTop);
        }

        public void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft, int targetTop, char sourceChar, ConsoleColor sourceForeColor, ConsoleColor sourceBackColor) {
            Console.MoveBufferArea(sourceLeft, sourceTop, sourceWidth, sourceHeight, targetLeft, targetTop, sourceChar, sourceForeColor, sourceBackColor);
        }

        public void SetBufferSize(int width, int height) {
            Console.SetBufferSize(width, height);
        }

        public void SetCursorPosition(int left, int top) {
            Console.SetCursorPosition(left, top);
        }

        public void SetWindowPosition(int left, int top) {
            Console.SetWindowPosition(left, top);
        }

        public void SetWindowSize(int width, int height) {
            Console.SetWindowSize(width, height);
        }

        public ConsoleKeyInfo ReadKey(bool intercept) {
            return Console.ReadKey(intercept);
        }

        public ConsoleKeyInfo ReadKey() {
            return Console.ReadKey();
        }

        public static void ResetColor() {
            Console.ResetColor();
        }
    }

}
