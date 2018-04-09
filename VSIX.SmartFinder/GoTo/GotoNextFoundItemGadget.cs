using System.Linq;
using Geeks.VSIX.SmartFinder.FileFinder;
using GeeksAddin;

namespace Geeks.VSIX.SmartFinder.GoTo
{
    internal class GotoNextFoundItemGadget : Gadget
    {
        public GotoNextFoundItemGadget()
        {
            Name = "GotoNextFoundItem";
            Title = "Go to Next Found Item";
            ShortKey = "CTRL+F8";
        }

        public override void Run(EnvDTE80.DTE2 app)
        {
            var basePath = Utils.FindSolutionDirectories(app);
            if (basePath == null || basePath.Count() == 0) return;

            if (FoundItemsBank.Items != null)
            {
                FoundItemsBank.Pointer++;
                if (FoundItemsBank.Pointer >= FoundItemsBank.Items.Count)
                    FoundItemsBank.Pointer = 0;
                FinderBaseGadget.GotoItem(app, basePath, FoundItemsBank.Items[FoundItemsBank.Pointer]);
            }
        }
    }
}
