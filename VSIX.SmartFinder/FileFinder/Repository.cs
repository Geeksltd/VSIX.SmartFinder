using System;
using System.Collections.Generic;

namespace Geeks.VSIX.SmartFinder.FileFinder
{
    internal class Repository
    {
        List<Item> Items = new List<Item>();
        // string[] BasePaths;
        bool Suspended;
        List<Item> ItemsWhileSuspension = new List<Item>();

        public Repository(/*string[] basePaths*/)
        {
            // BasePaths = basePaths;
        }

        public Item ItemAt(int index) => Items[index];

        public int Length => Items.Count;

        public void AppendRange(IEnumerable<Item> items)
        {
            if (Suspended)
            {
                ItemsWhileSuspension.AddRange(items);
            }
            else
            {
                Items.AddRange(items);
                OnItemsAppended(items);
            }
        }

        public event EventHandler<ItemsEventArgs> ItemsAppended;
        protected virtual void OnItemsAppended(IEnumerable<Item> items)
        {
            var handler = ItemsAppended;
            if (handler != null)
            {
                handler(this, new ItemsEventArgs { Items = items });
            }
        }

        internal void SuspendAppend() => Suspended = true;

        internal void ContinueAppend()
        {
            // TODO: make this atomic
            Suspended = false;
            AppendRange(ItemsWhileSuspension);
            ItemsWhileSuspension.Clear();
        }

        internal void Clear() => Items.Clear();
    }
}