using Geeks.VSIX.SmartFinder.Properties;
using System;
using System.Windows.Forms;

namespace Geeks.VSIX.SmartFinder.FileFinder
{
    public partial class FormOptions : Form
    {
        readonly string PreviousExcludedDirectoryText = string.Empty;

        public FormOptions()
        {
            InitializeComponent();
            PreviousExcludedDirectoryText = txtExcludedDirectories.Text;
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
    }
}
