using System;
using System.IO;
using System.Linq;
using Geeks.GeeksProductivityTools.Definition;

namespace Geeks.GeeksProductivityTools.FileToggle
{
    internal static class FileToggleExtensions
    {
        internal static string FindSisterFile(this string activeDocument)
        {
            if (activeDocument.IsZebbleOrOfFile())
                return activeDocument.FindZebblBasedSisterFile();

            else if (activeDocument.IsEntityOrLogicBasedFile())
                return activeDocument.FindLogicBasedSisterFile();

            else if (activeDocument.IsMvcBasedFile())
                return activeDocument.FindMvcBasedSisterFile();

            else
                return activeDocument.FindAspxBasedSisterFile();
        }

        static bool IsZebbleOrOfFile(this string activeDocument)
        {
            return Path.GetExtension(activeDocument) == FileExtensionTypes.ZEBBLE || activeDocument.ToLower().Contains("app.ui");
        }

        static bool IsMvcBasedFile(this string activeDocument)
        {
            if (activeDocument.Contains("\\Controllers\\Pages\\", caseSensitive: false))
                return true;

            if (activeDocument.Contains("\\Controllers\\Modules\\", caseSensitive: false))
                return true;

            if (activeDocument.Contains("\\Views\\Pages\\", caseSensitive: false))
                return true;

            if (activeDocument.Contains("\\Views\\Modules\\", caseSensitive: false))
                return true;

            return false;
        }

        static bool IsEntityOrLogicBasedFile(this string activeDocument)
        {
            if (activeDocument.Contains("\\Entities\\", caseSensitive: false))
                return true;

            if (activeDocument.Contains("\\@Entities", caseSensitive: false))
                return true;

            if (activeDocument.Contains("\\Test\\@Logic\\", caseSensitive: false) && activeDocument.EndsWith("Fixture.cs", StringComparison.OrdinalIgnoreCase))
                return true;

            if (activeDocument.Contains("\\-Logic\\", caseSensitive: false))
                return true;

            return false;
        }

        static string FindAspxBasedSisterFile(this string activeDocument)
        {
            if (Path.GetExtension(activeDocument).EndsWith(FileExtensionTypes.CSHARP))
                return activeDocument.Remove(activeDocument.Length - FileExtensionTypes.CSHARP.Length);
            else
                return activeDocument + FileExtensionTypes.CSHARP;
        }

        static string FindMvcBasedSisterFile(this string activeDocument)
        {
            if (activeDocument.Contains("\\Controllers\\Pages\\", caseSensitive: false) || activeDocument.Contains("\\Controllers\\Modules\\", caseSensitive: false))
            {
                return activeDocument.Replace("\\Controllers\\", "\\Views\\", caseSensitive: false)
                                     .Replace("Controller.cs", FileExtensionTypes.CSHTML, caseSensitive: false);
            }

            return activeDocument.Replace("\\Views\\", "\\Controllers\\", caseSensitive: false)
                                 .Replace(FileExtensionTypes.CSHTML, "Controller.cs", caseSensitive: false);
        }

        static string FindLogicBasedSisterFile(this string activeDocument)
        {
            if (activeDocument.Contains("\\Entities\\", caseSensitive: false))
                return activeDocument.Replace("\\Entities\\", "\\-Logic\\", caseSensitive: false);

            else if (activeDocument.Contains("\\@Entities", caseSensitive: false))
                return activeDocument.Replace("\\@Entities\\", "\\@Logic\\");

            else if (activeDocument.Contains("\\Test\\@Logic\\", caseSensitive: false) && activeDocument.EndsWith("Fixture.cs", StringComparison.OrdinalIgnoreCase))
                return GetFixtureFileInModelProject(activeDocument);

            else if (activeDocument.Contains("\\-Logic\\", caseSensitive: false))
            {
                var result = string.Empty;
                result = activeDocument.Replace("\\-Logic\\", "\\Entities\\", caseSensitive: false);

                // some projects has @Entities other than Entities
                if (!File.Exists(result))
                    return activeDocument.Replace("\\-Logic\\", "\\@Entities\\", caseSensitive: false);
                else
                    return result;
            }

            return activeDocument;
        }

        static string GetFixtureFileInModelProject(string thisFile)
        {
            var otherFile = thisFile.Replace("\\Test\\@Logic\\", "\\Model\\@Logic\\").Replace("Fixture.cs", ".cs");
            if (!File.Exists(otherFile))
            {
                otherFile = thisFile.Replace("\\Test\\@Logic\\", "\\Model\\@Entities\\").Replace("Fixture.cs", ".cs");
                if (!File.Exists(otherFile))
                {
                    otherFile = thisFile.Replace("\\Test\\@Logic\\", "\\Model\\Entities\\").Replace("Fixture.cs", ".cs");
                    if (!File.Exists(otherFile))
                    {
                        otherFile = "";
                    }
                }
            }

            return otherFile;
        }

        const string ZebbleGeneratedCssFileName = "zebble-generated-css.cs";
        const string ZebbleGeneratedCssClassFileName = "zebble-generated.cs";

        static string FindZebblBasedSisterFile(this string zebbleFile)
        {
            if (Path.GetExtension(zebbleFile) == FileExtensionTypes.ZEBBLE)
            {
                var sisterFile = zebbleFile.Replace(Path.GetFileName(zebbleFile),
                                                    (Path.GetFileNameWithoutExtension(zebbleFile) + FileExtensionTypes.ZEBBLE_CSHARP))
                                           .ToLower();

                if (File.Exists(sisterFile)) return sisterFile;

                sisterFile = sisterFile.Replace(FileExtensionTypes.ZEBBLE_CSHARP, FileExtensionTypes.CSHARP);

                if (File.Exists(sisterFile)) return sisterFile;

                return sisterFile.Substring(0, sisterFile.IndexOf("app.ui\\") + "app.ui\\".Length) + ZebbleGeneratedCssClassFileName;
            }

            else
            {
                string sisterFile;
                if (zebbleFile.Contains("." + ZebbleGeneratedCssFileName))
                {
                    sisterFile = zebbleFile.Replace("-css", string.Empty);
                }
                else if (zebbleFile.Contains("." + ZebbleGeneratedCssClassFileName))
                {
                    sisterFile = zebbleFile.Replace(ZebbleGeneratedCssClassFileName, ZebbleGeneratedCssFileName);
                }
                else
                {
                    sisterFile = zebbleFile.Remove(".cs");
                }

                if (!File.Exists(sisterFile) && !zebbleFile.Contains(ZebbleGeneratedCssFileName))
                    return sisterFile.Substring(0, sisterFile.IndexOf("app.ui\\") + "app.ui\\".Length) + ZebbleGeneratedCssClassFileName;

                return sisterFile;
            }
        }
    }
}
