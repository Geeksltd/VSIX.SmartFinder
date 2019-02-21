using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Geeks.SmartFinder.FileFinder;
using Geeks.VSIX.SmartFinder.Definition;
using Geeks.VSIX.SmartFinder.FileFinder.FileDrawers;
using Geeks.VSIX.SmartFinder.FileFinder.FinderDrawerUtility;
using Geeks.VSIX.SmartFinder.FileFinder.MemberDrawers;

namespace Geeks.VSIX.SmartFinder.FileFinder
{
    public sealed class FlickerFreeListBox : ListBox
    {
        Image LoadingIcon = FileTypesResources.loading2;
        StringFormat ItemFormat = new StringFormat();

        public FlickerFreeListBox()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, value: true);

            DrawMode = DrawMode.OwnerDrawFixed;

            ImageAnimator.Animate(LoadingIcon, OnFrameChanged);
        }

        public void SortList()
        {
            Sort();
        }
        protected override void Sort()
        {
            var sortedItems = Items.Cast<Item>().ToArray().OrderBy(i => i).ToArray();
            Items.Clear();
            Items.AddRange(sortedItems);
        }


        EmptyBehaviour emptyBehaviour;
        public EmptyBehaviour EmptyBehaviour
        {
            get => emptyBehaviour;
            set
            {
                emptyBehaviour = value;
                Invalidate();
            }
        }

        bool showLoadingAtTheEndOfList;
        public bool ShowLoadingAtTheEndOfList
        {
            get => showLoadingAtTheEndOfList;
            set
            {
                showLoadingAtTheEndOfList = value;
                Invalidate();
            }
        }

        void OnFrameChanged(object obj, EventArgs e)
        {
            if (ShowLoadingAtTheEndOfList) Invalidate();
        }

        // TODO: Ali - Apply Interface seggregation
        // -------------------------------------------- <Drawing Items> -------------------------------------------- //

        readonly Brush brshHighlight = new SolidBrush(Color.FromArgb(250, 209, 245, 247));

        public string[] HighlightWords { get; set; }

        Size DummySize = new Size(0, 0);

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (Items.Count == 0) return;

            e.DrawBackground();

            var graphic = e.Graphics;
            var item = (Item)Items[e.Index];
            var position = new Point(e.Bounds.X, e.Bounds.Y);
            var text = item.ToString();

            var iconImage = GetItemImage(item);
            graphic.DrawImage(iconImage, position);

            position.X += iconImage.Width + 6; // padding
            position.Y += 4; // padding

            var finderMode = FinderDrawer.DetectFinderMode(Parent);
            switch (finderMode)
            {
                case FinderMode.Member:
                    if (NeedsHighlighting(e))
                        FinderDrawer.HighLightSearchWords(text, e, position, HighlightWords, brshHighlight);

                    DrawMemberItems(e, item, text, position);
                    break;
                case FinderMode.File:
                    if (NeedsHighlighting(e))
                        FinderDrawer.HighLightSearchWords(text, e, position, HighlightWords, brshHighlight);

                    FileDrawer.DrawFiles(text, e, position);
                    break;
                case FinderMode.StyleSheet:
                    StyleSheetDrawer.
                        DrawStyleSheets(e, graphic, position, text, HighlightWords, DummySize, brshHighlight);
                    break;
                case FinderMode.Unspecified:
                    break;
                default:
                    break;
            }

            base.OnDrawItem(e);
        }

        static void DrawMemberItems(DrawItemEventArgs e, Item item, string text, Point position)
        {
            switch (item.MemberType)
            {
                case MemberType.Property:
                    PropertyDrawer.DrawProperties(text, e, position);
                    break;
                case MemberType.Method:
                    MethodDrawer.DrawMethods(text, e, position);
                    break;
                default:
                    break;
            }
        }

        static bool NeedsHighlighting(DrawItemEventArgs e)
        {
            return !string.IsNullOrEmpty(FinderForm.SearchTerm) && e.State == DrawItemState.Default;
        }

        // -------------------------------------------- </Drawing Items> --------------------------------------------- //

        Image GetItemImage(Item item)
        {
            var fileName = System.IO.Path.GetFileName(item.FileName);

            if (item.IsMSharp())
            {
                return IconDictionary.MSharpIcon;
            }
            if (item.IsMSharpUI())
            {
                return IconDictionary.MSharpUIIcon;
            }

            switch (item.Icon)
            {
                case IconType.Auto:
                {
                    var file = item.FileName.ToLowerInvariant();

                    foreach (var icon in IconDictionary.Icons)
                        if (file.EndsWith(icon.Key)) 
                            return icon.Value;
                    break;
                }
                case IconType.Method:
                    return FileTypesResources.method;
                case IconType.Class:
                    return FileTypesResources.klass;
                case IconType.Property:
                    return FileTypesResources.property;
                default:
                    break;
            }

            return FileTypesResources.generic;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var iRegion = new Region(e.ClipRectangle);
            var g = e.Graphics;

            g.FillRegion(new SolidBrush(BackColor), iRegion);

            if (Items.Count > 0)
            {
                for (var i = 0; i < Items.Count; ++i)
                {
                    var rect = GetItemRectangle(i);
                    if (!e.ClipRectangle.IntersectsWith(rect)) continue;

                    if ((SelectionMode == SelectionMode.One && SelectedIndex == i)
                        || (SelectionMode == SelectionMode.MultiSimple && SelectedIndices.Contains(i))
                        || (SelectionMode == SelectionMode.MultiExtended && SelectedIndices.Contains(i)))
                    {
                        OnDrawItem(new DrawItemEventArgs(g, Font,
                            rect, i,
                            DrawItemState.Selected, ForeColor,
                            BackColor));
                    }
                    else
                    {
                        OnDrawItem(new DrawItemEventArgs(g, Font,
                            rect, i,
                            DrawItemState.Default, ForeColor,
                            BackColor));
                    }

                    iRegion.Complement(rect);
                }
            }
            else
            {
                if (EmptyBehaviour == EmptyBehaviour.ShowNotFound)
                {
                    g.DrawString("Not found", Parent.Font, Brushes.Gray, 0, 0);
                }
            }

            if (ShowLoadingAtTheEndOfList)
            {
                var rect = new Rectangle(0, 0, Width, ItemHeight);
                if (Items.Count >= 1)
                {
                    var lastRect = GetItemRectangle(Items.Count - 1);
                    rect.Y = lastRect.Y + ItemHeight;
                }

                ImageAnimator.UpdateFrames(LoadingIcon);
                g.DrawImage(LoadingIcon, rect.X, rect.Y);
                g.DrawString("Loading...", Parent.Font, Brushes.Firebrick, LoadingIcon.Width + 3, rect.Y);
            }

            base.OnPaint(e);
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            Invalidate();
            base.OnSelectedIndexChanged(e);
        }
    }

}