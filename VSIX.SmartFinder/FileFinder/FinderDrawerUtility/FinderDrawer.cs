using System;
using System.Drawing;
using System.Windows.Forms;
using Geeks.VSIX.SmartFinder.Definition;

namespace Geeks.VSIX.SmartFinder.FileFinder.FinderDrawerUtility
{
    internal class FinderDrawer
    {
        const string PATH_INDICATOR = "----->";

        protected static string PathIndicator => PATH_INDICATOR;

        protected static int CalculateStringWidth(string value, DrawItemEventArgs e)
        {
            return TextRenderer.MeasureText(e.Graphics, value, e.Font, new Size(0, 0), TextFormatFlags.NoPadding).Width;
        }

        internal static FinderMode DetectFinderMode(Control parent)
        {
            if (!(parent is FinderForm))
            {
                return FinderMode.Unspecified;
            }

            var finderForm = (FinderForm)parent;
            switch (finderForm.Text)
            {
                case FinderLiterals.MemberFinderGadgetTitle:
                    return FinderMode.Member;
                case FinderLiterals.FileFinderGadgetTitle:
                    return FinderMode.File;
                case FinderLiterals.StyleFinderGadgetTitle:
                    return FinderMode.StyleSheet;
                default:
                    return FinderMode.Unspecified;
            }
        }

        internal static void HighLightSearchWords(string sourceString,
                                                  DrawItemEventArgs e,
                                                  Point position,
                                                  string[] highLightWords,
                                                  Brush highlightBrush)
        {
            var wordRangeList = StringUntils.BreakToRanges(sourceString, highLightWords);
            foreach (var range in wordRangeList)
            {
                var stringWidth = CalculateStringWidth(range.Text, e);
                if (range.IsHighlighted && e.State == DrawItemState.Default)
                {
                    e.Graphics.FillRectangle(highlightBrush, position.X, position.Y, stringWidth, e.Bounds.Height);
                }

                var widthValue = Convert.ToInt32(stringWidth);
                position.X += widthValue;
            }
        }
    }
}