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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using Carbonfrost.Commons.Spec.Resources;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    internal class LoaderPathCollection : PathCollection {

        public Func<string, Assembly> LoadAssemblyFromPath {
            get;
            set;
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

        internal List<Assembly> LoadAssemblies() {
            var list = new List<Assembly>();
            foreach (var asmPath in EnumerateFiles("*.dll")) {
                string fullPath = Path.GetFullPath(asmPath);
                if (!File.Exists(fullPath)) {
                    throw SpecFailure.FailedToLoadAssemblyPath(asmPath);
                }
                try {
                    SpecLog.LoadAssembly(fullPath);

                    var asmInfo = LoadAssemblyFromPath(fullPath);
                    list.Add(asmInfo);

                } catch (BadImageFormatException) {
                    throw SpecFailure.FailedToLoadAssembly(asmPath);

                } catch (FileNotFoundException ex) {
                    throw SpecFailure.FailedToLoadAssemblyPath(asmPath + " -> " + ex.FileName);

                } catch (IOException ex) {
                    throw SpecFailure.FailedToLoadAssemblyGeneralIO(asmPath, ex.Message);
                }
            }
            return list;
        }
    }
}
