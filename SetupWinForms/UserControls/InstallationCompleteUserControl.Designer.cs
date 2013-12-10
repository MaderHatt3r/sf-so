namespace Setup.UserControls
{
    partial class InstallationCompleteUserControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.descriptionTextBox = new System.Windows.Forms.TextBox();
            this.titleTextBox = new System.Windows.Forms.TextBox();
            this.supportSiteLinkLabel = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.descriptionTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.descriptionTextBox.Location = new System.Drawing.Point(138, 79);
            this.descriptionTextBox.Multiline = true;
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.ReadOnly = true;
            this.descriptionTextBox.Size = new System.Drawing.Size(519, 218);
            this.descriptionTextBox.TabIndex = 1;
            this.descriptionTextBox.TabStop = false;
            this.descriptionTextBox.Text = "Thank you for choosing Save Frequently · Save Often.";
            // 
            // titleTextBox
            // 
            this.titleTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.titleTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleTextBox.Location = new System.Drawing.Point(138, 28);
            this.titleTextBox.Multiline = true;
            this.titleTextBox.Name = "titleTextBox";
            this.titleTextBox.ReadOnly = true;
            this.titleTextBox.Size = new System.Drawing.Size(314, 45);
            this.titleTextBox.TabIndex = 2;
            this.titleTextBox.TabStop = false;
            this.titleTextBox.Text = "Installation Complete";
            // 
            // supportSiteLinkLabel
            // 
            this.supportSiteLinkLabel.AutoSize = true;
            this.supportSiteLinkLabel.Location = new System.Drawing.Point(346, 356);
            this.supportSiteLinkLabel.Name = "supportSiteLinkLabel";
            this.supportSiteLinkLabel.Size = new System.Drawing.Size(296, 13);
            this.supportSiteLinkLabel.TabIndex = 3;
            this.supportSiteLinkLabel.TabStop = true;
            this.supportSiteLinkLabel.Text = "Please feel free to visit CTDragon.com for any support needs.";
            this.supportSiteLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.supportSiteLinkLabel_LinkClicked);
            // 
            // InstallationCompleteUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.supportSiteLinkLabel);
            this.Controls.Add(this.titleTextBox);
            this.Controls.Add(this.descriptionTextBox);
            this.Name = "InstallationCompleteUserControl";
            this.Size = new System.Drawing.Size(660, 369);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox descriptionTextBox;
        private System.Windows.Forms.TextBox titleTextBox;
        private System.Windows.Forms.LinkLabel supportSiteLinkLabel;
    }
}
