using Geeks.GeeksProductivityTools.FileFinder;

namespace GeeksAddin.FileFinder
{
    internal class StyleFinderGadget : FinderBaseGadget
    {
        public StyleFinderGadget()
            : base()
        {
            Name = FinderLiterals.StyleFinderGadgetName;
            Title = FinderLiterals.StyleFinderGadgetTitle;
            ShortKey = FinderLiterals.StyleFinderGadgetShortKey;
        }

        public override Loader GetLoaderAgent(string[] basePaths, Repository repository)
        {
            return new StyleLoaderAgent(basePaths, repository);
        }

        public override System.Drawing.Color GetColor() => System.Drawing.Color.FromArgb(0xF0, 0xE3, 0xE5);

        public override string GetTitle() => FinderLiterals.StyleFinderGadgetTitle;
    }
}
