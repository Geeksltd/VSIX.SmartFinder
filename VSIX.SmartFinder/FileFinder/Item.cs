using System;
using System.IO;
using System.Linq;
using Geeks.VSIX.SmartFinder.Definition;

namespace Geeks.VSIX.SmartFinder.FileFinder
{
    internal class Item : IEquatable<Item>
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

        public override string ToString()
        {
            if (FileName.EndsWithAny(new[] { ".less", ".css", ".scss" }))
            {
                if (Phrase.HasValue() == false) return FileName;

                var splittedDisplayPath = DisplayPath.Split('\\');
                if (splittedDisplayPath.Length == 4 || splittedDisplayPath.Length < 4)
                {
                    return "{0} -----> {1}".FormatWith(Phrase, DisplayPath);
                }
                else
                {
                    var path = splittedDisplayPath.Skip(splittedDisplayPath.Length - 3).Aggregate((a, b) => a + "\\" + b);
                    return "{0} -----> {1}".FormatWith(Phrase, path);
                }
            }
            else
                return Phrase.HasValue() ? "{0} -----> {1}".FormatWith(Phrase, DisplayPath) : FileName;
        }

        public bool MatchesWith(string[] filterWords)
        {
            if (filterWords == null || filterWords.IsEmpty())
                return true;

            return filterWords.All(w => this.ToString().Contains(w, false));
        }

        public string FullPath
        {
            get
            {
                var projectName = Path.GetFileName(BasePath) + "\\";
                return Path.Combine(BasePath, FileName.TrimStart(projectName));
            }
        }

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
    }
}
