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
using System.IO;
using System.Linq;
using System.Reflection;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    class AssemblyLoader {

        public static Func<string, Assembly> DefaultLoadAssemblyFromPath;

        public Func<string, Assembly> LoadAssemblyFromPath {
            get;
            set;
        }

        public AssemblyLoader() {
            LoadAssemblyFromPath = DefaultLoadAssemblyFromPath;
        }

        internal void RegisterAssemblyResolve(IEnumerable<string> paths) {
            AppDomain currentDomain = AppDomain.CurrentDomain;

            var searchPath = paths.ToArray();
            currentDomain.AssemblyResolve += (_, e) => {
                var an = new AssemblyName(e.Name);
                if (an.Name == "Carbonfrost.Commons.Spec") {
                    return typeof(Assert).Assembly;
                }

                foreach (var folderPath in searchPath) {
                    string assemblyPath = Path.Combine(folderPath, an.Name + ".dll");
                    if (File.Exists(assemblyPath)) {
                        SpecLog.AssemblyResolved(assemblyPath);
                        return LoadAssemblyFromPath(assemblyPath);
                    }
                }

                return null;
            };
        }

        internal List<Assembly> LoadAssemblies(PathCollection path) {
            return LoadAssemblies(path.EnumerateFiles("*.dll"));
        }

        internal List<Assembly> LoadAssemblies(IEnumerable<string> items) {
            var list = new List<Assembly>();
            foreach (var asmPath in items) {
                var asmInfo = LoadAssembly(asmPath);
                list.Add(asmInfo);
            }
            return list;
        }

        internal Assembly LoadAssembly(string asmPath) {
            string fullPath = Path.GetFullPath(asmPath);
            if (!File.Exists(fullPath)) {
                throw SpecFailure.FailedToLoadAssemblyPath(asmPath);
            }
            try {
                SpecLog.LoadAssembly(fullPath);

                return LoadAssemblyFromPath(fullPath);

            } catch (BadImageFormatException) {
                throw SpecFailure.FailedToLoadAssembly(asmPath);

            } catch (FileNotFoundException ex) {
                throw SpecFailure.FailedToLoadAssemblyPath(asmPath + " -> " + ex.FileName);

            } catch (IOException ex) {
                throw SpecFailure.FailedToLoadAssemblyGeneralIO(asmPath, ex.Message);
            }
        }
    }
}
