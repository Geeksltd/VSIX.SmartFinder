using System.Drawing;
using System.Windows.Forms;
using Geeks.VSIX.SmartFinder.Definition;
using Geeks.VSIX.SmartFinder.FileFinder.FinderDrawerUtility;

namespace Geeks.VSIX.SmartFinder.FileFinder.FileDrawers
{
    internal class FileDrawer : FinderDrawer
    {
        internal static void DrawFiles(string text,
                                       DrawItemEventArgs e,
                                       Point position)
        {
            if (IsNestedClass(text))
            {
                ColorizeNestedClassFileItem(text, e, ref position);
            }
            else
            {
                ColorizeFileItem(text, e, ref position);
            }
        }

        static void ColorizeFileItem(string text,
                                             DrawItemEventArgs e,
                                             ref Point position)
        {
            var pathValue = text.Substring(0, text.LastIndexOf('\\') + 1);
            ColorizePathValue(string.Empty, pathValue, e, ref position);

            var fileName = text.Substring(text.LastIndexOf('\\') + 1);
            if (fileName.EndsWith(".cs"))
            {
                ColorizeClassName(position, fileName, e, isCshtml: false);
            }
            else if (fileName.EndsWith(FileExtensionTypes.CSHTML))
            {
                ColorizeClassName(position, fileName, e, isCshtml: true);
            }
            else
            {
                var textColor = e.State == DrawItemState.Selected ? Color.White : Color.Black;
                TextRenderer.DrawText(e.Graphics, fileName, e.Font, position, textColor, TextFormatFlags.NoPadding);
            }
        }

        static void ColorizeNestedClassFileItem(string text,
                                                        DrawItemEventArgs e,
                                                        ref Point position)
        {
            string className = string.Empty;
            var pathInidcatorStartChar = '-';
            className = text.Substring(0, text.IndexOf(pathInidcatorStartChar));

            ColorizeClassName(position, className, e, isCshtml: false);

            if (className != string.Empty)
            {
                position.X = CalculateStringWidth(className, e) + 4;
            }

            ColorizePathIndicator(e, ref position);
            ColorizePathValue(text, string.Empty, e, ref position);
        }

        static bool IsNestedClass(string text) => text.Contains(PathIndicator);

        static void ColorizeClassName(Point position,
                                              string className,
                                              DrawItemEventArgs e,
                                              bool isCshtml)
        {
            var textColor = e.State == DrawItemState.Selected ? Color.White : !isCshtml ? Color.DarkCyan : Color.Maroon;
            TextRenderer.DrawText(e.Graphics, className, e.Font, position, textColor, TextFormatFlags.NoPadding);
        }

        static void ColorizePathValue(string text,
                                             string pathValue,
                                             DrawItemEventArgs e,
                                             ref Point position)
        {
            if (!string.IsNullOrEmpty(text) ||
                !string.IsNullOrEmpty(pathValue))
            {
                string pathText = BuildPathTextValue(text, pathValue);
                var textColor = e.State == DrawItemState.Selected ? Color.White : Color.Gray;

                TextRenderer.DrawText(e.Graphics, pathText, e.Font, position, textColor, TextFormatFlags.NoPadding);

                position.X += CalculateStringWidth(pathValue, e);
            }
        }

        static string BuildPathTextValue(string text, string pathValue)
        {
            var pathText = string.Empty;
            if (!string.IsNullOrEmpty(text))
            {
                pathText = text.Substring(text.LastIndexOf('>') + 1);
            }

            if (!string.IsNullOrEmpty(pathValue))
            {
                pathText = pathValue;
            }

            return pathText;
        }

        static void ColorizePathIndicator(DrawItemEventArgs e, ref Point position)
        {
            var textColor = e.State == DrawItemState.Selected ? Color.White : Color.Purple;
            position.X += 10;

            TextRenderer.DrawText(e.Graphics, PathIndicator, e.Font, position, textColor, TextFormatFlags.NoPadding);
            position.X += CalculateStringWidth(PathIndicator, e);
        }
    }
}
