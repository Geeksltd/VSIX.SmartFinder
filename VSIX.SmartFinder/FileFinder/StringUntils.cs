using Geeks.VSIX.SmartFinder.FileFinder.FileFinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Geeks.VSIX.SmartFinder.FileFinder
{
    internal class StringUntils
    {
        /// <summary>
        /// Replaces invalid path characters in the text with space
        /// </summary>
        public static string ToValidFileName(string text)
        {
            return new string(text.ToArray().Select(c => (char.IsLetterOrDigit(c) || c == '.' || c == '\\') ? c : ' ').ToArray());
        }

        public static List<WordRange> BreakToRanges(string text, string[] words)
        {
            var ranges = new List<WordRange>();
            if (words == null || words.IsEmpty())
            {
                ranges.Add(new WordRange { Text = text, Index = 0, IsHighlighted = false });
                return ranges;
            }

            var pattern = words.Select(w => Regex.Escape(w)).ToString("|");
            var matches = Regex.Matches(text, pattern, RegexOptions.IgnoreCase);
            var counter = 0;
            for (int i = 0; i < matches.Count; i++)
            {
                var match = matches[i];
                var isLastMatch = (i == matches.Count - 1);

                if (match.Index != 0)
                {
                    var beforeWord = text.Substring(counter, match.Index - counter);
                    ranges.Add(new WordRange { IsHighlighted = false, Index = counter, Text = beforeWord });
                }

                ranges.Add(new WordRange { IsHighlighted = true, Index = match.Index, Text = match.Value });

                counter = match.Index + match.Length;

                if (counter < text.Length && isLastMatch)
                {
                    var afterWord = text.Substring(counter);
                    ranges.Add(new WordRange { IsHighlighted = false, Index = counter, Text = afterWord });
                }
            }

            return ranges;
        }
    }
}
