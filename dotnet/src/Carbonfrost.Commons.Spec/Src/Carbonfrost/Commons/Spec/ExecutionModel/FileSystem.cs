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
using System.Collections.Generic;
using System.IO;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    abstract class FileSystem {

        public static readonly FileSystem Real = new RealImpl();

        public abstract bool Exists(string path);
        public abstract bool IsDirectory(string path);
        public abstract IEnumerable<string> DirectoryEnumerateFiles(string path);
        public abstract IEnumerable<string> DirectoryEnumerateFiles(string item, string searchPattern, SearchOption searchOption);
        public abstract string WorkingDirectory { get; }

        class RealImpl : FileSystem {
            public override bool IsDirectory(string path) {
                return Directory.Exists(path);
            }
            public override IEnumerable<string> DirectoryEnumerateFiles(string path) {
                return Directory.EnumerateFiles(path);
            }

            public override IEnumerable<string> DirectoryEnumerateFiles(string item, string searchPattern, SearchOption searchOption) {
                return Directory.EnumerateFiles(item, searchPattern, searchOption);
            }

            public override bool Exists(string path) {
                return Directory.Exists(path) || File.Exists(path);
            }

            public override string WorkingDirectory {
                get {
                    return Directory.GetCurrentDirectory();
                }
            }
        }
    }
}
