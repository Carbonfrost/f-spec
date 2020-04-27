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

namespace Carbonfrost.Commons.Spec.ExecutionModel.Output {

    class ConsoleMessage : ConsoleOutputPart<TestMessageEventArgs> {

        protected override void RenderCore(TestMessageEventArgs e) {
            switch (e.Severity) {
                case TestMessageSeverity.Debug:
                    console.Gray();
                    break;
                case TestMessageSeverity.Trace:
                case TestMessageSeverity.Information:
                    console.White();
                    break;
                case TestMessageSeverity.Warning:
                    console.Yellow();
                    break;
                case TestMessageSeverity.Error:
                case TestMessageSeverity.Fatal:
                    console.Red();
                    break;
            }
            console.Write(e.Severity.ToString().ToLowerInvariant());
            console.Write(":  ");
            console.ResetColor();
            console.WriteLine(e.Message);
        }
    }

}
