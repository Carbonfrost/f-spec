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
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Carbonfrost.Commons.Spec {

    partial class JsonConverters {

        internal static readonly JsonNamingPolicy ScreamingSnakecase = new ScreamingSnakecaseNamingPolicy();

        class ScreamingSnakecaseNamingPolicy : JsonNamingPolicy {
            public override string ConvertName(string property) {
                return Regex.Replace(
                    property,
                    @"(^[a-z0-9]+|[A-Z]{2,}(?![a-z0-9])|[A-Z][a-z0-9]+)",
                    e => $"{e.Groups[1]}_"
                ).TrimEnd('_').ToUpper();
            }
        }
    }
}
