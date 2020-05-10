//
// Copyright 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.IO;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    public class PackageReference {

        public string Name {
            get;
            private set;
        }

        public string Version {
            get;
            private set;
        }

        static string NugetPackagesDirectory {
            get {
                var home = Environment.GetEnvironmentVariable("HOME");
                return Path.Combine(home, ".nuget", "packages");
            }
        }

        static string Framework {
            get {
                // TODO This should support configuration, inference from fspec
                return "netstandard2.0";
            }
        }

        public PackageReference(string name, string version = null) {
            Name = name;
            Version = version;
        }

        public static bool TryParse(string text, out PackageReference result) {
            var items = text.Split('/');
            if (items.Length == 1) {
                result = new PackageReference(items[0]);
                return true;
            }
            if (items.Length == 2) {
                result = new PackageReference(items[0], items[1]);
                return true;
            }
            result = null;
            return false;
        }

        public static PackageReference Parse(string text) {
            if (TryParse(text, out PackageReference result)) {
                return result;
            }
            throw new FormatException();
        }

        internal string ResolveAssembly() {
            if (Version == null) {
                // TODO Support packages referenced without version
                throw new NotImplementedException();
            }

            // as in ${NUGET_DIR}/System.Memory/4.5.2/lib/netstandard2.0/System.Memory.dll
            return Path.Combine(
                NugetPackagesDirectory,
                Name,
                Version,
                "lib",
                Framework,
                Name + ".dll"
            );
        }
    }
}
