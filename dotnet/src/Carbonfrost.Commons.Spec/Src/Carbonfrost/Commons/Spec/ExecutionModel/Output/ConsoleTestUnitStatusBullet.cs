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

    class ConsoleTestUnitStatusBullet : ConsoleOutputPart<TestUnitResult> {

        public bool ShowSkipped {
            get;
            set;
        }
        
        protected override void RenderCore(TestUnitResult result) {
            var bullet = console.IsUnicodeEncoding ? "•" : ".";
            if (result.Skipped) {
                if (ShowSkipped) {
                    console.Yellow();
                    console.Write("S");
                }

            } else if (result.IsPending) {
                console.Yellow();
                console.Write("*");

            } else if (result.Failed) {
                console.Red();
                console.Write("F");

            } else {
                console.White();
                console.Write(bullet);
            }
            console.ResetColor();
        }
    }
}
