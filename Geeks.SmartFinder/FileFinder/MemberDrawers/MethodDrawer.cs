using System;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Geeks.SmartFinder.Properties;
using Geeks.VSIX.SmartFinder.FileFinder.FinderDrawerUtility;


namespace Geeks.VSIX.SmartFinder.FileFinder.MemberDrawers
{
    internal class MethodDrawer : FinderDrawer
    {
        internal static void DrawMethods(string text,
                                         DrawItemEventArgs e,
                                         Point position)
        {
            if (!Settings.Default.ShowMethods) return;

            var className = ColorizeClassName(text, e, position);

            if (className != string.Empty)
            {
                position.X += CalculateStringWidth(className, e);
            }

            var methodPart = text.Substring(0, text.LastIndexOf('>') - 5);

            ColorizeMethod(methodPart, e, ref position);
            ColorizePathIndicator(e, ref position);
            ColorizePathValue(text, e, ref position);
        }

        static bool HasMultipleArguments(string methodArgumentType) => methodArgumentType.Contains(',');

        static void ColorizeMethod(string sourceString,
                                            DrawItemEventArgs e,
                                            ref Point position)
        {
            ColorizeMethodName(e, ref position, sourceString);
            ColorizeMethodArgumentTypes(e, ref position, sourceString);
            ColorizeMethodClosings(e, ref position);
            ColorizeMethodReturnValue(e, ref position, sourceString);
        }

        static void ColorizeMethodReturnValue(DrawItemEventArgs e,
                                                     ref Point position,
                                                     string sourceString)
        {
            if (Settings.Default.ShowMethodReturnTypes)
            {
                var methodReturnValue = sourceString.Split(':')[1].Split(' ')[0];
                var textColor = e.State == DrawItemState.Selected ? Color.White : Color.Olive;
                TextRenderer.DrawText(e.Graphics, methodReturnValue, e.Font, position, textColor, TextFormatFlags.NoPadding);
                position.X += CalculateStringWidth(methodReturnValue, e);
            }
        }

        static void ColorizeMethodClosings(DrawItemEventArgs e, ref Point position)
        {
            var methodClosingCharacters = Settings.Default.ShowMethodReturnTypes ? ") : " : ") ";
            var textColor = e.State == DrawItemState.Selected ? Color.White : Color.Black;
            TextRenderer.DrawText(e.Graphics, methodClosingCharacters, e.Font, position, textColor, TextFormatFlags.NoPadding);
            position.X += CalculateStringWidth(methodClosingCharacters, e);
        }

        static void ColorizeMethodArgumentTypes(DrawItemEventArgs e,
                                                       ref Point position,
                                                       string sourceString)
        {
            if (Settings.Default.ShowMethodParameters)
            {
                position.X = ColorizingMethodArguments(e, position, sourceString);
            }
        }

        static void ColorizeMethodName(DrawItemEventArgs e,
                                              ref Point position,
                                              string sourceString)
        {
            var restOfFirstPart = RemoveClassName(sourceString);
            var preMethodChar = Settings.Default.ShowClassNames ? "." : " ";
            var methodName = string.Format("{0}{1}", preMethodChar, restOfFirstPart.Substring(0, restOfFirstPart.IndexOf('(') + 1));

            var textColor = e.State == DrawItemState.Selected ? Color.White : Color.Black;
            TextRenderer.DrawText(e.Graphics, methodName, e.Font, position, textColor, TextFormatFlags.NoPadding);

            position.X += CalculateStringWidth(methodName, e);
        }

        static string RemoveClassName(string sourceString)
        {
            return sourceString.Substring(sourceString.IndexOf('.') + 1);
        }

        static string ColorizeClassName(string text, DrawItemEventArgs e, Point position)
        {
            var className = string.Empty;
            if (Settings.Default.ShowClassNames)
            {
                className = text.Substring(0, text.IndexOf('.'));
                var textColor = e.State == DrawItemState.Selected ? Color.White : Color.DarkCyan;
                TextRenderer.DrawText(e.Graphics, className, e.Font, position, textColor, TextFormatFlags.NoPadding);
            }

            return className;
        }

