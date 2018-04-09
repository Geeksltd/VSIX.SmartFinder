using System;
using System.IO;
using EnvDTE80;
using Geeks.VSIX.SmartFinder.Base;
using GeeksAddin;

namespace Geeks.VSIX.SmartFinder.FileToggle
{
    internal class FileToggleGadget : Gadget
    {
        public FileToggleGadget()
        {
            Name = "WebFileToggle";
            Title = "Web File Toggle";
            ShortKey = "F7";
        }

        public override void Run(DTE2 app)
        {
            try
            {
                if (app.ActiveDocument == null)
                {
                    app.StatusBar.Text = "No Active Document";
                    return;
                }

                var sisterFile = app.ActiveDocument.FullName.FindSisterFile();

                if (sisterFile.HasValue() && File.Exists(sisterFile))
                {
                    sisterFile = sisterFile.WrapInQuatation(); // handle white spaces in the path
                    app.ExecuteCommand("File.OpenFile", sisterFile);
                    app.StatusBar.Text = "Ready";
                }
                else
                    app.StatusBar.Text = "File Not found: " + sisterFile;
            }
            catch (Exception err)
            {
                ErrorNotification.EmailError(err);
            }
        }

        internal static string GetFixtureFileInModelProject(string thisFile)
        {
            var otherFile = thisFile.Replace("\\Test\\@Logic\\", "\\Model\\@Logic\\").Replace("Fixture.cs", ".cs");
            if (!File.Exists(otherFile))
            {
                otherFile = thisFile.Replace("\\Test\\@Logic\\", "\\Model\\@Entities\\").Replace("Fixture.cs", ".cs");
                if (!File.Exists(otherFile))
                {
                    otherFile = thisFile.Replace("\\Test\\@Logic\\", "\\Model\\Entities\\").Replace("Fixture.cs", ".cs");
                    if (!File.Exists(otherFile)) otherFile = "";
                }
            }

            return otherFile;
        }
    }
}
