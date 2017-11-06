using System;
using System.IO;
using EnvDTE80;
using GeeksAddin;
using Geeks.VSIX.SmartFinder.Base;

namespace Geeks.VSIX.SmartFinder.FileToggle
{
    internal class FixtureFileToggleGadget : Gadget
    {
        public FixtureFileToggleGadget()
        {
            Name = "FixtureFileToggle";
            Title = "Fixture File Toggle";
            ShortKey = "SHIFT+F7";
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

                var thisFile = app.ActiveDocument.FullName;
                var thisFilePath = Path.GetDirectoryName(thisFile);
                var otherFile = "";

                if (thisFile.Contains("\\Test\\@Logic\\") && thisFile.EndsWith("Fixture.cs"))
                {
                    // switch back from Fixture to logic
                    otherFile = FileToggleGadget.GetFixtureFileInModelProject(thisFile);
                }
                else
                {
                    var otherFilePath = thisFilePath.Replace("\\Model\\", "\\Test\\").Replace("\\Entities", "\\@Logic").Replace("\\@Entities", "\\@Logic");
                    otherFile = Path.Combine(
                        otherFilePath,
                        Path.GetFileNameWithoutExtension(thisFile) + "Fixture.cs");
                }

                if (File.Exists(otherFile))
                {
                    app.ExecuteCommand("File.OpenFile", otherFile.WrapInQuatation());
                    return;
                }

                app.StatusBar.Text = "File Not found: " + otherFile;
            }
            catch (Exception err)
            {
                ErrorNotification.EmailError(err);
            }
        }
    }
}
