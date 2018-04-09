using System.Linq;
using Geeks.VSIX.SmartFinder.FileFinder;
using GeeksAddin;

namespace Geeks.VSIX.SmartFinder.GoTo
{
    internal class GotoPreviousFoundItemGadget : Gadget
    {
        public GotoPreviousFoundItemGadget()
        {
            Name = "GotoPreviousFoundItem";
            Title = "Go to Previous Found Item";
            ShortKey = "CTRL+F7";
        }

        public override void Run(EnvDTE80.DTE2 app)
        {
            var basePaths = Utils.FindSolutionDirectories(app);
            if (basePaths == null || basePaths.Count() == 0) return;

            if (FoundItemsBank.Items != null)
            {
                FoundItemsBank.Pointer--;
                if (FoundItemsBank.Pointer < 0)
                    FoundItemsBank.Pointer = FoundItemsBank.Items.Count - 1;
                FinderBaseGadget.GotoItem(app, basePaths, FoundItemsBank.Items[FoundItemsBank.Pointer]);
            }
        }
    }
}
