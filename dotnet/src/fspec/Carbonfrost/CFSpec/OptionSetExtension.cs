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

using System.Collections.Generic;
using System.IO;
using System.Linq;
using NDesk.Options;

namespace Carbonfrost.CFSpec {

    class OptionSetExtension : OptionSet {

        private class OptionGroup {
            public string Label;
            public HashSet<string> Options;
        }

        private readonly List<OptionGroup> _groups = new List<OptionGroup>();

        public void WriteUsage(TextWriter o) {
            var groupedOptions = new HashSet<Option>();

            foreach (var group in _groups) {
                o.WriteLine(group.Label);

                // Keep order they were specified in Add(), not Group
                var options = this.Where(o => group.Options.Contains(o.Prototype));

                foreach (Option option in options) {
                    groupedOptions.Add(option);

                    WriteOptionDescription(o, option);
                }
                o.WriteLine();
            }

            foreach (Option p in this.Except(groupedOptions)) {
                WriteOptionDescription(o, p);
            }
        }

        public void Group(string label, params string[] options) {
            _groups.Add(new OptionGroup {
                Label = label,
                Options = new HashSet<string>(options),
            });
        }
    }
}