        static void ColorizePathValue(string text,
                                               DrawItemEventArgs e,
                                               ref Point position)
        {
            var pathText = text.Substring(text.LastIndexOf('>') + 1);
            var textColor = e.State == DrawItemState.Selected ? Color.White : Color.Gray;
            TextRenderer.DrawText(e.Graphics, pathText, e.Font, position, textColor, TextFormatFlags.NoPadding);
            position.X += CalculateStringWidth(pathText, e);
        }

        static void ColorizePathIndicator(DrawItemEventArgs e, ref Point position)
        {
            var textColor = e.State == DrawItemState.Selected ? Color.White : Color.Purple;
            TextRenderer.DrawText(e.Graphics, PathIndicator, e.Font, position, textColor, TextFormatFlags.NoPadding);
            position.X += CalculateStringWidth(PathIndicator, e);
        }

        static int ColorizingMethodArguments(DrawItemEventArgs e,
                                                       Point position,
                                                       string sourceString)
        {
            var methodArgumentType = Regex.Match(sourceString, @"\(([^\)]*)\)").Groups[1].Value;

            if (string.IsNullOrEmpty(methodArgumentType)) return position.X;

            if (HasMultipleArguments(methodArgumentType))
            {
                if (ContainsGenerics(methodArgumentType))
                {
                    methodArgumentType = Regex.Replace(methodArgumentType, @"<([^<>]+)>", m => string.Format("<{0}>", m.Groups[1].Value.Replace(",", "#COMMA#")));
                }

                var argumentArray = methodArgumentType.Split(',');
                for (var i = 0; i < argumentArray.Length; i++)
                {
                    var argumentType = argumentArray[i].Split(' ');

                    if (argumentType[0] == string.Empty)
                    {
                        argumentType = argumentType.Where(at => at != string.Empty).ToArray();
                    }

                    if (argumentType[0].Contains("#COMMA#"))
                    {
                        argumentType[0] = argumentType[0].Replace("#COMMA#", ",");
                    }

                    var textColor = e.State == DrawItemState.Selected ? Color.White : Color.Blue;
                    TextRenderer.DrawText(e.Graphics, argumentType[0], e.Font, position, textColor, TextFormatFlags.NoPadding);
                    position.X += CalculateStringWidth(argumentType[0], e);

                    if (argumentType.Length > 1)
                    {
                        var textColorType = e.State == DrawItemState.Selected ? Color.White : Color.Black;
                        TextRenderer.DrawText(e.Graphics, " " + argumentType[1], e.Font, position, textColorType, TextFormatFlags.NoPadding);
                        position.X += CalculateStringWidth(" " + argumentType[1], e);
                    }

                    if (i != argumentArray.Length - 1)
                    {
                        var textColorComma = e.State == DrawItemState.Selected ? Color.White : Color.Black;
                        TextRenderer.DrawText(e.Graphics, ", ", e.Font, position, textColorComma, TextFormatFlags.NoPadding);
                        position.X += CalculateStringWidth(", ", e);
                    }
                }
            }
            else
            {
                var argumentType = methodArgumentType.Split(' ');

                var textColor = e.State == DrawItemState.Selected ? Color.White : Color.Blue;
                TextRenderer.DrawText(e.Graphics, argumentType[0], e.Font, position, textColor, TextFormatFlags.NoPadding);
                position.X += CalculateStringWidth(argumentType[0], e);

                var textColorType = e.State == DrawItemState.Selected ? Color.White : Color.Black;
                TextRenderer.DrawText(e.Graphics, " " + argumentType[1], e.Font, position, textColorType, TextFormatFlags.NoPadding);
                position.X += CalculateStringWidth(" " + argumentType[1], e);
            }

            return position.X;
        }

        static bool ContainsGenerics(string methodArgumentType)
        {
            return methodArgumentType.Contains("<") && methodArgumentType.Contains(">");
        }
    }
}
