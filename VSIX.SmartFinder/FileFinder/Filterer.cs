using System;
using System.Collections.Generic;
using System.Linq;

namespace GeeksAddin.FileFinder
{
    internal class Filterer
    {
        const int MaxAnnounces = 20;

        public Repository Repository { get; private set; }
        string[] BasePaths;
        string Filter;
        public IEnumerable<string> ExcludedFileTypes { get; set; }
        public string[] Words { get; private set; }
        int AnnouncedItemsCount;
        public bool IsBusy { get; private set; }

        public Filterer(string[] basePaths, Repository repository)
        {
            Repository = repository;
            BasePaths = basePaths;

            Repository.ItemsAppended += new EventHandler<ItemsEventArgs>(Repository_ItemsAppended);
        }

        void Repository_ItemsAppended(object sender, ItemsEventArgs e)
        {
            foreach (var item in e.Items)
            {
                if (item.MatchesWith(Words))
                    AnnounceItem(item);
            }
        }

        List<Item> _FoundItems;

        public List<Item> FoundItems => _FoundItems;

        public void SetFilter(string filter)
        {
            AnnouncedItemsCount = 0;

            // Tell repository not notify item append may be necessary here
            //Repository.SuspendAppend();
            Filter = StringUntils.ToValidFileName(filter);
            Words = Filter.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            AnnounceMatchingItems();
            OnAnnouncementOfExistingItemsFinished();
            // Tell repository it can continue notifying may be necessary here
            //Repository.ContinueAppend();
        }

        void AnnounceMatchingItems()
        {
            IsBusy = true;
            _FoundItems = new List<Item>();
            var toAnnouncedItemsCount = AnnouncedItemsCount;

            for (int i = 0; i < Repository.Length; i++)
            {
                var item = Repository.ItemAt(i);
                if (ItemIsExcluded(item))
                    continue;
                if (item.MatchesWith(Words))
                {
                    _FoundItems.Add(item);
                    toAnnouncedItemsCount++;
                    if (toAnnouncedItemsCount >= MaxAnnounces)
                        break;
                }
            }

            var sortedFoundItems = _FoundItems.OrderBy(i => i.ToString().Length);
            foreach (var item in sortedFoundItems)
            {
                AnnounceItem(item);
            }

            IsBusy = false;
        }

        bool ItemIsExcluded(Item item)
        {
            if (ExcludedFileTypes != null)
                return item.FileName.ToLower().EndsWithAny(ExcludedFileTypes.Select(t => t.ToLower()).ToArray());
            return false;
        }

        void AnnounceItem(Item item)
        {
            if (!item.Exists())
                return;

            if (ItemIsExcluded(item)) // for calls directly from Loader (instead of AnnounceMatchingItems())
                return;

            if (AnnouncedItemsCount < MaxAnnounces)
            {
                OnItemsFound(new[] { item });
                AnnouncedItemsCount++;
            }
        }

        internal void UpdateRepositoryItems(Loader loader)
        {
            Repository.Clear();
            if (!loader.IsBusy) loader.RunWorkerAsync(this);
        }

        public event EventHandler<ItemsEventArgs> ItemsFound;
        protected virtual void OnItemsFound(IEnumerable<Item> items)
        {
            ItemsFound?.Invoke(this, new ItemsEventArgs { Items = items });
        }

        public event EventHandler AnnouncementOfExistingItemsFinished;
        protected virtual void OnAnnouncementOfExistingItemsFinished()
        {
            var handler = AnnouncementOfExistingItemsFinished;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
    }
}
