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
using System.Collections.Generic;
using System.Linq;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    class MiddlewareWinder<TMiddleware, TArg> {

        private readonly TMiddleware[] _commands;

        private readonly Action<TArg>[] _actionWrappers;
        private readonly MiddlewareFunc _invoke;

        internal delegate void MiddlewareFunc(TMiddleware middleware, TArg argument, Action<TArg> nextMiddleware);

        public MiddlewareWinder(IEnumerable<TMiddleware> commands, MiddlewareFunc invoke) {
            _commands = commands.ToArray();
            _actionWrappers = _commands.Select((c, i) => ActionWrapper(i)).ToArray();
            _invoke = invoke;
        }

        private Action<TArg> ActionWrapper(int index) {
            return tc => {
                _invoke(
                    _commands[index],
                    tc,
                    _actionWrappers.ElementAtOrDefault(index + 1) ?? EmptyAction
                );
            };
        }

        private static void EmptyAction(TArg context) {
        }

        public void RunAll(TArg context) {
            _actionWrappers[0].Invoke(context);
        }
    }
}
