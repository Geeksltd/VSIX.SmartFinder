using Geeks.VSIX.SmartFinder.FileFinder;
using Geeks.VSIX.SmartFinder.FileToggle;
using Geeks.VSIX.SmartFinder.GoTo;
using GeeksAddin;
using System.Collections.Generic;
////using GeeksAddin.Attacher;

namespace Geeks.VSIX.SmartFinder
{
    public static class All
    {
        static List<Gadget> _Gadgets;
        public static List<Gadget> Gadgets
        {
            get
            {
                if (_Gadgets == null)
                {
                    _Gadgets = new List<Gadget>();

                    _Gadgets.Add(new FileToggleGadget());
                    _Gadgets.Add(new FixtureFileToggleGadget());

                    _Gadgets.Add(new FileFinderGadget());
                    _Gadgets.Add(new StyleFinderGadget());
                    _Gadgets.Add(new MemberFinderGadget());
                    _Gadgets.Add(new GotoNextFoundItemGadget());
                    _Gadgets.Add(new GotoPreviousFoundItemGadget());
                }
                return _Gadgets;
            }
        }
    }
}
