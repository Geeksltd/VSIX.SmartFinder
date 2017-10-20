using System.Collections.Generic;
////using GeeksAddin.Attacher;
using GeeksAddin.FileFinder;
using GeeksAddin.FileToggle;

namespace GeeksAddin
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
