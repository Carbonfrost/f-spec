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

    class ConsoleTestName : ConsoleOutputPart<TestName> {

        //   1) TestNameTests > ToString_get_expected_test_name (
        // Carbonfrost.SelfTest.Spec.ExecutionModel.TestNameTests+TestUnitDisplayNameData
        //  )  (Carbonfrost.SelfTest.Spec.ExecutionModel)
        //

        protected override void RenderCore(TestName data) {
            console.Write(data.Class);
            console.Write(" ");

            if (data.Method != null) {
                console.Muted();
                console.Write("> ");
                console.ResetColor();
                console.Write(data.Method);
                console.Write(" ");
            }

            if (!string.IsNullOrEmpty(data.DataName)) {
                console.Write("#");
                console.Write(data.DataName);
                console.Write(" ");

            } else if (data.Position >= 0) {
                console.Write("#");
                console.Write(data.Position.ToString());
                console.Write(" ");
            }

            if (data.Arguments != null && data.Arguments.Count > 0) {
                console.PushIndent();
                console.PushIndent();
                console.Write("(");

                bool comma = false;
                foreach (var arg in data.Arguments) {
                    if (comma) {
                        console.Write(",");
                    }
                    comma = true;
                    console.Write(TextUtility.Truncate(arg));
                }
                console.PopIndent();
                console.PopIndent();
                console.Write(") ");
            }
            console.Muted();
            console.Write("(");
            console.Write(data.Namespace);
            console.Write(") ");
            console.ResetColor();
            console.WriteLine();
        }
    }
}
