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
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace Carbonfrost.Commons.Spec {

    sealed class DataStreamContext : StreamContext {

        private readonly string _baseUri;
        private readonly MemoryStream _data;
        private bool _isBase64;

        // "data:application/octet-stream;base64,"
        // data:[<MIME-type>][;charset=<encoding>][;base64],<data>

        public DataStreamContext(DataStreamContext source) {
            _baseUri = "data:;base64,";
            _data = new MemoryStream();
            _isBase64 = true;
            source._data.CopyTo(_data);
        }

        public DataStreamContext(MemoryStream data) {
            _baseUri = "data:;base64,";
            _data = data;
            _isBase64 = true;
        }

        public DataStreamContext(string data) {
            _baseUri = "data:,";
            _data = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(data));
            _isBase64 = false;
        }

        public DataStreamContext(Uri u) {
            string[] parts = u.PathAndQuery.Split(new []{','}, 2);
            if (parts.Length != 2) {
                throw SpecFailure.NotValidDataUri();
            }

            var ct = Regex.Replace(parts[0], ";base64", string.Empty);
            byte[] buffer;

            _isBase64 = ct.Length < parts[0].Length; // implied by replacement
            if (_isBase64) {
                buffer = Convert.FromBase64String(parts[1]);
            }
            else {
                buffer = System.Text.Encoding.ASCII.GetBytes(WebUtility.UrlDecode(parts[1]));
            }

            _baseUri = string.Concat("data:",
                _isBase64 ? ";base64" : string.Empty,
                ","
            );
            _data = new MemoryStream(buffer.Length);
            _data.Write(buffer, 0, buffer.Length);
        }

        public override StreamContext ChangePath(string relativePath) {
            throw new NotSupportedException();
        }

        public override Uri Uri {
            get {
                return new Uri(_baseUri + EncodedBytes());
            }
        }

        public override Stream Open() {
            _data.Position = 0;
            return _data;
        }

        string EncodedBytes() {
            if (_isBase64) {
                return Convert.ToBase64String(_data.ToArray());
            }

            string text = System.Text.Encoding.ASCII.GetString(_data.ToArray());
            return WebUtility.UrlEncode(text).Replace("+", "%20");
        }
    }
}
