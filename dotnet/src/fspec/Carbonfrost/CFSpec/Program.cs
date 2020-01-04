//
// Copyright 2016, 2019 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Collections.Generic;
using System.Linq;

namespace Carbonfrost.CFSpec {

    static class Program {

        public static int Main(string[] args) {
            ProgramOptions options = new ProgramOptions();

            List<string> unknown;
            try {
                unknown = options.Parse(args);

            } catch (NDesk.Options.OptionException e) {
                return ExitWithMessage(ExitCode.UsageError, e.Message);
            }

            int result = 0;

            IEnumerable<string> assemblies;
            string unrecognized;

            if (!CheckForPositionalArgs(unknown, out assemblies, out unrecognized)) {
                return ExitWithMessage(ExitCode.UsageError,
                                       "unrecognized option: " + unrecognized);
            }
            if (!options.Quit) {
                options.Assemblies.AddRange(assemblies);

                var app = new SpecApp();
                app.Options = options;
                result = app.Run();
            }

            if (options.DebugWait) {
                Console.WriteLine("Press ENTER to exit ...");
                Console.ReadLine();
            }
            return result;
        }

        static int ExitWithMessage(ExitCode exitCode, string message) {
            Console.WriteLine("spec: " + message);
            return (int) exitCode;
        }

        static bool CheckForPositionalArgs(IList<string> unknowns,
                                           out IEnumerable<string> assemblies,
                                           out string unrecognized) {

            unrecognized = null;
            assemblies = Enumerable.Empty<string>();

            if (unknowns.Count == 0) {
                return true;
            }

            // Determine if there is an unknown option --example
            if (!VerifyNotOptions(unknowns, out unrecognized)) {
                return false;
            }

            assemblies = unknowns;
            return true;
        }

        static bool VerifyNotOptions(IList<string> unknowns, out string unrecognized) {
            unrecognized = null;

            foreach (string s in unknowns) {
                if (s.StartsWith("-", StringComparison.Ordinal) || s.StartsWith("/", StringComparison.Ordinal)) {
                    unrecognized = s;
                    return false;
                }
            }

            return true;
        }
    }
}
