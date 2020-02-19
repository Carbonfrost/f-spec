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

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    public class PathCollection : Collection<string> {

        internal FileSystem FileSystem = FileSystem.Real;

         public PathCollection()
            : base(new MakeReadOnlyList<string>()) {}

        public IEnumerable<string> EnumerateDirectories() {
            return Items.Where(FileSystem.IsDirectory);
        }

        public IEnumerable<string> EnumerateFiles() {
            var result = Enumerable.Empty<string>();
            foreach (var item in Items) {
                if (FileSystem.IsDirectory(item)) {
                    result = result.Concat(FileSystem.DirectoryEnumerateFiles(item));
                } else {
                    result = result.Concat(new [] { item });
                }
            }
            return result;
        }

        public IEnumerable<string> EnumerateFiles(string searchPattern) {
            return EnumerateFiles(searchPattern, SearchOption.TopDirectoryOnly);
        }

        public IEnumerable<string> EnumerateFiles(string searchPattern, SearchOption searchOption) {
            var pattern = new WildcardPattern(searchPattern);
            var result = Enumerable.Empty<string>();
            foreach (var item in Items) {
                if (FileSystem.IsDirectory(item)) {
                    result = result.Concat(FileSystem.DirectoryEnumerateFiles(item, searchPattern, searchOption));

                } else if (pattern.IsMatch(item)) {
                    result = result.Concat(new [] { item });
                }
            }
            return result;
        }

        public string GetFullPath(string path) {
            foreach (var fd in EnumerateDirectories()) {
                string actualPath = Path.Combine(FileSystem.WorkingDirectory, fd, path);
                if (FileSystem.Exists(actualPath)) {
                    return actualPath;
                }
            }
            return null;
        }

        public override string ToString() {
            return string.Join(":", Items);
        }

        internal void MakeReadOnly() {
            ((MakeReadOnlyList<string>) Items).MakeReadOnly();
        }
    }
}
