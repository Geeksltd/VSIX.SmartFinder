using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Geeks.VSIX.SmartFinder.Definition;
using Geeks.VSIX.SmartFinder.Properties;

namespace Geeks.VSIX.SmartFinder.FileFinder
{
    partial class FinderForm : Form
    {
        Filterer Filterer;
        Loader Loader;

        internal static string SearchTerm;

        public FinderForm()
        {
            InitializeComponent();
        }

        public FinderForm(string title, Color color, Loader loader, Filterer filterer, string defaultSearchTerm)
        {
            InitializeComponent();

            Text = title;
            BackColor = color;

            Filterer = filterer;
            Filterer.ExcludedFileTypes = (Settings.Default.ExcludeResources) ? Settings.Default.ResourceFileTypes.Split(';') : null;

            Filterer.ItemsFound += new EventHandler<ItemsEventArgs>(Filterer_ItemsFound);
            Filterer.AnnouncementOfExistingItemsFinished += new EventHandler(Filterer_AnnouncementOfExistingItemsFinished);

            Loader = loader;
            Loader.RunWorkerAsync(Filterer);
            Loader.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Loader_RunWorkerCompleted);

            Loader.LoadOptions();

            SearchTerm = string.Empty;
            txtSearchBox.Text = defaultSearchTerm ?? "";
            Search();
        }

        public int CalculateItemHeight()
        {
            var area = SystemInformation.WorkingArea;
            var box = CreateGraphics();
            if (box.DpiX < 144) return 17;
            else if (box.DpiX >= 144 && box.DpiX <= 200) return 23;
            return 28;
        }

        void Filterer_AnnouncementOfExistingItemsFinished(object sender, EventArgs e)
        {
            SearchFinished();
        }

        void Loader_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SearchFinished();
        }

        void Filterer_ItemsFound(object sender, ItemsEventArgs e)
        {
            lstFiles.SafeAction(x => x.Items.AddRange(e.Items.ToArray()));
        }

        void SearchStarted()
        {
            lstFiles.Items.Clear();
            lstFiles.EmptyBehaviour = EmptyBehaviour.ShowLoading;
            lstFiles.ShowLoadingAtTheEndOfList = true;
        }

        void SearchFinished()
        {
            var searchedFinished = !(Loader.IsBusy || Filterer.IsBusy);
            if (searchedFinished)
            {
                lstFiles.EmptyBehaviour = EmptyBehaviour.ShowNotFound;
                lstFiles.ShowLoadingAtTheEndOfList = false;
            }
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;

            Close();
        }

        void btnSelect_Click(object sender, EventArgs e)
        {
            Settings.Default.Save();

            SelectCurrentItem();
        }

        void txtSearchBox_TextChanged(object sender, EventArgs e)
        {
            SearchTerm = txtSearchBox.Text;
            Search();
        }

        public void CallFiltererRepositoryUpdate() => Filterer.UpdateRepositoryItems(Loader);

        void Search()
        {
            Filterer.ExcludedFileTypes = (Settings.Default.ExcludeResources) ? Settings.Default.ResourceFileTypes.Split(';') : null;
            SearchStarted();
            Filterer.SetFilter(txtSearchBox.Text);
            lstFiles.HighlightWords = Filterer.Words;
        }

        #region Keyboard
        void txtSearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    if (lstFiles.SelectedIndex > 0)
                        lstFiles.SelectedIndex--;
                    e.Handled = true;
                    break;
                case Keys.Down:
                    if (lstFiles.Items.Count > lstFiles.SelectedIndex + 1)
                        lstFiles.SelectedIndex++;
                    e.Handled = true;
                    break;
            }

            if ((e.KeyCode == Keys.Back) && e.Control)
            {
                e.SuppressKeyPress = true;
                var selStart = txtSearchBox.SelectionStart;
                while (selStart > 0 && txtSearchBox.Text.Substring(selStart - 1, 1) == " ")
                    selStart--;

                var prevSpacePos = -1;
                if (selStart != 0)
                {
                    prevSpacePos = txtSearchBox.Text.LastIndexOf(' ', selStart - 1);
                }

                txtSearchBox.Select(prevSpacePos + 1, txtSearchBox.SelectionStart - prevSpacePos - 1);
                txtSearchBox.SelectedText = "";

                e.Handled = true;
                var currentCaretPostition = txtSearchBox.SelectionStart;
                txtSearchBox.Text = txtSearchBox.Text.Replace("\x7f", "");
                txtSearchBox.SelectionStart = Math.Max(0, currentCaretPostition - 1);
            }

            HandleCommonKeys(e);
        }

        void lstFiles_KeyDown(object sender, KeyEventArgs e) => HandleCommonKeys(e);

        void HandleCommonKeys(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.PageDown:
                    lstFiles.SelectedIndex = Math.Min(lstFiles.SelectedIndex + 10, lstFiles.Items.Count - 1);
                    e.Handled = true;
                    break;
                case Keys.PageUp:
                    lstFiles.SelectedIndex = Math.Max(lstFiles.SelectedIndex - 10, 0);
                    e.Handled = true;
                    break;
                case Keys.Escape:
                    Close();
                    e.Handled = true;
                    break;
                case Keys.Enter:
                    SelectCurrentItem();
                    break;
            }
        }

        #endregion

        void lstFiles_DoubleClick(object sender, EventArgs e) => SelectCurrentItem();

        void SelectCurrentItem()
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        public Item GetSelectedItem()
        {
            var index = -1;
            if (lstFiles.SelectedIndex >= 0)
                index = lstFiles.SelectedIndex;
            else if (lstFiles.Items.Count > 0) index = 0;

            if (index != -1)
            {
                return lstFiles.Items[index] as Item;
            }

            return null;
        }

        void FinderForm_Load(object sender, EventArgs e)
        {
            Width = Screen.PrimaryScreen.Bounds.Width / 2;
            StartPosition = FormStartPosition.CenterScreen;

            Loader.DisplayOptions(mnuOptions);
            foreach (ToolStripItem option in mnuOptions.Items)
            {
                if (option is ToolStripMenuItem)
                    option.Click += new EventHandler(option_Click);
            }
        }

        void option_Click(object sender, EventArgs e)
        {
            var searchAgain = false;
            var loadAgain = false;
            Loader.OptionClicked((ToolStripMenuItem)sender, ref searchAgain, ref loadAgain);
            if (loadAgain)
            {
                Filterer.Repository.Clear();
                if (!Loader.IsBusy)
                    Loader.RunWorkerAsync(Filterer);
            }

            if (searchAgain) Search();

            txtSearchBox.Focus();
        }

        void btnOptions_Click(object sender, EventArgs e)
        {
            mnuOptions.Show(btnOptions, new Point(0, btnOptions.Height), ToolStripDropDownDirection.BelowRight);
        }

        void btnShowOptions_Click(object sender, EventArgs e)
        {
            var form = new FormOptions();
            form.ShowDialog(this);
        }
    }
}