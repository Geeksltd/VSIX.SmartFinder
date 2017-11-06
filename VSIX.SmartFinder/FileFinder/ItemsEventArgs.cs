using System;
using System.Collections.Generic;

namespace Geeks.VSIX.SmartFinder.FileFinder
{
    internal class ItemsEventArgs : EventArgs
    {
        public IEnumerable<Item> Items;
    }
}
