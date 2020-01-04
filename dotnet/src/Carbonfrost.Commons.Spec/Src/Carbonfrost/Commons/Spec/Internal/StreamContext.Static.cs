//
// Copyright 2005, 2006, 2010, 2016, 2019 Carbonfrost Systems, Inc.
// (http://carbonfrost.com)
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
using System.Text;

namespace Carbonfrost.Commons.Spec {

    partial class StreamContext {

        public static StreamContext FromFile(string fileName) {
            if (fileName == null) {
                throw new ArgumentNullException("fileName");
            }
            if (string.IsNullOrEmpty(fileName)) {
                throw SpecFailure.EmptyString("fileName");
            }
            var uri = new Uri(fileName, UriKind.RelativeOrAbsolute);
            if (Path.IsPathRooted(fileName)) {
                uri = new Uri("file://" + fileName);
            }
            return new FileSystemStreamContext(uri);
        }

        public static StreamContext FromSource(Uri source) {
            if (source == null) {
                throw new ArgumentNullException("source");
            }

            // Look for native providers (file, res, iso, mem, stream)
            if (source.IsAbsoluteUri) {
                switch (source.Scheme) {
                    case "file":
                        return new FileSystemStreamContext(source);
                    case "data":
                        return new DataStreamContext(source);

                    default:
                        // Fall back to the URI
                        return new UriStreamContext(source);
                }
            } else {
                // Relative URIs must be handled in this way
                return FromFile(source.ToString());
            }
        }

        public static StreamContext FromText(string text) {
            return new DataStreamContext(text);
        }
    }

}
