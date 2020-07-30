#if SELF_TEST

//
// Copyright 2016, 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
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
using System.Threading.Tasks;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.SelfTest.Spec {

    public class RecordTests {

        [Fact]
        public void Exception_can_record_nominal() {
            Assert.IsInstanceOf<InvalidOperationException>(Record.Exception(() => { throw new InvalidOperationException(); }));
        }

        [Fact]
        public void Exception_can_record_null_when_no_error() {
            Assert.Null(Record.Exception(() => { }));
        }

        [Fact]
        public void Exception_can_record_thread_pool_exceptions() {
            Assert.IsInstanceOf<InvalidOperationException>(Record.Exception(async () => await ThrowsAnErrorAsync()));
        }

        [Fact]
        public void Exception_can_record_null_when_no_thread_pool_exceptions() {
            Assert.Null(Record.Exception(async () => await ThrowsNothing()));
        }

        [Fact]
        public void PropertyChanges_can_list_property_changes() {
            var inst = new PNotifyPropertyChanged();
            var evts = Record.PropertyChangedEvents(inst);
            inst.A = "A";
            inst.A = "B";
            Assert.HasCount(2, evts);
            Assert.Equal("A", evts[1].PropertyName);
        }

        [Fact]
        public void Events_can_attach_handler_using_name() {
            var inst = new PNotifyPropertyChanged();
            var evts = Record.Events<PropertyChangedEventArgs>(inst, "PropertyChanged");
            inst.A = "A";
            inst.A = "B";
            Assert.HasCount(2, evts);
            Assert.Equal("A", evts[1].PropertyName);
        }

        [Fact]
        public void Events_throws_on_missing_property_name() {
            Assert.Throws<ArgumentException>(
                () => Record.Events<PropertyChangedEventArgs>(new object(), "PropertyChanged")
            );
        }

        [Fact]
        public void Events_throws_on_wrong_event_handler_type() {
            Assert.Throws<ArgumentException>(
                () => Record.Events<AddingNewEventArgs>(new PNotifyPropertyChanged(), "PropertyChanged")
            );
        }

        class PNotifyPropertyChanged : INotifyPropertyChanged {
            public event PropertyChangedEventHandler PropertyChanged;
            private string _a;

            public string A {
                get {
                    return _a;
                }
                set {
                    _a = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("A"));
                }
            }
            protected void OnPropertyChanged(PropertyChangedEventArgs e) {
                if (PropertyChanged != null) {
                    PropertyChanged(this, e);
                }
            }
        }

#pragma warning disable 1998
        private async Task ThrowsAnErrorAsync() {
            throw new InvalidOperationException();
        }

        private async Task ThrowsNothing() {
        }
#pragma warning restore 1998

    }
}
#endif
