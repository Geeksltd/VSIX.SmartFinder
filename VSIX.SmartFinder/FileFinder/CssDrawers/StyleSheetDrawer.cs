using Geeks.VSIX.SmartFinder.FileFinder.FinderDrawerUtility;
using System.Drawing;
using System.Windows.Forms;

namespace Geeks.VSIX.SmartFinder.FileFinder.FileDrawers
{
    internal class StyleSheetDrawer : FinderDrawer
    {
        internal static void DrawStyleSheets(DrawItemEventArgs e,
                                             Graphics graphic,
                                             Point position,
                                             string text,
                                             string[] highlightedWords,
                                             Size dummySize,
                                             Brush brshHighlight)
        {
            var wordRanges = StringUntils.BreakToRanges(text, highlightedWords);
            foreach (var range in wordRanges)
            {
                var stringWidth = TextRenderer.MeasureText(graphic, range.Text, e.Font, dummySize, TextFormatFlags.NoPadding).Width;
                var textColor = e.State == DrawItemState.Selected ? Color.White : Color.Black;

                if (range.IsHighlighted && e.State == DrawItemState.Default)
                {
                    graphic.FillRectangle(brshHighlight, position.X, position.Y, stringWidth, e.Bounds.Height);
                }

                TextRenderer.DrawText(graphic, range.Text, e.Font, position, textColor, TextFormatFlags.NoPadding);
                position.X += stringWidth;
            }
        }
    }
}
