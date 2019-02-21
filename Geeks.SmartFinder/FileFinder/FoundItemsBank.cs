using System.Collections.Generic;

namespace Geeks.VSIX.SmartFinder.FileFinder
{
    internal static class FoundItemsBank
    {
        public static int Pointer { get; set; }

        static List<Item> items;
        public static List<Item> Items
        {
            get { return items; }
            set
            {
                items = value;
                Pointer = 0;
            }
        }
    }
}
