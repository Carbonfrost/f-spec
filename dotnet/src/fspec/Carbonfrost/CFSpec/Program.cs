//
// Copyright 2016, 2019, 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
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

namespace Carbonfrost.CFSpec {

    static class Program {

        public static int Main(string[] args) {
            ProgramOptions options = null;

            try {
                options = ProgramOptions.Parse(args);

            } catch (NDesk.Options.OptionException e) {
                return ExitWithMessage(ExitCode.UsageError, e.Message);
            }

            int result = 0;

            if (!options.Quit) {
                var app = new SpecApp(options);
                result = app.Run();
            }

            if (options.DebugWait) {
                Console.WriteLine("Press ENTER to exit ...");
                Console.ReadLine();
            }
            return result;
        }

        static int ExitWithMessage(ExitCode exitCode, string message) {
            Console.WriteLine("fspec: " + message);
            return (int) exitCode;
        }
    }
}
