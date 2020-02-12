//
// Copyright 2019-2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

namespace Carbonfrost.Commons.Spec.ExecutionModel.Output {

    class RenderContext {
        public IConsoleWrapper Console;
        public ConsoleOutputParts Parts;
    }

    abstract class ConsoleOutputPart<T> {

        protected RenderContext context;

        protected IConsoleWrapper console {
            get {
                return context.Console;
            }
        }

        protected ConsoleOutputParts parts {
            get {
                return context.Parts;
            }
        }

        protected ConsoleOutputPart() {

        }

        protected abstract void RenderCore(T result);

        public void Render(RenderContext context, T result) {
            try {
                this.context = context;
                RenderCore(result);

            } finally {
                this.context = null;
            }
        }
    }

    static class ConsoleOutputPart {

        public static ConsoleOutputPart<T> Compose<T>(params ConsoleOutputPart<T>[] items) {
            if (items == null || items.Length == 0) {
                return Null<T>();
            }
            if (items.Length == 1) {
                return items[0];
            }
            return new CompositePart<T>(items);
        }

        public static ConsoleOutputPart<T> Null<T>() {
            return new NullImpl<T>();
        }

        internal class NullImpl<T> : ConsoleOutputPart<T> {
            protected override void RenderCore(T result) {}
        }

        internal class CompositePart<T> : ConsoleOutputPart<T> {

            private readonly ConsoleOutputPart<T>[] _items;

            public CompositePart(ConsoleOutputPart<T>[] items) {
                _items = items;
            }

            protected override void RenderCore(T result) {
                foreach (var r in _items) {
                    r.Render(context, result);
                }
            }
        }
    }

}
