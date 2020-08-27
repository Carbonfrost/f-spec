//
// Copyright 2016 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Carbonfrost.Commons.Spec {

    static class Utility {

        static readonly RandomNumberGenerator rng = RandomNumberGenerator.Create();
        static readonly Uri current = new Uri("file://" + Directory.GetCurrentDirectory() + "/");

        static readonly char[] PATH_UNSAFE_CHARS = {
            '\x0',
            '/',
            '\\',
            ':',
            '@'
        };

        static readonly string PathSafeChars = string.Format(
            @"[{0}]+",
            string.Concat(PATH_UNSAFE_CHARS.Select(s => Regex.Escape(s.ToString()))));

        public static T OptimalComposite<T>(IEnumerable<T> items, Func<T[], T> compositeFactory, T nullInstance)
            where T : class
        {
            if (items == null) {
                return nullInstance;
            }

            items = items.Where(t => t != null && !object.ReferenceEquals(t, nullInstance));
            if (!items.Any()) {
                return nullInstance;
            }
            if (items.Skip(1).Any()) { // 2 or more
                return compositeFactory(items.ToArray());
            }

            return items.First();
        }

        internal static string RandomName() {
            byte[] data = new byte[6];
            rng.GetBytes(data);
            return BitConverter.ToString(data).ToLowerInvariant().Replace("-", string.Empty);
        }

        internal static string PathSafe(string nameHint) {
            return Regex.Replace(nameHint ?? string.Empty, PathSafeChars, "-");
        }

        internal static string PrettyCodeBase(Assembly assembly, bool makeRelative = false) {
            string result = assembly.CodeBase;
            if (Uri.TryCreate(result, UriKind.Absolute, out Uri uri)) {
                // Make it relative
                if (makeRelative) {
                    return MakeRelativePath(uri);
                }
                return uri.LocalPath;
            }
            return result;
        }

        internal static string MakeRelativePath(string path) {
            if (Uri.TryCreate(path, UriKind.Absolute, out Uri uri)) {
                if (uri.IsFile) {
                    return current.MakeRelativeUri(uri).ToString();
                }
            }
            return uri.ToString();
        }

        internal static string MakeRelativePath(Uri uri) {
            return current.MakeRelativeUri(uri).ToString();
        }
    }
}
