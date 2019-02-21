namespace Geeks.VSIX.SmartFinder.FileFinder
{
    internal class FileFinderGadget : FinderBaseGadget
    {
        public FileFinderGadget()
        {
            Name = FinderLiterals.FileFinderGadgetName;
            Title = FinderLiterals.FileFinderGadgetTitle;
            ShortKey = FinderLiterals.FileFinderGadgetShortKey;
        }

        public override Loader GetLoaderAgent(string[] basePaths, Repository repository)
        {
            return new FileLoaderAgent(basePaths, repository);
        }

        public override System.Drawing.Color GetColor()
        {
            return System.Drawing.Color.FromArgb(0xE1, 0xE6, 0xDA); // 0xEB, 0xF0, 0xE3   0xCD,0xDA,0xC9
        }

        public override string GetTitle() => FinderLiterals.FileFinderGadgetTitle;
    }
}
