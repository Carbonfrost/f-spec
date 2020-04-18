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
using System.Text.RegularExpressions;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    public class PathCollection : Collection<string>, IFormattable {

        internal FileSystem FileSystem = FileSystem.Real;

        private static bool IsUnix {
            get {
                return Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX;
            }
        }

        public PathCollection()
            : base(new MakeReadOnlyList<string>()) {}

        public PathCollection(params string[] items) : this((IEnumerable<string>) items) {
        }

        public PathCollection(IEnumerable<string> items) : this() {
            if (items is null) {
                throw new ArgumentNullException(nameof(items));
            }

            Items.AddAll(items);
        }

        internal static PathCollection FromEnvironment(string variable) {
            string value = Environment.GetEnvironmentVariable(variable);
            if (string.IsNullOrEmpty(value)) {
                return new PathCollection();
            }
            return Parse(value);
        }

        public static PathCollection Parse(string text) {
            if (text.Contains("\n")) {
                return ParseExact(text, "r");
            }
            if (IsUnix) {
                return ParseExact(text, "l");
            }
            return ParseExact(text, "w");
        }

        public static PathCollection ParseExact(string text, string format) {
            return ParseExact(text, GetFormat(format));
        }

        enum Format {
            Linux,
            Windows,
            General,
        }

        static Format GetFormat(string format) {
            if (string.IsNullOrEmpty(format)) {
                return Format.General;
            }
            if (format.Length == 1) {
                switch (char.ToLowerInvariant(format[0])) {
                    case 'g':
                    case 'r':
                        return Format.General;
                    case 'l':
                        return Format.Linux;
                    case 'w':
                        return Format.Windows;
                }
            }
            throw new FormatException();
        }

        private static PathCollection ParseExact(string text, Format format) {
            string delim;
            switch (format) {
            case Format.Linux:
                delim = ":";
                break;
            case Format.Windows:
                delim = ";";
                break;
            default:
                delim = @"\s*[\n\r\n]\s*";
                break;
            }
            return new PathCollection(
                Regex.Split(text, delim).Select(EmptyAsCurrentDir)
            );
        }

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
            if (!IsUnix) {
                return string.Join(";", Items);
            }
            return string.Join(":", Items);
        }

        internal void MakeReadOnly() {
            ((MakeReadOnlyList<string>) Items).MakeReadOnly();
        }

        static string EmptyAsCurrentDir(string path) {
            if (path.Length == 0) {
                return Environment.CurrentDirectory;
            }
            return path;
        }

        public string ToString(string format) {
            string delim;
            switch (GetFormat(format)) {
            case Format.Linux:
                delim = ":";
                break;
            case Format.Windows:
                delim = ";";
                break;
            default:
                delim = "\n";
                break;
            }
            return string.Join(delim, Items);
        }

        string IFormattable.ToString(string format, IFormatProvider formatProvider) {
            return ToString(format);
        }
    }
}
