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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Carbonfrost.Commons.Spec {

    public class TestEventListener<TEventArgs> : IReadOnlyList<TEventArgs>
        where TEventArgs : EventArgs
    {

        private readonly List<TEventArgs> _events = new List<TEventArgs>();
        private readonly IDictionary<Type, Delegate> _handlerCache = new Dictionary<Type, Delegate>();

        public EventHandler<TEventArgs> Handler {
            get {
                return _Handler;
            }
        }

        private MethodInfo HandlerMethod {
            get {
                return GetType().GetMethod("_Handler", BindingFlags.Instance | BindingFlags.NonPublic);
            }
        }

        public IReadOnlyList<TEventArgs> Events {
            get {
                return _events;
            }
        }

        public TEventArgs LastEvent {
            get {
                return _events.LastOrDefault();
            }
        }

        public bool Handled {
            get {
                return Events.Any();
            }
        }

        public int HandledCount {
            get {
                return Events.Count;
            }
        }

        public TEventArgs this[int index] {
            get {
                return Events[index];
            }
        }

        public int Count {
            get {
                return Events.Count;
            }
        }

        private void _Handler(object sender, TEventArgs value) {
            _events.Add(value);
        }

        public IEnumerator<TEventArgs> GetEnumerator() {
            return Events.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public TDelegate GetHandler<TDelegate>() where TDelegate : Delegate {
            return (TDelegate) GetHandler(typeof(TDelegate));
        }

        public Delegate GetHandler(Type eventHandlerType) {
            if (eventHandlerType == null || eventHandlerType == typeof(EventHandler<TEventArgs>)) {
                return Handler;
            }

            return _handlerCache.GetValueOrCache(eventHandlerType, GetHandlerCore);
        }

        private Delegate GetHandlerCore(Type eventHandlerType) {
            return Delegate.CreateDelegate(eventHandlerType, this, HandlerMethod);
        }
    }
}
