using Geeks.VSIX.SmartFinder.Definition;

namespace Geeks.VSIX.SmartFinder.FileFinder

{
    partial class FinderForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FinderForm));
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtSearchBox = new System.Windows.Forms.TextBox();
            this.mnuOptions = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btnShowOptions = new System.Windows.Forms.Button();
            this.FileFilderButton = new System.Windows.Forms.Button();
            this.CssFinder = new System.Windows.Forms.Button();
            this.MemberFinder = new System.Windows.Forms.Button();
            this.lstFiles = new Geeks.VSIX.SmartFinder.FileFinder.FlickerFreeListBox();
            this.SuspendLayout();
            // 
            // btnSelect
            // 
            this.btnSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelect.Location = new System.Drawing.Point(811, 684);
            this.btnSelect.Margin = new System.Windows.Forms.Padding(4);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(77, 28);
            this.btnSelect.TabIndex = 9;
            this.btnSelect.Text = "Select";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(728, 684);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 28);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtSearchBox
            // 
            this.txtSearchBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearchBox.BackColor = System.Drawing.SystemColors.Window;
            this.txtSearchBox.Location = new System.Drawing.Point(16, 68);
            this.txtSearchBox.Margin = new System.Windows.Forms.Padding(4);
            this.txtSearchBox.Name = "txtSearchBox";
            this.txtSearchBox.Size = new System.Drawing.Size(871, 22);
            this.txtSearchBox.TabIndex = 5;
            this.txtSearchBox.TextChanged += new System.EventHandler(this.txtSearchBox_TextChanged);
            this.txtSearchBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearchBox_KeyDown);
            // 
            // mnuOptions
            // 
            this.mnuOptions.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mnuOptions.Name = "contextMenuStrip1";
            this.mnuOptions.Size = new System.Drawing.Size(61, 4);
            // 
            // btnShowOptions
            // 
            this.btnShowOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowOptions.Location = new System.Drawing.Point(629, 684);
            this.btnShowOptions.Margin = new System.Windows.Forms.Padding(4);
            this.btnShowOptions.Name = "btnShowOptions";
            this.btnShowOptions.Size = new System.Drawing.Size(91, 28);
            this.btnShowOptions.TabIndex = 8;
            this.btnShowOptions.Text = "Options...";
            this.btnShowOptions.UseVisualStyleBackColor = true;
            this.btnShowOptions.Click += new System.EventHandler(this.btnShowOptions_Click);
            // 
            // FileFilderButton
            // 
            this.FileFilderButton.AutoSize = true;
            this.FileFilderButton.BackColor = System.Drawing.Color.Transparent;
            this.FileFilderButton.Location = new System.Drawing.Point(16, 12);
            this.FileFilderButton.Margin = new System.Windows.Forms.Padding(4);
            this.FileFilderButton.Name = "FileFilderButton";
            this.FileFilderButton.Size = new System.Drawing.Size(123, 48);
            this.FileFilderButton.TabIndex = 12;
            this.FileFilderButton.Tag = "File";
            this.FileFilderButton.Text = "File Finder";
            this.FileFilderButton.UseVisualStyleBackColor = false;
            this.FileFilderButton.Click += new System.EventHandler(this.FileFilderButton_Click);
            // 
            // CssFinder
            // 
            this.CssFinder.AutoSize = true;
            this.CssFinder.BackColor = System.Drawing.Color.Transparent;
            this.CssFinder.Location = new System.Drawing.Point(277, 12);
            this.CssFinder.Margin = new System.Windows.Forms.Padding(4);
            this.CssFinder.Name = "CssFinder";
            this.CssFinder.Size = new System.Drawing.Size(123, 48);
            this.CssFinder.TabIndex = 13;
            this.CssFinder.Tag = "CSS";
            this.CssFinder.Text = "Css Finder";
            this.CssFinder.UseVisualStyleBackColor = false;
            this.CssFinder.Click += new System.EventHandler(this.CssFinder_Click);
            // 
            // MemberFinder
            // 
            this.MemberFinder.AutoSize = true;
            this.MemberFinder.BackColor = System.Drawing.Color.Transparent;
            this.MemberFinder.Location = new System.Drawing.Point(147, 12);
            this.MemberFinder.Margin = new System.Windows.Forms.Padding(4);
            this.MemberFinder.Name = "MemberFinder";
            this.MemberFinder.Size = new System.Drawing.Size(123, 48);
            this.MemberFinder.TabIndex = 14;
            this.MemberFinder.Tag = "Member";
            this.MemberFinder.Text = "Member Finder";
            this.MemberFinder.UseVisualStyleBackColor = false;
            this.MemberFinder.Click += new System.EventHandler(this.MemberFinder_Click);
            // 
            // lstFiles
            // 
            this.lstFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstFiles.BackColor = System.Drawing.SystemColors.Window;
            this.lstFiles.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lstFiles.EmptyBehaviour = Geeks.VSIX.SmartFinder.Definition.EmptyBehaviour.None;
            this.lstFiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstFiles.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lstFiles.FormattingEnabled = true;
            this.lstFiles.HighlightWords = null;
            this.lstFiles.Location = new System.Drawing.Point(16, 108);
            this.lstFiles.Margin = new System.Windows.Forms.Padding(4);
            this.lstFiles.Name = "lstFiles";
            this.lstFiles.ShowLoadingAtTheEndOfList = false;
            this.lstFiles.Size = new System.Drawing.Size(871, 511);
            this.lstFiles.TabIndex = 7;
            this.lstFiles.DoubleClick += new System.EventHandler(this.lstFiles_DoubleClick);
            this.lstFiles.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstFiles_KeyDown);
            // 
            // FinderForm
            // 
            this.AcceptButton = this.btnSelect;
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(904, 727);
            this.Controls.Add(this.MemberFinder);
            this.Controls.Add(this.CssFinder);
            this.Controls.Add(this.FileFilderButton);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lstFiles);
            this.Controls.Add(this.btnShowOptions);
            this.Controls.Add(this.txtSearchBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FinderForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Finder";
            this.Load += new System.EventHandler(this.FinderForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        System.Windows.Forms.Button btnSelect;
        System.Windows.Forms.Button btnCancel;
        FlickerFreeListBox lstFiles;
        System.Windows.Forms.TextBox txtSearchBox;
        public System.Windows.Forms.ContextMenuStrip mnuOptions;
        System.Windows.Forms.Button btnShowOptions;
        private System.Windows.Forms.Button FileFilderButton;
        private System.Windows.Forms.Button CssFinder;
        private System.Windows.Forms.Button MemberFinder;
    }
}

