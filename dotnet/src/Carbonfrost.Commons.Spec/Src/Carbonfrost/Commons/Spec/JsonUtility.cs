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
using System.Text.Json;

namespace Carbonfrost.Commons.Spec {

    static class JsonUtility {

        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions {
            WriteIndented = true,
            IgnoreNullValues = true,
            Converters = {
                JsonConverters.DateTime,
                JsonConverters.NullableDateTime,
                JsonConverters.TimeSpan,
                JsonConverters.NullableTimeSpan,
                JsonConverters.Enum,
            },
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        public static string ToJson(object any) {
            return JsonSerializer.Serialize(any, Options);
        }

        public static T LoadJson<T>(string json) {
            return JsonSerializer.Deserialize<T>(json, Options);
        }
    }
}