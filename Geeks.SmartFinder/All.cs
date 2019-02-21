using System.Collections.Generic;
using Geeks.VSIX.SmartFinder.FileFinder;
using Geeks.VSIX.SmartFinder.FileToggle;
using Geeks.VSIX.SmartFinder.GoTo;
using GeeksAddin;
// //using GeeksAddin.Attacher;

namespace Geeks.VSIX.SmartFinder
{
    public static class All
    {
        static List<Gadget> gadgets;
        public static List<Gadget> Gadgets => gadgets ?? (gadgets = new List<Gadget>
        {
            new FileToggleGadget(),
            new FixtureFileToggleGadget(),
            new FileFinderGadget(),
            new StyleFinderGadget(),
            new MemberFinderGadget(),
            new GotoNextFoundItemGadget(),
            new GotoPreviousFoundItemGadget()
        });
    }
}