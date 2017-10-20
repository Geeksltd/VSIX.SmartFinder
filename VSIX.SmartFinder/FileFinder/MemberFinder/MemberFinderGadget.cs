using Geeks.GeeksProductivityTools.FileFinder;

namespace GeeksAddin.FileFinder
{
    internal class MemberFinderGadget : FinderBaseGadget
    {
        public MemberFinderGadget()
            : base()
        {
            Name = FinderLiterals.MemberFinderGadgetName;
            Title = FinderLiterals.MemberFinderGadgetTitle;
            ShortKey = FinderLiterals.MemberFinderGadgetShortKey;
        }

        public override Loader GetLoaderAgent(string[] basePaths, Repository repository)
        {
            return new MemberLoaderAgent(basePaths, repository);
        }

        public override System.Drawing.Color GetColor() => System.Drawing.Color.FromArgb(0xE3, 0xEB, 0xF0);

        public override string GetTitle() => FinderLiterals.MemberFinderGadgetTitle;
    }
}
