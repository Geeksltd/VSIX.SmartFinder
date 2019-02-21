using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Geeks.VSIX.SmartFinder.Definition;

namespace Geeks.VSIX.SmartFinder.FileFinder
{
    public class Item : IEquatable<Item>, IComparable<Item>
    {
        public Item(string projectBasePath, string fullFileName)
        {
            BasePath = projectBasePath;
            FileName = Path.GetFileName(projectBasePath) + "\\" + fullFileName.Replace(projectBasePath, "").TrimStart("\\");

            BuildDisplayPath();
        }

        public string BasePath { get; private set; }
        public string DisplayPath { get; set; }
        public string Phrase { get; set; }
        public string FileName { get; set; }
        public int LineNumber { get; set; }
        public int Column { get; set; }
        public IconType Icon { get; set; }
        public MemberType MemberType { get; set; }

        void BuildDisplayPath()
        {
            var splittedBasePath = BasePath.Split('\\').Where(path => path != string.Empty).ToArray();
            var currentProjectName = splittedBasePath[splittedBasePath.Length - 1];

            if (currentProjectName == FileName.Split('\\')[0])
                DisplayPath = FileName;
            else
                DisplayPath = currentProjectName + FileName;
        }


        readonly string[] styleExtensions = { ".css", ".scss", ".less" };

        public override string ToString()
        {
            const string format = "{0}    -----> {1}";
            if (!FileName.EndsWithAny(styleExtensions))
                return Phrase.HasValue() ? format.FormatWith(Phrase, DisplayPath) : FileName;

            if (!Phrase.HasValue()) return FileName;

            var splittedDisplayPath = DisplayPath.Split('\\');
            if (splittedDisplayPath.Length <= 4)
                return format.FormatWith(Phrase, DisplayPath);

            var path = splittedDisplayPath.Skip(splittedDisplayPath.Length - 3).Aggregate((a, b) => a + "\\" + b);
            return format.FormatWith(Phrase, path);

        }

        public bool MatchesWith(string[] filterWords)
        {
            if (filterWords == null || filterWords.IsEmpty())
                return true;

            return filterWords.All(w => ToString().Contains(w, false));
        }

        public string FullPath
        {
            get
            {
                var projectName = Path.GetFileName(BasePath) + "\\";
                return Path.Combine(BasePath, FileName.TrimStart(projectName));
            }
        }

        public string[] Words { get; set; }

        public bool Exists() => File.Exists(FullPath);

        #region IEquatable<Item> Members

        public bool Equals(Item other)
        {
            return other != null &&
                   FileName == other.FileName &&
                   LineNumber == other.LineNumber &&
                   Phrase == other.Phrase;
        }

        #endregion

        public int CompareTo(Item other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;

            var extension1 = Path.GetExtension(FullPath)?.ToLower();
            var extension2 = Path.GetExtension(other.FullPath)?.ToLower();
            if (extension1 == null || extension2 == null) return 0;

            var result = string.Compare(extension1, extension2, StringComparison.CurrentCultureIgnoreCase);
            if (result == 0)
            {
                var a = HasSearchKeywordInFileName(Words, FullPath);
                var b = HasSearchKeywordInFileName(other.Words, other.FullPath);

                if (a && b)
                {
                    if (Phrase.HasValue() == other.Phrase.HasValue())
                    {
                        if (this.IsMSharp() == other.IsMSharp())
                            result = string.Compare(FullPath, other.FullPath, StringComparison.Ordinal);
                        else
                            return this.IsMSharp() ? -1 : 1;
                    }
                    else
                        return Phrase.HasValue() ? -1 : 1;
                } else if (a)
                    return -1;
                else
                    return 1;

            }
            else if (styleExtensions.Contains(extension1) || styleExtensions.Contains(extension2))
            {
                return extension1.EndsWith(".css") && (extension2.EndsWithAny(styleExtensions))
                            ? 1 : -1;
            }


            return result;

        }

        bool HasSearchKeywordInFileName(string[] words, string fullPath)
        {
            if (words == null)
                return false;

            return (from word in words
                    let l1 = fullPath.LastIndexOf('\\')
                    let l2 = fullPath.LastIndexOf(word, StringComparison.OrdinalIgnoreCase)
                    where l2 > l1
                    select word).Any();
        }
    }
}
