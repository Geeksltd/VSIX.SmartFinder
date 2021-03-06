using System;
using System.Drawing;
using System.Linq;
using EnvDTE;
using EnvDTE80;
using Geeks.SmartFinder.Properties;
using Geeks.VSIX.SmartFinder.Base;
//using Geeks.VSIX.SmartFinder.Properties;
using GeeksAddin;

namespace Geeks.VSIX.SmartFinder.FileFinder
{
    internal abstract class FinderBaseGadget : Gadget
    {
        protected FinderBaseGadget() { }

        public abstract Loader GetLoaderAgent(string[] basePaths, Repository repository);

        public virtual string GetTitle() => FinderLiterals.BaseFileFinderTitle;

        public virtual Color GetColor() => Color.FromArgb(0xF0, 0xF0, 0xF0);
        string GetSourceFilePath(DTE2 app)
        {
            var uih = app.ToolWindows.SolutionExplorer;
            var selectedItems = (Array)uih.SelectedItems;
            if (null == selectedItems) return string.Empty;

            foreach (UIHierarchyItem selItem in selectedItems)
            {
                var projectItem = selItem.Object as ProjectItem;
                if (projectItem == null)
                    return selItem.Name;

                var itemProperties = projectItem?.Properties;
                return itemProperties?.Item("FullPath").Value.ToString();
            }

            return string.Empty;
        }

        public override void Run(DTE2 app)
        {
            try
            {
                var currentSelectedFilepath = GetSourceFilePath(app) ?? "";

                var basePathsList = Utils.FindSolutionDirectories(app).ToList();

                if (!basePathsList.Any()) return;

                var basePath = basePathsList.FirstOrDefault(p => currentSelectedFilepath.StartsWith(p, StringComparison.OrdinalIgnoreCase)) ??
                               basePathsList.FirstOrDefault(p => p.EndsWith($"{currentSelectedFilepath}\\".Replace("#", null), StringComparison.OrdinalIgnoreCase));

                if (basePath.HasValue())
                {
                    basePathsList.Remove(basePath);
                    basePathsList.Insert(0, basePath);
                }

                var basePaths = basePathsList.ToArray();
                var repository = new Repository( /*basePath*/);
                var loader = GetLoaderAgent(basePaths, repository);
                var filterer = new Filterer(basePaths, repository);
                var form = new FinderForm(GetTitle(),
                    GetColor(),
                    loader,
                    filterer,
                    defaultSearchTerm: app.GetSelectedText());

                if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var foundItem = form.GetSelectedItem();
                    FoundItemsBank.Items = filterer.FoundItems;
                    GotoItem(app, basePaths, foundItem);
                }

                else app.StatusBar.Text = "Ready ...";
            }
            catch (Exception e)
            {
                ErrorNotification.EmailError(e);
            }
        }

        public static void GotoItem(DTE2 app, string[] basePaths, Item item)
        {
            if (item != null)
            {
                try
                {
                    if (!item.FileName.HasValue()) return;

                    app.ItemOperations.OpenFile(item.FullPath, EnvDTE.Constants.vsViewKindAny);
                    var selection = app.ActiveDocument.Selection as TextSelection;

                    selection?.MoveTo(item.LineNumber, item.Column, Extend: false);

                    if (Settings.Default.TrackItemInSolutionExplorer)
                        TrackInSolutionExplorer(app, item);
                }
                catch (Exception err)
                {
                    ErrorNotification.EmailError(err);
                }
            }
        }

        static void TrackInSolutionExplorer(DTE2 app, Item item)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            try
            {
                app.ExecuteCommand("SolutionExplorer.SyncWithActiveDocument");
            }
            catch
            {
                app.Solution.FindProjectItem(item.FullPath).ExpandView();
                FindItem(app.ToolWindows.SolutionExplorer.UIHierarchyItems, item.FullPath,
                    out var solutionExplorerPath);

                if (solutionExplorerPath.HasValue())
                {
                    app.ToolWindows.SolutionExplorer.GetItem(solutionExplorerPath)
                        .Select(vsUISelectionType.vsUISelectionTypeSelect);
                }
            }
        }

        static object FindItem(UIHierarchyItems children, string fileName, out string solutionExplorerPath)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            foreach (UIHierarchyItem CurrentItem in children)
            {
                if (CurrentItem.Object is ProjectItem projectItem)
                {
                    short i = 1;
                    while (i <= projectItem.FileCount)
                    {
                        if (projectItem.FileNames[i] == fileName)
                        {
                            solutionExplorerPath = CurrentItem.Name;
                            return CurrentItem;
                        }

                        i = (short)(i + 1);
                    }
                }

                if (!(FindItem(CurrentItem.UIHierarchyItems, fileName, out solutionExplorerPath)
                    is UIHierarchyItem childItem)) continue;

                solutionExplorerPath = CurrentItem.Name + "\\" + solutionExplorerPath;
                return childItem;
            }

            solutionExplorerPath = "";
            return null;
        }
    }
}