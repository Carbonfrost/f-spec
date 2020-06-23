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

namespace Carbonfrost.Commons.Spec.ExecutionModel.Output {

    class ConsoleResultDetails : ConsoleOutputPart<TestUnitResult> {

        protected override void RenderCore(TestUnitResult result) {
            if (result.IsSlow) {
                console.Write("[");
                console.Write(TextUtility.FormatDuration(result.ExecutionTime.Value));
                console.Write("]  ");
            }
            console.Write(result.DisplayName);
            if (result.IsFocused) {
                console.Write(" (focused)");
            }
            console.WriteLine();

            console.ColorFor(result);
            console.PushIndent();

            if (result.Failed) {
                console.WriteLineIfNotEmpty(result.Reason);
            } else if (result.IsPending) {
                console.Muted();
                if (!string.IsNullOrEmpty(result.Reason)) {
                    console.WriteLine("// " + result.Reason);
                }
            }

            context.Parts.forExceptionInfo.Render(context, result.ExceptionInfo);

            console.ResetColor();
            foreach (var m in result.Messages) {
                context.Parts.forMessage.Render(context, m);
            }
            console.PopIndent();
            console.WriteLine();
        }
    }

}
