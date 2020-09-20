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
using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.Commons.Spec {

    static partial class JsonConverters {

        private static readonly JsonConverter<DateTime> _DateTime = new DateTimeConverter();
        private static readonly JsonConverter<TimeSpan> _TimeSpan = new TimeSpanConverter();
        private static readonly JsonConverter<TestName> _TestName = Default<TestName>();
        private static readonly JsonConverter<TestId> _TestId = Parser(TestId.Parse);

        private static readonly JsonSerializerOptions RecursiveOptions = new JsonSerializerOptions {
            WriteIndented = true,
            IgnoreNullValues = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        public static readonly JsonConverter[] All = {
            _DateTime,
            _TimeSpan,
            _TestName,
            _TestId,
            new EnumConverterFactory(),
            Parser(TestTag.Parse),
            Parser(TestTagPredicate.Parse),
            Nullable(_TestId),
            Nullable(_DateTime),
            Nullable(_TimeSpan),
            Nullable(_TestName),
        };

        static JsonConverter<T?> Nullable<T>(JsonConverter<T> item) where T:struct {
            return new NullableConverter<T>(item);
        }

        static JsonConverter<T> Default<T>() where T:struct {
            return new DefaultConverter<T>();
        }

        static JsonConverter<T> Parser<T>(Func<string, T> parser) {
            return new ParserConverter<T>(parser);
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

        class DefaultConverter<T> : JsonConverter<T> {

            public override T Read(
                ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options
            ) {
                return JsonSerializer.Deserialize<T>(ref reader, RecursiveOptions);
            }

            public override void Write(
                Utf8JsonWriter writer, T value, JsonSerializerOptions options
            ) {
                JsonSerializer.Serialize<T>(writer, value, RecursiveOptions);
            }
        }

        class TestIdConverter : JsonConverter<TestId> {

            public override TestId Read(
                ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options
            ) {
                return JsonSerializer.Deserialize<TestId>(ref reader, RecursiveOptions);
            }

            public override void Write(
                Utf8JsonWriter writer, TestId value, JsonSerializerOptions options
            ) {
                JsonSerializer.Serialize<TestId>(writer, value, RecursiveOptions);
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
                var actual = reader.GetString()
                    .Replace("|", ",")
                    .Replace("_", "")
                    .ToLowerInvariant();
                return (T) Enum.Parse(typeof(T), actual, true);
            }

            public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options) {
                writer.WriteStringValue(
                    string.Join(" | ", value.ToString().Split(',').Select(s => ScreamingSnakecase.ConvertName(s).Trim()))
                );
            }
        }

        class TimeSpanConverter : JsonConverter<TimeSpan> {
            public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
                if (Time.TryParse(reader.GetString(), out Time time)) {
                    return time.Value;
                }
                return TimeSpan.Parse(reader.GetString());
            }

            public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options) {
                writer.WriteStringValue(((Time) value).ToString("G"));
            }
        }

        class ParserConverter<T> : JsonConverter<T> {
            private readonly Func<string, T> _parser;

            public ParserConverter(Func<string, T> parser) {
                _parser = parser;
            }

            public override bool CanConvert(Type typeToConvert) {
                return typeof(T).IsAssignableFrom(typeToConvert);
            }

            public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
                return _parser(reader.GetString());
            }

            public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options) {
                writer.WriteStringValue(value.ToString());
            }
        }
    }
}
