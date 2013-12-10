namespace Setup.UserControls
{
    partial class InstallationUserControl
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
            this.currentTaskProgressBar = new System.Windows.Forms.ProgressBar();
            this.overallProgressBar = new System.Windows.Forms.ProgressBar();
            this.descriptionTextBox = new System.Windows.Forms.TextBox();
            this.titleTextBox = new System.Windows.Forms.TextBox();
            this.currentTaskTextBox = new System.Windows.Forms.TextBox();
            this.overallTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // currentTaskProgressBar
            // 
            this.currentTaskProgressBar.Location = new System.Drawing.Point(138, 142);
            this.currentTaskProgressBar.Name = "currentTaskProgressBar";
            this.currentTaskProgressBar.Size = new System.Drawing.Size(384, 23);
            this.currentTaskProgressBar.TabIndex = 0;
            // 
            // overallProgressBar
            // 
            this.overallProgressBar.Location = new System.Drawing.Point(138, 202);
            this.overallProgressBar.Name = "overallProgressBar";
            this.overallProgressBar.Size = new System.Drawing.Size(384, 23);
            this.overallProgressBar.TabIndex = 1;
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.descriptionTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.descriptionTextBox.Location = new System.Drawing.Point(138, 79);
            this.descriptionTextBox.Multiline = true;
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.ReadOnly = true;
            this.descriptionTextBox.Size = new System.Drawing.Size(384, 21);
            this.descriptionTextBox.TabIndex = 2;
            this.descriptionTextBox.TabStop = false;
            this.descriptionTextBox.Text = "Please press the install button to begin installation.";
            // 
            // titleTextBox
            // 
            this.titleTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.titleTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleTextBox.Location = new System.Drawing.Point(138, 28);
            this.titleTextBox.Multiline = true;
            this.titleTextBox.Name = "titleTextBox";
            this.titleTextBox.ReadOnly = true;
            this.titleTextBox.Size = new System.Drawing.Size(236, 45);
            this.titleTextBox.TabIndex = 3;
            this.titleTextBox.TabStop = false;
            this.titleTextBox.Text = "Ready to Install";
            // 
            // currentTaskTextBox
            // 
            this.currentTaskTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.currentTaskTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentTaskTextBox.Location = new System.Drawing.Point(138, 118);
            this.currentTaskTextBox.Multiline = true;
            this.currentTaskTextBox.Name = "currentTaskTextBox";
            this.currentTaskTextBox.ReadOnly = true;
            this.currentTaskTextBox.Size = new System.Drawing.Size(384, 18);
            this.currentTaskTextBox.TabIndex = 4;
            this.currentTaskTextBox.TabStop = false;
            this.currentTaskTextBox.Text = "Initializing...";
            // 
            // overallTextBox
            // 
            this.overallTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.overallTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.overallTextBox.Location = new System.Drawing.Point(138, 178);
            this.overallTextBox.Multiline = true;
            this.overallTextBox.Name = "overallTextBox";
            this.overallTextBox.ReadOnly = true;
            this.overallTextBox.Size = new System.Drawing.Size(384, 18);
            this.overallTextBox.TabIndex = 5;
            this.overallTextBox.TabStop = false;
            this.overallTextBox.Text = "Overall Progress";
            // 
            // InstallationUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.overallTextBox);
            this.Controls.Add(this.currentTaskTextBox);
            this.Controls.Add(this.titleTextBox);
            this.Controls.Add(this.descriptionTextBox);
            this.Controls.Add(this.overallProgressBar);
            this.Controls.Add(this.currentTaskProgressBar);
            this.Name = "InstallationUserControl";
            this.Size = new System.Drawing.Size(660, 369);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar currentTaskProgressBar;
        private System.Windows.Forms.ProgressBar overallProgressBar;
        private System.Windows.Forms.TextBox descriptionTextBox;
        private System.Windows.Forms.TextBox titleTextBox;
        private System.Windows.Forms.TextBox currentTaskTextBox;
        private System.Windows.Forms.TextBox overallTextBox;
    }
}
