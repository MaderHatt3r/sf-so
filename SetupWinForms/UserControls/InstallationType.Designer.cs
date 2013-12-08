namespace Setup.UserControls
{
    partial class InstallationType
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
            this.titleTextBox = new System.Windows.Forms.TextBox();
            this.descriptionTextBox = new System.Windows.Forms.TextBox();
            this.customInstallationRadioButton = new System.Windows.Forms.RadioButton();
            this.fullInstallationRadioButton = new System.Windows.Forms.RadioButton();
            this.customInstallatinDescriptionTextBox = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // titleTextBox
            // 
            this.titleTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.titleTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleTextBox.Location = new System.Drawing.Point(138, 28);
            this.titleTextBox.Multiline = true;
            this.titleTextBox.Name = "titleTextBox";
            this.titleTextBox.ReadOnly = true;
            this.titleTextBox.Size = new System.Drawing.Size(291, 45);
            this.titleTextBox.TabIndex = 3;
            this.titleTextBox.TabStop = false;
            this.titleTextBox.Text = "Installation Type";
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.descriptionTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.descriptionTextBox.Location = new System.Drawing.Point(138, 79);
            this.descriptionTextBox.Multiline = true;
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.ReadOnly = true;
            this.descriptionTextBox.Size = new System.Drawing.Size(519, 24);
            this.descriptionTextBox.TabIndex = 4;
            this.descriptionTextBox.TabStop = false;
            this.descriptionTextBox.Text = "Please select the type of installation you would like to perfrom.";
            // 
            // customInstallationRadioButton
            // 
            this.customInstallationRadioButton.AutoSize = true;
            this.customInstallationRadioButton.Location = new System.Drawing.Point(238, 109);
            this.customInstallationRadioButton.Name = "customInstallationRadioButton";
            this.customInstallationRadioButton.Size = new System.Drawing.Size(113, 17);
            this.customInstallationRadioButton.TabIndex = 5;
            this.customInstallationRadioButton.Text = "Custom Installation";
            this.customInstallationRadioButton.UseVisualStyleBackColor = true;
            this.customInstallationRadioButton.CheckedChanged += new System.EventHandler(this.customInstallationRadioButton_CheckedChanged);
            // 
            // fullInstallationRadioButton
            // 
            this.fullInstallationRadioButton.AutoSize = true;
            this.fullInstallationRadioButton.Checked = true;
            this.fullInstallationRadioButton.Location = new System.Drawing.Point(238, 185);
            this.fullInstallationRadioButton.Name = "fullInstallationRadioButton";
            this.fullInstallationRadioButton.Size = new System.Drawing.Size(94, 17);
            this.fullInstallationRadioButton.TabIndex = 6;
            this.fullInstallationRadioButton.TabStop = true;
            this.fullInstallationRadioButton.Text = "Full Installation";
            this.fullInstallationRadioButton.UseVisualStyleBackColor = true;
            // 
            // customInstallatinDescriptionTextBox
            // 
            this.customInstallatinDescriptionTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.customInstallatinDescriptionTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customInstallatinDescriptionTextBox.Location = new System.Drawing.Point(275, 132);
            this.customInstallatinDescriptionTextBox.Multiline = true;
            this.customInstallatinDescriptionTextBox.Name = "customInstallatinDescriptionTextBox";
            this.customInstallatinDescriptionTextBox.ReadOnly = true;
            this.customInstallatinDescriptionTextBox.Size = new System.Drawing.Size(382, 47);
            this.customInstallatinDescriptionTextBox.TabIndex = 7;
            this.customInstallatinDescriptionTextBox.TabStop = false;
            this.customInstallatinDescriptionTextBox.Text = "You will be given the option to choose which Add-In\'s you would like to install.";
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(275, 208);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(382, 47);
            this.textBox1.TabIndex = 8;
            this.textBox1.TabStop = false;
            this.textBox1.Text = "This will install all of the Add-In\'s currently available for the Microsoft Offic" +
    "e Suite.";
            // 
            // InstallationType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.customInstallatinDescriptionTextBox);
            this.Controls.Add(this.fullInstallationRadioButton);
            this.Controls.Add(this.customInstallationRadioButton);
            this.Controls.Add(this.descriptionTextBox);
            this.Controls.Add(this.titleTextBox);
            this.Name = "InstallationType";
            this.Size = new System.Drawing.Size(660, 369);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox titleTextBox;
        private System.Windows.Forms.TextBox descriptionTextBox;
        private System.Windows.Forms.RadioButton customInstallationRadioButton;
        private System.Windows.Forms.RadioButton fullInstallationRadioButton;
        private System.Windows.Forms.TextBox customInstallatinDescriptionTextBox;
        private System.Windows.Forms.TextBox textBox1;
    }
}
