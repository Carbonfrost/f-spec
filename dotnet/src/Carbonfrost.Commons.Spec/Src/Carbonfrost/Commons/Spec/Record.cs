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
using System.ComponentModel;
using System.Reflection;
using System.Linq.Expressions;

namespace Carbonfrost.Commons.Spec {

    public static class Record {

        public static TestEventListener<TEventArgs> Events<TEventArgs>(object instance, string eventName) where TEventArgs: EventArgs {
            if (instance is null) {
                throw new ArgumentNullException(nameof(instance));
            }

            return Events<TEventArgs>(instance, instance.GetType().GetEvent(eventName));
        }

        public static TestEventListener<TEventArgs> Events<TEventArgs>(object instance, EventInfo eventInfo) where TEventArgs: EventArgs {
            if (instance is null) {
                throw new ArgumentNullException(nameof(instance));
            }
            if (eventInfo is null) {
                throw new ArgumentNullException(nameof(eventInfo));
            }
            var result = new TestEventListener<TEventArgs>();
            eventInfo.AddEventHandler(instance, result.GetHandler(eventInfo.EventHandlerType));
            return result;
        }

        public static TestEventListener<PropertyChangedEventArgs> PropertyChangedEvents(INotifyPropertyChanged instance) {
            if (instance is null) {
                throw new ArgumentNullException(nameof(instance));
            }
            var result = new TestEventListener<PropertyChangedEventArgs>();
            instance.PropertyChanged += result.GetHandler<PropertyChangedEventHandler>();
            return result;
        }

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
            if (flags.HasFlag(RecordExceptionFlags.UnwindTargetExceptions)) {
                ex = UnwindTargetException(ex);
            }
            if (ex is AssertException && flags.HasFlag(RecordExceptionFlags.StrictVerification)) {
                throw SpecFailure.CannotAssertAssertExceptions();
            }

            return ex;
        }

        internal static Exception UnwindTargetException(Exception ex) {
            if (ex is TargetInvocationException) {
                return ex.InnerException;
            }
            return ex;
        }
    }
}
