namespace InternalLibrary.Forms
{
    partial class ConflictingVersionDialog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConflictingVersionDialog));
            this.conflictOptionsTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.pullLatestVersionButton = new System.Windows.Forms.Button();
            this.overwriteDriveVersionButton = new System.Windows.Forms.Button();
            this.mergeChangesButton = new System.Windows.Forms.Button();
            this.createNewCopyButton = new System.Windows.Forms.Button();
            this.messagePanel = new System.Windows.Forms.Panel();
            this.messageLabel = new System.Windows.Forms.Label();
            this.messageTextBox = new System.Windows.Forms.TextBox();
            this.conflictOptionsTableLayoutPanel.SuspendLayout();
            this.messagePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // conflictOptionsTableLayoutPanel
            // 
            this.conflictOptionsTableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.conflictOptionsTableLayoutPanel.ColumnCount = 1;
            this.conflictOptionsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.conflictOptionsTableLayoutPanel.Controls.Add(this.pullLatestVersionButton, 0, 0);
            this.conflictOptionsTableLayoutPanel.Controls.Add(this.overwriteDriveVersionButton, 0, 1);
            this.conflictOptionsTableLayoutPanel.Controls.Add(this.mergeChangesButton, 0, 2);
            this.conflictOptionsTableLayoutPanel.Controls.Add(this.createNewCopyButton, 0, 3);
            this.conflictOptionsTableLayoutPanel.Location = new System.Drawing.Point(12, 96);
            this.conflictOptionsTableLayoutPanel.Name = "conflictOptionsTableLayoutPanel";
            this.conflictOptionsTableLayoutPanel.RowCount = 4;
            this.conflictOptionsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.conflictOptionsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.conflictOptionsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.conflictOptionsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.conflictOptionsTableLayoutPanel.Size = new System.Drawing.Size(416, 364);
            this.conflictOptionsTableLayoutPanel.TabIndex = 0;
            // 
            // pullLatestVersionButton
            // 
            this.pullLatestVersionButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pullLatestVersionButton.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.pullLatestVersionButton.Location = new System.Drawing.Point(3, 3);
            this.pullLatestVersionButton.Name = "pullLatestVersionButton";
            this.pullLatestVersionButton.Size = new System.Drawing.Size(410, 85);
            this.pullLatestVersionButton.TabIndex = 0;
            this.pullLatestVersionButton.Text = "Pull the latest version from Google Drive";
            this.pullLatestVersionButton.UseVisualStyleBackColor = false;
            this.pullLatestVersionButton.Click += new System.EventHandler(this.pullLatestVersionButton_Click);
            // 
            // overwriteDriveVersionButton
            // 
            this.overwriteDriveVersionButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.overwriteDriveVersionButton.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.overwriteDriveVersionButton.Location = new System.Drawing.Point(3, 94);
            this.overwriteDriveVersionButton.Name = "overwriteDriveVersionButton";
            this.overwriteDriveVersionButton.Size = new System.Drawing.Size(410, 85);
            this.overwriteDriveVersionButton.TabIndex = 1;
            this.overwriteDriveVersionButton.Text = "Ignore and save over changes on Google Drive";
            this.overwriteDriveVersionButton.UseVisualStyleBackColor = false;
            this.overwriteDriveVersionButton.Click += new System.EventHandler(this.overwriteDriveVersionButton_Click);
            // 
            // mergeChangesButton
            // 
            this.mergeChangesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mergeChangesButton.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.mergeChangesButton.Location = new System.Drawing.Point(3, 185);
            this.mergeChangesButton.Name = "mergeChangesButton";
            this.mergeChangesButton.Size = new System.Drawing.Size(410, 85);
            this.mergeChangesButton.TabIndex = 2;
            this.mergeChangesButton.Text = "Merge the changes from Google Drive into this document";
            this.mergeChangesButton.UseVisualStyleBackColor = false;
            this.mergeChangesButton.Click += new System.EventHandler(this.mergeChangesButton_Click);
            // 
            // createNewCopyButton
            // 
            this.createNewCopyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.createNewCopyButton.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.createNewCopyButton.Location = new System.Drawing.Point(3, 276);
            this.createNewCopyButton.Name = "createNewCopyButton";
            this.createNewCopyButton.Size = new System.Drawing.Size(410, 85);
            this.createNewCopyButton.TabIndex = 3;
            this.createNewCopyButton.Text = "Create a new copy in Google Drive using this document";
            this.createNewCopyButton.UseVisualStyleBackColor = false;
            this.createNewCopyButton.Click += new System.EventHandler(this.createNewCopyButton_Click);
            // 
            // messagePanel
            // 
            this.messagePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.messagePanel.Controls.Add(this.messageTextBox);
            this.messagePanel.Controls.Add(this.messageLabel);
            this.messagePanel.Location = new System.Drawing.Point(12, 12);
            this.messagePanel.Name = "messagePanel";
            this.messagePanel.Size = new System.Drawing.Size(413, 78);
            this.messagePanel.TabIndex = 1;
            // 
            // messageLabel
            // 
            this.messageLabel.AutoSize = true;
            this.messageLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.messageLabel.Location = new System.Drawing.Point(3, 3);
            this.messageLabel.Margin = new System.Windows.Forms.Padding(3);
            this.messageLabel.Name = "messageLabel";
            this.messageLabel.Size = new System.Drawing.Size(145, 13);
            this.messageLabel.TabIndex = 0;
            this.messageLabel.Text = "Newer Version Available";
            // 
            // messageTextBox
            // 
            this.messageTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.messageTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.messageTextBox.Location = new System.Drawing.Point(6, 22);
            this.messageTextBox.Multiline = true;
            this.messageTextBox.Name = "messageTextBox";
            this.messageTextBox.Size = new System.Drawing.Size(404, 53);
            this.messageTextBox.TabIndex = 1;
            this.messageTextBox.Text = resources.GetString("messageTextBox.Text");
            // 
            // ConflictingSaveDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(440, 472);
            this.Controls.Add(this.messagePanel);
            this.Controls.Add(this.conflictOptionsTableLayoutPanel);
            this.Name = "ConflictingSaveDialog";
            this.Text = "Outdated Version Warning";
            this.conflictOptionsTableLayoutPanel.ResumeLayout(false);
            this.messagePanel.ResumeLayout(false);
            this.messagePanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel conflictOptionsTableLayoutPanel;
        private System.Windows.Forms.Button pullLatestVersionButton;
        private System.Windows.Forms.Button overwriteDriveVersionButton;
        private System.Windows.Forms.Button mergeChangesButton;
        private System.Windows.Forms.Button createNewCopyButton;
        private System.Windows.Forms.Panel messagePanel;
        private System.Windows.Forms.TextBox messageTextBox;
        private System.Windows.Forms.Label messageLabel;
    }
}