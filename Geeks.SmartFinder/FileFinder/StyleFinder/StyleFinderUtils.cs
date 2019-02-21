using System;
using System.IO;

namespace Geeks.VSIX.SmartFinder.FileFinder
{
    internal class StyleFinderUtils
    {
        static string[] MsharpFrontEndProjectNames = new[] { "website", "website\\" };
        static string[] ZebbleFrontEndProjectNames = new[] { "app.ui", "app.ui\\" };

        internal static bool IsFrontEndProject(string projectBasePath)
        {
            if (projectBasePath.ToLower().EndsWithAny(MsharpFrontEndProjectNames))
                return true;

            else if (projectBasePath.ToLower().EndsWithAny(ZebbleFrontEndProjectNames))
                return true;

            else
                return false;
        }

        internal static bool IsMSharpFrontEnd(string projectPath)
        {
            return projectPath.ToLower().EndsWithAny(MsharpFrontEndProjectNames);
        }

        internal static bool IsNewLine(char currentCharacter, char nextCharacter)
        {
            return currentCharacter == '\r' && nextCharacter == '\n' ||
                   currentCharacter == '\n' && nextCharacter == '\r' ||
                   (currentCharacter == '\n' || currentCharacter == '\r');
        }

        internal static bool IsCommentSection(char currentCharacter, char nextCharacter)
        {
            return currentCharacter == '/' && nextCharacter == '*';
        }

        internal static bool ContainsValidCharacter(char currentCharacter)
        {
            return currentCharacter != '@' &&
                   currentCharacter != '\r' &&
                   currentCharacter != '\n' &&
                   currentCharacter != '\t' &&
                   currentCharacter != '{' &&
                   currentCharacter != '}' &&
                   currentCharacter != '*' &&
                   currentCharacter != '/' &&
                   currentCharacter != '\\' &&
                   currentCharacter != ',' &&
                   currentCharacter != ' ' &&
                   currentCharacter != ':' &&
                   currentCharacter != ';' &&
                   currentCharacter.ToString() != "\"" &&
                   currentCharacter != '_';
        }

        internal static bool IsImportCommand(char currentChar, int index, string content, int length)
        {
            if (currentChar == '@' && index + 6 < length && content.Substring(index + 1, 6) == "import")
                return true;
            return false;
        }

        internal static bool IsCompiledFromLess(string cssFile)
        {
            var directory = Path.GetDirectoryName(cssFile);
            var fileName = Path.GetFileNameWithoutExtension(cssFile);

            if (fileName.EndsWith(".min"))
                fileName = Path.GetFileNameWithoutExtension(fileName);

            return File.Exists(Path.Combine(directory, fileName + ".less"));
        }
    }
}
