using Geeks.VSIX.SmartFinder.FileFinder.FileFinder;
using System.Collections.Generic;
using System.Drawing;

namespace Geeks.VSIX.SmartFinder.Definition
{
    public class IconDictionary
    {
        public static Image MSharpIcon => FileTypesResources.MSharp;

        public static Dictionary<string, Image> Icons;
        static IconDictionary()
        {
            Icons = new Dictionary<string, Image>
            {
            { ".7z", FileTypesResources._7z},
            { ".zip", FileTypesResources._7z},
            { ".rar", FileTypesResources._7z},
            { ".addin", FileTypesResources.addin},
            { ".asax", FileTypesResources.asax},
            { ".ascx", FileTypesResources.ascx},
            { ".ashx", FileTypesResources.ashx},
            { ".aspx", FileTypesResources.aspx},
            { ".config", FileTypesResources.config},
            { ".cs", FileTypesResources.cs},
            { ".cshtml", FileTypesResources.cshtml},
            { ".csproj", FileTypesResources.csproj},
            { ".css", FileTypesResources.css},
            { ".designer.cs", FileTypesResources.designer_cs},
            { ".designer.vb", FileTypesResources.designer_cs},
            { ".dll", FileTypesResources.dll},
            { ".doc", FileTypesResources.doc},
            { ".rtf", FileTypesResources.doc},
            { ".exe", FileTypesResources.exe},
            { ".gif", FileTypesResources.gif},
            { ".png", FileTypesResources.gif},
            { ".bmp", FileTypesResources.gif},
            { ".jpg", FileTypesResources.gif},
            { ".jpeg", FileTypesResources.gif},
            { ".htm", FileTypesResources.htm},
            { ".html", FileTypesResources.htm},
            { ".js", FileTypesResources.js},
            { ".javascript", FileTypesResources.js},
            { ".less", FileTypesResources.less},
            { ".scss", FileTypesResources.scss},
            { ".msi", FileTypesResources.msi},
            { "setup.exe", FileTypesResources.msi},
            { ".pdb", FileTypesResources.pdb},
            { ".pdf", FileTypesResources.pdf},
            { ".resx", FileTypesResources.resx},
            { ".sitemap", FileTypesResources.sitemap},
            { ".sln", FileTypesResources.sln},
            { ".suo", FileTypesResources.suo},
            { ".swf", FileTypesResources.swf},
            { ".ts", FileTypesResources.ts},
            { ".txt", FileTypesResources.txt},
            { ".user", FileTypesResources.user},
            { ".vb", FileTypesResources.vb},
            { ".vbproj", FileTypesResources.vbproj},
            { ".vdproj", FileTypesResources.vdproj},
            { ".xml", FileTypesResources.xml},
        };
        }
    }
}
