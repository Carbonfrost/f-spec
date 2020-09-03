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
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Carbonfrost.Commons.Spec {

    static class JsonConverters {

        public static readonly JsonConverter<DateTime> DateTime = new DateTimeConverter();
        public static readonly JsonConverter<DateTime?> NullableDateTime = Nullable(DateTime);
        public static readonly JsonConverter<TimeSpan> TimeSpan = new TimeSpanConverter();
        public static readonly JsonConverter<TimeSpan?> NullableTimeSpan = Nullable(TimeSpan);
        public static readonly JsonConverter Enum = new EnumConverterFactory();

        static JsonConverter<T?> Nullable<T>(JsonConverter<T> item) where T:struct {
            return new NullableConverter<T>(item);
        }

        public class EnumConverterFactory : JsonConverterFactory {

            private readonly IDictionary<Type, JsonConverter> _cache = new Dictionary<Type, JsonConverter>();

            public override bool CanConvert(Type typeToConvert) {
                if (typeToConvert.IsEnum) {
                    return true;
                }
                return false;
            }

            public override JsonConverter CreateConverter(Type type, JsonSerializerOptions options) {
                return _cache.GetValueOrCache(
                    type,
                    t => (JsonConverter) Activator.CreateInstance(
                        typeof(EnumConverter<>).MakeGenericType(t)
                    ));
            }
        }

        class DateTimeConverter : JsonConverter<DateTime> {

            public override DateTime Read(
                ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options
            ) {
                return System.DateTime.ParseExact(reader.GetString(), "O", CultureInfo.InvariantCulture);
            }

            public override void Write(
                Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options
            ) {
                writer.WriteStringValue(value.ToUniversalTime().ToString("O"));
            }
        }

        class NullableConverter<T> : JsonConverter<T?> where T : struct {

            private readonly JsonConverter<T> _inner;

            public NullableConverter(JsonConverter<T> inner) {
                _inner = inner;
            }
            public override T? Read(
                ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options
            ) {
                return _inner.Read(ref reader, typeof(T), options);
            }

            public override void Write(
                Utf8JsonWriter writer, T? value, JsonSerializerOptions options
            ) {
                if (value.HasValue) {
                    _inner.Write(writer, value.Value, options);
                } else {
                    writer.WriteNullValue();
                }
            }
        }

        class EnumConverter<T> : JsonConverter<T> where T : Enum {
            public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
                return (T) System.Enum.Parse(typeof(T), reader.GetString(), true);
            }

            public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options) {
                writer.WriteStringValue(
                    string.Join(" ", value.ToString().Split(' ').Select(s => JsonNamingPolicy.CamelCase.ConvertName(s)))
                );
            }
        }

        class TimeSpanConverter : JsonConverter<TimeSpan> {
            public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
                if (Time.TryParse(reader.GetString(), out Time time)) {
                    return time.Value;
                }
                return System.TimeSpan.Parse(reader.GetString());;
            }

            public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options) {
                writer.WriteStringValue(((Time) value).ToString("G"));
            }
        }
    }
}
