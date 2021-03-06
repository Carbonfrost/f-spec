//
// Copyright 2016, 2017, 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
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

using Carbonfrost.Commons.Spec.Resources;
using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.Commons.Spec {

    static class SpecWarning {

        public static void TheoryHasNoDataProviders(this TestLog log) {
            log.Warn(SR.TheoryHasNoDataProviders());
        }

        public static void NoTestSubjects(this TestLog log, object clazz) {
            log.Warn(SR.NoTestSubjects(clazz));
        }

        public static void NoTestMethods(this TestLog log, object clazz) {
            log.Warn(SR.NoTestMethods(clazz));
        }

        public static void AsserterDisabled(this TestLog log, Exception ex) {
            log.Warn(SR.AsserterDisabled(TextUtility.ConvertToString(ex)));
        }
    }
}
