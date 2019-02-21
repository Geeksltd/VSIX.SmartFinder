using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Geeks.VSIX.SmartFinder.Properties;

namespace Geeks.VSIX.SmartFinder.FileFinder
{
    class FileLoaderAgent : Loader
    {
        string[] BasePaths;
        Repository Repository;

        public FileLoaderAgent(string[] basePaths, Repository repository)
        {
            BasePaths = basePaths;
            Repository = repository;

            WorkerSupportsCancellation = true;
        }

        protected override void OnDoWork(DoWorkEventArgs e) => AddFilesInPaths(e);

        static string[] ExcludedFileTypes = { ".dll", ".exe", ".pdb" , ".zebble-generated.cs" , ".zebble-generated-css.cs" };

        void AddFilesInPaths(DoWorkEventArgs e)
        {
            if (e.Cancel) return;

            foreach (var basePath in BasePaths)
                AddFilesInPath(e, projectBasePath: basePath.TrimEnd("\\"), directory: basePath);
        }

        void AddFilesInPath(DoWorkEventArgs e, string projectBasePath, string directory)
        {
            if (!Directory.Exists(directory) || DirectoryExcluded(directory)) return;

            AddFilesInDirectory(projectBasePath, directory);

            if (e.Cancel) return;

            AddSubdirectories(e, projectBasePath, directory);
        }

        void AddSubdirectories(DoWorkEventArgs e, string projectBasePath, string directory)
        {
            try
            {
                var directories = Directory.GetDirectories(directory);
                foreach (var d in directories)
                {
                    if (e.Cancel) return;
                    AddFilesInPath(e, projectBasePath, d);
                }
            }
            catch (UnauthorizedAccessException err)
            {
                throw err;
                MessageBox.Show(err.Message,
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        void AddFilesInDirectory(string projectBasePath, string directory)
        {
            try
            {
                var files = Directory.GetFiles(directory);
                if (!files.IsEmpty())
                {
                    var items = files.Where(f => !f.EndsWithAny(ExcludedFileTypes)).SelectMany(f => ConvertToItem(projectBasePath, f));

                    if (!items.IsEmpty())
                        Repository.AppendRange(items);
                }
            }
            catch (UnauthorizedAccessException err)
            {
                throw err;
                MessageBox.Show(err.Message,
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        static readonly Regex ClassPattern = new Regex(@"class ([^\s]+)", RegexOptions.Compiled);

        IEnumerable<Item> ConvertToItem(string projectBasePath, string file)
        {
            // try to extract classes inside the file
            if (file.EndsWith(".cs"))
            {
                var lines = File.ReadAllLines(file);
                for (var index = 1; index < lines.Length; index++)
                {
                    var m = ClassPattern.Match(lines[index]);
                    if (m.Success)
                    {
                        var className = m.Groups[1].Value;
                        var fileName = Path.GetFileNameWithoutExtension(file);
                        if (!className.Equals(fileName, StringComparison.InvariantCultureIgnoreCase))
                        {
                            yield return new Item(projectBasePath, file)
                            {
                                LineNumber = index + 1,
                                Phrase = className,
                                Icon = Geeks.VSIX.SmartFinder.Definition.IconType.Class
                            };
                        }
                    }
                }
            }

            yield return new Item(projectBasePath, file)
            {
                LineNumber = 1
            };
        }

        bool DirectoryExcluded(string directory)
        {
            string sex = Settings.Default.ExcludedDirectories;
            var dirs = Settings.Default.ExcludedDirectories.Split(';');
            foreach (var dir in dirs)
                if (directory.Contains(dir)) return true;
            return false;
        }

        ToolStripMenuItem excludeResourcesToolStripMenuItem = new ToolStripMenuItem("Exclude Resources");
        ToolStripMenuItem trackFoundIteToolStripMenuItem = new ToolStripMenuItem("Track Found Item");

        public override void LoadOptions()
        {
            excludeResourcesToolStripMenuItem.Checked = Settings.Default.ExcludeResources;
            trackFoundIteToolStripMenuItem.Checked = Settings.Default.TrackItemInSolutionExplorer;
        }

        public override void DisplayOptions(ContextMenuStrip mnuOptions)
        {
            mnuOptions.Items.Add(excludeResourcesToolStripMenuItem);
            mnuOptions.Items.Add(trackFoundIteToolStripMenuItem);
        }

        public override void OptionClicked(ToolStripMenuItem toolStripMenuItem, ref bool searchAgain, ref bool loadAgain)
        {
            if (toolStripMenuItem == excludeResourcesToolStripMenuItem)
            {
                excludeResourcesToolStripMenuItem.Checked = !excludeResourcesToolStripMenuItem.Checked;
                Settings.Default.ExcludeResources = excludeResourcesToolStripMenuItem.Checked;
                Settings.Default.Save();
                searchAgain = true;
            }

            if (toolStripMenuItem == trackFoundIteToolStripMenuItem)
            {
                trackFoundIteToolStripMenuItem.Checked = !trackFoundIteToolStripMenuItem.Checked;
                Settings.Default.TrackItemInSolutionExplorer = trackFoundIteToolStripMenuItem.Checked;
                Settings.Default.Save();
            }
        }
    }
}