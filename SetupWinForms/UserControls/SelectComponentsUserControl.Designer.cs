namespace Setup.UserControls
{
    partial class SelectComponentsUserControl
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
            this.versionCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.descriptionTextBox = new System.Windows.Forms.TextBox();
            this.titleTextBox = new System.Windows.Forms.TextBox();
            this.selectionTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // versionCheckedListBox
            // 
            this.versionCheckedListBox.FormattingEnabled = true;
            this.versionCheckedListBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.versionCheckedListBox.Items.AddRange(new object[] {
            "Word",
            "Excel"});
            this.versionCheckedListBox.Location = new System.Drawing.Point(138, 109);
            this.versionCheckedListBox.Name = "versionCheckedListBox";
            this.versionCheckedListBox.Size = new System.Drawing.Size(275, 94);
            this.versionCheckedListBox.TabIndex = 0;
            this.versionCheckedListBox.UseCompatibleTextRendering = true;
            this.versionCheckedListBox.SelectedIndexChanged += new System.EventHandler(this.versionCheckedListBox_SelectedItemChanged);
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
            this.descriptionTextBox.TabIndex = 1;
            this.descriptionTextBox.TabStop = false;
            this.descriptionTextBox.Text = "Select the applications you would like to install this Add-In for from the box be" +
    "low:";
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
            this.titleTextBox.TabIndex = 2;
            this.titleTextBox.TabStop = false;
            this.titleTextBox.Text = "Select Components";
            // 
            // selectionTextBox
            // 
            this.selectionTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.selectionTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectionTextBox.Location = new System.Drawing.Point(419, 109);
            this.selectionTextBox.Multiline = true;
            this.selectionTextBox.Name = "selectionTextBox";
            this.selectionTextBox.ReadOnly = true;
            this.selectionTextBox.Size = new System.Drawing.Size(238, 94);
            this.selectionTextBox.TabIndex = 3;
            this.selectionTextBox.TabStop = false;
            this.selectionTextBox.Text = "Selection:\r\n\r\n";
            // 
            // SelectComponentsUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.selectionTextBox);
            this.Controls.Add(this.titleTextBox);
            this.Controls.Add(this.descriptionTextBox);
            this.Controls.Add(this.versionCheckedListBox);
            this.Name = "SelectComponentsUserControl";
            this.Size = new System.Drawing.Size(660, 369);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox versionCheckedListBox;
        private System.Windows.Forms.TextBox descriptionTextBox;
        private System.Windows.Forms.TextBox titleTextBox;
        private System.Windows.Forms.TextBox selectionTextBox;
    }
}
