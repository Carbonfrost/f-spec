//
// Copyright 2018, 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
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
using System.Reflection;

namespace Carbonfrost.Commons.Spec {

    public static class Record {

        public static TestCodeDispatchInfo DispatchInfo(Action action) {
            return DispatchInfo(action, RecordExceptionFlags.None);
        }

        public static TestCodeDispatchInfo DispatchInfo(Action action, RecordExceptionFlags flags) {
            return new TestCodeDispatchInfo(
                Exception(action, flags)
            );
        }

        public static Exception Exception(Action action) {
            return Exception(action, RecordExceptionFlags.None);
        }

        public static Exception Exception(Action action, RecordExceptionFlags flags) {
            try {
                SyncContextImpl.Run(action);
                return null;

            } catch (Exception ex) {
                if (ex is AssertException && flags.HasFlag(RecordExceptionFlags.IgnoreAssertExceptions)) {
                    throw;
                }
                return ApplyFlags(ex, flags);
            }
        }

        internal static Exception ApplyFlags(Exception ex, RecordExceptionFlags flags) {
            if (ex is TargetInvocationException && flags.HasFlag(RecordExceptionFlags.UnwindTargetExceptions)) {
                ex = ex.InnerException;
            }
            if (ex is AssertException && flags.HasFlag(RecordExceptionFlags.StrictVerification)) {
                throw SpecFailure.CannotAssertAssertExceptions();
            }

            return ex;
        }
    }
}
