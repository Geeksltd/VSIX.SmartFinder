using System;
using System.Drawing;
using System.Windows.Forms;
using Geeks.VSIX.SmartFinder.Definition;
using Geeks.VSIX.SmartFinder.FileFinder.FileDrawers;
using Geeks.VSIX.SmartFinder.FileFinder.FinderDrawerUtility;
using Geeks.VSIX.SmartFinder.FileFinder.MemberDrawers;
using Geeks.VSIX.SmartFinder.FileFinder.FileFinder;

namespace Geeks.VSIX.SmartFinder.FileFinder
{
    public class FlickerFreeListBox : ListBox
    {
        Image LoadingIcon = FileTypesResources.loading2;
        StringFormat ItemFormat = new StringFormat();

        public FlickerFreeListBox()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, value: true);

            DrawMode = DrawMode.OwnerDrawFixed;

            ImageAnimator.Animate(LoadingIcon, new EventHandler(OnFrameChanged));
        }

        EmptyBehaviour _EmptyBehaviour;
        public EmptyBehaviour EmptyBehaviour
        {
            get { return _EmptyBehaviour; }
            set
            {
                _EmptyBehaviour = value;
                Invalidate();
            }
        }

        bool _ShowLoadingAtTheEndOfList;
        public bool ShowLoadingAtTheEndOfList
        {
            get { return _ShowLoadingAtTheEndOfList; }
            set
            {
                _ShowLoadingAtTheEndOfList = value;
                Invalidate();
            }
        }

        void OnFrameChanged(object obj, EventArgs e)
        {
            if (ShowLoadingAtTheEndOfList)
            {
                Invalidate();
            }
        }

        // TODO: Ali - Apply Interface seggregation
        // -------------------------------------------- <Drawing Items> -------------------------------------------- //

        Brush brshHighlight = new SolidBrush(Color.FromArgb(250, 209, 245, 247));

        public string[] HighlightWords { get; set; }

        Size DummySize = new Size(0, 0);

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (Items.Count == 0) return;

            e.DrawBackground();

            var graphic = e.Graphics;
            var item = Items[e.Index] as Item;
            var position = new Point(e.Bounds.X, e.Bounds.Y);
            var text = item.ToString();

            var iconImage = GetItemImage(item);
            graphic.DrawImage(iconImage, position);

            position.X += iconImage.Width + 2; // padding
            position.Y += 2; // padding

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
        const string MSharp_File1 = "@Model.cs";
        const string MSharp_File2 = "@UI.cs";

        Image GetItemImage(Item item)
        {
            var fileName = System.IO.Path.GetFileName(item.FileName);

            if (string.Compare(MSharp_File1, fileName, true) == 0 || string.Compare(MSharp_File2, fileName, true) == 0)
            {
                return IconDictionary.MSharpIcon;
            }

            if (item.Icon == IconType.Auto)
            {
                var file = item.FileName.ToLowerInvariant();

                foreach (var icon in IconDictionary.Icons)
                    if (file.EndsWith(icon.Key)) return icon.Value;
            }

            else if (item.Icon == IconType.Method)
                return FileTypesResources.method;

            else if (item.Icon == IconType.Class)
                return FileTypesResources.klass;

            else if (item.Icon == IconType.Property)
                return FileTypesResources.property;

            return FileTypesResources.generic;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var iRegion = new Region(e.ClipRectangle);
            var g = e.Graphics;

            g.FillRegion(new SolidBrush(this.BackColor), iRegion);

            if (this.Items.Count > 0)
            {
                for (int i = 0; i < this.Items.Count; ++i)
                {
                    var irect = this.GetItemRectangle(i);
                    if (e.ClipRectangle.IntersectsWith(irect))
                    {
                        if ((this.SelectionMode == SelectionMode.One && this.SelectedIndex == i)
                        || (this.SelectionMode == SelectionMode.MultiSimple && this.SelectedIndices.Contains(i))
                        || (this.SelectionMode == SelectionMode.MultiExtended && this.SelectedIndices.Contains(i)))
                        {
                            OnDrawItem(new DrawItemEventArgs(g, this.Font,
                                irect, i,
                                DrawItemState.Selected, this.ForeColor,
                                this.BackColor));
                        }
                        else
                        {
                            OnDrawItem(new DrawItemEventArgs(g, this.Font,
                                irect, i,
                                DrawItemState.Default, this.ForeColor,
                                this.BackColor));
                        }
                        iRegion.Complement(irect);
                    }
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
                    var lastRect = this.GetItemRectangle(Items.Count - 1);
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