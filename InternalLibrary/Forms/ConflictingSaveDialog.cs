using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InternalLibrary.Data;

namespace InternalLibrary.Forms
{
    public partial class ConflictingSaveDialog : Form
    {
        private ConflictResolutionOptions selectedOption;
        
        public ConflictingSaveDialog()
        {
            InitializeComponent();
        }

        public void ShowDialog(ref ConflictResolutionOptions selectedOption)
        {
            this.Show();
            this.selectedOption = selectedOption;
        }

        private void pullLatestVersionButton_Click(object sender, EventArgs e)
        {
            selectedOption = ConflictResolutionOptions.PULL;
            this.Close();
        }

        private void overwriteDriveVersionButton_Click(object sender, EventArgs e)
        {
            selectedOption = ConflictResolutionOptions.FORCE_PUSH;
            this.Close();
        }

        private void mergeChangesButton_Click(object sender, EventArgs e)
        {
            selectedOption = ConflictResolutionOptions.MERGE;
            this.Close();
        }

        private void createNewCopyButton_Click(object sender, EventArgs e)
        {
            selectedOption = ConflictResolutionOptions.CREATE_NEW;
            this.Close();
        }
    }
}
