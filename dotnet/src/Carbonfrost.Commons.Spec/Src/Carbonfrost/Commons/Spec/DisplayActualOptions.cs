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

namespace Carbonfrost.Commons.Spec {

    [Flags]
    enum DisplayActualOptions {
        None = 0,
        ShowWhitespace = 1 << 1,
        ShowType = 1 << 2,
        ShowNoisyStackTrace = 1 << 4,
    }

    partial class Extensions {

        internal static bool ShowType(this DisplayActualOptions opts) {
            return (opts & DisplayActualOptions.ShowType) > 0;
        }

        internal static bool ShowWhitespace(this DisplayActualOptions opts) {
            return (opts & DisplayActualOptions.ShowWhitespace) > 0;
        }

        internal static bool ShowNoisyStackTrace(this DisplayActualOptions opts) {
            return (opts & DisplayActualOptions.ShowNoisyStackTrace) > 0;
        }
    }
}
