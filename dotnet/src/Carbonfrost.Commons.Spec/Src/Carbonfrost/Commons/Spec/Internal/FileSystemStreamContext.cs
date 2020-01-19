//
// Copyright 2005, 2015, 2016 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

namespace Carbonfrost.Commons.Spec {

    class FileSystemStreamContext : StreamContext {

        private readonly Uri _uri;

        public FileSystemStreamContext(Uri uri) {
            _uri = uri;
        }

        public override Uri Uri {
            get {
                return _uri;
            }
        }

        public override StreamContext ChangePath(string relativePath) {
            var newUri = new Uri(Uri, relativePath);
            return new FileSystemStreamContext(newUri);
        }

        public override Stream OpenRead() {
            return Open(FileMode.Open);
        }

        public override Stream Open() {
            return Open(FileMode.OpenOrCreate);
        }

        public Stream Open(FileMode mode) {
            string realPath = Uri.IsAbsoluteUri ? Uri.LocalPath : Uri.ToString();
            return new FileStream(realPath, mode, FileAccess.Read);
        }
    }
}
