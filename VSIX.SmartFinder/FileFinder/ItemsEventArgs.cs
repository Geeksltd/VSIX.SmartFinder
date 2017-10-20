using System;
using System.Collections.Generic;

namespace GeeksAddin.FileFinder
{
    internal class ItemsEventArgs : EventArgs
    {
        public IEnumerable<Item> Items;
    }
}
