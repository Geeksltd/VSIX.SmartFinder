using System.Collections.Generic;

namespace Geeks.VSIX.SmartFinder.FileFinder
{
    internal class StyleScanner
    {
        public IEnumerable<Item> ExtractClasses(StyleFinderFileInfoDto fileInfo)
        {
            var token = string.Empty;
            var contentLength = fileInfo.FileContent.Length;
            var lineNumber = 1;

            for (var i = 0; i < contentLength; i++)
            {
                var currentCharacter = fileInfo.FileContent[i];
                if (currentCharacter == ' ') continue;
                var nextCharacter = (i == contentLength - 1) ? currentCharacter : fileInfo.FileContent[i + 1];
                if (StyleFinderUtils.IsNewLine(currentCharacter, nextCharacter))
                {
                    lineNumber++;
                    continue;
                }

                #region skip comments

                if (StyleFinderUtils.IsCommentSection(currentCharacter, nextCharacter))
                {
                    while (!(currentCharacter == '*' && nextCharacter == '/') && i < contentLength)
                    {
                        i++;

                        currentCharacter = fileInfo.FileContent[i];
                        nextCharacter = (i == contentLength - 1) ? currentCharacter : fileInfo.FileContent[i + 1];

                        if (StyleFinderUtils.IsNewLine(currentCharacter, nextCharacter)) lineNumber++;
                    }

                    i += 1;

                    if (i >= contentLength) yield break;

                    continue;
                }

                #endregion skip comments

                #region skip import commands
                if (StyleFinderUtils.IsImportCommand(currentCharacter, i, fileInfo.FileContent, contentLength))
                {
                    while (!StyleFinderUtils.IsNewLine(currentCharacter, nextCharacter) && i < contentLength)
                    {
                        i++;

                        try
                        {
                            currentCharacter = fileInfo.FileContent[i];
                            nextCharacter = (i == contentLength - 1) ? currentCharacter : fileInfo.FileContent[i + 1];
                        }
                        catch
                        {
                            nextCharacter = '\n';
                        }

                    }

                    if (i >= contentLength) yield break;

                    lineNumber++;
                    continue;
                }
                #endregion skip import commands

                #region catch high level normal classes ".normal",#selectors and selectors

                if (currentCharacter == '.' || (currentCharacter >= 65 && currentCharacter <= 122) || currentCharacter == '#')
                {
                    while (currentCharacter != '{')
                    {
                        if (currentCharacter == ':' || currentCharacter == ';')
                            token = string.Empty;

                        if (currentCharacter == ',')
                        {
                            yield return new Item(fileInfo.ProjectBasePath, fileInfo.FileName)
                            {
                                Phrase = token.Trim(),
                                LineNumber = lineNumber
                            };

                            token = string.Empty;
                            if (i >= contentLength) yield break;
                        }

                        i++;

                        if (i >= contentLength) yield break;

                        if (StyleFinderUtils.ContainsValidCharacter(currentCharacter))
                            token += currentCharacter;

                        currentCharacter = fileInfo.FileContent[i];
                        nextCharacter = (i == contentLength - 1) ? currentCharacter : fileInfo.FileContent[i + 1];
                    }

                    yield return new Item(fileInfo.ProjectBasePath, fileInfo.FileName)
                    {
                        Phrase = token.Trim(),
                        LineNumber = lineNumber
                    };

                    token = string.Empty;

                    if (i >= contentLength) yield break;
                    continue;
                }

                #endregion catch high level normal classes ".normal",#selectors and selectors

                if (currentCharacter == '\t' || StyleFinderUtils.IsNewLine(currentCharacter, nextCharacter))
                    currentCharacter = ' ';

                if (StyleFinderUtils.ContainsValidCharacter(currentCharacter))
                    token += currentCharacter;
            }
        }
    }
}
