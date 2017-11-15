using EnvDTE;
using EnvDTE80;
using Geeks.VSIX.SmartFinder.Base;
using Geeks.VSIX.SmartFinder.Properties;
using GeeksAddin;
using System;
using System.Drawing;
using System.Linq;

namespace Geeks.VSIX.SmartFinder.FileFinder
{
    internal abstract class FinderBaseGadget : Gadget
    {
        public FinderBaseGadget() { }

        public abstract Loader GetLoaderAgent(string[] basePaths, Repository repository);

        public virtual string GetTitle() => FinderLiterals.BaseFileFinderTitle;

        public virtual Color GetColor() => Color.FromArgb(0xF0, 0xF0, 0xF0);

        public override void Run(DTE2 app)
        {
            try
            {
                var basePaths = Utils.FindSolutionDirectories(app);

                if (basePaths == null || basePaths.Count() == 0) return;

                var repository = new Repository(/*basePath*/);
                var loader = GetLoaderAgent(basePaths, repository);
                var filterer = new Filterer(basePaths, repository);
                var form = new FinderForm(GetTitle(), GetColor(), loader, filterer, defaultSearchTerm: app.GetSelectedText());

                if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var foundItem = form.GetSelectedItem();
                    FoundItemsBank.Items = filterer.FoundItems;
                    GotoItem(app, basePaths, foundItem);
                }

                else app.StatusBar.Text = "Ready";
            }
            catch (Exception e) { ErrorNotification.EmailError(e); }
        }

        public static void GotoItem(DTE2 app, string[] basePaths, Item item)
        {
            try
            {
                if (item.FileName.HasValue())
                {
                    app.ItemOperations.OpenFile(item.FullPath, EnvDTE.Constants.vsViewKindAny);
                    var selection = app.ActiveDocument.Selection as TextSelection;

                    selection?.MoveTo(item.LineNumber, item.Column, Extend: false);

                    if (Settings.Default.TrackItemInSolutionExplorer)
                        TrackInSolutionExplorer(app, item);
                }
            }
            catch (Exception err)
            {
                ErrorNotification.EmailError(err);
            }
        }

        static void TrackInSolutionExplorer(DTE2 app, Item item)
        {
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
            foreach (UIHierarchyItem CurrentItem in children)
            {
                if (CurrentItem.Object is EnvDTE.ProjectItem projectitem)
                {
                    short i = 1;
                    while (i <= projectitem.FileCount)
                    {
                        if (projectitem.get_FileNames(i) == fileName)
                        {
                            solutionExplorerPath = CurrentItem.Name;
                            return CurrentItem;
                        }

                        i = (short)(i + 1);
                    }
                }

                if (FindItem(CurrentItem.UIHierarchyItems, fileName, out solutionExplorerPath)
                    is UIHierarchyItem childItem)
                {
                    solutionExplorerPath = CurrentItem.Name + "\\" + solutionExplorerPath;
                    return childItem;
                }
            }

            solutionExplorerPath = "";
            return null;
        }
    }
}