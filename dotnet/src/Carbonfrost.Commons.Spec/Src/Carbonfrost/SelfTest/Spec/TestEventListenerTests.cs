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
using Carbonfrost.Commons.Spec;
using System.ComponentModel;

namespace Carbonfrost.SelfTest.Spec {

    public class TestEventListenerTests {

        [Fact]
        public void Events_will_contain_matching_events() {
            var listener = new TestEventListener<PropertyChangedEventArgs>();
            var pp = new PNotifyPropertyChanged();
            pp.PropertyChanged += listener.GetHandler<PropertyChangedEventHandler>();

            pp.A = "OK";
            Assert.HasCount(1, listener.Events);
            Assert.Equal("A", listener.Events[0].PropertyName);
            Assert.True(listener.Handled);
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
    }
}
