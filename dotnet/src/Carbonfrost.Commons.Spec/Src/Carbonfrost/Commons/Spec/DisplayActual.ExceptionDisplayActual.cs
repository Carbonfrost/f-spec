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
using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.Commons.Spec {

    partial class DisplayActual {

        class ExceptionDisplayActual : IDisplayActual {

            public Exception Exception  {
                get;
            }

            public ExceptionDisplayActual(Exception exception) {
                Exception = exception;
            }

            public string Format(DisplayActualOptions options) {
                if (Exception == null) {
                    return "<no exception>";
                }
                var f = new ExceptionStackTraceFilter(Exception);
                return f.ToString(options.ShowNoisyStackTrace());
            }
        }
    }

}
