//
// Copyright 2016, 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
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

namespace Carbonfrost.Commons.Spec.ExecutionModel.Output {

    static class ConsoleWrapper {

        public static readonly IConsoleWrapper Default;

        static ConsoleWrapper() {
            Default = new BclConsole();

            if (!Environment.OSVersion.Platform.ToString().Contains("Win")) {
                Default = new AnsiConsole();
            }
        }

        internal static void Muted(this IConsoleWrapper self){
            self.Cyan();
        }

        internal static void WriteLineIfNotEmpty(this IConsoleWrapper self, string text) {
            if (string.IsNullOrEmpty(text)) {
                return;
            }
            self.WriteLine(text);
        }

        internal static void Black(this IConsoleWrapper self) {
            self.SetForeground(ConsoleColor.Black);
        }

        internal static void DarkBlue(this IConsoleWrapper self) {
            self.SetForeground(ConsoleColor.DarkBlue);
        }

        internal static void DarkGreen(this IConsoleWrapper self) {
            self.SetForeground(ConsoleColor.DarkGreen);
        }

        internal static void DarkCyan(this IConsoleWrapper self) {
            self.SetForeground(ConsoleColor.DarkCyan);
        }

        internal static void DarkRed(this IConsoleWrapper self) {
            self.SetForeground(ConsoleColor.DarkRed);
        }

        internal static void DarkMagenta(this IConsoleWrapper self) {
            self.SetForeground(ConsoleColor.DarkMagenta);
        }

        internal static void DarkYellow(this IConsoleWrapper self) {
            self.SetForeground(ConsoleColor.DarkYellow);
        }

        internal static void Gray(this IConsoleWrapper self) {
            self.SetForeground(ConsoleColor.Gray);
        }

        internal static void DarkGray(this IConsoleWrapper self) {
            self.SetForeground(ConsoleColor.DarkGray);
        }

        internal static void Blue(this IConsoleWrapper self) {
            self.SetForeground(ConsoleColor.Blue);
        }

        internal static void Green(this IConsoleWrapper self) {
            self.SetForeground(ConsoleColor.Green);
        }

        internal static void Cyan(this IConsoleWrapper self) {
            self.SetForeground(ConsoleColor.Cyan);
        }

        internal static void Red(this IConsoleWrapper self) {
            self.SetForeground(ConsoleColor.Red);
        }

        internal static void Magenta(this IConsoleWrapper self) {
            self.SetForeground(ConsoleColor.Magenta);
        }

        internal static void Yellow(this IConsoleWrapper self) {
            self.SetForeground(ConsoleColor.Yellow);
        }

        internal static void White(this IConsoleWrapper self) {
            self.SetForeground(ConsoleColor.White);
        }

        internal static void ColorFor(this IConsoleWrapper self, TestUnitResult result) {
            if (result.IsPending) {
                self.Yellow();
            } else if (result.Failed) {
                self.Red();
            } else if (result.Passed) {
                self.Green();
            }
        }

        internal static void ColorFor(this IConsoleWrapper self, TestMessageSeverity result) {
            switch (result) {
                case TestMessageSeverity.Debug:
                case TestMessageSeverity.Trace:
                    self.Gray();
                    break;
                case TestMessageSeverity.Information:
                    self.ResetColor();
                    break;
                case TestMessageSeverity.Warning:
                    self.Yellow();
                    break;
                case TestMessageSeverity.Error:
                    self.Red();
                    break;
                case TestMessageSeverity.Fatal:
                    self.Red();
                    self.Underline();
                    break;
            }
        }

        internal static void HeadlineColorFor(this IConsoleWrapper self, TestUnit unit) {
            switch (unit.Type) {
                case TestUnitType.Assembly:
                    self.DarkCyan();
                    self.Underline();
                    break;

                case TestUnitType.Namespace:
                    self.DarkCyan();
                    break;

                case TestUnitType.Class:
                    self.DarkGreen();
                    break;

                case TestUnitType.Theory:
                    break;
            }
        }
    }
}
