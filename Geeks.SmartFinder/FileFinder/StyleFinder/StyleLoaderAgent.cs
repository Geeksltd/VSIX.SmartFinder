using Geeks.SmartFinder.Properties;

namespace Geeks.VSIX.SmartFinder.FileFinder
{
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    //using Geeks.VSIX.SmartFinder.Properties;

    class StyleLoaderAgent : Loader
    {
        string[] BasePaths;
        Repository Repository;

        public StyleLoaderAgent(string[] basePaths, Repository repository)
        {
            BasePaths = basePaths;
            Repository = repository;

            WorkerSupportsCancellation = true;
        }

        protected override void OnDoWork(DoWorkEventArgs e) => AddFilesInPaths(e);

        void AddFilesInPaths(DoWorkEventArgs e)
        {
            if (e.Cancel) return;

            var basePaths = BasePaths.Where(Directory.Exists);
            foreach (var directoryPath in basePaths)
                AddFilesInPath(e, directoryPath, directoryPath);
        }

        void AddFilesInPath(DoWorkEventArgs e, string projectBasePath, string directory)
        {
            if (StyleFinderUtils.IsMSharpFrontEnd(projectBasePath))
            {
                if (directory.Contains("styles"))
                {
                    AddStyleFiles(projectBasePath, directory, "*.scss");
                    AddStyleFiles(projectBasePath, directory, "*.scss");
                    AddStyleFiles(projectBasePath, directory, "*.less");
                }
            }
            else
            {
                AddStyleFiles(projectBasePath, directory, "*.sass");
                AddStyleFiles(projectBasePath, directory, "*.scss");
                AddStyleFiles(projectBasePath, directory, "*.less");
                AddStyleFiles(projectBasePath, directory, "*.css");

            }

            if (e.Cancel) return;

            AddSubdirectories(e, projectBasePath, directory);
        }

        void AddSubdirectories(DoWorkEventArgs e, string projectBasePath, string directory)
        {
            var directories = Directory.GetDirectories(directory);
            foreach (var d in directories)
            {
                if (e.Cancel) return;
                AddFilesInPath(e, projectBasePath, d);
            }
        }

        void AddStyleFiles(string projectBasePath, string directory, string filter)
        {
            var scanner = new StyleScanner();

            var files = Directory.GetFiles(directory, filter);
            if (filter == "*.css")
                files = files.Where(f => StyleFinderUtils.IsCompiledFromLess(f) == false).ToArray();

            foreach (var file in files)
            {
                using (var content = File.OpenText(file))
                {
                    var classes = scanner.ExtractClasses(new StyleFinderFileInfoDto
                    {
                        FileContent = content.ReadToEnd(),
                        FileName = file,
                        ProjectBasePath = projectBasePath
                    });

                    Repository.AppendRange(classes);
                }
            }
        }

        ToolStripMenuItem trackFoundIteToolStripMenuItem = new ToolStripMenuItem("Track Found Item");

        public override void LoadOptions() => trackFoundIteToolStripMenuItem.Checked = Settings.Default.TrackItemInSolutionExplorer;

        public override void DisplayOptions(ContextMenuStrip mnuOptions) => mnuOptions.Items.Add(trackFoundIteToolStripMenuItem);

        public override void OptionClicked(ToolStripMenuItem toolStripMenuItem, ref bool searchAgain, ref bool loadAgain)
        {
            if (toolStripMenuItem == trackFoundIteToolStripMenuItem)
            {
                trackFoundIteToolStripMenuItem.Checked = !trackFoundIteToolStripMenuItem.Checked;
                Settings.Default.TrackItemInSolutionExplorer = trackFoundIteToolStripMenuItem.Checked;
                Settings.Default.Save();
            }
        }
    }
}
