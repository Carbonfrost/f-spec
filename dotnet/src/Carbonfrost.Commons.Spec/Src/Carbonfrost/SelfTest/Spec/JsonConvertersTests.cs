#if SELF_TEST
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

using System;
using System.Net.Sockets;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.SelfTest.Spec {

    public class JsonConvertersTests {

        struct PDateTimeObject {
            public DateTime? DateTime {
                get;
                set;
            }
        }

        [Fact]
        public void NullableDateTime_will_parse_null_as_DateTime() {
            Assert.Equal((DateTime?) null, JsonUtility.LoadJson<PDateTimeObject>("{ \"DateTime\": null }").DateTime);
        }

        [Fact]
        public void EnumConverter_will_parse_and_convert() {
            Assert.Equal(
                SocketFlags.OutOfBand | SocketFlags.Peek | SocketFlags.DontRoute,
                JsonUtility.LoadJson<SocketFlags>("\"OUT_OF_BAND | PEEK | DONT_ROUTE\"")
            );

            Assert.Equal(
                "\"OUT_OF_BAND | PEEK | DONT_ROUTE\"",
                JsonUtility.ToJson(SocketFlags.OutOfBand | SocketFlags.Peek | SocketFlags.DontRoute)
            );
        }

        [Theory]
        [InlineData("hello", "HELLO")]
        [InlineData("helloProperty", "HELLO_PROPERTY")]
        [InlineData("helloIOProperty", "HELLO_IO_PROPERTY")]
        [InlineData("helloWAPProperty", "HELLO_WAP_PROPERTY")]
        [InlineData("hello123OK", "HELLO123_OK")]
        public void ScreamingSnakeCase_ConvertName_should_generate_correct_values(string property, string expected) {
            Assert.Equal(expected, JsonConverters.ScreamingSnakecase.ConvertName(property));
        }
    }
}

#endif
