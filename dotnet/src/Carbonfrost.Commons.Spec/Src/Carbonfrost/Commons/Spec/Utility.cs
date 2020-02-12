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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Carbonfrost.Commons.Spec {

    static class Utility {

        static readonly RandomNumberGenerator rng = RandomNumberGenerator.Create();

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
            Uri uri;
            if (Uri.TryCreate(result, UriKind.Absolute, out uri)) {
                // Make it relative
                if (makeRelative) {
                    var current = new Uri("file://" + Directory.GetCurrentDirectory());
                    return current.MakeRelativeUri(uri).ToString();
                }
                return uri.LocalPath;
            }
            return result;
        }
    }
}
