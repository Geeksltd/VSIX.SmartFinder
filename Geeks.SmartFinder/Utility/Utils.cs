using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EnvDTE;
using EnvDTE80;
using Geeks.VSIX.SmartFinder;
using Geeks.VSIX.SmartFinder.Base;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;
using Microsoft.VisualStudio.Shell;
using System.Windows.Forms;
using System.Drawing;
using System.Windows;

namespace GeeksAddin
{
    public static class Utils
    {
        public static string GetSolutionName(DTE2 app)
        {
            if (app == null || app.Solution == null || string.IsNullOrEmpty(app.Solution.FullName)) return "";
            return Path.GetFileNameWithoutExtension(app.Solution.FullName);
        }

        public static string[] FindSolutionDirectories(DTE2 app)
        {
            var basePaths = new List<string>();

            if (app.Solution != null)
            {
                for (var i = 1; i <= app.Solution.Projects.Count; i++)
                {
                    var projectItem = app.Solution.Projects.Item(i);
                    AddPathFromProjectItem(basePaths, projectItem);
                }

                return basePaths.ToArray();
            }

            app.StatusBar.Text = "No solution or project is identified. app.Solution is " +
                (app.Solution?.GetType().Name).Or("NULL");

            App.DTE = (DTE2)Package.GetGlobalService(typeof(SDTE));

            return null;
        }

        static void AddPathFromProjectItem(List<string> basePaths, Project projectItem)
        {
            if (projectItem == null) return;

            try
            {
                // Project
                var projectFileName = projectItem.FileName;

                if (!string.IsNullOrWhiteSpace(projectFileName))
                {
                    if (projectItem.Properties.Item("FullPath").Value is string fullPath)
                        basePaths.Add(fullPath);
                }
                else
                {
                    // Folder
                    for (var i = 1; i <= projectItem.ProjectItems.Count; i++)
                        AddPathFromProjectItem(basePaths, projectItem.ProjectItems.Item(i).Object as Project);
                }
            }
            //An unloaded project
            catch (NotImplementedException ex)
            {
                Debug.WriteLine(ex);
            }
            catch (Exception err)
            {
                ErrorNotification.EmailError(err);
            }
        }

        public static IEnumerable<string> SplitCommandLine(string commandLine)
        {
            var inQuotes = false;

            return commandLine.Split(c =>
            {
                if (c == '\"') inQuotes = !inQuotes;
                return !inQuotes && c == ' ';
            }).Select(arg => arg.Trim().TrimMatchingQuotes())
              .Where(arg => !string.IsNullOrEmpty(arg));
        }

        public static IEnumerable<string> Split(this string str, Func<char, bool> controller)
        {
            var nextPiece = 0;

            for (var c = 0; c < str.Length; c++)
            {
                if (controller(str[c]))
                {
                    yield return str.Substring(nextPiece, c - nextPiece);
                    nextPiece = c + 1;
                }
            }

            yield return str.Substring(nextPiece);
        }

        public static string TrimMatchingQuotes(this string input, char quote = '\"')
        {
            if ((input.Length >= 2) &&
                (input[0] == quote) && (input[input.Length - 1] == quote))
                return input.Substring(1, input.Length - 2);

            return input;
        }

        public static bool ContainsAny(this string str, params string[] subStrings)
        {
            foreach (var subString in subStrings)
                if (str.Contains(subString)) return true;

            return false;
        }

        /// <summary>
        /// Changes fonts of controls contained in font collection recursively. <br/>
        /// <b>Usage:</b> <c><br/>
        /// SetAllControlsFont(this.Controls, 20); // This makes fonts 20% bigger. <br/>
        /// SetAllControlsFont(this.Controls, -4, false); // This makes fonts smaller by 4.</c>
        /// </summary>
        /// <param name="ctrls">Control collection containing controls</param>
        /// <param name="amount">Amount to change: posive value makes it bigger, 
        /// negative value smaller</param>
        /// <param name="amountInPercent">True - grow / shrink in percent, 
        /// False - grow / shrink absolute</param>
        public static void SetAllControlsFontSize(
                           System.Windows.Forms.Control.ControlCollection ctrls,
                           int amount = 0, bool amountInPercent = true)
        {
            if (amount == 0) return;
            foreach (Control ctrl in ctrls)
            {
                // recursive
                if (ctrl.Controls != null) SetAllControlsFontSize(ctrl.Controls,
                                                                  amount, amountInPercent);
                if (ctrl != null)
                {
                    var oldSize = ctrl.Font.Size;
                    float newSize =
                       (amountInPercent) ? oldSize + oldSize * (amount / 100) : oldSize + amount;
                    if (newSize < 4) newSize = 4; // don't allow less than 4
                    var fontFamilyName = ctrl.Font.FontFamily.Name;
                    ctrl.Font = new Font(fontFamilyName, newSize);
                };
            };
        }

        public static int GetWindowsScaling()
        {
            return (int)(100 * Screen.PrimaryScreen.Bounds.Width / SystemParameters.PrimaryScreenWidth);
        }
    }
}
