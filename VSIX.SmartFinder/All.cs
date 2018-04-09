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
        public static List<Gadget> Gadgets
        {
            get
            {
                if (gadgets == null)
                {
                    gadgets = new List<Gadget>();

                    gadgets.Add(new FileToggleGadget());
                    gadgets.Add(new FixtureFileToggleGadget());

                    gadgets.Add(new FileFinderGadget());
                    gadgets.Add(new StyleFinderGadget());
                    gadgets.Add(new MemberFinderGadget());
                    gadgets.Add(new GotoNextFoundItemGadget());
                    gadgets.Add(new GotoPreviousFoundItemGadget());
                }

                return gadgets;
            }
        }
    }
}