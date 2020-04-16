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

using System.Text;
using System.Text.RegularExpressions;

namespace Carbonfrost.Commons.Spec {

    partial class FixtureParser {

        delegate void FoldLines(StringBuilder sb, string line, string previousLine);
        delegate string FinalFold(StringBuilder sb);

        // Gets the fold operation that occurs on each line and the operation that
        // occurs on the combined output
        static (FoldLines, FinalFold) GetFoldType(string value) {
            switch (value) {
                case "_":
                    return (TotalFold, NoFold);
                case "|":
                    return (LiteralAndClippedFold, RemoveTrailingWSAddEOL);
                case ">":
                    return (StandardFold, RemoveTrailingWSAddEOL);
                case ">-":
                    return (StandardFold, RemoveTrailingWS);
                case ">+":
                    return (StandardFold, AddEOL);

                default:
                    return (StandardFold, NoFold);
            }
        }

        static string NoFold(StringBuilder sb) {
            return sb.ToString();
        }

        static string AddEOL(StringBuilder sb) {
            return sb.ToString() + "\n";
        }

        static string RemoveTrailingWS(StringBuilder sb) {
            while (sb.Length > 0 && char.IsWhiteSpace(sb[sb.Length - 1])) {
                sb.Length--;
            }
            return sb.ToString();
        }

        static string RemoveTrailingWSAddEOL(StringBuilder sb) {
            while (sb.Length > 0 && char.IsWhiteSpace(sb[sb.Length - 1])) {
                sb.Length--;
            }
            return sb.ToString() + "\n";
        }

        static void StandardFold(StringBuilder sb, string line, string previousLine) { // >
            if (!string.IsNullOrEmpty(previousLine) && line.Length > 0) {
                sb.Append(" "); // fold lines
            }
            if (line.Length == 0) {
                sb.AppendLine();
            }
            sb.Append(line);
        }

        static void LiteralAndClippedFold(StringBuilder sb, string line, string previousLine) { // |
            sb.AppendLine(line.TrimStart());
        }

        static void TotalFold(StringBuilder sb, string line, string previousLine) { // _
            // remove all whitespace (useful for binary data)
            sb.Append(Regex.Replace(line, @"\s+", ""));
        }

    }
}
