using System.Drawing;
using System.Windows.Forms;
using Geeks.GeeksProductivityTools.FileFinder.FinderDrawerUtility;
using Geeks.VSIX.SmartFinder.Properties;

namespace Geeks.GeeksProductivityTools.FileFinder.MemberDrawers
{
    internal class PropertyDrawer : FinderDrawer
    {
        internal static void DrawProperties(string text,
                                            DrawItemEventArgs e,
                                            Point position)
        {
            if (!Settings.Default.ShowProperties) return;

            var splittedText = text.Split(':');
            var classAndProperty = splittedText[0].Split('.');

            var showCalssNames = Settings.Default.ShowClassNames;
            if (showCalssNames)
            {
                DrawClassName(e, classAndProperty, ref position);
            }

            DrawProperyName(e, ref position, showCalssNames, classAndProperty);
            DrawDataType(e, ref position, splittedText);
            DrawPathIndicator(e, ref position);
            DrawPathValue(text, e, ref position);
        }

        static void DrawPathValue(string text, DrawItemEventArgs e, ref Point position)
        {
            var pathText = text.Substring(text.LastIndexOf('>') + 1);
            var textColor = e.State == DrawItemState.Selected ? Color.White : Color.Gray;
            TextRenderer.DrawText(e.Graphics, pathText, e.Font, position, textColor, TextFormatFlags.NoPadding);
            position.X += CalculateStringWidth(pathText, e);
        }

        static void DrawPathIndicator(DrawItemEventArgs e, ref Point position)
        {
            var textColor = e.State == DrawItemState.Selected ? Color.White : Color.Purple;
            TextRenderer.DrawText(e.Graphics, PathIndicator, e.Font, position, textColor, TextFormatFlags.NoPadding);
            position.X += CalculateStringWidth(PathIndicator, e);
        }

        static void DrawDataType(DrawItemEventArgs e, ref Point position, string[] splittedText)
        {
            var dataType = string.Format("{0}", splittedText[1].Substring(0, splittedText[1].LastIndexOf('>') - 5));
            var textColor = e.State == DrawItemState.Selected ? Color.White : Color.Blue;
            TextRenderer.DrawText(e.Graphics, dataType, e.Font, position, textColor, TextFormatFlags.NoPadding);
            position.X += CalculateStringWidth(dataType, e);
        }

        static void DrawProperyName(DrawItemEventArgs e,
                                           ref Point position,
                                             bool showCalssNames,
                                             string[] classAndProperty)
        {
            var propertyName = string.Empty;

            if (showCalssNames)
            {
                propertyName = string.Format(".{0}:", classAndProperty[1]);
            }
            else
            {
                propertyName = string.Format("{0}:", classAndProperty[0]);
            }

            var textColor = e.State == DrawItemState.Selected ? Color.White : Color.Black;
            TextRenderer.DrawText(e.Graphics, propertyName, e.Font, position, textColor, TextFormatFlags.NoPadding);
            position.X += CalculateStringWidth(propertyName, e);
        }

        static void DrawClassName(DrawItemEventArgs e, string[] classAndProperty, ref Point position)
        {
            var textColor = e.State == DrawItemState.Selected ? Color.White : Color.DarkCyan;
            TextRenderer.DrawText(e.Graphics, classAndProperty[0], e.Font, position, textColor, TextFormatFlags.NoPadding);
            position.X += CalculateStringWidth(classAndProperty[0], e);
        }
    }
}
