using System;
using System.Drawing;
using System.Windows.Forms;
using Geeks.SmartFinder.Properties;
using GeeksAddin;
using Microsoft.Win32;

namespace Geeks.VSIX.SmartFinder.FileFinder
{
    public partial class FormOptions : Form
    {
        readonly string PreviousExcludedDirectoryText = string.Empty;

        public FormOptions()
        {
            InitializeComponent();
            PreviousExcludedDirectoryText = txtExcludedDirectories.Text;
            this.Font = SystemFonts.IconTitleFont;
            SystemEvents.UserPreferenceChanged += new UserPreferenceChangedEventHandler(SystemEvents_UserPreferenceChanged);
            this.FormClosing += new FormClosingEventHandler(Form_FormClosing);
        }

        void btnSaveOptions_Click(object sender, EventArgs e)
        {
            Settings.Default.Save();
            if (PreviousExcludedDirectoryText != Settings.Default.ExcludedDirectories)
            {
                ((FinderForm)Owner).CallFiltererRepositoryUpdate();
            }

            Close();
        }

        void chkBoxMethods_Click(object sender, EventArgs e)
        {
            chkBoxMethodParams.Enabled = chkBoxMethods.Checked;
            chkBoxMethodReturnTypes.Enabled = chkBoxMethods.Checked;
            chkBoxClassNames.Enabled = chkBoxMethods.Checked;

            if (!chkBoxMethods.Checked)
            {
                chkBoxMethodParams.Checked = false;
                chkBoxMethodReturnTypes.Checked = false;
                chkBoxClassNames.Checked = false;
            }
        }

        void FormOptions_Load(object sender, EventArgs e)
        {
            if (!chkBoxMethods.Checked)
            {
                chkBoxMethodParams.Enabled = false;
                chkBoxMethodReturnTypes.Enabled = false;
                chkBoxClassNames.Enabled = false;
            }
        }
        void SystemEvents_UserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
        {
            if (e.Category == UserPreferenceCategory.Window)
            {
                this.Font = SystemFonts.IconTitleFont;
            }
        }

        void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            SystemEvents.UserPreferenceChanged -= new UserPreferenceChangedEventHandler(SystemEvents_UserPreferenceChanged);
        }
    }
}