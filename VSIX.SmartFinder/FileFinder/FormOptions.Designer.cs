namespace Geeks.VSIX.SmartFinder.FileFinder
{
    partial class FormOptions
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
            this.lblExcludeDirectories = new System.Windows.Forms.Label();
            this.txtExcludedDirectories = new System.Windows.Forms.TextBox();
            this.lblFileTypes = new System.Windows.Forms.Label();
            this.txtResources = new System.Windows.Forms.TextBox();
            this.chkBoxExcludeResources = new System.Windows.Forms.CheckBox();
            this.chkBoxMethodParams = new System.Windows.Forms.CheckBox();
            this.chkBoxMethods = new System.Windows.Forms.CheckBox();
            this.chkBoxMethodReturnTypes = new System.Windows.Forms.CheckBox();
            this.chkBoxClassNames = new System.Windows.Forms.CheckBox();
            this.chkBoxProperties = new System.Windows.Forms.CheckBox();
            this.btnSaveOptions = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblExcludeDirectories
            // 
            this.lblExcludeDirectories.AutoSize = true;
            this.lblExcludeDirectories.Location = new System.Drawing.Point(12, 7);
            this.lblExcludeDirectories.Name = "lblExcludeDirectories";
            this.lblExcludeDirectories.Size = new System.Drawing.Size(150, 13);
            this.lblExcludeDirectories.TabIndex = 15;
            this.lblExcludeDirectories.Text = "Exclude directories starting by:";
            // 
            // txtExcludedDirectories
            // 
            this.txtExcludedDirectories.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtExcludedDirectories.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Geeks.VSIX.SmartFinder.Properties.Settings.Default, "ExcludedDirectories", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtExcludedDirectories.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtExcludedDirectories.Location = new System.Drawing.Point(12, 25);
            this.txtExcludedDirectories.Name = "txtExcludedDirectories";
            this.txtExcludedDirectories.Size = new System.Drawing.Size(498, 23);
            this.txtExcludedDirectories.TabIndex = 14;
            this.txtExcludedDirectories.Text = global::Geeks.VSIX.SmartFinder.Properties.Settings.Default.ExcludedDirectories;
            // 
            // lblFileTypes
            // 
            this.lblFileTypes.AutoSize = true;
            this.lblFileTypes.Location = new System.Drawing.Point(12, 67);
            this.lblFileTypes.Name = "lblFileTypes";
            this.lblFileTypes.Size = new System.Drawing.Size(177, 13);
            this.lblFileTypes.TabIndex = 16;
            this.lblFileTypes.Text = "Files types to consider as Resource:";
            // 
            // txtResources
            // 
            this.txtResources.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtResources.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Geeks.VSIX.SmartFinder.Properties.Settings.Default, "ResourceFileTypes", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtResources.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtResources.Location = new System.Drawing.Point(12, 83);
            this.txtResources.Name = "txtResources";
            this.txtResources.Size = new System.Drawing.Size(498, 23);
            this.txtResources.TabIndex = 17;
            this.txtResources.Text = global::Geeks.VSIX.SmartFinder.Properties.Settings.Default.ResourceFileTypes;
            // 
            // chkBoxExcludeResources
            // 
            this.chkBoxExcludeResources.AutoSize = true;
            this.chkBoxExcludeResources.Checked = global::Geeks.VSIX.SmartFinder.Properties.Settings.Default.ExcludeResources;
            this.chkBoxExcludeResources.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBoxExcludeResources.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::Geeks.VSIX.SmartFinder.Properties.Settings.Default, "ExcludeResources", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkBoxExcludeResources.Location = new System.Drawing.Point(15, 134);
            this.chkBoxExcludeResources.Name = "chkBoxExcludeResources";
            this.chkBoxExcludeResources.Size = new System.Drawing.Size(118, 17);
            this.chkBoxExcludeResources.TabIndex = 18;
            this.chkBoxExcludeResources.Text = "Exclude Resources";
            this.chkBoxExcludeResources.UseVisualStyleBackColor = true;
            // 
            // chkBoxMethodParams
            // 
            this.chkBoxMethodParams.AutoSize = true;
            this.chkBoxMethodParams.Checked = global::Geeks.VSIX.SmartFinder.Properties.Settings.Default.ShowMethodParameters;
            this.chkBoxMethodParams.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::Geeks.VSIX.SmartFinder.Properties.Settings.Default, "ShowMethodParameters", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkBoxMethodParams.Location = new System.Drawing.Point(255, 134);
            this.chkBoxMethodParams.Name = "chkBoxMethodParams";
            this.chkBoxMethodParams.Size = new System.Drawing.Size(148, 17);
            this.chkBoxMethodParams.TabIndex = 20;
            this.chkBoxMethodParams.Text = "Show Method Parameters";
            this.chkBoxMethodParams.UseVisualStyleBackColor = true;
            // 
            // chkBoxMethods
            // 
            this.chkBoxMethods.AutoSize = true;
            this.chkBoxMethods.Checked = global::Geeks.VSIX.SmartFinder.Properties.Settings.Default.ShowMethods;
            this.chkBoxMethods.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBoxMethods.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::Geeks.VSIX.SmartFinder.Properties.Settings.Default, "ShowMethods", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkBoxMethods.Location = new System.Drawing.Point(255, 157);
            this.chkBoxMethods.Name = "chkBoxMethods";
            this.chkBoxMethods.Size = new System.Drawing.Size(97, 17);
            this.chkBoxMethods.TabIndex = 21;
            this.chkBoxMethods.Text = "Show Methods";
            this.chkBoxMethods.UseVisualStyleBackColor = true;
            this.chkBoxMethods.Click += new System.EventHandler(this.chkBoxMethods_Click);
            // 
            // chkBoxMethodReturnTypes
            // 
            this.chkBoxMethodReturnTypes.AutoSize = true;
            this.chkBoxMethodReturnTypes.Checked = global::Geeks.VSIX.SmartFinder.Properties.Settings.Default.ShowMethodReturnTypes;
            this.chkBoxMethodReturnTypes.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::Geeks.VSIX.SmartFinder.Properties.Settings.Default, "ShowMethodReturnTypes", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkBoxMethodReturnTypes.Location = new System.Drawing.Point(255, 180);
            this.chkBoxMethodReturnTypes.Name = "chkBoxMethodReturnTypes";
            this.chkBoxMethodReturnTypes.Size = new System.Drawing.Size(159, 17);
            this.chkBoxMethodReturnTypes.TabIndex = 22;
            this.chkBoxMethodReturnTypes.Text = "Show Method Return Types";
            this.chkBoxMethodReturnTypes.UseVisualStyleBackColor = true;
            // 
            // chkBoxClassNames
            // 
            this.chkBoxClassNames.AutoSize = true;
            this.chkBoxClassNames.Checked = global::Geeks.VSIX.SmartFinder.Properties.Settings.Default.ShowClassNames;
            this.chkBoxClassNames.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::Geeks.VSIX.SmartFinder.Properties.Settings.Default, "ShowClassNames", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkBoxClassNames.Location = new System.Drawing.Point(255, 203);
            this.chkBoxClassNames.Name = "chkBoxClassNames";
            this.chkBoxClassNames.Size = new System.Drawing.Size(117, 17);
            this.chkBoxClassNames.TabIndex = 23;
            this.chkBoxClassNames.Text = "Show Class Names";
            this.chkBoxClassNames.UseVisualStyleBackColor = true;
            // 
            // chkBoxProperties
            // 
            this.chkBoxProperties.AutoSize = true;
            this.chkBoxProperties.Checked = global::Geeks.VSIX.SmartFinder.Properties.Settings.Default.ShowProperties;
            this.chkBoxProperties.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBoxProperties.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::Geeks.VSIX.SmartFinder.Properties.Settings.Default, "ShowProperties", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkBoxProperties.Location = new System.Drawing.Point(255, 227);
            this.chkBoxProperties.Name = "chkBoxProperties";
            this.chkBoxProperties.Size = new System.Drawing.Size(103, 17);
            this.chkBoxProperties.TabIndex = 24;
            this.chkBoxProperties.Text = "Show Properties";
            this.chkBoxProperties.UseVisualStyleBackColor = true;
            // 
            // btnSaveOptions
            // 
            this.btnSaveOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveOptions.Location = new System.Drawing.Point(415, 327);
            this.btnSaveOptions.Name = "btnSaveOptions";
            this.btnSaveOptions.Size = new System.Drawing.Size(95, 23);
            this.btnSaveOptions.TabIndex = 25;
            this.btnSaveOptions.Text = "Save Now";
            this.btnSaveOptions.UseVisualStyleBackColor = true;
            this.btnSaveOptions.Click += new System.EventHandler(this.btnSaveOptions_Click);
            // 
            // FormOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(522, 362);
            this.Controls.Add(this.btnSaveOptions);
            this.Controls.Add(this.chkBoxProperties);
            this.Controls.Add(this.chkBoxClassNames);
            this.Controls.Add(this.chkBoxMethodReturnTypes);
            this.Controls.Add(this.chkBoxMethods);
            this.Controls.Add(this.chkBoxMethodParams);
            this.Controls.Add(this.chkBoxExcludeResources);
            this.Controls.Add(this.txtResources);
            this.Controls.Add(this.lblFileTypes);
            this.Controls.Add(this.lblExcludeDirectories);
            this.Controls.Add(this.txtExcludedDirectories);
            this.Name = "FormOptions";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Options";
            this.Load += new System.EventHandler(this.FormOptions_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        System.Windows.Forms.Label lblExcludeDirectories;
        System.Windows.Forms.TextBox txtExcludedDirectories;
        System.Windows.Forms.Label lblFileTypes;
        System.Windows.Forms.TextBox txtResources;
        System.Windows.Forms.CheckBox chkBoxExcludeResources;
        System.Windows.Forms.CheckBox chkBoxMethodParams;
        System.Windows.Forms.CheckBox chkBoxMethods;
        System.Windows.Forms.CheckBox chkBoxMethodReturnTypes;
        System.Windows.Forms.CheckBox chkBoxClassNames;
        System.Windows.Forms.CheckBox chkBoxProperties;
        System.Windows.Forms.Button btnSaveOptions;
    }
}