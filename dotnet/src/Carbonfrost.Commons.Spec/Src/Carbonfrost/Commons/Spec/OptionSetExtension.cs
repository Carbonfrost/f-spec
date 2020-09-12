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

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NDesk.Options;

namespace Carbonfrost.Commons.Spec {

    class OptionSetExtension : OptionSet {

        private class OptionGroup {
            public string Label;
            public HashSet<string> Options;
            public bool Sort;
        }

        private readonly List<OptionGroup> _groups = new List<OptionGroup>();

        public void WriteSuccintUsage(TextWriter o) {
            const string usageLabel = "usage: fspec ";
            o.Write(usageLabel);

            int INITIAL = usageLabel.Length;
            int pos = INITIAL;
            foreach (var opt in Items.Select(GetOptionProto)) {
                if (pos + opt.Length >= 80) {
                    o.WriteLine();
                    o.Write(new string(' ', INITIAL));
                    pos = INITIAL;
                }
                pos += opt.Length + 1;
                o.Write(opt + " ");
            }
            o.WriteLine("[assembly]...");
            o.WriteLine();
            o.WriteLine("Run tests in the specified assemblies");
            o.WriteLine();
        }

        private string GetOptionProto(Option p) {
            var names = p.Names.Except(new [] { "<>" });
            var localizer = MessageLocalizer;
            if (!names.Any()) {
                return null;
            }
            var sb = new StringBuilder();
            sb.Append("[");
            bool comma = false;
            foreach (var name in names) {
                if (comma) {
                    sb.Append("|");
                }
                if (name.Length == 1) {
                    sb.Append("-");
                } else {
                    sb.Append("--");
                }
                sb.Append(name);
                comma = true;
            }

            if (p.OptionValueType == OptionValueType.Optional ||
                p.OptionValueType == OptionValueType.Required) {
                if (p.OptionValueType == OptionValueType.Optional) {
                    sb.Append("[");
                }
                sb.Append(localizer("=" + GetArgumentName(0, p.MaxValueCount, p.Description)));

                string sep = p.ValueSeparators != null && p.ValueSeparators.Length > 0
                    ? p.ValueSeparators [0]
                    : " ";
                for (int c = 1; c < p.MaxValueCount; ++c) {
                    sb.Append(localizer(sep + GetArgumentName (c, p.MaxValueCount, p.Description)));
                }
                if (p.OptionValueType == OptionValueType.Optional) {
                    sb.Append("]");
                }
            }
            sb.Append("]");
            return sb.ToString();
        }

        public void WriteUsage(TextWriter o) {
            var groupedOptions = new HashSet<Option>();
            WriteSuccintUsage(o);

            foreach (var group in _groups) {
                o.WriteLine(group.Label + ":");

                // Keep order they were specified in Add(), not Group
                var options = this.Where(ox => group.Options.Contains(ox.Prototype));
                if (group.Sort) {
                    options = options.OrderBy(ox => ox.Names[0]);
                }

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

        public void Group(string label, bool sort = false, params string[] options) {
            _groups.Add(new OptionGroup {
                Label = label,
                Sort = sort,
                Options = new HashSet<string>(options),
            });
        }
    }
}
