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

using System.Linq;
using Carbonfrost.Commons.Spec.Resources;

namespace Carbonfrost.Commons.Spec.ExecutionModel.Output {

    class ConsoleUserData : ConsoleOutputPart<UserDataCollection> {

        const int bufferWidth = 6;

        public AssertionMessageFormatModes AssertionMessageFormat {
            get;
            set;
        }

        protected override void RenderCore(UserDataCollection data) {
            if (data.Keys.Count <= 0) {
                return;
            }

            int maxLength = data.VisibleKeys.Max(t => t.Length);

            WriteLine();
            foreach (var kvp in data) {
                if (data.IsHiddenFromTable(kvp.Key)) {
                    continue;
                }
                string format = data.FormatValue(kvp.Key, AssertionMessageFormat);
                WriteLineItem(kvp.Key, format, maxLength);
            }

            if (data.Diff != null) {
                WriteLineItem("Diff", "", maxLength);
                parts.forPatch.Render(context, data.Diff);
            }
        }

        private void WriteLineItem(string caption, string data, int length) {
            caption = Caption(caption);
            Write(string.Format("{0," + (bufferWidth + length) + "}: ", caption));
            WriteLine(data);
        }

        internal static string Caption(string caption) {
            var cap = "Label" + caption;
            return SR.ResourceManager.GetString(cap) ?? TestMatcherLocalizer.MissingLocalization(caption);
        }
    }
}
