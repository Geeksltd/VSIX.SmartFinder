using System.Collections.Generic;

namespace GeeksAddin.FileFinder
{
    internal static class FoundItemsBank
    {
        public static int Pointer { get; set; }

        static List<Item> _Items;
        public static List<Item> Items
        {
            get { return _Items; }
            set
            {
                _Items = value;
                Pointer = 0;
            }
        }
    }
}
